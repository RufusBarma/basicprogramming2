using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Take3
{
    class Program
    {
        private static IEnumerable<T> Take<T>(IEnumerable<T> source, int count)
        {
            var ie = source.GetEnumerator();
            while (count-- > 0)
            {
                if (!ie.MoveNext())
                    break;
                yield return ie.Current;
            }
            foreach (var e in source)
                if (count-- > 0)                
                    yield return e;                
                else
                    break;
            
        }

        public static void Main()
        {
            Func<int[], int, string> take = (source, count) => string.Join(" ", Take(source, count));

            var a1 = take(new[] { 1, 2, 3, 4 }, 2);
            var a2 = take(new[] { 4 }, 1);
            var a3 = take(new[] { 5 }, 0);
            Assert.AreEqual("1 2", take(new[] { 1, 2, 3, 4 }, 2));
            Assert.AreEqual("4", take(new[] { 4 }, 1));
            Assert.AreEqual("", take(new[] { 5 }, 0));

            var num = new Random().Next(0, 1000);
            Assert.AreEqual(num.ToString(), take(new[] { num }, 100500));
        }
    }
}
