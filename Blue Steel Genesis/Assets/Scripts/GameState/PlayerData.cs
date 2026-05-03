using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class PlayerData : ISerializationCallbackReceiver
{
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



    public void OnBeforeSerialize() {
        modules_serializable_ = modules.Select(m => new GameModuleSerializable(m)).ToList();
    }
    public void OnAfterDeserialize() {
        modules = modules_serializable_.Select(m => m.create()).ToList();
    }
    [SerializeField]
    private List<GameModuleSerializable> modules_serializable_;



    [field: SerializeField]
    public URangeValue currentHealth { get; set; } = new();
    public uint maxHealth {
        get => currentHealth.Max;
        set => currentHealth.Max = value;
    }

    [field: SerializeField]
    public uint maxEnergy { get; set; }

    [field: SerializeField]
    public URangeValue money { get; set; } = new();

    [field: SerializeField]
    public URangeValue materials { get; set; } = new();

    [field: SerializeField]
    public uint GoldenTickets { get; private set; }

    public List<GameModule> modules = new();
}
