using System.Linq;
using UnityEngine;
public class Wolf : Enemy
{
    public Wolf() : base(12, 2, 20)
    {
        Name = "Wolf";
        Description = "A fierce wolf, loyal to its leader.";

        addModule(new BasicAttack());
        addModule(new BasicMovement());

        SetPriorityModules();
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
        var path = Navigation.Dijkstra.getPath(Position, getEnemies().SelectMany(e => e.Position.NeighborPositions()),
            p => !tracker.IsOccupied(p) && !tracker.OutOfBounds(p)) ?? new();
        foreach (var move in path)
            if (moveRange.Contains(targetPos + move))
                targetPos += move;
            else break;
        return targetPos != Position.LeftBottom;
    }
}

