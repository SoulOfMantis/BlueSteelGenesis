using System;
using UnityEngine;

[Serializable]
public class EventChoice
{
    public string buttonText;
    [TextArea]
    public string description;
    public bool isRandom;
    [Range(0, 100)]
    public int successChance = 50;
    public EventEffect successEffect;
    public EventEffect failureEffect;

    [Tooltip("0 Ц выход из событи€, >0 Ц идентификатор следующего состо€ни€")]
    public uint nextStateId;
}