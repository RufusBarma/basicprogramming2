using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Greedy.Architecture;
using System.Drawing;

namespace Greedy
{
    // public class DijkstraData
    // {
    //     public Point BestPoint { get; private set; }
    //     
    //     public readonly HashSet<Point> NotVisited;
    //     
    //     public readonly Dictionary<Point, int> Scores;
    //     
    //     private readonly Dictionary<Point, Point> track;
    //     
    //     public DijkstraData(List<Point> cells, Point start)
    //     {
    //         NotVisited = cells.ToHashSet();
    //         Scores = NotVisited.ToDictionary(point => point, point => Int32.MaxValue);
    //         track = new Dictionary<Point, Point>();
    //         
    //         BestPoint = start;
    //         Scores[start] = 0;
    //         track.Add(start, start);
    //     }
    //
    //     public void CheckCell(Point previous, Point next, int cost)
    //     {
    //         NotVisited.Remove(previous);
    //         if (Scores[next] + cost < Scores[next] || Scores[next] == int.MaxValue)
    //             UpdateCell(next, previous, cost);
    //         //if (Scores[next] < Scores[BestPoint]) BestPoint = next;
    //     }
    //
    //     private void UpdateCell(Point next, Point previous, int cost)
    //     {
    //         Scores[next] = Scores[previous] + cost;
    //         if (track.ContainsKey(next))
    //             track[next] = previous;
    //         else
    //             track.Add(next, previous);
    //     }
    //     
    //     
    //     public IEnumerable<Point> GetAdjacentPoints(Point point)
    //     {
    //         for(var x = -1; x <= 1; x++)
    //         for (var y = -1; y <= 1; y++)
    //         {
    //             if (!(x == 0 && y != 0 || y == 0 && x != 0)) continue;
    //             var aPoint = new Point(point.X+x, point.Y+y);
    //             if (NotVisited.Contains(aPoint)) yield return aPoint;
    //         }
    //     }
    //
    //     public Point[] GetPath(Point point)
    //     {
    //         var path = new List<Point>();
    //         path.Add(point);
    //         var prev = point;
    //         var prevPrev = track[point];
    //         while (prev != prevPrev)
    //         {
    //             path.Add(prevPrev);
    //             prev = prevPrev;
    //             prevPrev = track[prev];
    //         }
    //         path.Reverse();
    //         return path.ToArray();
    //     }
    //
    //     public void UpdateBestPoint()
    //     {
    //         BestPoint = NotVisited.First(p => Scores[p] == Scores.Min(pair => pair.Value));
    //     }
    // }
    
    // public class DijkstraPathFinder
    // {
    //     public IEnumerable<PathWithCost> GetPathsByDijkstra(State state, Point start,
    //         IEnumerable<Point> targets)
    //     {
    //         var notFoundTargets = targets.ToList();
    //         var data = new DijkstraData(GetNotWallPoints(state).ToList(), start);
    //
    //         while (notFoundTargets.Count > 0 && data.NotVisited.Count > 0)
    //         {
    //             data.UpdateBestPoint();
    //             var previous = data.BestPoint;
    //             if (notFoundTargets.Contains(previous))
    //             {
    //                 yield return new PathWithCost(data.Scores[previous], data.GetPath(previous));
    //                 notFoundTargets.Remove(previous);
    //             }
    //
    //             var adjacentPoints = data.GetAdjacentPoints(previous).ToList();
    //             if (adjacentPoints.Count == 0)
    //                 break;
    //             foreach (var next in adjacentPoints)
    //                 data.CheckCell(previous, next, state.CellCost[next.X, next.Y]);
    //         }
    //     }
    //
    //     private IEnumerable<Point> GetNotWallPoints(State state)
    //     {
    //         for(var x = 0; x < state.CellCost.GetLength(0); x++)
    //         for(var y = 0; y < state.CellCost.GetLength(1); y++)
    //             if (!state.IsWallAt(x, y)) 
    //                 yield return  new Point(x, y);
    //     }
    //}

    public class DijkstraData<TNode, TValue>: IComparable where TValue: IComparable
    {
        public TNode Previous { get; set; }
        public TValue Value { get; set; }

        public DijkstraData(TNode previous, TValue value)
        {
            Previous = previous;
            Value = value;
        }

        public static int ValueComparer(TValue x, TValue y) => x.CompareTo(y);
        public int CompareTo(object obj)
        {
            if (!(obj is DijkstraData<TNode, TValue> otherDijkstraData)) 
                throw new ArgumentException("Try to compare different types!");
            return ValueComparer(Value, otherDijkstraData.Value);
        }
    }
   
    public class DijkstraPathFinder
    {
        public IEnumerable<PathWithCost> GetPathsByDijkstra(State state, Point start, IEnumerable<Point> targets)
        {
            var notVisited = new SortedSet<DijkstraData<Point, double>>();
            notVisited.Add(new DijkstraData<Point, double>(start, 0))
            return null;
        }
    }
}

