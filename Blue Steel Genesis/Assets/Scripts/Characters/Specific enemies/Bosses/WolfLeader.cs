using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
public class WolfLeader : Enemy
{
    private int remainingSummons = 2; // Сколько раз можно использовать призыв (каждый раз пытается призвать  до 2 волков)

    public WolfLeader() : base(40, 5, 70) 
    {
        Name = "Wolf Leader";
        Description = "The mighty leader of the wolf pack. Can summon smaller wolves to aid him!";


        addModule(new BiteModule());         
        addModule(new ClawModule());
        addModule(new BasicMovement());        
        addModule(new SummonWolfModule(this)); 

        SetPriorityModules(); 
    }

 
    public void UseSummon()
    {
        remainingSummons--;
    }

    public bool HasSummonsLeft() => remainingSummons > 0;

    protected override bool TryGetTargetForZero(out Vector3Int targetPos)
    {
        var possibleTargets = priorityModules[0].getCellsInRange(Position)
            .Where(p => getEnemies().SelectMany(e => e.Position).Contains(p));
        targetPos = possibleTargets.FirstOrDefault();
        return possibleTargets.Count() != 0;
    }

    protected override bool TryGetTargetForOne(out Vector3Int targetPos)
    {
        var possibleTargets = priorityModules[0].getCellsInRange(Position)
            .Where(p => getEnemies().SelectMany(e => e.Position).Contains(p));
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

   
    protected override bool TryGetTargetForThree(out Vector3Int targetPos)
    {
        var summonRangeCells = priorityModules[3].getCellsInRange(Position);
        var freeCells = summonRangeCells.Where(cell => !tracker.IsOccupied(cell) && !tracker.OutOfBounds(cell)).ToList();
        if (freeCells.Any())
        {
            targetPos = freeCells.First();
            return true;
        }
        targetPos = default;
        return false;
    }
}

