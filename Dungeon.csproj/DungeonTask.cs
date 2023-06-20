using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Dungeon
{
	public class DungeonTask
	{
		public static MoveDirection[] FindShortestPath(Map map)
		{
			var start = map.InitialPosition;
			var exit = map.Exit;
			var chests = map.Chests;
			var routeWithNoChest = BfsTask.FindPaths(map, start, new Point[] { exit }).FirstOrDefault();
			if (routeWithNoChest == null) return new MoveDirection[0];
			var moveToExit = routeWithNoChest.ToList();
			moveToExit.Reverse();
			if (chests.Any(c => moveToExit.Contains(c)))
				return moveToExit.Zip(moveToExit.Skip(1), Move).ToArray();
			var routeStartToChests = BfsTask.FindPaths(map, start, chests);
			var routeExitToChests = BfsTask.FindPaths(map, exit, chests);
			if (routeStartToChests.FirstOrDefault() == null)
				return moveToExit.Zip(moveToExit.Skip(1), Move).ToArray();
			var routeStartToExit = new List<Route>();
			foreach (var route in routeStartToChests)
				foreach (var chest in routeExitToChests)
					if (route.Value == chest.Value) routeStartToExit.Add(new Route(route, chest));
			var listsTuple = routeStartToExit.OrderBy(l => l.Length).First();
			listsTuple.MoveToChest.Reverse();
			listsTuple.MoveToChest.AddRange(listsTuple.FromChestToExit.Skip(1));
			return listsTuple.MoveToChest.Zip(listsTuple.MoveToChest.Skip(1), (Move)).ToArray();
		}
		private class Route
        {
			public int Length { get; set; }
			public List<Point> MoveToChest { get; set; }
			public List<Point> FromChestToExit { get; set; }
			public Route(SinglyLinkedList<Point> moveToChest, SinglyLinkedList<Point> fromChestToExit)
            {
				MoveToChest = moveToChest.ToList();
				FromChestToExit = fromChestToExit.ToList();
				Length = MoveToChest.Count+FromChestToExit.Count;
			}

		}
		private static MoveDirection Move(Point start, Point finish)
		{
			var d = new Point(finish.X - start.X, finish.Y - start.Y);
			if (d.X == 1) return MoveDirection.Right;
			else if (d.X == -1) return MoveDirection.Left;
			return d.Y == 1 ? MoveDirection.Down : MoveDirection.Up;
		}
	}
}

