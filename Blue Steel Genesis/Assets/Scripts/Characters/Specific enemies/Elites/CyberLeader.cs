using System.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using System.Linq;

public class CyberLeader : Enemy
{
    public CyberLeader() : base(16, 8, 64)
    {
        Name = "CyberLeader";
        Description = "Came back to life with the power of technology. And is now ready to take revenge! " +
            "His minions have also been improved!";
        addModule(new BurnBite());
        addModule(new RoboWolfSummoner());
        addModule(new AcceleratedMovement());

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
        var possibleTargets = priorityModules[1].getCellsInRange(Position)
            .Where(p => !Entity.tracker.IsOccupied(p));
        targetPos = possibleTargets.FirstOrDefault();
        return possibleTargets.Count() != 0;
    }


    protected override bool TryGetTargetForTwo(out Vector3Int targetPos)
    {
        targetPos = Position.LeftBottom;
        var moveRange = priorityModules[2].getCellsInRange(Position).Concat(Position).ToHashSet();
        var path = Navigation.Dijkstra.getPath(Position, getEnemies().SelectMany(e => e.Position.NeighborPositions()),
            p => !tracker.IsOccupied(p) && !tracker.OutOfBounds(p)) ?? new();
        foreach (var move in path)
            if (moveRange.Contains(targetPos + move))
                targetPos += move;
            else break;
        return targetPos != Position.LeftBottom;
    }



    
}