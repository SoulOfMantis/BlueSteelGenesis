using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


namespace BlueSteelGenesis.Character_Modules
{
    /// <summary>
    /// Базовый модуль движения (BMM сокращение)
    /// </summary>
    public class BasicMovement : ActiveModule
    {
        public BasicMovement()
        {
            range = 3;
            energyCost = 1;
        }

        public override async Task Effect(Character user, Vector3Int pos)
        {
            var path = getOptimalPath(user, pos);
            if (path?.Count <= range)
                await user.move(path);
            Debug.Log("BMM executed");
        }
        public override List<Vector3Int> getCellsInRange(Vector3Int start)
        {
            List<Vector3Int> reachable = new();
            int[,] distance = distanceMatrix(start);
            for (var i = -range; i <= range; ++i)
                for (var j = -range; j <= range; ++j)
                {
                    var pos = start + new Vector3Int(i, j, 0);
                    if (!Character.tracker.OutOfBounds(pos) && distance[pos.x, pos.y] <= range)
                        reachable.Add(pos);
                }
            return reachable;
        }
        protected int[,] distanceMatrix(Vector3Int start_pos) {
            int[,] distance = new int[Character.tracker.max_x + 1, Character.tracker.max_y + 1];
            for (int i = 0; i < distance.GetLength(0); ++i)
                for (int j = 0; j < distance.GetLength(1); ++j)
                    distance[i, j] = int.MaxValue;
            distance[start_pos.x, start_pos.y] = 0;

            var least_dist_comparer = Comparer<(int, Vector3Int)>.Create(((int, Vector3Int) x, (int, Vector3Int) y) => {
                if (x.Item1 != y.Item1)
                    return x.Item1.CompareTo(y.Item1);
                if (x.Item2.x != y.Item2.x)
                    return x.Item2.x.CompareTo(y.Item2.x);
                return x.Item2.y.CompareTo(y.Item2.y);
            });
            SortedSet<(int dist, Vector3Int pos)> to_handle = new(least_dist_comparer);
            to_handle.Add((0, start_pos));
            while (to_handle.Count > 0) {
                var cur = to_handle.Min;
                to_handle.Remove(cur);

                foreach (var neighbor_pos in Character.tracker.GetNeighborTiles(cur.pos).Where(p => !Character.tracker.IsOccupied(p))) {
                    if (neighbor_pos.ManhattanDistance(start_pos) > range)
                        continue;
                    var nb = (dist: distance[neighbor_pos.x, neighbor_pos.y], pos: neighbor_pos);
                    if (nb.dist > cur.dist + 1) {
                        to_handle.Remove(nb);
                        nb.dist = cur.dist + 1;
                        distance[neighbor_pos.x, neighbor_pos.y] = nb.dist;
                        to_handle.Add(nb);
                        distance[neighbor_pos.x, neighbor_pos.y] = distance[cur.pos.x, cur.pos.y] + 1;
                    }
                }
            }
            distance[start_pos.x, start_pos.y] = int.MaxValue;
            return distance;
        }

        protected List<Vector3Int> getOptimalPath(Character issuer, Vector3Int to) {
            Vector3Int[] neighbor_offset = { Vector3Int.left, Vector3Int.right, Vector3Int.down, Vector3Int.up };
        
            int[,] visit_penalty = new int[2 * range + 1, 2 * range + 1];
            int[,,] accumulated_penalty = new int[range + 1, 2 * range + 1, 2 * range + 1];
            byte[,,] parent = new byte[range + 1, 2 * range + 1, 2 * range + 1];
            for (int i = 0; i < visit_penalty.GetLength(0); ++i)
                for (int j = 0; j < visit_penalty.GetLength(1); ++j)
                {
                    visit_penalty[i, j] = Character.tracker.OutOfBounds(issuer.Position + new Vector3Int(i - range, j - range)) ?
                                          int.MaxValue : issuer.stepPenalty(issuer.Position + new Vector3Int(i - range, j - range));
                    for (int k = 0; k < accumulated_penalty.GetLength(0); ++k)
                        accumulated_penalty[k, i, j] = int.MaxValue;
                }
            accumulated_penalty[0, range, range] = 0;

            HashSet<Vector3Int> cur_queue = new(), next_queue = new();
            cur_queue.Add(new(range, range));
            for (int distance = 1; distance < accumulated_penalty.GetLength(0); ++distance)
            {
                foreach (var pos in cur_queue)
                    for (int offset_idx = 0; offset_idx < neighbor_offset.Length; ++offset_idx)
                    {
                        var nb_pos = pos + neighbor_offset[offset_idx];
                        if (nb_pos.x < 0 || nb_pos.y < 0 || nb_pos.x > 2 * range || nb_pos.y > 2 * range)
                            continue;
                        if (accumulated_penalty[distance, nb_pos.x, nb_pos.y] - visit_penalty[nb_pos.x, nb_pos.y] > accumulated_penalty[distance - 1, pos.x, pos.y])
                        {
                            accumulated_penalty[distance, nb_pos.x, nb_pos.y] = visit_penalty[nb_pos.x, nb_pos.y] + accumulated_penalty[distance - 1, pos.x, pos.y];
                            parent[distance, nb_pos.x, nb_pos.y] = (byte)offset_idx;
                            next_queue.Add(nb_pos);
                        }
                    }
                cur_queue.Clear();
                (cur_queue, next_queue) = (next_queue, cur_queue);
            }

            to = to - issuer.Position + new Vector3Int(range, range);
            int optimal_path_len = 0, best_penalty = int.MaxValue;
            for (int dist = 0; dist < accumulated_penalty.GetLength(0); ++dist)
                if (best_penalty > accumulated_penalty[dist, to.x, to.y])
                {
                    optimal_path_len = dist;
                    best_penalty = accumulated_penalty[dist, to.x, to.y];
                }
            if (best_penalty == int.MaxValue)
                return null;
            List<Vector3Int> path = new();
            for (int dist = optimal_path_len; dist > 0; --dist)
            {
                path.Add(neighbor_offset[parent[dist, to.x, to.y]]);
                to -= path[^1];
            }
            path.Reverse();
            return path;
        }
    }
}