using System.Threading.Tasks;
using System;
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
            .Where(p => getEnemies().SelectMany(e => e.Position).Contains(p));
        targetPos = possibleTargets.FirstOrDefault();
        return possibleTargets.Count() != 0;
    }
    protected override bool TryGetTargetForOne(out Vector3Int targetPos) =>
        GetDirectApproachTarget(out targetPos, priorityModules[1], getEnemies()) ||
        GetApproachTarget(out targetPos, priorityModules[1], getEnemies());

    public override async Task damage(uint dmg, ActionContext prevAction = null)
    {
        Debug.Log($"Собака получила {dmg} урона!");
        await base.damage(dmg, prevAction);
        //play taking damage animation
    }

    public override async Task heal(uint hp, ActionContext prevAction = null)
    {
        Debug.Log($"Собака восстановила {hp} здоровья!");
        await base.heal(hp, prevAction);
        //play healing animation
    }

    public override async Task startBattle()
    {
        await base.startBattle();
    }
}