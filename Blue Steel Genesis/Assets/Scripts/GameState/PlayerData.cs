using System;
using System.Collections.Generic;

public class PlayerData
{
    public URangeValue currentHealth { get; set; } = new();


    public uint maxHealth {
        get => currentHealth.Max;
        set => currentHealth.Max = value;
    }
    public uint maxEnergy { get; set; }


    public URangeValue money { get; set; } = new();
    public URangeValue materials { get; set; } = new();


    public List<GameModule> modules = new();
}
