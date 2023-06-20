using System;
using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;
using System.Drawing;

namespace Greedy
{
    public class DijkstraCell
    {
        public Point PreviousInCell { get; set; }
        public int Price { get; set; }
        public DijkstraCell(Point point, int price)
        {
            PreviousInCell = point;
            Price = price;
        }
    }

    public class DijkstraPathFinder
    {
        private static IEnumerable<PathWithCost> DoDijkstraalgorithm(State state, List<Point> notVisited, Dictionary<Point, DijkstraCell> track, List<Point> chest, Point[] directions)
        {
            while (true)
            {
                var toOpen = new Point(-1, -1);
                var bestPrice = int.MaxValue;

                foreach (var element in notVisited)
                    if (track.ContainsKey(element) && track[element].Price < bestPrice)
                    {
                        bestPrice = track[element].Price;
                        toOpen = element;
                    }

                if (toOpen == new Point(-1, -1))
                    yield break;
                var neighbours = Neighbours(toOpen, state, directions);
                foreach (var neighbour in neighbours)
                {
                    var currentPrice = track[toOpen].Price + state.CellCost[neighbour.X, neighbour.Y];
                    var nextPoint = neighbour;
                    if (!(track.ContainsKey(nextPoint) && track[nextPoint].Price <= currentPrice))
                        track[nextPoint] = new DijkstraCell(toOpen, currentPrice);
                }
                notVisited.Remove(toOpen);

                foreach (var element in GetPathWithPoints(chest, toOpen, track))
                    yield return element;
            }
        }

        public IEnumerable<PathWithCost> GetPathsByDijkstra(State state, Point start,
        IEnumerable<Point> targets)
        {
            var notVisited = new List<Point>();
            for (var dx = 0; dx < state.MapWidth; dx++)
                for (var dy = 0; dy < state.MapHeight; dy++)
                    notVisited.Add(new Point(dx, dy));
            var way = new Dictionary<Point, DijkstraCell> { [start] = new DijkstraCell(new Point(-1, -1),0) };
            Point[] directions = { new Point(0, 1), new Point(0, -1), new Point(1, 0), new Point(-1, 0) };
            var chests = targets.ToList();

            foreach (var element in DoDijkstraalgorithm(state, notVisited, way, chests, directions))
                yield return element;
        }

        private static List<Point> Neighbours(Point point, State state, Point[] directions)
        {
            var neighbours = new List<Point>();
            foreach (var direction in directions)
            {
                var neighbour = new Point(direction.X + point.X, direction.Y + point.Y);
                if (state.InsideMap(neighbour) && !(state.IsWallAt(neighbour)))
                    neighbours.Add(neighbour);
            }
            return neighbours;
        }

        public static IEnumerable<PathWithCost> GetPathWithPoints(List<Point> chests, Point toOpen, Dictionary<Point, DijkstraCell> way)
        {
            var ways = new List<PathWithCost>();
            if (chests.Contains(toOpen))
            {
                chests.Remove(toOpen);
                var wayToChest = new List<Point>();
                var target = toOpen;
                while (!(target == new Point(-1, -1)))
                {
                    wayToChest.Add(target);
                    target = way[target].PreviousInCell;
                }

                wayToChest.Reverse();
                ways.Add(new PathWithCost(way[toOpen].Price, wayToChest.ToArray()));
                if (0 - chests.Count < Math.Pow(10,-8)) return ways;
            }
            return ways;
        }
    }
}
