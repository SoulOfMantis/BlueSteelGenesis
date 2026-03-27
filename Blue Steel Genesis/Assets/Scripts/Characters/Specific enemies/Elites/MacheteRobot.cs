using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MacheteRobot : Enemy
{
    public MacheteRobot() : base(32, 4, 64)
    {
        addModule(new LongAttack());
        addModule(new WideAttack());
        addModule(new BasicMovement());
        Name = "Saber Robot";
        Description = "A mad robot with long machete.";
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

    protected override IEnumerable<Entity> getEnemies()
    {
        return base.getEnemies().Concat(tracker.Entities.Where(e => e is Bush));
    }

}
