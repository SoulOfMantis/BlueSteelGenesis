using System;

public class Expedition
{
    public Expedition(Map.BiomeInfo biome)
    {
        Biome = biome;
    }

    public void startNextStage()
    {
        ++BiomeStage;
        Map = global::Map.ExpeditionMap.generate(
            5, 9,
            BitConverter.GetBytes(GameState.Run.GlobalSeed),
            Biome, (uint)BiomeStage,
            GameState.Run.PlayerLivesCount,
            Array.Empty<byte>() //TODO: pass actual data
        );
        map_progress_ = new(Map);

        int local_seed = 0; //TODO: Add local seed
        combat_system = new CombatSystem(Biome.id, BiomeStage, local_seed);
    }

    public void displayMap(ExpeditionMapView view)
    {
        if (Map == null || map_progress_ == null)
            throw new InvalidOperationException("Невозможно отобразить карту до начала этапа");
        if (view != null)
            view.make(Map, map_progress_);
    }

    public Map.ExpeditionMap Map { get; private set; } = null;
    public Map.BiomeInfo Biome { get; private set; }

    private ExpeditionMapProgressInfo map_progress_ = null;
    public int BiomeStage { get; private set; } = -1;

    CombatSystem combat_system;


}
