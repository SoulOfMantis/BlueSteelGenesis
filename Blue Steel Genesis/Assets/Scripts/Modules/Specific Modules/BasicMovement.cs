using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


/// <summary>
/// Базовый модуль движения (BMM сокращение)
/// </summary>
public class BasicMovement : ActiveModule
{
    public BasicMovement() : base()
    {
        range = 3;
        energyCost = 1;
        //AddKeywords(new CommonKeyword(), new MobilityKeyword());
        Icon_name = "Module_movement";
    }
    public override string Description()
    {
        return $"Move to an unoccupied space within {range} cells. Can't jump over creatures or obstacles.";
    }
    public override async Task Effect(Character user, Vector3Int pos)
    {
        await user.move(pos, getCellsInRange(user.Position));
        Debug.Log("BMM executed");
    }
    protected override bool checkIntermediatePosition(Vector3Int pos)
    {
        return base.checkIntermediatePosition(pos) && !Character.tracker.IsOccupied(pos);
    }
    protected override bool checkFinalPosition(Vector3Int pos)
    {
        return !Character.tracker.IsOccupied(pos);
    }

}
