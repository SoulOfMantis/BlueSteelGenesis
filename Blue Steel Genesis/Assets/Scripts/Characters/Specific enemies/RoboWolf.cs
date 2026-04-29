using System.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using System.Linq;

public class RoboWolf : Enemy
{
    public RoboWolf() : base(32, 4, 32)
    {
        Name = "RoboWolf";
        Description = "A robotic copy of regular wolf. Got some pretty cool enhancements!";
        addModule(new BurnBite());
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
    protected override bool TryGetTargetForOne(out Vector3Int targetPos) =>
        GetDirectApproachTarget(out targetPos, priorityModules[1], getEnemies()) ||
        GetApproachTarget(out targetPos, priorityModules[1], getEnemies());
}