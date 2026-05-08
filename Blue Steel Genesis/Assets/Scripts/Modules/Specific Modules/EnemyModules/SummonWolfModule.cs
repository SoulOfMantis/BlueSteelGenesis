using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


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

    public override async Task Effect(Character user, Vector3Int pos, ActionContext ctx)
    {
        await user.summon<Wolf>(new PositionCollection(pos, 1));
        await user.visualHandler.PlaySummonAnimation();
    }

    protected override bool checkFinalPosition(Vector3Int pos)
    {
        return !Entity.tracker.OutOfBounds(pos) && !Entity.tracker.IsOccupied(pos);
    }
}