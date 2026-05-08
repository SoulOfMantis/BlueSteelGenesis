using System.Linq;
using UnityEngine;

public class LivingAcid : Enemy
{
    public LivingAcid() : base(50, 3, -10) 
    {
        Name = "the Living Acid";
        Description = "The biggest slime there ever was. Will bite. Will shoot. Will... EXPLODE!";
    }
    protected override void Init()
    {
        addModule(new AcidBiteModule());
        addModule(new AcidShotModule());
        addModule(new BasicMovementModule(2));
        addModule(new ExplodeWithSlimeModule(bodySize - 1));
        base.Init();
    }
    protected override bool TryGetTargetForZero(out Vector3Int targetPos)
    {
        var possibleTargets = priorityModules[0].getCellsInRange(Position).Where(p => getEnemies().SelectMany(e => e.Position).Contains(p));
        targetPos = possibleTargets.FirstOrDefault();
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


}

