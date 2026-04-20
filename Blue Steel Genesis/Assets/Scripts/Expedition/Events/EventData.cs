using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEvent", menuName = "Game/Event Data")]
public class EventData : ScriptableObject
{
    public string eventName;
    public string sceneName;          // им€ сцены, которую нужно загрузить
    public List<uint> allowedStages; // пустой список,тогда любой этап
    public Sprite eventImage;
    [TextArea(3, 5)]
    public string eventDescription;
    public List<uint> allowedBiomes;
    public List<EventChoice> choices;

    [Tooltip("—писок состо€ний событи€. ѕервое состо€ние Ц начальное.")]
    public List<EventState> states;

    [System.Serializable]
    public class EventState
    {
        public string stateId;              // уникальный идентификатор состо€ни€
        [TextArea(3, 5)] public string description;
        public List<EventChoice> choices;
    }
}