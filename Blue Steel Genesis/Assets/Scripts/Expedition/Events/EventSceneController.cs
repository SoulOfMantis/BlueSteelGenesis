using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EventSceneController : MonoBehaviour
{
    [Header("UI References")]
    public Image eventImage;
    public TMP_Text eventNameText;
    public TMP_Text eventDescriptionText;
    public Transform buttonsContainer;
    public GameObject buttonPrefab;

    [Header("Reward Screen")]
    public GameObject rewardPanelPrefab;

    private const string MAP_SCENE_NAME = "ExpeditionMapTest_usingGameState";

    private EventData currentEvent;
    private EventData.EventState currentState;

    void Start()
    {
        currentEvent = CurrentEventHolder.Event;
        if (currentEvent == null || currentEvent.states == null || currentEvent.states.Count == 0)
        {
            Debug.LogError("Нет данных о событии!");
            ReturnToMap();
            return;
        }

        SetState(currentEvent.states[0].stateId);
    }

    private void SetState(string stateId)
    {
        currentState = currentEvent.states.Find(s => s.stateId == stateId);
        if (currentState == null)
        {
            Debug.LogError($"Состояние '{stateId}' не найдено!");
            ReturnToMap();
            return;
        }

        if (eventImage != null && currentEvent.eventImage != null)
            eventImage.sprite = currentEvent.eventImage;
        if (eventNameText != null)
            eventNameText.text = currentEvent.eventName;
        if (eventDescriptionText != null)
            eventDescriptionText.text = currentState.description;

        foreach (Transform child in buttonsContainer)
            Destroy(child.gameObject);

        foreach (var choice in currentState.choices)
        {
            GameObject btnObj = Instantiate(buttonPrefab, buttonsContainer);
            Button btn = btnObj.GetComponent<Button>();
            TMP_Text btnText = btnObj.GetComponentInChildren<TMP_Text>();

            string effectDesc = "";
            if (choice.isRandom)
            {
                effectDesc = $"\nУспех ({choice.successChance}%): {choice.successEffect.GetDescription()}" +
                             $"\nНеудача: {choice.failureEffect.GetDescription()}";
            }
            else
            {
                effectDesc = $"\n{choice.successEffect.GetDescription()}";
            }
            btnText.text = choice.buttonText + effectDesc;

            EventChoice choiceCopy = choice;
            btn.onClick.AddListener(() => OnChoiceSelected(choiceCopy));
        }
    }

    void OnChoiceSelected(EventChoice choice)
    {
        bool success = true;
        if (choice.isRandom)
        {
            int roll = Random.Range(1, 101);
            success = roll <= choice.successChance;
        }

        EventEffect effect = success ? choice.successEffect : choice.failureEffect;

        ApplyEffects(effect, () =>
        {
            var player = GameState.Run?.Expedition?.Player;
            if (player != null && player.currentHealth.Value <= 0)
            {
                Debug.Log("Игрок погиб в событии!");
                ReturnToMap();
                return;
            }

            if (!string.IsNullOrEmpty(choice.nextStateId))
            {
                SetState(choice.nextStateId);
            }
            else
            {
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
                    case EventOutcome.EnterEliteBattle:
                        StartEliteBattle();
                        break;
                    case EventOutcome.EnterBossBattle:
                        StartBossBattle();
                        break;
                }
            }
        });
    }

    void ApplyEffects(EventEffect effect, System.Action onComplete)
    {
        var player = GameState.Run?.Expedition?.Player;
        if (player == null)
        {
            onComplete?.Invoke();
            return;
        }


        player.currentHealth.Value = (uint)Mathf.Clamp(
            player.currentHealth.Value + effect.healthChange,
            0,
            player.maxHealth
        );
        player.maxHealth = (uint)Mathf.Max(1, player.maxHealth + effect.maxHealthChange);


        player.currentEnergy.Value = (uint)Mathf.Clamp(
            player.currentEnergy.Value + effect.energyChange,
            0,
            player.maxEnergy
        );
        player.maxEnergy = (uint)Mathf.Max(0, player.maxEnergy + effect.maxEnergyChange);

        if (effect.moneyChange > 0)
            player.GiveMoney((uint)effect.moneyChange);
        else if (effect.moneyChange < 0)
            player.LoseMoney((uint)(-effect.moneyChange));


        if (effect.materialChange > 0)
            player.GiveMaterials((uint)effect.materialChange);

        if (effect.addModules != null)
            foreach (var mod in effect.addModules)
                player.AddModule(mod);
        if (effect.removeModuleIds != null)
            player.modules.RemoveAll(m => effect.removeModuleIds.Contains(m.Name));

        bool hasReward = (effect.addModules != null && effect.addModules.Count > 0) || effect.materialChange > 0 || effect.moneyChange > 0;

        if (hasReward && rewardPanelPrefab != null)
        {
            GameObject rewardPanel = Instantiate(rewardPanelPrefab, transform);
            var rewardController = rewardPanel.GetComponent<RewardPanelController>();
            rewardController.Initialize(effect, () =>
            {
                Destroy(rewardPanel);
                onComplete?.Invoke();
            });
        }
        else
        {
            onComplete?.Invoke();
        }
    }

    void ReturnToMap()
    {
        CurrentEventHolder.Event = null;
        SceneManager.LoadScene(MAP_SCENE_NAME);
    }

    void StartBattle() => GameState.Run.Expedition.CombatSystem.TriggerNormalEncounter();
    void StartEliteBattle() => GameState.Run.Expedition.CombatSystem.TriggerEliteEncounter();
    void StartBossBattle() => GameState.Run.Expedition.CombatSystem.TriggerBossEncounter();
    void StartShop() => GameState.Run.Expedition.Shop.TriggerShop();
}
public static class CurrentEventHolder
{
    public static EventData Event { get; set; }
}