using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class FlightMovement : ActiveModule
{
    public FlightMovement() : base()
    {
        price = 50;
        range = 2;
        maxUpgradeLevel = 1;
        energyCost = 1;
        AddConstKeywords(new MobilityKeyword(), new FlightKeyword(), new CommonKeyword());
    }
    public FlightMovement(uint range) : this()
    {
        this.range = range;
    }
    public override string Description() {
        return $"Move to an unoccupied space within {range} cells.\n" + base.Description();
    }

    public override async Task Effect(Character user, Vector3Int pos, ActionContext ctx) {
        await user.move(
            new PositionCollection(pos, user.Position.SideSize),
            Navigation.Dijkstra.listReachable(
                user.Position,
                p => !Entity.tracker.OutOfBounds(p) && !Entity.tracker.IsOccupiedByCharacter(p),
                range
            ).ToList(),
            MakeContext(user, pos));
    }

    protected override bool checkIntermediatePosition(Vector3Int pos) =>
        !Entity.tracker.OutOfBounds(pos) && !Entity.tracker.IsOccupiedByCharacter(pos);
    protected override bool checkFinalPosition(Vector3Int pos) =>
        !Entity.tracker.IsOccupied(pos);
    public override void ApplyUpgrade()
    {
        base.ApplyUpgrade();
        range += 1;
        price += 50;
    }

}
