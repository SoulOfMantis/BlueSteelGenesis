using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MacheteRobot : Enemy
{
    public MacheteRobot() : base(50, 5, 40)
    {
        Name = "Saber Robot";
        Description = "A mad robot with long machete.";
    }

    protected override void Init()
    {
        addModule(new LongAttack());
        addModule(new WideAttack());
        addModule(new BasicMovement());
        SetPriorityModules();

        base.Init();
    }

    protected override bool TryGetTargetForZero(out Vector3Int targetPos)
    {
        var possibleTargets = getEnemies().SelectMany(x => x.Position).Where(p => Position.Any(pp => pp.x == p.x || pp.y == p.y));
        var enemyPosition = possibleTargets.FirstOrDefault();

        var ourAttackPosition = Position.Where(x => x.x == enemyPosition.x || x.y == enemyPosition.y)
            .MinBy(x => x.ManhattanDistance(enemyPosition));
        var direction = enemyPosition - ourAttackPosition;
        direction.Clamp(new(-1, -1), new(1, 1));
        targetPos = ourAttackPosition + direction;

        return possibleTargets.Count() != 0;
    }
    protected override bool TryGetTargetForOne(out Vector3Int targetPos)
    {
        var possibleTargets = priorityModules[1].getCellsInRange(Position).Where(p => getEnemies().SelectMany(e => e.Position).Contains(p));
        targetPos = possibleTargets.FirstOrDefault();
        return possibleTargets.Count() != 0;
    }
    protected override bool TryGetTargetForTwo(out Vector3Int targetPos) =>
        GetDirectApproachTarget(out targetPos, priorityModules[2], getEnemies()) ||
        GetApproachTarget(out targetPos, priorityModules[2], getEnemies());

    protected override IEnumerable<Entity> getEnemies()
    {
        return base.getEnemies().Concat(tracker.Entities.Where(e => e is Bush));
    }

}
