using System;
using System.Collections.Generic;

public class CombatSystem
{
    uint biome_id, stage_id;

    Random gen;

    const int max_enc_id = 3;
    const int materials_given = 1;
    const int money_given = 1;


    List<int> elite_variation_list = new List<int>() { 1, 2, 3 };
    const int default_elite_variation = 0;

    public CombatSystem(uint biome, uint stage, int local_seed)
    {
        biome_id = biome;
        stage_id = stage;
        gen = new Random(local_seed);
    }

    string NextNormalEncounter()
    {
        int enc_id = gen.Next(max_enc_id);
        return $"b{biome_id}_st{stage_id}_Normal{enc_id}";
    }

    string NextEliteEncounter()
    {
        int elite_variation;
        if (elite_variation_list.Count != 0)
        {
            int elite_variation_ind = gen.Next(elite_variation_list.Count);
            elite_variation = elite_variation_list[elite_variation_ind];
            elite_variation_list.RemoveAt(elite_variation_ind);
        }
        else elite_variation = default_elite_variation;

        int enc_id = gen.Next(max_enc_id);
        return $"b{biome_id}_st{stage_id}_Elite{enc_id}_{elite_variation}";
    }

    public void TriggerNormalEncounter()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(NextNormalEncounter());
    }

    public void TriggerEliteEncounter()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(NextEliteEncounter());
    }

    public void Defeat()
    {
        GameState.Run.endExpedition();
    }

    public void Victory()
    {
        GameState.Run.Expedition.Player.GiveMaterials(materials_given);
        GameState.Run.Expedition.Player.GiveMoney(money_given);
        UnityEngine.SceneManagement.SceneManager.LoadScene("ExpeditionMapTest_usingGameState");
    }
}   