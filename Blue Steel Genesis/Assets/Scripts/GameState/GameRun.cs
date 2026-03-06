using UnityEngine;

public class GameRun
{
    public GameRun(int seed)
    {
        GlobalSeed = seed;
    }

    public void start()
    {
        playerLivesCount = 3;
    }

    public void startExpedition(uint biome_id)
    {
        if (Expedition != null) {
            Debug.LogWarning("Попытка начать новую экспедицию до окончания предыдущей");
            return;
        }
        Expedition = new(new Map.BiomeInfo() { // TODO: handle biomes
            id = biome_id,
            missing_node_rate = .3f
        });
        Expedition.start();
    }

    public void endExpedition()
    {
        Expedition = null;
    }

    public int GlobalSeed { get; private set; }
    public Expedition Expedition { get; private set; } = null;
    
    public uint playerLivesCount { get; set; }

    // ship data
    // available biomes
    // ...
}
