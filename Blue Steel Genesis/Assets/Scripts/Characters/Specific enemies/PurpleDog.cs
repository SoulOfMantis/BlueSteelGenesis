using System.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using System.Linq;

public class PurpleDog : Enemy
{
    public PurpleDog() : base(5, 3, 60)
    {
        Name = "Purple Dog";
        Description = "The first enemy. Will move closer to you and bite, if it has an opportunity!";
        addModule(new BasicAttack());
        addModule(new BasicMovement());

        SetPriorityModules();
    }
    protected override bool TryGetTargetForZero(out Vector3Int targetPos)
    {
        var possibleTargets = priorityModules[0].getCellsInRange(Position)
            .Where(p => tracker.getPlayer().Position.Contains(p));
        targetPos = possibleTargets.FirstOrDefault();
        return possibleTargets.Count() != 0;
    }
    protected override bool TryGetTargetForOne(out Vector3Int targetPos)
    {
        targetPos = Position.LeftBottom;
        var moveRange = priorityModules[1].getCellsInRange(Position).Concat(Position).ToHashSet();
        var path = Navigation.Dijkstra.getPath(Position, tracker.getPlayer().Position.NeighborPositions(),
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