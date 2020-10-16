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
			if (startToChests == null)
			{
				var startToExit = BfsTask.FindPaths(map, map.InitialPosition, new Point[1] {map.Exit});
				return PointsToMoveDirection(startToExit.First());
			}

			var exitToChests = BfsTask.FindPaths(map, map.Exit, map.Chests).Reverse();
			var startToChestToExit = startToChests
				.Join(exitToChests, sToCh => sToCh.Value, eToCh => eToCh.Value,
				(sToCh, eToCh) =>
				{
					var connect = new SinglyLinkedList<Point>(eToCh.Value, sToCh);
					return eToCh.Skip(1).Aggregate(connect, (current, point) => new SinglyLinkedList<Point>(point, current));
				});
			var shortest = startToChestToExit.OrderBy(list => list.Length).First();
			return PointsToMoveDirection(shortest);
		}

		private static MoveDirection[] PointsToMoveDirection(SinglyLinkedList<Point> path)
		{
			return path
				.Zip(path.Skip(1), ((leftPoint, rightPoint) =>
				{
					if (rightPoint.X > leftPoint.X)
						return MoveDirection.Right;
					else if (rightPoint.X < leftPoint.X)
						return MoveDirection.Left;
					else if (rightPoint.Y > leftPoint.Y)
						return MoveDirection.Up;
					else
						return MoveDirection.Down;
				}))
				.ToArray();
		}
	}
}
