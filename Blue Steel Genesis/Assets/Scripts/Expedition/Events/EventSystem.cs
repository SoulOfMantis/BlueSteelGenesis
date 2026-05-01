using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class EventSystem
{
    private List<EventData> eventPool = new List<EventData>();
    public EventData CurrentEvent { get; private set; }

    public EventSystem(uint biomeId)
    {
        PrepareEvents(biomeId);
    }

    private void PrepareEvents(uint biomeId)
    {
        var allEvents = Resources.LoadAll<EventData>("Events");
        if (allEvents == null || allEvents.Length == 0)
        {
            Debug.LogError("Не найдены EventData в Resources/Events!");
            return;
        }

        eventPool = allEvents.Where(e => e.allowedBiomes == null || e.allowedBiomes.Count == 0 || e.allowedBiomes.Contains(biomeId)).ToList();

        Debug.Log($"[EventSystem] Подготовлено событий для биома {biomeId}: {eventPool.Count}");
    }

    public bool HasEvent()
    {
        return eventPool != null && eventPool.Count > 0;
    }


    public void PickRandomEvent()
    {
        if (!HasEvent())
        {
            Debug.LogWarning("Пул событий пуст! Будет использовано событие‑заглушка.");
            CurrentEvent = CreateFallbackEvent();
            return;
        }

        int index = Random.Range(0, eventPool.Count);
        CurrentEvent = eventPool[index];
        eventPool.RemoveAt(index);
    }

    public void TriggerEvent()
    {
        PickRandomEvent();
        if (CurrentEvent == null)
        {
            Debug.LogError("Не удалось получить событие для триггера.");
            SceneManager.LoadScene(GameEventConstants.MAP_SCENE_NAME); 
            return;
        }
        SceneManager.LoadScene(GameEventConstants.EVENT_SCENE_NAME);
    }


    public void ClearCurrentEvent()
    {
        CurrentEvent = null;
    }

    private EventData CreateFallbackEvent()
    {
        EventData fallback = ScriptableObject.CreateInstance<EventData>();
        fallback.eventName = "Что-то привлекло ваш взгляд";
        fallback.states = new List<EventData.EventState>
        {
            new EventData.EventState
            {
                stateId = 1,
                description = "Вы находите странный предмет, но решаете не рисковать.",
                choices = new List<EventChoice>
                {
                    new EventChoice
                    {
                        buttonText = "Уйти",
                        nextStateId = 0,
                        isRandom = false,
                        successEffect = new EventEffect()
                    }
                }
            }
        };
        return fallback;
    }
}