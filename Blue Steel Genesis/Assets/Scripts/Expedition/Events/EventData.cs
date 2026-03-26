using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEvent", menuName = "Game/Event Data")]
public class EventData : ScriptableObject
{
    public string eventName;
    public string sceneName;          // имя сцены, которую нужно загрузить
    public Sprite eventImage;
    [TextArea(3, 5)]
    public string eventDescription;
    public List<uint> allowedBiomes;
    public List<EventChoice> choices;
}