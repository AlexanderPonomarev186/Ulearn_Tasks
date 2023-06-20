using System;
using System.Collections.Generic;
using System.Drawing;

namespace Rivals
{
	public class RivalsTask
	{
		public static IEnumerable<OwnedLocation> AssignOwners(Map map)
		{
			var queue = new Queue<OwnedLocation>();
			var visited = new HashSet<Point>();
			var players = map.Players;
			for (var i = 0; i < players.Length; i++)
				queue.Enqueue(new OwnedLocation(i, players[i], 0));
			while (queue.Count > 0)
			{
				var location = queue.Dequeue();
				if (visited.Contains(location.Location) ||
					!map.InBounds(location.Location) ||
					map.Maze[location.Location.X, location.Location.Y] != MapCell.Empty)
					continue;
				visited.Add(location.Location);
				yield return location;
				AddToQueue(queue, location);
			}
		}

		private static void AddToQueue(Queue<OwnedLocation> queue, OwnedLocation location)
		{
			for (var dy = -1; dy <= 1; dy++)
				for (var dx = -1; dx <= 1; dx++)
					if ((dy == 0 || dx == 0) && dy != dx)
					{
						var newLocation = new OwnedLocation(location.Owner,
						new Point(location.Location.X + dx, location.Location.Y + dy),
						location.Distance + 1);
						queue.Enqueue(newLocation);
					}
		}
	}
}

