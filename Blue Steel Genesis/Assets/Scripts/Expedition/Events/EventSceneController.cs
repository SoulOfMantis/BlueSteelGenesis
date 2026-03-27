using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EventSceneController : MonoBehaviour
{
    public Image eventImage;
    public TMP_Text eventNameText;
    public TMP_Text eventDescriptionText;
    public Transform buttonsContainer;
    public GameObject buttonPrefab;

    private EventData currentEvent;

    void Start()
    {
        currentEvent = CurrentEventHolder.Event;
        if (currentEvent == null)
        {
            Debug.LogError("Нет данных о событии!");
            ReturnToMap();
            return;
        }

        if (eventImage != null && currentEvent.eventImage != null)
            eventImage.sprite = currentEvent.eventImage;
        if (eventNameText != null)
            eventNameText.text = currentEvent.eventName;
        if (eventDescriptionText != null)
            eventDescriptionText.text = currentEvent.eventDescription;

        foreach (var choice in currentEvent.choices)
        {
            GameObject btnObj = Instantiate(buttonPrefab, buttonsContainer);
            Button btn = btnObj.GetComponent<Button>();
            TMP_Text btnText = btnObj.GetComponentInChildren<TMP_Text>();
            if (btnText != null) btnText.text = choice.buttonText;

            EventChoice choiceCopy = choice;
            btn.onClick.AddListener(() => OnChoiceSelected(choiceCopy));
        }
    }

    void OnChoiceSelected(EventChoice choice)
    {
        bool success = true;
        if (choice.isRandom)
        {
            int roll = Random.Range(1, 11);
            success = roll <= choice.successChance / 10;
        }

        EventEffect effect = success ? choice.successEffect : choice.failureEffect;
        ApplyEffects(effect);

        switch (choice.outcome)
        {
            case EventOutcome.Exit:
                ReturnToMap();
                break;
            case EventOutcome.EnterBattle:
                StartBattle();
                break;
            case EventOutcome.EnterShop:
                StartShop();
                break;
        }
    }

    void ApplyEffects(EventEffect effect)
    {
        var player = GameState.Run?.Expedition?.Player;
        if (player == null) return;

        if (effect.healthChange != 0)
        {
            player.currentHealth += effect.healthChange;
        }

        if (effect.moneyChange != 0)
        {
            if (effect.moneyChange > 0)
                player.GiveMoney((uint)effect.moneyChange);
            else if (effect.moneyChange < 0)
            {
                uint toLose = (uint)(-effect.moneyChange);
                if (player.money >= toLose)
                    player.LoseMoney(toLose);
                else
                    player.LoseMoney(player.money);
            }
        }

        Debug.Log($"Применены эффекты: здоровье {effect.healthChange}, деньги {effect.moneyChange}");
    }

    void StartBattle()
    {
        GameState.Run.Expedition.CombatSystem.TriggerNormalEncounter();
    }

    void StartShop()
    {
        GameState.Run.Expedition.Shop.TriggerShop();
    }

    void ReturnToMap()
    {
        CurrentEventHolder.Event = null;
        SceneManager.LoadScene("ExpeditionMapTest_usingGameState");
    }
}

public static class CurrentEventHolder
{
    public static EventData Event { get; set; }
}