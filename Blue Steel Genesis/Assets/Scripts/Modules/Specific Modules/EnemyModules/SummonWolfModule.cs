using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


/// <summary>
/// Ìîäóëü ïðèçûâà âîëêîâ äëÿ âîæàêà.
/// </summary>
public class SummonWolfModule : ActiveModule
{
    public SummonWolfModule()
    {
        Name = "Summon Wolf";
        energyCost = 5;
        range = 1;              
        Icon_name = "SummonWolfModule";
    }

    public override string Description() =>
        "Summons a Wolf on an adjacent free cell.\n" + base.Description();

    public override Task Effect(Character user, Vector3Int pos)
    {
        Entity.summon<Wolf>(new PositionCollection(pos, 1));
        return Task.CompletedTask;
    }

    protected override bool checkFinalPosition(Vector3Int pos)
    {
        return !Entity.tracker.OutOfBounds(pos) && !Entity.tracker.IsOccupied(pos);
    }
}