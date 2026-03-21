using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Navigation
{
    public class Dijkstra
    {
        public static List<Vector3Int> listReachable(Vector3Int initial_pos, Predicate<Vector3Int> is_allowed, uint max_length = uint.MaxValue - 1) =>
            listReachable(new PositionCollection(initial_pos, 1), is_allowed, max_length);
        public static List<Vector3Int> getPath(Vector3Int initial_pos, Vector3Int target, Predicate<Vector3Int> is_allowed) =>
            getPath(new PositionCollection(initial_pos, 1), target, is_allowed);
        public static List<Vector3Int> getPath(PositionCollection initial_pos, Vector3Int target, Predicate<Vector3Int> is_allowed) =>
            getPath(initial_pos, Enumerable.Repeat(target, 1), is_allowed);

        public static List<Vector3Int> listReachable(PositionCollection initial_pos, Predicate<Vector3Int> is_allowed, uint max_length = uint.MaxValue - 1) =>
            new Dijkstra(initial_pos, is_allowed)
                .listReachable(max_length)
                .SelectMany(p => new PositionCollection(p, initial_pos.SideSize))
                .Distinct().ToList();
        public static List<Vector3Int> getPath(PositionCollection initial_pos, IEnumerable<Vector3Int> targets, Predicate<Vector3Int> is_allowed) {
            Dijkstra d = new(initial_pos, is_allowed);
            return targets
                .SelectMany(t => PositionCollection.ContainingPositions(t, initial_pos.SideSize)).Distinct()
                .Select(t => d.getPath(t))
                .Where(p => p != null).MinBy(p => p.Count);
        }



        public List<Vector3Int> listReachable(uint max_length = uint.MaxValue - 1)
        {
            var reachable = new List<Vector3Int>();
            for (int x = 0; x < distance_.GetLength(0); ++x)
                for (int y = 0; y < distance_.GetLength(1); ++y)
                    if (distance_[x, y] <= max_length && distance_[x, y] != 0)
                        reachable.Add(new Vector3Int(x, y));
            return reachable;
        }

        public List<Vector3Int> getPath(PositionCollection target)
        {
            if (target.Any(Entity.tracker.OutOfBounds) || distance_[target.LeftBottom.x, target.LeftBottom.y] == int.MaxValue)
                return null;

            Vector3Int getParent(Vector3Int pos)
            {
                foreach (var neighbor in Entity.tracker.GetNeighborTiles(pos))
                    if (neighbor.x < distance_.GetLength(0) &&
                        neighbor.y < distance_.GetLength(1) &&
                        distance_[neighbor.x, neighbor.y] + 1 == distance_[pos.x, pos.y])
                        return neighbor;
                throw new InvalidOperationException();
            }

            var path = new List<Vector3Int>();
            var to = target.LeftBottom;
            while (distance_[to.x, to.y] != 0)
            {
                var parent = getParent(to);
                path.Add(to - parent);
                to = parent;
            }
            path.Reverse();
            return path;
        }

        public Dijkstra(PositionCollection initial_pos, Predicate<Vector3Int> is_allowed) =>
            calculateDistanceMatrix(initial_pos, is_allowed);

        protected void calculateDistanceMatrix(PositionCollection initial_pos, Predicate<Vector3Int> is_allowed)
        {
            distance_ ??=
                new int[Entity.tracker.max_x - initial_pos.SideSize + 2,
                        Entity.tracker.max_y - initial_pos.SideSize + 2];
            for (int i = 0; i < distance_.GetLength(0); ++i)
                for (int j = 0; j < distance_.GetLength(1); ++j)
                    distance_[i, j] = int.MaxValue;
            distance_[initial_pos.LeftBottom.x, initial_pos.LeftBottom.y] = 0;

            var least_dist_comparer = Comparer<(int, Vector3Int)>.Create(
                ((int, Vector3Int) x, (int, Vector3Int) y) =>
                {
                    if (x.Item1 != y.Item1)
                        return x.Item1.CompareTo(y.Item1);
                    if (x.Item2.x != y.Item2.x)
                        return x.Item2.x.CompareTo(y.Item2.x);
                    return x.Item2.y.CompareTo(y.Item2.y);
                }
            );
            SortedSet<(int dist, Vector3Int pos)> to_handle =
                new(least_dist_comparer) { (0, initial_pos.LeftBottom) };
            while (to_handle.Count > 0)
            {
                var cur = to_handle.Min;
                to_handle.Remove(cur);

                foreach (var neighbor_pos in Entity.tracker.GetNeighborTiles(cur.pos))
                {
                    if (neighbor_pos.x >= distance_.GetLength(0) ||
                        neighbor_pos.y >= distance_.GetLength(1) ||
                        new PositionCollection(neighbor_pos, initial_pos.SideSize).Except(initial_pos).Any(p => !is_allowed(p))
                        )
                        continue;
                    var nb = (dist: distance_[neighbor_pos.x, neighbor_pos.y], pos: neighbor_pos);
                    if (nb.dist > cur.dist + 1)
                    {
                        to_handle.Remove(nb);
                        nb.dist = cur.dist + 1;
                        to_handle.Add(nb);
                        distance_[neighbor_pos.x, neighbor_pos.y] = distance_[cur.pos.x, cur.pos.y] + 1;
                    }
                }
            }
        }

        private int[,] distance_ = null;
    }
}
