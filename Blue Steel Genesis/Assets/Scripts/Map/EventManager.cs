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
            Debug.LogWarning($"Нет событий для биома {biomeId}, используется дефолтное событие");
            EventData fallback = Resources.Load<EventData>("Events/DefaultEvent");
            if (fallback == null)
            {
                Debug.LogError("DefaultEvent не найден! Создайте его в Resources/Events");
                fallback = ScriptableObject.CreateInstance<EventData>();
                fallback.eventName = "Странная штуковина";
                fallback.narrativeText = "Вы не нашли ничего интересного";
                fallback.choices = new List<EventData.EventChoice>
            {
                new EventData.EventChoice
                {
                    buttonText = "Уйти",
                    effectDescription = "Ничего не происходит",
                    outcome = EventOutcome.Exit
                }
            };
            }
            return fallback;
        }

        System.Random prng = new System.Random(seed);
        int index = prng.Next(valid.Count);
        return valid[index];
    }
}