using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Dungeon
{
	public class BfsTask
	{
		public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Point[] chests)
		{
			var queue = new Queue<SinglyLinkedList<Point>>();
			var visitedPoints = new HashSet<Point>() { start };
			var startedPoint = new SinglyLinkedList<Point>(start, null);
			var dungeonMasterRoute = Walker.PossibleDirections.Select(p => new Point(p));
			queue.Enqueue(startedPoint);
			while (queue.Count != 0)
			{
				var routeToChest = queue.Dequeue();
				var dungeonMasterPosition = routeToChest.Value;
				if (!map.InBounds(dungeonMasterPosition)) continue;
				if (map.Dungeon[dungeonMasterPosition.X, dungeonMasterPosition.Y] == 0) continue;
				if (chests.Contains(dungeonMasterPosition)) yield return routeToChest;
				foreach (var e in dungeonMasterRoute)
				{
					var point = new Point(dungeonMasterPosition.X + e.X, dungeonMasterPosition.Y + e.Y);
					if (!visitedPoints.Contains(point))
					{
						queue.Enqueue(new SinglyLinkedList<Point>(point, routeToChest));
						visitedPoints.Add(point);
					}
				}
			}
		}
	}
}