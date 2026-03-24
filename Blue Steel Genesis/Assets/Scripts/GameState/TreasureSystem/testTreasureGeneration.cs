using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class testTreasureGeneration : MonoBehaviour
{
    [SerializeField] Button exit;
    [SerializeField] Button openTheChest;
    [SerializeField] ModuleTooltipTrigger trigger;

    void Take()
    {
        if (ModuleManager.AddModule(GameState.Run.Expedition.TreasureSubsystem.Treasure))
        {
            trigger.gameObject.SetActive(false);
            Destroy(trigger);
            openTheChest.gameObject.SetActive(false);
            Destroy(openTheChest);
        }
    }
    void Exit() => UnityEngine.SceneManagement.SceneManager.LoadScene("ExpeditionMapTest_usingGameState");
    private void Start()
    {
        GameState.Run.Expedition.TreasureSubsystem.RerollTreasure();
        trigger.updateModuleTrigger(GameState.Run.Expedition.TreasureSubsystem.Treasure);
        exit.onClick.AddListener(Exit);
        openTheChest.onClick.AddListener(Take);
    }
}
