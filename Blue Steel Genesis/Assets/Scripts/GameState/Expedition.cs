using System;
using System.Collections.Generic;
using HKDF = HKDF<System.Security.Cryptography.HMACSHA1>;

public class Expedition
{
    public Expedition(Map.BiomeInfo biome)
    {
        Biome = biome;
        BiomeSeed = generateBiomeSeed(GameState.Run.GlobalSeed, biome.id);
    }

    public void start()
    {
        // TODO: handle player creation properly
        Player.modules = new List<GameModule>{
            new DefaultAdaptiveTEST_ONLY(),
            new MechanicStinger(),
            new BasicMovement(),
            new DogSummoner_TEST_ONLY()
        };
        Player.maxHealth = 10;
        Player.maxEnergy = 3;
        Player.currentHealth.Value = Player.maxHealth;
        Player.materials.Value = 3;
        Player.money.Value = 10;

        startNextStage();
    }

    public void startNextStage()
    {
        ++BiomeStage;

        LocalSeed = generateLocalSeed(
            GameState.Run.GlobalSeed,
            Biome.id, (uint)BiomeStage,
            GameState.Run.playerLivesCount,
            Array.Empty<byte>() //TODO: pass actual data
        );
        Map = global::Map.ExpeditionMap.generate(
            BiomeSeed, LocalSeed,
            Biome, (uint)BiomeStage
        );
        map_progress_ = new(Map);
        ModuleGen = new(LocalSeed);
        TreasureSubsystem = new(Biome.id);
    }


    public void displayMap(ExpeditionMapView view)
    {
        if (Map == null || map_progress_ == null)
            throw new InvalidOperationException("Невозможно отобразить карту до начала этапа");
        if (view != null)
            view.make(Map, map_progress_);
    }

    public int LocalSeed { get; private set; }
    public int BiomeSeed { get; private set; }

    public PlayerData Player { get; private set; } = new();
    public Map.ExpeditionMap Map { get; private set; } = null;
    public Map.BiomeInfo Biome { get; private set; }

    private ExpeditionMapProgressInfo map_progress_ = null;
    public int BiomeStage { get; private set; } = -1;



    private static int generateLocalSeed(int global_seed, uint biome_id, uint biome_stage, uint lives_count, byte[] ship_parts_data)
    {
        HKDF hkdf = new();
        hkdf.extract(null, BitConverter.GetBytes(global_seed));
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
    private static int generateBiomeSeed(int global_seed, uint biome_id)
    {
        HKDF hkdf = new();
        hkdf.extract(null, BitConverter.GetBytes(global_seed));
        int seed = BitConverter.ToInt32(
            hkdf.expand(BitConverter.GetBytes(biome_id), sizeof(int)));
        return seed;
    }
    public TreasureSubsystem TreasureSubsystem;
    public ModuleGenerator ModuleGen;
}
