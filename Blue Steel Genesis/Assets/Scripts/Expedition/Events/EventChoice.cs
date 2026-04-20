using UnityEngine;

using System;

[Serializable]
public class EventChoice
{
    public string buttonText;       
    [TextArea]
    public string description;      
    public bool isRandom;            
    [Range(0, 100)]
    public int successChance = 50;  
    public EventEffect successEffect; // эффект при успехе
    public EventEffect failureEffect; // эффект при неудаче
    public EventOutcome outcome;     // что произойдёт после выбора
    [Tooltip("Если не пусто, событие перейдёт в указанное состояние вместо завершения")]
    public string nextStateId;   // идентификатор состояния 
}