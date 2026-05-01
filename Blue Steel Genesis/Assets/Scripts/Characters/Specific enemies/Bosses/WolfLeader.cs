using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
public class WolfLeader : Enemy
{
    public WolfLeader() : base(40, 5, 70)
    {
        Name = "Wolf Leader";
        Description = "The mighty leader of the wolf pack. Can summon wolves to aid him!";

        addModule(new BiteModule());
        addModule(new ClawModule());
        addModule(new SummonWolfModule());
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
        var possibleTargets = priorityModules[0].getCellsInRange(Position)
            .Where(p => getEnemies().SelectMany(e => e.Position).Contains(p));
        targetPos = possibleTargets.FirstOrDefault();
        return possibleTargets.Count() != 0;
    }

    protected override bool TryGetTargetForTwo(out Vector3Int targetPos) =>
        GetDirectApproachTarget(out targetPos, priorityModules[2], getEnemies()) ||
        GetApproachTarget(out targetPos, priorityModules[2], getEnemies());

   
    protected override bool TryGetTargetForThree(out Vector3Int targetPos)
    {
        var summonRangeCells = priorityModules[3].getCellsInRange(Position);
        var freeCells = summonRangeCells.Where(cell => !tracker.IsOccupied(cell) && !tracker.OutOfBounds(cell)).ToList();
        if (freeCells.Any())
        {
            targetPos = freeCells.First();
            return true;
        }
        targetPos = default;
        return false;
    }
}

