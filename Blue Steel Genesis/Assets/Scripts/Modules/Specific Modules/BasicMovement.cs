using System.Threading.Tasks;
using UnityEngine;


/// <summary>
/// Базовый модуль движения (BMM сокращение)
/// </summary>
public class BasicMovement : ActiveModule
{
    public BasicMovement()
    {
        range = 3;
        energyCost = 1;
        Name = "BasicMovement";
    }
    public override string Description()
    {
        return $"Basic movement: move to an unoccupied space within {range} cells. Can't jump over creatures or obstacles.";
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
