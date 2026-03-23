using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EventSceneController : MonoBehaviour
{
    [Header("UI References")]
    public Text eventNameText;
    public Text narrativeText;
    public Transform buttonsContainer;       
    public GameObject buttonPrefab;        

    private EventData currentEvent;

    void Start()
    {

        currentEvent = GlobalEventStorage.CurrentEvent;
        if (currentEvent == null)
        {
            Debug.LogError("Нет данных о событии!");
            ReturnToMap();
            return;
        }


        eventNameText.text = currentEvent.eventName;
        narrativeText.text = currentEvent.narrativeText;

 
        foreach (var choice in currentEvent.choices)
        {
            CreateChoiceButton(choice);
        }
    }

    void CreateChoiceButton(EventData.EventChoice choice)
    {
        GameObject btnObj = Instantiate(buttonPrefab, buttonsContainer);
        Button btn = btnObj.GetComponent<Button>();

        Text btnText = btnObj.GetComponentInChildren<Text>();
        if (btnText != null)
            btnText.text = choice.buttonText;

        Text descText = btnObj.transform.Find("Description")?.GetComponent<Text>();
        if (descText != null)
            descText.text = choice.effectDescription;

        btn.onClick.AddListener(() => OnChoiceSelected(choice));
    }

    void OnChoiceSelected(EventData.EventChoice choice)
    {
        ApplyEffects(choice);


        Expedition expedition = GameState.CurrentExpedition;
        if (expedition != null)
        {
            expedition.HandleEventOutcome(choice.outcome);
        }
        else
        {
            Debug.LogError("Экспедиция не найдена!");
            ReturnToMap();
        }
    }

    void ApplyEffects(EventData.EventChoice choice)
    {
        var player = GameState.CurrentExpedition?.Player;
        if (player == null) return;

        player.money += (uint)choice.moneyChange;
        player.materials += (uint)choice.materialChange;
        player.currentHealth += choice.healthChange;
        player.currentHealth = Mathf.Clamp(player.currentHealth, 0, player.maxHealth);

        Debug.Log($"Применены эффекты: деньги {choice.moneyChange}, материалы {choice.materialChange}, здоровье {choice.healthChange}");
    }

    void ReturnToMap()
    {
        // TODO добавить название сцены карты 
        UnityEngine.SceneManagement.SceneManager.LoadScene("ExpeditionMapTest_usingGameState");
    }
}