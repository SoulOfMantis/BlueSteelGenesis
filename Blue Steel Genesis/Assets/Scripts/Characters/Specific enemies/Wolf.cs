using System.Linq;
using UnityEngine;
public class Wolf : Enemy
{
    public Wolf() : base(8, 3, 20)
    {
        Name = "Wolf";
        Description = "A fierce wolf, loyal to its leader.";
    }

    protected override void Init()
    {
        addModule(new BasicAttackModule());
        addModule(new BasicMovementModule());
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
        GetDirectApproachTarget(out targetPos, priorityModules[1] as BasicMovementModule, getEnemies()) ||
        GetApproachTarget(out targetPos, priorityModules[1] as BasicMovementModule, getEnemies());
}

