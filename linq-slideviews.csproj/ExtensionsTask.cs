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
			if (!items.Any())
				throw new InvalidOperationException();
			var lst = items.ToList();
			lst.Sort();
			if (lst.Count % 2 == 0)
            {
				return (lst[lst.Count/2] + lst[lst.Count/2-1])/2;
            }
			else return lst[lst.Count/2];
		}

		/// <returns>
		/// Возвращает последовательность, состоящую из пар соседних элементов.
		/// Например, по последовательности {1,2,3} метод должен вернуть две пары: (1,2) и (2,3).
		/// </returns>
		public static IEnumerable<Tuple<T, T>> Bigrams<T>(this IEnumerable<T> items)
		{
				T prevItem = default(T);
				var flag = true;
				foreach (var item in items)
				{
					if (flag)
					{
						prevItem = item;
						flag = false;
						continue;
					}
					yield return Tuple.Create(prevItem, item);
					prevItem = item;
				}
		}
	}
}