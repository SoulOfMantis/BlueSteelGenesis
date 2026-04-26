using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEvent", menuName = "Game/Event Data")]
public class EventData : ScriptableObject
{
    public string eventName;
    public Sprite eventImage;
    [TextArea(3, 5)]
    public string eventDescription;
    public List<uint> allowedBiomes;    // пустой список Ц доступно во всех биомах
    public List<uint> allowedStages;    // пустой список Ц доступно на всех этапах

    [Tooltip("—осто€ни€ событи€. ѕервое состо€ние должно иметь stateId = 1.")]
    public List<EventState> states;

    [System.Serializable]
    public class EventState
    {
        public uint stateId;           
        [TextArea(3, 5)] public string description;
        public List<EventChoice> choices;
    }
}