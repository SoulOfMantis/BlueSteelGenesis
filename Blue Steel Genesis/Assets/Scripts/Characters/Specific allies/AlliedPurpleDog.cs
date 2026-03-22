using System.Threading.Tasks;
using System;
using UnityEngine;
using System.Linq;

public class AlliedPurpleDog : Ally
{
    public AlliedPurpleDog() : base(5, 3, 61)
    {
        Name = "Allied Purple Dog";
        Description = "The first ally. Will move closer to your enemies and bite, if it has an opportunity!";
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
    public override async Task damage(uint dmg)
    {
        Debug.Log($"Собака получила {dmg} урона!");
        await base.damage(dmg);
        //play taking damage animation
    }

    public override async Task heal(uint hp)
    {
        Debug.Log($"Собака восстановила {hp} здоровья!");
        await base.heal(hp);
        //play healing animation
    }

    public override async Task startBattle()
    {
        await base.startBattle();
    }
}