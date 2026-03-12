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
    public int PlayerMoney
    {
        get => playerMoney;
        protected set => playerMoney = Math.Max(value, 0);
    }
    private int playerMoney = 0;
    public int PlayerMaterials
    {
        get => playerMaterials;
        protected set => playerMaterials = Math.Max(value, 0);
    }
    private int playerMaterials = 0;

    public void GiveMoney(int value) => PlayerMoney += value;
    public void LoseMoney(int value) => PlayerMoney -= value;
    public bool HasEnoughMoney(int value) => PlayerMoney >= value;

    public void GiveMaterials(int value) => PlayerMaterials += value;
    public void LoseMaterials(int value) => PlayerMaterials -= value;
    public bool HasEnoughMaterials(int value) => PlayerMaterials >= value;

    public int currentHealth
    {
        get => current_health_;
        set => current_health_ = Math.Clamp(value, 0, maxHealth);
    }
    private int current_health_;


    public int maxHealth { get; set; }
    public int maxEnergy { get; set; }

    public List<GameModule> modules = new();

    public void AddModule(GameModule module)
    {
        if (modules.Count > 5) modules.RemoveRange(5, modules.Count - 5);
        //TODO: give player choice of which to throw away
        if (modules.Count == 5)
            modules[4] = module;
        else modules.Add(module);
    }
    public bool RemoveModule(GameModule module)
    {
        return modules.Remove(module);
    }
}
