using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


/// <summary>
/// ������� ������ �������� (BMM ����������)
/// </summary>
public class BasicMovement : ActiveModule
{
    public BasicMovement() : base()
    {
        range = 3;
        energyCost = 1;
        Icon_name = "Module_movement";
        AddConstKeywords(new MobilityKeyword(), new CommonKeyword());
    }
    public BasicMovement(uint speed) : this()
    {
        range = speed;
    }
    public override string Description()
    {
        return $"Move to an unoccupied space within {range} cells.\n" + base.Description();
    }
    public override async Task Effect(Character user, Vector3Int pos)
    {
        await user.move(pos, getCellsInRange(user.Position));
        Debug.Log("BMM executed");
    }
    public override List<Vector3Int> getCellsInRange(PositionCollection start) =>
        Navigation.Dijkstra.listReachable(start, p => !Entity.tracker.OutOfBounds(p) && !Entity.tracker.IsOccupied(p), range).ToList();
}
