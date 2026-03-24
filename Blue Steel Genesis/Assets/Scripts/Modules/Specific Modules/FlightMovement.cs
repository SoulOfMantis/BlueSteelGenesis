using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class FlightMovement : ActiveModule
{
    public FlightMovement(uint range = 3) {
        this.range = range;
        energyCost = 1;
        AddConstKeywords(new MobilityKeyword());
    }
    public override string Description() {
        return $"Fly to an unoccupied space within {range} cells.\n" + base.Description();
    }

    public override async Task Effect(Character user, Vector3Int pos) {
        await user.move(pos,
            Navigation.Dijkstra.listReachable(
                user.Position,
                p => !Entity.tracker.OutOfBounds(p) && !Entity.tracker.IsOccupiedByCharacter(p),
                range
            ).ToList());
    }

    protected override bool checkIntermediatePosition(Vector3Int pos) =>
        !Entity.tracker.OutOfBounds(pos) && !Entity.tracker.IsOccupiedByCharacter(pos);
    protected override bool checkFinalPosition(Vector3Int pos) =>
        !Entity.tracker.IsOccupied(pos);
}
