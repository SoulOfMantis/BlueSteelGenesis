using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MacheteRobot : Enemy
{
    public MacheteRobot() : base(35, 5, 40)
    {
        Name = "Machete Robot";
        Description = "A mad robot with long machete.";
    }

    protected override void Init()
    {
        addModule(new WideAttack());
        addModule(new LongAttack());
        addModule(new BasicMovement());
        SetPriorityModules();

        base.Init();
    }
    int possibleTargetsWide(out Vector3Int targetPos)
    {
        var possibleTargets = priorityModules[0].getCellsInRange(Position).Where(p => getEnemies().SelectMany(e => e.Position).Contains(p));
        targetPos = possibleTargets.FirstOrDefault();
        return possibleTargets.Count();
    }

    int possibleTargetsLong(out Vector3Int targetPos)
    {
        var possibleTargets = getEnemies().SelectMany(x => x.Position).Where(p => Position.Any(pp => pp.x == p.x || pp.y == p.y));
        var enemyPosition = possibleTargets.FirstOrDefault();

        var ourAttackPosition = Position.Where(x => x.x == enemyPosition.x || x.y == enemyPosition.y)
            .MinBy(x => x.ManhattanDistance(enemyPosition));
        var direction = enemyPosition - ourAttackPosition;
        direction.Clamp(new(-1, -1), new(1, 1));
        targetPos = ourAttackPosition + direction;
        return possibleTargets.Count();
    }

    protected override bool TryGetTargetForZero(out Vector3Int targetPos)
    {
        int Long = possibleTargetsLong(out targetPos), Wide = possibleTargetsWide(out targetPos);
        return Wide >= Long && Wide > 0;
    }
    protected override bool TryGetTargetForOne(out Vector3Int targetPos)
    {
        int Wide = possibleTargetsWide(out targetPos), Long = possibleTargetsLong(out targetPos);
        return Long >= Wide && Long > 0;
    }
    protected override bool TryGetTargetForTwo(out Vector3Int targetPos) =>
        GetDirectApproachTarget(out targetPos, priorityModules[2], getEnemies()) ||
        GetApproachTarget(out targetPos, priorityModules[2], getEnemies());

    protected override IEnumerable<Entity> getEnemies()
    {
        return base.getEnemies().Concat(tracker.Entities.Where(e => e is Bush));
    }

}
