using System.Collections.Generic;
using System.Drawing;
using Greedy.Architecture;
using Greedy.Architecture.Drawing;
using System.Linq;

namespace Greedy
{
    public class GreedyPathFinder : IPathFinder
    {
        public static IEnumerable<PathWithCost> GetPath(DijkstraPathFinder pathFinder, State state, List<Point> chests)
        {
            var path = pathFinder.GetPathsByDijkstra(
                state, state.Position, chests);
            if (path.FirstOrDefault() == null) return null;
            return path;
        }
        public List<Point> FindPathToCompleteGoal(State state)
        {
            var energy = state.InitialEnergy;
            var pathFinder = new DijkstraPathFinder();
            var pathToEnd = new List<Point>();
            var chests = new List<Point>();
            chests.AddRange(state.Chests);
            if (chests.Count < state.Goal) return new List<Point>();
            while (state.Scores < state.Goal)
            {
                PathWithCost pathToBestChest = null;
                if (GetPath(pathFinder, state, chests) == null)
                    return new List<Point>();
                else
                    pathToBestChest = GetPath(pathFinder, state, chests)
                    .SkipWhile(path => path.Path == new List<Point>())
                    .FirstOrDefault();
                energy -= pathToBestChest.Cost;
                if (energy < 0) return new List<Point>();
                pathToEnd.AddRange(pathToBestChest.Path.Skip(1));
                state.Position = pathToBestChest.Path.Last();
                chests.Remove(state.Position);
                state.Scores++;
            }

            return pathToEnd;
        }
    }
}
