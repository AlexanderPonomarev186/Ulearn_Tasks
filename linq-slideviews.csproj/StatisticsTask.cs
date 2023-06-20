using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
	public class StatisticsTask
	{
		public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
		{
			return visits.OrderBy(x => x.DateTime)
				.GroupBy(x => x.UserId)
				.SelectMany(group =>
				{
					var result = group.Bigrams();
					return result.Where(x => x.Item1.SlideType == slideType);
				})
				.Select(x =>
				{
					var time1 = x.Item1.DateTime;
					var time2 = x.Item2.DateTime;
					return time2.Subtract(time1).TotalMinutes;
				})
				.Where(x => x >= 1 && x <= 120)
				.DefaultIfEmpty(0)
				.Median();
		}
	}
}

