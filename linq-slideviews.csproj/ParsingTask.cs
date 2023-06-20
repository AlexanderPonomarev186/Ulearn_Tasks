using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
	public class ParsingTask
	{
		/// <param name="lines">все строки файла, которые нужно распарсить. Первая строка заголовочная.</param>
		/// <returns>Словарь: ключ — идентификатор слайда, значение — информация о слайде</returns>
		/// <remarks>Метод должен пропускать некорректные строки, игнорируя их</remarks>
		public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
		{
			return lines.Select(line => line.Split(';'))
				.Where(list =>  list.Length == 3 && list[0] != "SlideId")
				.Select(line => SlideCreate(line))
				.Where(list => list != null)
				.ToDictionary(s => s.SlideId, s => s);
				
		}
		private static List<string> slideTypes = new List<string>() { "theory", "exercise", "quiz", };
		private static SlideRecord SlideCreate(string[] part)
		{
			int id;
			if (int.TryParse(part[0], out id))
				return new SlideRecord(id, (SlideType)slideTypes.IndexOf(part[1]), part[2]);
			return null;
		}

		private static VisitRecord VisitCreate(string line,IDictionary<int,SlideRecord> slides)
        {
			string[] lineMass = line.Split(';');
			int userId;
			int slideId;
			DateTime dataTime;
			if (!(lineMass.Length == 4 &&
				int.TryParse(lineMass[0], out userId) &&
				int.TryParse(lineMass[1], out slideId) &&
				DateTime.TryParse(lineMass[2], out dataTime) &&
				DateTime.TryParse(lineMass[3], out dataTime) &&
				slides.ContainsKey(slideId)))
					throw new FormatException("Wrong line [" + line + "]");
			return new VisitRecord(userId, slideId, DateTime.Parse(lineMass[2]+" "+lineMass[3]), slides[slideId].SlideType);
        }

		/// <param name="lines">все строки файла, которые нужно распарсить. Первая строка — заголовочная.</param>
		/// <param name="slides">Словарь информации о слайдах по идентификатору слайда. 
		/// Такой словарь можно получить методом ParseSlideRecords</param>
		/// <returns>Список информации о посещениях</returns>
		/// <exception cref="FormatException">Если среди строк есть некорректные</exception>
		public static IEnumerable<VisitRecord> ParseVisitRecords(
			IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
		{
			return lines.Where(list => list != "UserId;SlideId;Date;Time")
				.Select(list => VisitCreate(list, slides));
		}
	}
}