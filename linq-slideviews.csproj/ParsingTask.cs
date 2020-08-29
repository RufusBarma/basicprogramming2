using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace linq_slideviews
{
	public static class ExtensionString
    {
		public static T ToEnum<T>(this string value, bool ignoreCase = true)
		{
			return (T)Enum.Parse(typeof(T), value, ignoreCase);
		}
	}

	public class ParsingTask
	{
		/// <param name="lines">все строки файла, которые нужно распарсить. Первая строка заголовочная.</param>
		/// <returns>Словарь: ключ — идентификатор слайда, значение — информация о слайде</returns>
		/// <remarks>Метод должен пропускать некорректные строки, игнорируя их</remarks>
		public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
		{
			return lines
				.Skip(1)
				.Select(line => line.Split(';'))
				.Where(fields => fields.Length == 3)
				.Where(fields => 
				       Int32.TryParse(fields[0], out int result) &&
					   Enum.TryParse(fields[1], true, out SlideType slideType))
				.ToDictionary(fields => Int32.Parse(fields[0]),
				fields => new SlideRecord(
					Int32.Parse(fields[0]),
					fields[1].ToEnum<SlideType>(true),
					fields[2]));				
		}

		/// <param name="lines">все строки файла, которые нужно распарсить. Первая строка — заголовочная.</param>
		/// <param name="slides">Словарь информации о слайдах по идентификатору слайда. 
		/// Такой словарь можно получить методом ParseSlideRecords</param>
		/// <returns>Список информации о посещениях</returns>
		/// <exception cref="FormatException">Если среди строк есть некорректные</exception>
		public static IEnumerable<VisitRecord> ParseVisitRecords(
			IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
		{
			return lines
				.Skip(1)
				.Select(line => {
					var fields = line.Split(';');
					if (fields.Length != 4)
						throw new FormatException("Wrong line [" + line + "]");
					return fields;
					})
				.Where(fields =>
				{
					string dt = fields[2] + ' ' + fields[3];
					if ( Int32.TryParse(fields[0], out int userId) && Int32.TryParse(fields[1], out int slideId) &&
					slides.ContainsKey(Int32.Parse(fields[1])) && DateTime.TryParseExact(dt, "yyyy-MM-dd HH:mm:ss",
					CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
						return true;
					else
						throw new FormatException(String.Format("Wrong line [{0};{1};{2};{3}]", 
							fields[0], fields[1], fields[2], fields[3]));					
				})
				.Select(fields => new VisitRecord( Int32.Parse(fields[0]),	Int32.Parse(fields[1]),
						DateTime.Parse(fields[2] + ' ' + fields[3]), slides[Int32.Parse(fields[1])].SlideType));			
		}
	}
}