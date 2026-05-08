using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


/// <summary>
/// ������� ������ �������� (BMM ����������)
/// </summary>
public class BasicMovementModule : ActiveModule
{
    public BasicMovementModule() : base()
    {
        price = 5;
        range = 3;
        energyCost = 1;
        Icon_name = "BasicMovementModule";
        maxUpgradeLevel = 3;
        AddConstKeywords(new MobilityKeyword(), new CommonKeyword());
    }
    public BasicMovementModule(uint speed) : this()
    {
        range = speed;
    }
    public override string Description()
    {
        return $"Move to an unoccupied space within {range} cells.\n" + base.Description();
    }
    public override async Task Effect(Character user, Vector3Int pos, ActionContext ctx)
    {
        await user.move(new PositionCollection(pos, user.Position.SideSize), getCellsInRange(user.Position), MakeContext(user, pos));
        Debug.Log("BMM executed");
    }
    public override List<Vector3Int> getCellsInRange(PositionCollection start) =>
        Navigation.Dijkstra.listReachable(start, p => !Entity.tracker.OutOfBounds(p) && !Entity.tracker.IsOccupied(p), range).ToList();

    public override void ApplyUpgrade()
    {
        base.ApplyUpgrade();
        range += 1;
        price += 15;
    }

}
