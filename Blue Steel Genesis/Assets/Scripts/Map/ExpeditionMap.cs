using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Map
{
    [Serializable]
    public class BiomeInfo : ISerializationCallbackReceiver
    {
        public uint id;
        public float missing_node_rate = .3f;

        public BiomeInfo(uint id, 
                        Dictionary<(uint stage, uint elite_id), Type> elites, 
                        Dictionary<(uint stage, uint boss_id), uint> bosses)
        {
            this.id = id;
            this.elites = elites;
            this.bosses = bosses;
        }

        public void OnBeforeSerialize() {}
        public void OnAfterDeserialize() {
            elites = GameRun.GetElitesByBiomeId(id);
            bosses = GameRun.GetBossesByBiomeId(id);
        }

        public Dictionary<(uint stage, uint elite_id), Type> elites;
        // (stage, boss_id) => boss_variation_count
        public Dictionary<(uint stage, uint boss_id), uint> bosses;
    }

    [Serializable]
    public class ExpeditionMap : ISerializationCallbackReceiver
    {
        /// <summary>
        /// Перечисляет вершины, достижимые непосредственно из pos
        /// </summary>
        public IEnumerable<Vector2Int> listTargets(Vector2Int pos) {
            int target_y = pos.y + (upside_down ? -1 : 1);
            if (target_y < -2 || target_y > map.GetLength(0))
                yield break;
            if (target_y == -2) {
                if (upside_down)
                    yield return black_market_node;
                yield break;
            }
            if (target_y == -1 || target_y == map.GetLength(0)) {
                yield return new(-1, target_y);
                yield break;
            }

            int min_x = Math.Max(pos.x - 1, 0);
            int max_x = pos.x == -1 ?
                map.GetLength(1) :
                Math.Min(map.GetLength(1), pos.x + 2);
            for (int x = min_x; x < max_x; ++x)
                if (map[target_y, x] != Node.DISABLED)
                    yield return new(x, target_y);
        }
        /// <summary>
        /// Возвращает множество вершин, достижимых из pos
        /// </summary>
        public HashSet<Vector2Int> listReachable(Vector2Int pos) {
            HashSet<Vector2Int> reachable = listTargets(pos).ToHashSet();
            HashSet<Vector2Int> to_handle_cur = new(), to_handle_next = new(reachable);
            while (to_handle_next.Count > 0) {
                (to_handle_cur, to_handle_next) = (to_handle_next, to_handle_cur);
                to_handle_next.Clear();

                foreach (var cur_pos in to_handle_cur)
                    foreach (var target in listTargets(cur_pos)) {
                        to_handle_next.Add(target);
                        reachable.Add(target);
                    }
            }
            
            return reachable;
        }


        public void OnBeforeSerialize() {
            if (map == null)
                return;

            map_serializable_ = new Node[width * height];
            for (int i = 0; i < height; ++i)
                for (int j = 0; j < width; ++j)
                    map_serializable_[i * width + j] = map[i, j];
        }
        public void OnAfterDeserialize() {
            if (map_serializable_?.Length != height * width)
                return;

            map = new Node[height, width];
            for (int i = 0; i < height; ++i)
                for (int j = 0; j < width; ++j)
                    map[i, j] = map_serializable_[i * width + j];
        }
        [SerializeField]
        private Node[] map_serializable_;


        public const int width = 5;
        public const int height = 7;
        /// <summary>
        /// Специальная позиция, соответствующая начальному узлу (не содержится в map)
        /// </summary>
        public Vector2Int start_node_pos => new(-1, upside_down ? height : -1);
        /// <summary>
        /// Специальная позиция, соответствующая узлу босса (не содержится в map)
        /// </summary>
        public Vector2Int boss_node_pos => new(-1, upside_down ? -1 : height);
        /// <summary>
        /// Специальная позиция, соответствующая узлу черного рынка (не содержится в map)
        /// </summary>
        public Vector2Int black_market_node => new(-1, -2);


        public Node[,] map { get; private set; }
        [field: SerializeField]
        public bool upside_down { get; private set; }



        public static ExpeditionMap generate(int biome_seed, int local_seed, BiomeInfo biome, uint biome_stage)
        {
            ExpeditionMap map = new() {
                map = new Node[height, width],
                upside_down = biome_stage % 2 == 0
            };

            var biome_map = generateBiomeMap(width, height, biome, biome_seed);
            var type_map = generateNodeTypeMap(width, height, map.upside_down, local_seed);

            for (int line = 0; line < height; ++line)
                for (int x = 0; x < width; ++x)
                    map.map[line, x] = biome_map[line, x] & type_map[line, x];
            return map;
        }


        private static Node[,] generateNodeTypeMap(uint width, uint height, bool upside_down, int local_seed)
        {
            //TODO: BRING BACK REST!
            var map = new Node[height + 2, width + 2];
            for (int x = 1; x <= width; ++x) {
                map[1, x] = upside_down ? Node.TREASURE : Node.REGULAR_ENEMY;
                map[1 + height/2, x] = Node.TREASURE;
                map[height, x] = upside_down ? Node.REGULAR_ENEMY : Node.TREASURE;
            }

            System.Random prng = new(local_seed);
            Node getRandomNode(short allowed_mask) {
                int popcnt(short b) {
                    int cnt = 0;
                    while (b > 0) {
                        if ((b & 1) != 0) ++cnt;
                        b >>= 1;
                    }
                    return cnt;
                }

                allowed_mask = Math.Max(allowed_mask, (short)Node.REGULAR_ENEMY);
                int target_idx = prng.Next(popcnt(allowed_mask)) + 1;
                short node = 1;
                for (int cur_idx = node & allowed_mask; cur_idx < target_idx;) {
                    node <<= 1;
                    if ((node & allowed_mask) != 0)
                        ++cur_idx;
                }
                return (Node)node;
            }
            

            int progress_limit_distance = Mathf.CeilToInt(height / 3),
                start_y = upside_down ? (int)height : 1;
            // Запрещает раннее появление элиток и мест отдыха
            Node progress_limit(int line) =>
                Math.Abs(line - start_y) < progress_limit_distance ? (Node.ELITE_ENEMY | Node.REST) : 0;

            // Запрещает появление магазинов, элиток, мест отдыха несколько раз подряд
            Node link_limit(int line, int x) =>
                (map[line-1, x-1] | map[line-1, x] | map[line-1, x+1] |
                 map[line+1, x-1] | map[line+1, x] | map[line+1, x+1])
                & (Node.SHOP | Node.REST | Node.ELITE_ENEMY);

            // Запрещает соседним по горизонтали узлам иметь один тип
            Node group_limit(int line, int x) =>
                x == 1 ? 0 : map[line, x-2] | map[line, x-1];


            for (int line = 1; line <= height; ++line)
                for (int x = 1; x <= width; ++x)
                    if (map[line, x] == Node.DISABLED) {
                        Node mask =
                            ~(progress_limit(line) | link_limit(line, x) | group_limit(line, x))
                            & Node.RANDOMLY_GENERATABLE;
                        map[line, x] = getRandomNode((short)mask);
                    }

            var final_map = new Node[height, width];
            for (int line = 0; line < height; ++line)
                for (int x = 0; x < width; ++x)
                    final_map[line, x] = map[line + 1, x + 1];
            return final_map;
        }


        private static Node[,] generateBiomeMap(uint width, uint height, BiomeInfo biome, int biome_seed)
        {
            var map = new Node[height, width];

            System.Random prng = new(biome_seed);
            var line_gen = new Node[width + 2];
            void generateLine() {
                Array.Fill(line_gen, Node.DISABLED);
                for (int x = 1; x < width + 1; ++x)
                    line_gen[x] = prng.NextDouble() < biome.missing_node_rate ?
                        Node.DISABLED : Node.ALL_REGULAR;

                // В каждой тройке по горизонтали есть хотя бы один узел:
                // гарантирует проходимость карты и достижимость всех узлов.
                // Принудительная вставка начинается из центра во избежание "перекоса" карты в одну сторону
                for (int x = (int)width / 2 + 1; x < width + 1; ++x)
                    if ((line_gen[x - 1] | line_gen[x] | line_gen[x + 1]) == Node.DISABLED)
                        line_gen[x] = Node.ALL_REGULAR;
                for (int x = (int)width / 2 + 1; --x != 0;)
                    if ((line_gen[x - 1] | line_gen[x] | line_gen[x + 1]) == Node.DISABLED)
                        line_gen[x] = Node.ALL_REGULAR;
            }

            for (int line = 0; line < height; ++line) {
                generateLine();
                for (int x = 0; x < width; ++x)
                    map[line, x] = line_gen[x + 1];
            }

            return map;
        }
    }
}