using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardPanelController : MonoBehaviour
{
    public TMP_Text rewardDescriptionText;
    public Transform rewardOptionsContainer;
    public GameObject rewardOptionPrefab;   
    public Button continueButton;

    private System.Action onCloseCallback;

    public void Initialize(EventEffect effect, System.Action onClose)
    {
        onCloseCallback = onClose;
        continueButton.onClick.AddListener(() => onCloseCallback?.Invoke());


        string desc = "Вы получили:";
        if (effect.materialChange > 0)
            desc += $"\n• {effect.materialChange}";
        if (effect.addModules != null && effect.addModules.Count > 0)
            foreach (var mod in effect.addModules)
                desc += $"\n• Модуль: {mod.Name}";

        rewardDescriptionText.text = desc;


    }
}