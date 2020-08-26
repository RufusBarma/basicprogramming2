using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
	public static class ExtensionsTask
	{
		/// <summary>
		/// Медиана списка из нечетного количества элементов — это серединный элемент списка после сортировки.
		/// Медиана списка из четного количества элементов — это среднее арифметическое 
        /// двух серединных элементов списка после сортировки.
		/// </summary>
		/// <exception cref="InvalidOperationException">Если последовательность не содержит элементов</exception>
		public static double Median(this IEnumerable<double> items)
		{			
			var sortedItems = items.OrderBy(item => item).ToList();
			int count = sortedItems.Count();
			if (count == 0)
				throw new InvalidOperationException();
			double median;
			
			if (count % 2 == 0)
				median = (sortedItems.ElementAt(count / 2) + sortedItems.ElementAt((count - 1) / 2)) / 2;
			else
				median = sortedItems.ElementAt(count / 2);
			return median;
		}

		/// <returns>
		/// Возвращает последовательность, состоящую из пар соседних элементов.
		/// Например, по последовательности {1,2,3} метод должен вернуть две пары: (1,2) и (2,3).
		/// </returns>
		/// <exception cref="InvalidOperationException">Если последовательность не содержит элементов</exception>
		public static IEnumerable<Tuple<T, T>> Bigrams<T>(this IEnumerable<T> items)
		{
			var itemsEnumerator = items.GetEnumerator();
			T prevItem;
			if (itemsEnumerator.MoveNext())
				prevItem = itemsEnumerator.Current;
			else
				throw new InvalidOperationException();
			
			while (itemsEnumerator.MoveNext())
			{      
				yield return Tuple.Create(prevItem, itemsEnumerator.Current);
				prevItem = itemsEnumerator.Current;
			}			
		}
	}
}