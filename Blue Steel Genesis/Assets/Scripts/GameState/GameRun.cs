using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameRun
{
    public GameRun(int seed)
    {
        GlobalSeed = seed;
    }

    public void start()
    {
        playerLivesCount = new(3, 3);
    }

    public static Dictionary<(uint stage, uint elite_id), System.Type> GetElitesByBiomeId(uint biomeId)
    {
        switch (biomeId)
        {
            case 1:
                var res = new Dictionary<(uint stage, uint elite_id), System.Type>();
                res[(0, 1)] = typeof(WideAttack);
                return res;
            default:
                return new Dictionary<(uint stage, uint elite_id), System.Type>();
        }
    }

    public static Dictionary<(uint stage, uint boss_id), uint> GetBossesByBiomeId(uint biomeId)
    {
        switch (biomeId)
        {
            default:
                return new Dictionary<(uint stage, uint boss_id), uint>();
        }
    }

    public void startExpedition(uint biome_id)
    {
        if (Expedition != null) {
            Debug.LogWarning("Попытка начать новую экспедицию до окончания предыдущей");
            return;
        }
        Expedition = new(new Map.BiomeInfo(biome_id, GetElitesByBiomeId(biome_id), GetBossesByBiomeId(biome_id)) { // TODO: handle biomes
            id = biome_id,
            missing_node_rate = .3f
        });
        Expedition.start();
    }

    public void endExpedition()
    {
        Expedition = null;
    }

    [field: SerializeField]
    public int GlobalSeed { get; private set; }

    [field: SerializeField]
    public Expedition Expedition { get; private set; } = null;
    
    [field: SerializeField]
    public URangeValue playerLivesCount { get; set; }

    // ship data
    // available biomes
    // ...
}
