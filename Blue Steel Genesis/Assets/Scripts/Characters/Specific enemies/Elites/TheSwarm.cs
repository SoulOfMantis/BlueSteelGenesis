using System.Linq;
using UnityEngine;

public class TheSwarm : Enemy
{
    public TheSwarm() : base(20, 3, 100) {
        Name = "The Swarm";
        Description = "A cloud of aggressive insects. These minuscule creatures are not what your metal hands were made for!";
        addModule(new SwarmMandibles(1, 3));
        addModule(new FlightMovement(4));
        addModule(new SwarmScatter());
    }

    protected override bool TryGetTargetForZero(out Vector3Int targetPos)
    {
        var possibleTargets = priorityModules[0].getCellsInRange(Position)
            .Where(p => getEnemies().SelectMany(e => e.Position).Contains(p));
        targetPos = possibleTargets.FirstOrDefault();
        return possibleTargets.Count() != 0;
    }
    protected override bool TryGetTargetForOne(out Vector3Int targetPos)
    {
        targetPos = Position.LeftBottom;
        var moveRange = priorityModules[1].getCellsInRange(Position).Concat(Position).ToHashSet();
        Vector3Int closestTarget = getEnemies()
            .SelectMany(e => e.Position.NeighborPositions())
            .Where(p => !tracker.IsOccupied(p))
            .MinBy(p => p.ManhattanDistance(Position.LeftBottom));
        targetPos = moveRange
            .Where(p => !tracker.IsOccupied(p))
            .DefaultIfEmpty(targetPos)
            .MinBy(p => p.ManhattanDistance(closestTarget));
        return targetPos != Position.LeftBottom;
    }
}
