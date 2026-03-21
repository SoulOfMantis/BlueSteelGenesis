using System;
using System.Collections.Generic;

public class PlayerData
{
    public uint GoldenTickets { get; private set; }
    public bool HasGoldenTickets() => GoldenTickets > 0;
    public void GetGoldenTicket() => GoldenTickets++;
    public void SpendGoldenTicket()
    {
        if (GoldenTickets >= 1) GoldenTickets--;
    }

    public void GiveMoney(uint value) => money += value;
    public void LoseMoney(uint value) => money -= value;
    public bool HasEnoughMoney(uint value) => money.Value >= value;

    public void GiveMaterials(uint value) => materials += value;
    public void LoseMaterials(uint value) => materials -= value;
    public bool HasEnoughMaterials(uint value) => materials.Value >= value;
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
