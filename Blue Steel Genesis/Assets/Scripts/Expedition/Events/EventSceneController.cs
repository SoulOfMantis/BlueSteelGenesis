using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EventSceneController : MonoBehaviour
{
    [Header("UI References")]
    public Image eventImage;
    public TMP_Text eventNameText;
    public TMP_Text eventDescriptionText;
    public Transform buttonsContainer;
    public GameObject buttonPrefab;
    public GameObject rewardPanelPrefab;
    public GameObject moduleTooltipPrefab;      // Префаб тултипа, содержащий ModuleTooltipTrigger

    private EventData currentEvent;
    private uint currentStateId;



    private void SetState(uint stateId)
    {
        EventData.EventState state = FindState(stateId);
        if (state == null)
        {
            Debug.LogError($"Состояние с id={stateId} не найдено!");
            ReturnToMap();
            return;
        }

        currentStateId = stateId;

        if (eventImage != null && currentEvent.eventImage != null)
            eventImage.sprite = currentEvent.eventImage;
        if (eventNameText != null)
            eventNameText.text = currentEvent.eventName;
        if (eventDescriptionText != null)
            eventDescriptionText.text = state.description;

        foreach (Transform child in buttonsContainer)
            Destroy(child.gameObject);

        foreach (var choice in state.choices)
        {
            GameObject btnObj = Instantiate(buttonPrefab, buttonsContainer);
            Button btn = btnObj.GetComponent<Button>();
            TMP_Text btnText = btnObj.GetComponentInChildren<TMP_Text>();

            string effectText = "";
            if (choice.isRandom)
            {
                effectText = $"\nУспех ({choice.successChance}%): {choice.successEffect.GetDescription()}" +
                             $"\nНеудача: {choice.failureEffect.GetDescription()}";
            }
            else
            {
                effectText = $"\n{choice.successEffect.GetDescription()}";
            }

            btnText.text = choice.buttonText;


            string details = "";
            if (choice.isRandom)
            {
                details += $"Шанс успеха: {choice.successChance}%\n";
                details += choice.successEffect.GetDescription() + "\n";
                details += "Неудача: " + choice.failureEffect.GetDescription();
            }
            else
            {
                details = choice.successEffect.GetDescription();
            }


            var effectTexts = btnObj.transform.Find("EffectDetails")?.GetComponent<TMP_Text>();
            if (effectTexts != null)
            {
                effectTexts.text = details;
            }

            List<GameModule> affectedModules = new();
            var player = GameState.Run?.Expedition?.Player;

            if (choice.successEffect != null)
            {
                if (choice.successEffect.addModules != null)
                    affectedModules.AddRange(choice.successEffect.addModules);
                if (choice.successEffect.removeModuleIds != null && player != null)
                    foreach (var id in choice.successEffect.removeModuleIds)
                    {
                        var mod = player.modules.Find(m => m.Name == id);
                        if (mod != null) affectedModules.Add(mod);
                    }
            }
            if (choice.failureEffect != null)
            {
                if (choice.failureEffect.addModules != null)
                    affectedModules.AddRange(choice.failureEffect.addModules);
                if (choice.failureEffect.removeModuleIds != null && player != null)
                    foreach (var id in choice.failureEffect.removeModuleIds)
                    {
                        var mod = player.modules.Find(m => m.Name == id);
                        if (mod != null) affectedModules.Add(mod);
                    }
            }

            if (affectedModules.Count > 0 && moduleTooltipPrefab != null)
            {
                var moduleSet = new EventModuleSet(affectedModules);
                var tooltipGO = Instantiate(moduleTooltipPrefab, btnObj.transform);
                var trigger = tooltipGO.GetComponent<ModuleTooltipTrigger>();
                if (trigger != null)
                    trigger.updateModuleTrigger(moduleSet);
            }

            EventChoice choiceCopy = choice;
            btn.onClick.AddListener(() => OnChoiceSelected(choiceCopy));
        }
    }

    void OnChoiceSelected(EventChoice choice)
    {
        bool success = true;
        if (choice.isRandom)
        {
            success = SuccessChecker.RollSuccess(choice.successChance);
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

            if (choice.nextStateId == 0)
            {
                ReturnToMap();
            }
            else
            {
                SetState(choice.nextStateId);
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

        player.ApplyEventEffects(effect);

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



    private EventData.EventState FindState(uint id)
    {
        return currentEvent.states.Find(s => s.stateId == id);
    }



    void Start()
    {
        var eventSystem = GameState.Run?.Expedition?.EventSystem;
        currentEvent = eventSystem?.CurrentEvent;
        if (currentEvent == null || currentEvent.states == null || currentEvent.states.Count == 0)
        {
            ReturnToMap();
            return;
        }
        SetState(1);
    }



    void ReturnToMap()
    {
        var eventSystem = GameState.Run?.Expedition?.EventSystem;
        eventSystem?.ClearCurrentEvent();   
        SceneManager.LoadScene(GameEventConstants.MAP_SCENE_NAME);
    }


    public void SetEvent(EventData newEvent)
    {
        currentEvent = newEvent;
        // Ручной запуск первого состояния
        if (currentEvent != null && currentEvent.states != null && currentEvent.states.Count > 0)
            SetState(1); 
    }
}
