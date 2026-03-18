// EventManager.cs
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EventManager
{
    private static List<EventData> allEvents = new List<EventData>();


    public static void LoadAllEvents()
    {
        allEvents = Resources.LoadAll<EventData>("Events").ToList();

    }

    /// <summary>
    /// Возвращает случайное событие, подходящее для указанного биома
    /// </summary>
    public static EventData GetRandomEventForBiome(uint biomeId, int seed)
    {
        var valid = allEvents.Where(e => e.allowedBiomes.Count == 0 || e.allowedBiomes.Contains(biomeId)).ToList();
        if (valid.Count == 0)
        {
            Debug.LogWarning($"Нет событий для биома {biomeId}");
            EventData fallback = ScriptableObject.CreateInstance<EventData>();
            fallback.eventName = "Странная аномалия";
            fallback.sceneName = "Event_Default";
            return fallback;
        }

        System.Random prng = new System.Random(seed);
        int index = prng.Next(valid.Count);
        return valid[index];
    }
}