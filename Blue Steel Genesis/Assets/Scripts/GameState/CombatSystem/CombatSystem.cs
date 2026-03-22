using Map;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CombatSystem
{
    uint biome_id, stage_id;
    Type reward;
    internal enum EncounterType
    {
        Normal,
        Elite,
        Boss
    }
    EncounterType current_enc;

    Map.BiomeInfo BiomeInfo;
    System.Random gen;

    const uint max_enc_id = 3;
    const uint max_materials_given = 30;
    const uint max_money_given = 30;
    const uint min_materials_given = 5;
    const uint min_money_given = 5;

    const uint normal_reward_modifier = 1;
    const uint elite_reward_modifier = 3;
    const uint boss_reward_modifier = 5;



    const uint default_elite = 0;
    //const uint elite_variation_count = 3;

    List<uint> boss_list = new List<uint>() { 1, 2, 3 };
    const uint default_boss = 0;
    const uint boss_variation_count = 3;

    public CombatSystem(uint biome, uint stage, int local_seed)
    {
        biome_id = biome;
        stage_id = stage;
        gen = new System.Random(local_seed);
    }

    string NextNormalEncounter()
    {
        uint enc_id = (uint)gen.Next((int)max_enc_id);
        return $"b{biome_id}_st{stage_id}_Normal{enc_id}";
    }

    string NextEliteEncounter()
    {
        uint elite_id;
        List<uint> elite_list = BiomeInfo.elites.Keys.Where(x => x.stage == stage_id).Select(y => y.elite_id).ToList();

        if (elite_list.Count != 0)
        {
            int elite_ind = gen.Next(elite_list.Count);
            elite_id = elite_list[elite_ind];
            BiomeInfo.elites.Remove((stage_id, elite_id));
        }
        else elite_id = default_elite;
        reward = BiomeInfo.elites[(stage_id, elite_id)];

        return $"b{biome_id}_st{stage_id}_Elite{elite_id}"; // Добавить вариации
    }

    string NextBossEncounter()
    {
        uint boss_id;
        if (boss_list.Count != 0)
        {
            int boss_ind = gen.Next(boss_list.Count);
            boss_id = boss_list[boss_ind];
            boss_list.RemoveAt(boss_ind);
        }
        else boss_id = default_boss;

        uint boss_variation = (uint)gen.Next((int)boss_variation_count);
        return $"b{biome_id}_st{stage_id}_Boss{boss_id}_{boss_variation}";
    }

    public void TriggerNormalEncounter()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(NextNormalEncounter());
        current_enc = EncounterType.Normal;
    }

    public void TriggerEliteEncounter()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(NextEliteEncounter());
        current_enc = EncounterType.Elite;
    }

    public void TriggerBossEncounter()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(NextBossEncounter());
        current_enc = EncounterType.Boss;
    }

    void GiveReward(uint modifier)
    {
        uint materials_given = min_materials_given;
        uint money_given = min_money_given;

        for (int i = 0; i < modifier; i++)
        {
            materials_given += (uint)gen.Next((int)min_materials_given, (int)max_materials_given);
            money_given += (uint)gen.Next((int)min_money_given, (int)max_money_given);
        }

        GameState.Run.Expedition.Player.GiveMaterials(materials_given);
        GameState.Run.Expedition.Player.GiveMoney(money_given);

        UnityEngine.SceneManagement.SceneManager.LoadScene("ExpeditionMapTest_usingGameState");
    }

    public void Defeat()
    {
        GameState.Run.endExpedition();
    }

    public void Victory()
    {
        switch (current_enc)
        {
            case EncounterType.Normal: 
                VictoryNormal(); 
                break;
            case EncounterType.Elite:
                VictoryElite();
                break;
            case EncounterType.Boss:
                VictoryBoss();
                break;
        }
    }

    void VictoryNormal()
    {
        GiveReward(normal_reward_modifier);
    }

    void VictoryElite()
    {
        GiveReward(elite_reward_modifier);
        UnityEngine.Debug.Log($"Reward: {reward.Name}");
    }

    void VictoryBoss()
    {
        GiveReward(boss_reward_modifier);
        // GameState.Run.Expedition.startNextStage();
    }
}   