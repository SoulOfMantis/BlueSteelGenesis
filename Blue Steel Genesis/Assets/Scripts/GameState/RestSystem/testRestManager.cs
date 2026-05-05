using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class testRestManager : MonoBehaviour
{
    [SerializeField] private Button freeHealButton;
    [SerializeField] private Button paidHealButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private TMP_Text healthDisplay;
    [SerializeField] private TMP_Text materialDisplay;

    private void Start()
    {
        UpdateUI();
        freeHealButton.onClick.AddListener(OnFreeHeal);
        paidHealButton.onClick.AddListener(OnPaidHeal);
        exitButton.onClick.AddListener(OnExit);
    }

    private void UpdateUI()
    {
        var player = GameState.Run.Expedition.Player;
        healthDisplay.text = $"HP: {player.currentHealth}/{player.maxHealth}";
        materialDisplay.text = $"Materials: {player.materials}";
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