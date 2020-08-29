using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
	public class StatisticsTask
	{
		public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
		{		
			return visits
				.OrderBy(visit => visit.UserId)
				.ThenBy(visit => visit.DateTime)
				.GroupBy(visit => visit.UserId)
				.SelectMany(group => group.Bigrams().Where(visit => visit.Item1.SlideType == slideType))
				.Select(bigram => (bigram.Item2.DateTime - bigram.Item1.DateTime).TotalMinutes)
				.Where(time => time >= 1 && time <= 120)
				.DefaultIfEmpty(0)
				.Median();
		}
	}
}