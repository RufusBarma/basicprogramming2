using System.Collections.Generic;
using System.Drawing;

namespace Rivals
{
    public class RivalsTask
    {
        public static IEnumerable<OwnedLocation> AssignOwners(Map map)
        {
            var visitedPoints = new HashSet<Point>();
            var stack = new Queue<OwnedLocation>();
            for (var i = 0; i < map.Players.Length; i++)
                stack.Enqueue(new OwnedLocation(i, map.Players[i], 0));

            while (stack.Count > 0)
            {
                var location = stack.Dequeue();
                if (!map.InBounds(location.Location) ||
                    map.Maze[location.Location.X, location.Location.Y] == MapCell.Wall ||
                    visitedPoints.Contains(location.Location))
                    continue;
                visitedPoints.Add(location.Location);
                stack.Enqueue(new OwnedLocation(location.Owner, new Point(location.Location.X + 1, location.Location.Y),
                    location.Distance + 1));
                stack.Enqueue(new OwnedLocation(location.Owner, new Point(location.Location.X - 1, location.Location.Y),
                    location.Distance + 1));
                stack.Enqueue(new OwnedLocation(location.Owner, new Point(location.Location.X, location.Location.Y + 1),
                    location.Distance + 1));
                stack.Enqueue(new OwnedLocation(location.Owner, new Point(location.Location.X, location.Location.Y - 1),
                    location.Distance + 1));
                yield return location;
            }
        }
    }
}