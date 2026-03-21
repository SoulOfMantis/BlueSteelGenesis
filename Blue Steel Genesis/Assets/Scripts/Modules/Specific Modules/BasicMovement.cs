using System.Collections.Generic;
using System.Linq;
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
        changeName("BasicMovement");
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
    public override List<Vector3Int> getCellsInRange(PositionCollection start) =>
        Navigation.Dijkstra.listReachable(start, p => !Entity.tracker.OutOfBounds(p) && !Entity.tracker.IsOccupied(p), range).Except(start).ToList();
}
