using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EventManager
{
    private static List<EventData> allEvents;
    private static List<EventData> availableEventsForStage = new List<EventData>();

    public static void LoadAllEvents()
    {
        allEvents = Resources.LoadAll<EventData>("Events").ToList();
        Debug.Log($"Загружено событий: {allEvents.Count}");
    }

    public static void PrepareEventsForStage(uint biomeId)
    {
        if (allEvents == null) LoadAllEvents();

        availableEventsForStage = allEvents
            .Where(e => e.allowedBiomes.Count == 0 || e.allowedBiomes.Contains(biomeId))
            .ToList();

        Debug.Log($"Подготовлено событий для биома {biomeId}: {availableEventsForStage.Count}");
    }

    public static EventData GetRandomEventForStage()
    {
        if (availableEventsForStage == null || availableEventsForStage.Count == 0)
        {
            Debug.LogWarning("Пул событий пуст! Используется заглушка.");
            return CreateFallbackEvent();
        }

        int index = Random.Range(0, availableEventsForStage.Count);
        EventData selected = availableEventsForStage[index];
        availableEventsForStage.RemoveAt(index);
        return selected;
    }

    public static void ClearStageEvents()
    {
        availableEventsForStage.Clear();
    }

    private static EventData CreateFallbackEvent()
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
                        isRandom = false
                    }
                }
            }
        };
        return fallback;
    }
}