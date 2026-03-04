using System;

public class CombatSystem
{
    uint biome_id, stage_id;

    Random gen;

    const int max_enc_id = 3;
    const int materials_given = 1;
    const int money_given = 1;

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

    public void TriggerNormalEncounter()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(NextNormalEncounter());
    }

    public void Defeat()
    {
        GameState.Run.endExpedition();
    }

    public void Victory()
    {
        GameState.Run.Player.GiveMaterials(materials_given);
        GameState.Run.Player.GiveMoney(money_given);
        UnityEngine.SceneManagement.SceneManager.LoadScene("ExpeditionMapTest_usingGameState");
    }
}   