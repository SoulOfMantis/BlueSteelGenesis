using System.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using System.Linq;

public class CyberLeader : Enemy
{
    public CyberLeader() : base(32, 8, 64)
    {
        Name = "CyberLeader";
        Description = "Came back to life with the power of technology. And is now ready to take revenge! " +
            "His minions have also been improved!";
    }

    protected override void Init()
    {
        addModule(new BurnBite());
        addModule(new AcceleratedMovement());
        addModule(new RoboWolfSummoner());
        SetPriorityModules();

        base.Init();
    }

    protected override bool TryGetTargetForZero(out Vector3Int targetPos)
    {
        var possibleTargets = priorityModules[0].getCellsInRange(Position)
            .Where(p => getEnemies().SelectMany(e => e.Position).Contains(p));
        targetPos = possibleTargets.FirstOrDefault();
        return possibleTargets.Count() != 0;
    }

    protected override bool TryGetTargetForOne(out Vector3Int targetPos) =>
        GetDirectApproachTarget(out targetPos, priorityModules[1], getEnemies());

    protected override bool TryGetTargetForTwo(out Vector3Int targetPos)
    {
        var possibleTargets = priorityModules[2].getCellsInRange(Position)
            .Where(p => !Entity.tracker.IsOccupied(p));
        targetPos = possibleTargets.FirstOrDefault();
        return possibleTargets.Count() != 0;
    }

}