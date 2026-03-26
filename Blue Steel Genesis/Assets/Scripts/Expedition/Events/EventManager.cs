using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EventManager
{
    private static List<EventData> allEvents;

     
    public static void LoadAllEvents()
    {
        allEvents = Resources.LoadAll<EventData>("Events").ToList();
        Debug.Log($"Загружено событий: {allEvents.Count}");
    }

    public static EventData GetRandomEventForBiome(uint biomeId)
    {
        if (allEvents == null) LoadAllEvents();

        var valid = allEvents.Where(e => e.allowedBiomes.Count == 0 || e.allowedBiomes.Contains(biomeId)).ToList();
        if (valid.Count == 0)
        {
            Debug.LogWarning($"Нет событий для биома {biomeId}, используется заглушка");
            EventData fallback = ScriptableObject.CreateInstance<EventData>();
            fallback.eventName = "Что-то привлекло ваш взгляд";
            fallback.choices = new List<EventChoice>
            {
                new EventChoice { buttonText = "Уйти", outcome = EventOutcome.Exit, isRandom = false }
            };
            return fallback;
        }

        int index = Random.Range(0, valid.Count);
        return valid[index];
    }
}