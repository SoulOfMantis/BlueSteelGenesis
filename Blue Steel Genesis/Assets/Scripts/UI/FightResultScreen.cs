using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FightResultScreen : MonoBehaviour
{
    public void Awake() {
        if (!Application.isPlaying)
            return;
        if (victory_panel != null)
            victory_panel.SetActive(false);
        if (defeat_panel != null)
            defeat_panel.SetActive(false);
    }

    public void ShowVictory(uint money_reward, uint materials_reward, uint golden_tickets_reward, GameModule module_reward)
    {
        if (victory_description != null)
            victory_description.text =
                (money_reward != 0 ? $"Money: +{money_reward}\n" : "") +
                (materials_reward != 0 ? $"Spare parts: +{materials_reward}\n" : "") +
                (golden_tickets_reward != 0 ? $"Golden tickets: +{golden_tickets_reward}\n" : "") +
                (module_reward != null ? $"Module: {module_reward.Name}" : "");

        if (module_tooltip_trigger != null && module_info != null && take_module_button != null) {
            module_tooltip_trigger.updateModuleTrigger(module_reward);
            module_info.SetActive(module_reward != null);

            take_module_button.onClick.RemoveAllListeners();
            take_module_button.onClick.AddListener(
                () => TakeModuleReward(module_reward)
            );
            take_module_button.interactable = true;
        }

        if (victory_panel != null)
            victory_panel.SetActive(true);
    }

    public void ShowDefeat() {
        if (defeat_panel != null)
            defeat_panel.SetActive(true);
    }

    private void TakeModuleReward(GameModule reward) {
        if (ModuleManager.Modules.Count >= 5)
            return;
        ModuleManager.AddModule(reward);

        if (take_module_button != null) {
            take_module_button.onClick.RemoveAllListeners();
            take_module_button.interactable = false;
        }
    }

    public void ExitNode() =>
        GameState.Run.Expedition.exitNode();

    public void StartNewExpedition() {
        GameState.endGameRun();
        GameState.startGameRun();
        GameState.Run.startExpedition(1);
        GameState.Run.Expedition.showExpeditionMap();
    }

    public void ReturnToMainMenu() {
        //TODO
    }



    [SerializeField] private GameObject victory_panel;
    [SerializeField] private TMP_Text victory_description;
    [SerializeField] private GameObject module_info;
    [SerializeField] private ModuleTooltipTrigger module_tooltip_trigger;
    [SerializeField] private Button take_module_button;

    [SerializeField] private GameObject defeat_panel;
}
