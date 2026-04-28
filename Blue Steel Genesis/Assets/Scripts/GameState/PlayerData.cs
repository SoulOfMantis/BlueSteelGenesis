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

    public void GiveMoney(uint value) => money.Value += value;
    public void LoseMoney(uint value) => money.Value -= value;
    public bool HasEnoughMoney(uint value) => money.Value >= value;

    public void GiveMaterials(uint value) => materials.Value += value;
    public void LoseMaterials(uint value) => materials.Value -= value;
    public bool HasEnoughMaterials(uint value) => materials.Value >= value;

    public URangeValue currentHealth { get; set; } = new();
    public uint maxHealth
    {
        get => currentHealth.Max;
        set => currentHealth.Max = value;
    }
    public uint maxEnergy { get; set; }

    public URangeValue money { get; set; } = new();
    public URangeValue materials { get; set; } = new();
    public URangeValue currentEnergy { get; set; } = new();

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

    /// <summary>
    /// Применяет все поля EventEffect к текущему игроку
    /// </summary>
    public void ApplyEventEffects(EventEffect effect)
    {
        if (effect == null) return;

        long newHealth = (long)currentHealth.Value + effect.healthChange;
        currentHealth.Value = (uint)Math.Max(0, Math.Min(newHealth, maxHealth));
        maxHealth = (uint)Math.Max(1, (long)maxHealth + effect.maxHealthChange);

       
        //long newEnergy = (long)currentEnergy.Value + effect.energyChange;
        //currentEnergy.Value = (uint)Math.Max(0, Math.Min(newEnergy, maxEnergy));
        maxEnergy = (uint)Math.Max(0, (long)maxEnergy + effect.maxEnergyChange);

        if (effect.moneyChange > 0)
            GiveMoney((uint)effect.moneyChange);
        else if (effect.moneyChange < 0)
            LoseMoney((uint)(-effect.moneyChange));
       
        if (effect.materialChange > 0)
            GiveMaterials((uint)effect.materialChange);
        else if (effect.materialChange < 0)
            LoseMaterials((uint)(-effect.materialChange));

        if (effect.addModules != null)
            foreach (var mod in effect.addModules)
                AddModule(mod);

        if (effect.removeModuleIds != null)
            modules.RemoveAll(m => effect.removeModuleIds.Contains(m.Name));
    }
}