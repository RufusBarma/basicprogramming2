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
			var startToChests = BfsTask.FindPaths(map, map.InitialPosition, map.Chests).ToList();
			if (!startToChests.Any())
			{
				var startToExit = BfsTask.FindPaths(map, map.InitialPosition, new Point[1] {map.Exit}).ToList();
				if (!startToExit.Any())
					return new MoveDirection[0];
				var pathToExit = startToExit.OrderBy(list => list.Length).First();
				return pathToExit == null ? new MoveDirection[0] : PointsToMoveDirection(pathToExit);
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
			var toChestToExit = startToChestToExit.ToList();
			if (!toChestToExit.Any())
				return new MoveDirection[0];
			var shortest = toChestToExit.OrderBy(list => list.Length).First();

			return PointsToMoveDirection(shortest);
		}

		private static MoveDirection[] PointsToMoveDirection(IEnumerable<Point> path)
		{
			var pathDistinct = path.Distinct().Reverse().ToList();
			return pathDistinct
				.Zip(pathDistinct.Skip(1), ((leftPoint, rightPoint) =>
				{
					if (rightPoint.X > leftPoint.X)
						return MoveDirection.Right;
					else if (rightPoint.X < leftPoint.X)
						return MoveDirection.Left;
					else if (rightPoint.Y > leftPoint.Y)
						return MoveDirection.Down;
					else if (rightPoint.Y < leftPoint.Y)
						return MoveDirection.Up;
					else
						throw new Exception("Two points are similar");
				}))
				.ToArray();
		}
	}
}
