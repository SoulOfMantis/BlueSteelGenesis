using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEvent", menuName = "Game/Event Data")]
public class EventData : ScriptableObject
{
    public string eventName;
    public string narrativeText;
    public List<EventChoice> choices;
    public List<uint> allowedBiomes;

    [System.Serializable]
    public class EventChoice
    {
        public string buttonText;
        public string effectDescription;
        public EventOutcome outcome;
        public int moneyChange;
        public int materialChange;
        public int healthChange;
    }
}
