using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEvent", menuName = "Game/Event Data")]
public class EventData : ScriptableObject
{
    public string eventName;        // отображаемое название
    public string sceneName;         // имя сцены, которую надо загрузить
    public List<uint> allowedBiomes; // ID биомов, где может встретиться (пусто = везде)
}