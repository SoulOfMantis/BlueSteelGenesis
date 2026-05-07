using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


/// <summary>
/// Модуль призыва волков для вожака.
/// </summary>
public class SummonWolfModule : ActiveModule
{
    public SummonWolfModule() : base()
    {
        Name = "Summon Wolves";
        energyCost = 2;      
        range = 2;
        Icon_name = "SummonWolfModule";
    }
    public override string Description()
    {
        return "Summons 2 wolves.\n" + base.Description();
    }

    public override async Task Effect(Character user, Vector3Int pos)
    {
        var freeCells = getCellsInRange(user).Where(cell => !Character.tracker.IsOccupied(cell) && !Character.tracker.OutOfBounds(cell)).ToList();

        int summonedCount = 0;
        foreach (var cell in freeCells)
        {
            if (summonedCount >= 2) break;
            var wolfPos = new PositionCollection(cell, 1);
            if (Entity.summon<Wolf>(wolfPos))
                summonedCount++;
        }
        await user.visualHandler.PlaySummonAnimation();

        if (summonedCount > 0)
            Debug.Log($"{user.Name} summoned {summonedCount} wolf(s)!");
        else
            Debug.LogWarning("No free cells to summon wolves!");
    }
}