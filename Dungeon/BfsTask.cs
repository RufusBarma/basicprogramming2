using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
	public class BfsTask
	{
		public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Point[] chests)
		{
			var chestsPositions = new HashSet<Point>(chests);
			var visitedPoints = new HashSet<Point>();
			var stack = new Queue<SinglyLinkedList<Point>>();
			stack.Enqueue(new SinglyLinkedList<Point>(start));
			while (chestsPositions.Count > 0 && stack.Count > 0)
			{
				var way = stack.Dequeue();
				if (!map.InBounds(way.Value) ||
					map.Dungeon[way.Value.X, way.Value.Y] == MapCell.Wall ||
					visitedPoints.Contains(way.Value))
					continue;
				if (chestsPositions.Contains(way.Value))
				{
					chestsPositions.Remove(way.Value);
					yield return way;
				}
				visitedPoints.Add(way.Value);
				stack.Enqueue(new SinglyLinkedList<Point>(new Point(way.Value.X + 1, way.Value.Y), way));
				stack.Enqueue(new SinglyLinkedList<Point>(new Point(way.Value.X - 1, way.Value.Y), way));
				stack.Enqueue(new SinglyLinkedList<Point>(new Point(way.Value.X, way.Value.Y + 1), way));
				stack.Enqueue(new SinglyLinkedList<Point>(new Point(way.Value.X, way.Value.Y - 1), way));
			}
			yield break;
		}
	}
}