using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
	public class DungeonTask
	{
		public static MoveDirection[] FindShortestPath(Map map)
		{
			var startToChests = BfsTask.FindPaths(map, map.InitialPosition, map.Chests);
			if (!startToChests.Any())
			{
				var startToExit = BfsTask.FindPaths(map, map.InitialPosition, new Point[1] {map.Exit});
				if (!startToExit.Any())
					return new MoveDirection[0];
				var pathToExit = startToExit.OrderBy(list => list.Length).First();
				return pathToExit == null ? new MoveDirection[0] : PointsToMoveDirection(pathToExit).ToArray();
			}

			var exitToChests = BfsTask.FindPaths(map, map.Exit, map.Chests);
			
			var startToChestToExit = startToChests
				.Join(exitToChests, sToCh => sToCh.Value, eToCh => eToCh.Value,
				(sToCh, eToCh) =>
				{
					var connect = new SinglyLinkedList<Point>(eToCh.Value, sToCh);
					return eToCh.Skip(1).Aggregate(connect, (current, point) => new SinglyLinkedList<Point>(point, current));
				})
				.Where(sTcTe => sTcTe != null);
			if (!startToChestToExit.Any())
				return new MoveDirection[0];
			var shortest = startToChestToExit.OrderBy(list => list.Length).First();
			return PointsToMoveDirection(shortest).ToArray();
		}

		private static IEnumerable<MoveDirection> PointsToMoveDirection(IEnumerable<Point> path)
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
