using System;
using System.Collections.Generic;
using UnityEngine;

using HKDF = HKDF<System.Security.Cryptography.HMACSHA1>;
namespace Map
{
    // Placholder type
    public class BiomeInfo
    {
        public uint id;
        public float missing_node_rate = .3f;
    }

    public class ExpeditionMap
    {
        public IEnumerable<Vector2Int> listTargets(Vector2Int from) {
            int target_y = from.y + (upside_down ? -1 : 1);
            if (target_y < -1 || target_y > map.GetLength(0))
                yield break;
            if (target_y == -1 || target_y == map.GetLength(0)) {
                yield return new(-1, target_y);
                yield break;
            }

            int min_x = Math.Max(from.x - 1, 0);
            int max_x = from.x == -1 ?
                map.GetLength(1) :
                Math.Min(map.GetLength(1), from.x + 2);
            for (int x = min_x; x < max_x; ++x)
                if (map[target_y, x] != Node.DISABLED)
                    yield return new(x, target_y);
        }

        public int width => map?.GetLength(1) ?? 0;
        public int height => map?.GetLength(0) ?? 0;
        public Vector2Int start_node_pos => new(-1, upside_down ? height : -1);
        public Vector2Int boss_node_pos => new(-1, upside_down ? -1 : height);

        public Node[,] map { get; private set; }
        public int biome_seed { get; private set; }
        public int local_seed { get; private set; }
        public bool upside_down { get; private set; }



        public static ExpeditionMap generate(uint width, uint height, byte[] global_seed, BiomeInfo biome, uint biome_stage, uint lives_count, byte[] ship_parts_data)
        {
            ExpeditionMap map = new() {
                map = new Node[height, width],
                biome_seed = generateBiomeSeed(global_seed, biome.id),
                local_seed = generateLocalSeed(global_seed, biome.id, biome_stage, lives_count, ship_parts_data),
                upside_down = biome_stage % 2 == 1
            };

            var biome_map = generateBiomeMap(width, height, biome, map.biome_seed);
            var type_map = generateNodeTypeMap(width, height, map.upside_down, map.local_seed);

            for (int line = 0; line < height; ++line)
                for (int x = 0; x < width; ++x)
                    map.map[line, x] = biome_map[line, x] & type_map[line, x];
            return map;
        }


        private static Node[,] generateNodeTypeMap(uint width, uint height, bool upside_down, int local_seed)
        {
            var map = new Node[height + 2, width + 2];
            for (int x = 1; x <= width; ++x) {
                map[1, x] = upside_down ? Node.REST : Node.REGULAR_ENEMY;
                map[1 + height/2, x] = Node.TREASURE;
                map[height, x] = upside_down ? Node.REGULAR_ENEMY : Node.REST;
            }

            
            System.Random prng = new(local_seed);
            Node getRandomNode(byte allowed_mask) {
                int popcnt(byte b) {
                    int cnt = 0;
                    while (b > 0) {
                        if ((b & 1) != 0) ++cnt;
                        b >>= 1;
                    }
                    return cnt;
                }

                allowed_mask = Math.Max(allowed_mask, (byte)Node.REGULAR_ENEMY);
                int target_idx = prng.Next(popcnt((byte)allowed_mask)) + 1;
                byte node = 1;
                for (int cur_idx = node & allowed_mask; cur_idx < target_idx;) {
                    node <<= 1;
                    if ((node & allowed_mask) != 0)
                        ++cur_idx;
                }
                return (Node)node;
            }
            
            int first_third = 1 + Mathf.CeilToInt(height / 3);
            for (int line = 1; line <= height; ++line)
                for (int x = 1; x <= width; ++x)
                    if (map[line, x] == Node.DISABLED) {
                        Node mask;
                        Node progress_limit =
                            line <= first_third ? (Node.ELITE_ENEMY | Node.REST) : Node.DISABLED;
                        Node link_limit =
                            (map[line-1, x-1] | map[line-1, x] | map[line-1, x+1] |
                             map[line+1, x-1] | map[line+1, x] | map[line+1, x+1])
                            & (Node.SHOP | Node.REST | Node.ELITE_ENEMY);

                        if (x == 1) {
                            mask = ~(progress_limit | link_limit) & Node.RANDOMLY_GENERATABLE;
                            map[line, x] = getRandomNode((byte)mask);
                            continue;
                        }
                        Node group_limit = map[line, x-2] | map[line, x-1];
                        mask = ~(progress_limit | link_limit | group_limit) & Node.RANDOMLY_GENERATABLE;
                        map[line, x] = getRandomNode((byte)mask);
                    }

            var final_map = new Node[height, width];
            for (int line = 0; line < height; ++line)
                for (int x = 0; x < width; ++x)
                    final_map[line, x] = map[line + 1, x + 1];
            return final_map;
        }
        private static int generateLocalSeed(byte[] global_seed, uint biome_id, uint biome_stage, uint lives_count, byte[] ship_parts_data)
        {
            HKDF hkdf = new();
            hkdf.extract(null, global_seed);
            int seed = BitConverter.ToInt32(
                hkdf.expand(ArrayUtil.join(
                    BitConverter.GetBytes(biome_id),
                    BitConverter.GetBytes(biome_stage),
                    BitConverter.GetBytes(lives_count),
                    ship_parts_data
                ),
                sizeof(int)));
            return seed;
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
        private static int generateBiomeSeed(byte[] global_seed, uint biome_id)
        {
            HKDF hkdf = new();
            hkdf.extract(null, global_seed);
            int seed = BitConverter.ToInt32(
                hkdf.expand(BitConverter.GetBytes(biome_id), sizeof(int)));
            return seed;
        }
    }
}