using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Dungeon
{
    public class DungeonTask
    {
        public static MoveDirection[] FindShortestPath(Map map)
        {
            var startToExit = BfsTask.FindPaths(map, map.InitialPosition, new[] {map.Exit}).FirstOrDefault();
            if (startToExit == null)
                return new MoveDirection[0];
            if (map.Chests.Any(chest => startToExit.Contains(chest)))
                return startToExit.PointsToMoveDirection().ToArray();
            var startToChests = BfsTask.FindPaths(map, map.InitialPosition, map.Chests);
            if (!startToChests.Any())
                return startToExit.PointsToMoveDirection().ToArray();

            return startToChests.GetShortestPath(map).PointsToMoveDirection().ToArray();
        }
    }


    public static class PathExtensions
    {
        public static IEnumerable<Point> GetShortestPath(this IEnumerable<SinglyLinkedList<Point>> startToChests,
            Map map)
        {
            var exitToChests = BfsTask.FindPaths(map, map.Exit, map.Chests);

            var startToChestToExit = startToChests
                .Join(exitToChests, sToCh => sToCh.Value, eToCh => eToCh.Value,
                    (sToCh, eToCh) =>
                    {
                        var connect = new SinglyLinkedList<Point>(eToCh.Value, sToCh);
                        return eToCh.Skip(1).Aggregate(connect,
                            (current, point) => new SinglyLinkedList<Point>(point, current));
                    })
                .Where(sTcTe => sTcTe != null);

            return startToChestToExit.OrderBy(list => list.Length).First();
        }

        public static IEnumerable<MoveDirection> PointsToMoveDirection(this IEnumerable<Point> path)
        {
            var leftPoint = path.Reverse().Take(path.Count()).GetEnumerator();
            var rightPoint = path.Reverse().Skip(1).GetEnumerator();
            while (leftPoint.MoveNext() && rightPoint.MoveNext())
            {
                if (leftPoint.Current == rightPoint.Current)
                    continue;
                if (rightPoint.Current.X > leftPoint.Current.X)
                    yield return MoveDirection.Right;
                else if (rightPoint.Current.X < leftPoint.Current.X)
                    yield return MoveDirection.Left;
                else if (rightPoint.Current.Y > leftPoint.Current.Y)
                    yield return MoveDirection.Down;
                else if (rightPoint.Current.Y < leftPoint.Current.Y)
                    yield return MoveDirection.Up;
            }
        }
    }
}