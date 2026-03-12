using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class testTreasureGeneration : MonoBehaviour
{
    [SerializeField] Button exit;
    [SerializeField] Button reroll;
    [SerializeField] TMP_Text moduleName;

    void Reroll()
    {
        GameState.Run.Expedition.TreasureSubsystem.RerollTreasure();
        UpdateModuleName();
    }
    void Exit() => UnityEngine.SceneManagement.SceneManager.LoadScene("ExpeditionMapTest_usingGameState");
    private void Start()
    {
        exit.onClick.AddListener(Exit);
        reroll.onClick.AddListener(Reroll);
        Reroll();
    }
    void UpdateModuleName()
    {
        moduleName.text = GameState.Run.Expedition.TreasureSubsystem.Treasure.Name;
    }

}
