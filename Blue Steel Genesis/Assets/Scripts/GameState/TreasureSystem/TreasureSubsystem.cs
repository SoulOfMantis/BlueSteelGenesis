using System;
using UnityEngine;

[Serializable]
public class TreasureSubsystem
{
    [SerializeField]
    uint biomeId;

    public GameModule Treasure { get; private set; }
    public TreasureSubsystem(uint b)
    {
        biomeId = b;
    }
    public void RerollTreasure()
    {
        Treasure = GameState.Run.Expedition.ModuleGen.GetNextCommonModule(GameState.Run.Expedition.Player.modules);
    }
    void LoadTreasureScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene($"treasure_room_b{biomeId}");
    }
    public void Trigger()
    {
        RerollTreasure();
        LoadTreasureScene();
    }
}
