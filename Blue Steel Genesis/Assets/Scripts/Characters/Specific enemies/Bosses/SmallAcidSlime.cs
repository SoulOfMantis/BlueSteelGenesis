using System.Linq;
using UnityEngine;

public class SmallAcidSlime : Enemy
{
    public SmallAcidSlime() : base(3, 2, 15)
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
    protected override bool TryGetTargetForOne(out Vector3Int targetPos)
    {
        targetPos = Position.LeftBottom;
        var moveRange = priorityModules[1].getCellsInRange(Position).Concat(Position).ToHashSet();
        var path = Navigation.Dijkstra.getPath(Position, getEnemies().SelectMany(e => e.Position.NeighborPositions()),
            p => !tracker.IsOccupied(p) && !tracker.OutOfBounds(p)) ?? new();

        Vector3Int offset = new();
        foreach (var move in path)
            if ((Position + offset + move).All(p => moveRange.Contains(p)))
                offset += move;
            else break;
        targetPos = Position.LeftBottom + offset;
        return offset != Vector3Int.zero;
    }

}

