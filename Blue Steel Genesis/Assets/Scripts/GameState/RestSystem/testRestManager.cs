using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class testRestManager : MonoBehaviour
{
    [SerializeField] private Button freeHealButton;
    [SerializeField] private Button paidHealButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private TMP_Text healthDisplay;
    [SerializeField] private TMP_Text freeHealDisplay;
    [SerializeField] private TMP_Text paidHealDisplay;
    [SerializeField] private TMP_Text materialDisplay;
    [SerializeField] private ModuleManagementUI moduleManagementUI;

    private void Start()
    {
        UpdateUI();
        freeHealButton.onClick.AddListener(OnFreeHeal);
        paidHealButton.onClick.AddListener(OnPaidHeal);
        exitButton.onClick.AddListener(OnExit);
        moduleManagementUI.gameObject.SetActive(false);
    }

    private void UpdateUI()
    {
        var player = GameState.Run.Expedition.Player;
        var rest = GameState.Run.Expedition.Rest;
        healthDisplay.text = $"HP: {player.currentHealth}/{player.maxHealth}";
        freeHealDisplay.text = $"Heal {rest.FreeHealRestores()} for free";
        paidHealDisplay.text = $"Heal {rest.PaidHealRestores()} for {Rest.PaidHealCost} spare parts";
        materialDisplay.text = $"Spare parts: {player.materials}";
    }

    private void OnFreeHeal()
    {
        GameState.Run.Expedition.Rest.FreeHeal();
        UpdateUI();
        DisableHealButtons();
    }

    private void OnPaidHeal()
    {
        GameState.Run.Expedition.Rest.PaidHeal();
        UpdateUI();
        DisableHealButtons();
    }

    private void DisableHealButtons()
    {
        freeHealButton.interactable = false;
        paidHealButton.interactable = false;
    }

    private void OnExit()
    {
        GameState.Run.Expedition.Rest.Exit();
    }
}