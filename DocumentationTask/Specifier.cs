using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace Documentation
{
    public class Specifier<T> : ISpecifier
    {
        public string GetApiDescription()
        {
            return typeof(T)
                .GetCustomAttributes(false)
                .OfType<ApiDescriptionAttribute>()
                .FirstOrDefault()
                ?.Description;
        }

        public string[] GetApiMethodNames()
        {
            var methodNames = new List<string>();
            foreach (var apiMethod in typeof(T)
                .GetMethods()
                .Where(info => info.GetCustomAttributes(false)
                    .OfType<ApiMethodAttribute>().FirstOrDefault() != null))
            {
                methodNames.Add(apiMethod.Name);
            }

            return methodNames.ToArray();
        }

        public string GetApiMethodDescription(string methodName)
        {
            return typeof(T)
                .GetMethod(methodName)
                ?.GetCustomAttributes()
                .OfType<ApiDescriptionAttribute>()
                .FirstOrDefault()
                ?.Description;
        }

        public string[] GetApiMethodParamNames(string methodName)
        {
            string[] param;
            try
            {
                param = typeof(T)
                    .GetMethod(methodName)
                    ?.GetParameters()
                    .Select(p => p.Name)
                    .ToArray();
            }
            catch
            {
                param = new string[0];
            }

            return param;
        }

        public string GetApiMethodParamDescription(string methodName, string paramName)
        {
            return typeof(T)
                .GetMethod(methodName)
                ?.GetParameters()
                .Where(p => p.Name == paramName).FirstOrDefault()?
                .GetCustomAttributes(false)
                .OfType<ApiDescriptionAttribute>()
                .FirstOrDefault()?.Description;
        }

        public ApiParamDescription GetApiMethodParamFullDescription(string methodName, string paramName)
        {
            var method = typeof(T).GetMethod(methodName);
            var param = method
                ?.GetParameters()
                .FirstOrDefault(p => p.Name == paramName);
            var pd = new ApiParamDescription();
            pd.ParamDescription = new CommonDescription(method == null? "some" : paramName,
                param?
                    .GetCustomAttributes()
                    .OfType<ApiDescriptionAttribute>()
                    .FirstOrDefault()
                    ?.Description);
            var required = param?.GetCustomAttributes().OfType<ApiRequiredAttribute>().FirstOrDefault();
            pd.Required = required != null && required.Required;
            var validation = param?.GetCustomAttributes().OfType<ApiIntValidationAttribute>().FirstOrDefault();
            pd.MaxValue = validation?.MaxValue;
            pd.MinValue = validation?.MinValue;
            return pd;
        }

        public ApiMethodDescription GetApiMethodFullDescription(string methodName)
        {
            var method = typeof(T).GetMethod(methodName);
            if (method?.GetCustomAttributes().OfType<ApiMethodAttribute>().FirstOrDefault() == null) return null;
            var apiMethodDescription = new ApiMethodDescription()
            {
                MethodDescription = new CommonDescription()
                {
                    Name = methodName,
                    Description = GetApiMethodDescription(methodName)
                },
                ParamDescriptions = GetApiMethodParamDescriptions(methodName).ToArray()
            };
            var required = method.ReturnParameter
                ?.GetCustomAttributes().OfType<ApiRequiredAttribute>().FirstOrDefault();
            var validation = method.ReturnParameter
                ?.GetCustomAttributes().OfType<ApiIntValidationAttribute>().FirstOrDefault();
            if (required != null || validation != null)
                apiMethodDescription.ReturnDescription = new ApiParamDescription
                {
                    Required = required?.Required ?? false,
                    MaxValue = validation?.MaxValue,
                    MinValue = validation?.MinValue
                };            
            return apiMethodDescription;
        }

        private IEnumerable<ApiParamDescription> GetApiMethodParamDescriptions(string methodName)
        {
            return typeof(T)
                .GetMethod(methodName)
                ?.GetParameters()
                .Select(p => GetApiMethodParamFullDescription(methodName, p.Name));
        }
    }
}