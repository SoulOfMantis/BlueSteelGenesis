using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class testRestManager : MonoBehaviour
{
    [SerializeField] private Button freeHealButton;
    [SerializeField] private Button paidHealButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private TMP_Text healthDisplay;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMP_Text freeHealDisplay;
    [SerializeField] private TMP_Text paidHealDisplay;
    [SerializeField] private ModuleManagementUI moduleManagementUI;
    [SerializeField] private TryButtonSFX healSFX;

    private void Start()
    {
        UpdateUI();
        freeHealButton.onClick.AddListener(OnFreeHeal);
        paidHealButton.onClick.AddListener(OnPaidHeal);
        paidHealButton.interactable = GameState.Run.Expedition.Player.HasEnoughMaterials(Rest.PaidHealCost);
        exitButton.onClick.AddListener(OnExit);
        moduleManagementUI.gameObject.SetActive(false);
    }

    private void UpdateUI()
    {
        var player = GameState.Run.Expedition.Player;
        var rest = GameState.Run.Expedition.Rest;
        healthDisplay.text = $"{player.currentHealth}/{player.maxHealth}";
        healthSlider.maxValue = player.maxHealth;
        healthSlider.value = player.currentHealth.Value;
        freeHealDisplay.text = $"Heal {rest.FreeHealRestores()} for free";
        paidHealDisplay.text = $"Heal {rest.PaidHealRestores()} for {Rest.PaidHealCost} spare parts";
    }

    private void OnFreeHeal()
    {
        GameState.Run.Expedition.Rest.FreeHeal();
        UpdateUI();
        DisableHealButtons();
        healSFX.playSuccess();
    }

    private void OnPaidHeal()
    {
        if (GameState.Run.Expedition.Rest.PaidHeal())
            healSFX.playSuccess();
        else
            healSFX.playFailure();
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