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


    /// <summary>
    /// Подготавливает пул событий для нового этапа в указанном биоме.
    /// </summary>
    public static void PrepareEventsForStage(uint biomeId)
    {
        if (allEvents == null) LoadAllEvents();

        availableEventsForStage = allEvents
            .Where(e => e.allowedBiomes.Count == 0 || e.allowedBiomes.Contains(biomeId))
            .ToList();

        Debug.Log($"Подготовлено событий для биома {biomeId}: {availableEventsForStage.Count}");
    }


    //public static EventData GetRandomEventForBiome(uint biomeId)
    //{
    //    if (allEvents == null) LoadAllEvents();

    //    var valid = allEvents.Where(e => e.allowedBiomes.Count == 0 || e.allowedBiomes.Contains(biomeId)).ToList();
    //    if (valid.Count == 0)
    //    {
    //        Debug.LogWarning($"Нет событий для биома {biomeId}, используется заглушка");
    //        EventData fallback = ScriptableObject.CreateInstance<EventData>();
    //        fallback.eventName = "Что-то привлекло ваш взгляд";
    //        fallback.choices = new List<EventChoice>
    //        {
    //            new EventChoice { buttonText = "Уйти", outcome = EventOutcome.Exit, isRandom = false }
    //        };
    //        return fallback;
    //    }

    //    int index = Random.Range(0, valid.Count);
    //    return valid[index];
    //}


    //public static List<EventData> GetEventsForBiomeAndStage(uint biomeId, uint stage)
    //{
    //    if (allEvents == null) LoadAllEvents();
    //    return allEvents.Where(e =>
    //        (e.allowedBiomes.Count == 0 || e.allowedBiomes.Contains(biomeId)) &&
    //        (e.allowedStages.Count == 0 || e.allowedStages.Contains(stage))
    //    ).ToList();
    //}

    /// <summary>
    /// Возвращает случайное событие из пула и удаляет его, чтобы не повторялось.
    /// Если пул пуст – перезаполняет его (или можно вернуть заглушку).
    /// </summary>
    /// 
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

    /// <summary>
    /// Очищает пул событий (вызывается при завершении экспедиции).
    /// </summary>
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
                stateId = "start",
                description = "Вы находите странный предмет, но решаете не рисковать.",
                choices = new List<EventChoice>
                {
                    new EventChoice { buttonText = "Уйти", outcome = EventOutcome.Exit, isRandom = false }
                }
            }
        };
        return fallback;
    }
}
