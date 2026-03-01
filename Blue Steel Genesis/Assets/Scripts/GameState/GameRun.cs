using System.Collections.Generic;
using UnityEngine;

public class GameRun
{
    public GameRun(int seed)
    {
        GlobalSeed = seed;
    }

    public void start()
    {
        // TODO: handle player creation properly
        Player.modules = new List<GameModule>{
            new PoisonStinger(),
            new BasicMovement()
        };
        Player.livesCount = 3;
        Player.maxHealth = 10;
        Player.maxEnergy = 3;
        Player.currentHealth = Player.maxHealth;
        Player.materials = 3;
        Player.money = 10;
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
        Expedition.startNextStage();
    }

    public void endExpedition()
    {
        Expedition = null;
    }

    public int GlobalSeed { get; private set; }
    public Expedition Expedition { get; private set; } = null;
    
    public PlayerData Player { get; private set; } = new();
    public uint PlayerLivesCount { get; private set; } = 3; // TODO: move to player data
    // ship data
    // player data
    // available biomes
    // ...
}
