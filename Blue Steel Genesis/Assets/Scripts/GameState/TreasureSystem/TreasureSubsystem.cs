using Map;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
public class TreasureSubsystem
{
    uint biomeId;
    uint stage; //Потребуется, если у разных этапов будут разные сцены сокровищниц, иначе -- удалить
    public GameModule Treasure { get; private set; }
    public TreasureSubsystem(uint b, uint st)
    {
        UpdateInfo(b, st);
    }
    public void RerollTreasure()
    {
        Treasure = GameState.Run.Expedition.GetNextModule();
    }
    public void UpdateInfo(uint new_biome, uint new_stage)
    {
        biomeId = new_biome;
        stage = new_stage;
    }
    void LoadTreasureScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene($"treasure_room_b{biomeId}st{stage}");
        //UnityEngine.SceneManagement.SceneManager.LoadScene($"treasure_room_b{biomeId}");
    }
    public void Trigger()
    {
        RerollTreasure();
        LoadTreasureScene();
    }
}
