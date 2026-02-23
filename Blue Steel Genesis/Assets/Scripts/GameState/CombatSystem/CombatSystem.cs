using System;

public class CombatSystem
{
    int biome_id, stage_id;

    Random gen;

    static const int max_enc_id = 11;

    public CombatSystem(int biome, int stage, int local_seed)
    {
        biome_id = biome;
        stage_id = stage;
        gen = new Random(local_seed);
    }

    public string NextNormalEncounter(int biome_id, int stage_id)
    {
        int enc_id = gen.Next(max_enc_id);
        return $"Biome{biome_id}Stage{stage_id}Normal_{enc_id}";
    }
}