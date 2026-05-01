using System.Linq;
using UnityEngine;

public class SmallAcidSlime : Enemy
{
    public SmallAcidSlime() : base(5, 2, 30)
    {
        Name = "Small Acid Slime";
        Description = "Pathetic remains of slime's glory. Can't even shoot anymore.";
    }
    protected override void Init()
    {
        addModule(new AcidBite());
        addModule(new BasicMovement(4));
        base.Init();
    }
    protected override bool TryGetTargetForZero(out Vector3Int targetPos)
    {
        var possibleTargets = priorityModules[0].getCellsInRange(Position).Where(p => getEnemies().SelectMany(e => e.Position).Contains(p));
        targetPos = possibleTargets.FirstOrDefault();
        return possibleTargets.Count() != 0;
    }
    protected override bool TryGetTargetForOne(out Vector3Int targetPos) =>
        GetDirectApproachTarget(out targetPos, priorityModules[1], getEnemies()) ||
        GetApproachTarget(out targetPos, priorityModules[1], getEnemies());

}

