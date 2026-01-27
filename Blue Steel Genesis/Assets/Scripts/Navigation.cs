using BlueSteelGenesis.Character_Modules;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BlueSteelGenesis.Navigation {
    public class Dijkstra {
        public static List<Vector3Int> listReachable(Vector3Int initial_pos, Predicate<Vector3Int> is_allowed) =>
            new Dijkstra(initial_pos, is_allowed).listReachable();
        public static List<Vector3Int> getPath(Vector3Int initial_pos, Vector3Int target, Predicate<Vector3Int> is_allowed) =>
            new Dijkstra(initial_pos, is_allowed).getPath(target);

        public List<Vector3Int> listReachable() {
            var reachable = new List<Vector3Int>();
            for (int x = 0; x < distance_.GetLength(0); ++x)
                for (int y = 0; y < distance_.GetLength(1); ++y)
                    if (distance_[x, y] != int.MaxValue && distance_[x, y] != 0)
                        reachable.Add(new Vector3Int(x, y));
            return reachable;
        }

        public List<Vector3Int> getPath(Vector3Int target) {
            if (Character.tracker.OutOfBounds(target) || distance_[target.x, target.y] == int.MaxValue)
                return null;

            Vector3Int getParent(Vector3Int pos) {
                foreach (var neighbor in Character.tracker.GetNeighborTiles(pos))
                    if (distance_[neighbor.x, neighbor.y] + 1 == distance_[pos.x, pos.y])
                        return neighbor;
                throw new InvalidOperationException();
            }

            var path = new List<Vector3Int>();
            var to = target;
            while (distance_[to.x, to.y] != 0) {
                var parent = getParent(to);
                path.Add(to - parent);
                to = parent;
            }
            path.Reverse();
            return path;
        }

        public Dijkstra(Vector3Int initial_pos, Predicate<Vector3Int> is_allowed) =>
            calculateDistanceMatrix(initial_pos, is_allowed);

        protected void calculateDistanceMatrix(Vector3Int initial_pos, Predicate<Vector3Int> is_allowed) {
            distance_ ??= new int[Character.tracker.max_x + 1, Character.tracker.max_y + 1];
            for (int i = 0; i < distance_.GetLength(0); ++i)
                for (int j = 0; j < distance_.GetLength(1); ++j)
                    distance_[i, j] = int.MaxValue;
            distance_[initial_pos.x, initial_pos.y] = 0;

            var least_dist_comparer = Comparer<(int, Vector3Int)>.Create(((int, Vector3Int) x, (int, Vector3Int) y) => {
                if (x.Item1 != y.Item1)
                    return x.Item1.CompareTo(y.Item1);
                if (x.Item2.x != y.Item2.x)
                    return x.Item2.x.CompareTo(y.Item2.x);
                return x.Item2.y.CompareTo(y.Item2.y);
            });
            SortedSet<(int dist, Vector3Int pos)> to_handle = new(least_dist_comparer) {
                (0, initial_pos)
            };
            while (to_handle.Count > 0) {
                var cur = to_handle.Min;
                to_handle.Remove(cur);

                foreach (var neighbor_pos in Character.tracker.GetNeighborTiles(cur.pos)) {
                    if (!is_allowed(neighbor_pos))
                        continue;
                    var nb = (dist: distance_[neighbor_pos.x, neighbor_pos.y], pos: neighbor_pos);
                    if (nb.dist > cur.dist + 1) {
                        to_handle.Remove(nb);
                        nb.dist = cur.dist + 1;
                        distance_[neighbor_pos.x, neighbor_pos.y] = nb.dist;
                        to_handle.Add(nb);
                        distance_[neighbor_pos.x, neighbor_pos.y] = distance_[cur.pos.x, cur.pos.y] + 1;
                    }
                }
            }
        }

        private int[,] distance_ = null;
    }
}
