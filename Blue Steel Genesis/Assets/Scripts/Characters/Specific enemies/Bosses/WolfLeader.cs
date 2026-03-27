using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public class WolfLeader : Enemy
{
    private int remainingSummons = 2; // —колько раз можно использовать призыв (каждый раз призывает 2 волков)

    public WolfLeader() : base(40, 5, 70) 
    {
        Name = "Wolf Leader";
        Description = "The mighty leader of the wolf pack. Can summon smaller wolves to aid him!";


        addModule(new BasicAttack());         
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

   
    protected override bool TryGetTargetForTwo(out Vector3Int targetPos)
    {
        var summonRangeCells = priorityModules[2].getCellsInRange(Position);
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


public class Wolf : Enemy
{
    public Wolf() : base(12, 2, 20) 
    {
        Name = "Wolf";
        Description = "A fierce wolf, loyal to its leader.";

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
}


public class SummonWolfModule : ActiveModule
{
    private WolfLeader owner; 

    public SummonWolfModule(WolfLeader owner)
    {
        this.owner = owner;
        energyCost = 2;      
        range = 2;          
    }

    public override bool CanBeUsed()
    {
        return owner != null && owner.HasSummonsLeft() && base.CanBeUsed();
    }

    public override async Task<bool> Use(Vector3Int targetPos)
    {
        if (owner == null || !owner.HasSummonsLeft())
            return false;
        bool success1 = false, success2 = false;
        var freeCells = getCellsInRange(owner.Position).Where(cell => !tracker.IsOccupied(cell) && !tracker.OutOfBounds(cell)).ToList();
        if (freeCells.Count >= 2)
        {
            success1 = Entity.summon<Wolf>(new PositionCollection(freeCells[0], 1));
            success2 = Entity.summon<Wolf>(new PositionCollection(freeCells[1], 1));
        }
        else if (freeCells.Count == 1)
        {
            success1 = Entity.summon<Wolf>(new PositionCollection(freeCells[0], 1));
        }

        if (success1 || success2)
        {
            owner.UseSummon(); 
            return true;
        }
        return false;
    }

    public override List<Vector3Int> getCellsInRange(PositionCollection position)
    {
        // ¬озвращаем все клетки в радиусе range от позиции
        var cells = new List<Vector3Int>();
        for (int x = (int)-range; x <= range; x++)
        {
            for (int y = (int)-range; y <= range; y++)
            {
                var offset = new Vector3Int(x, y, 0);
                var cell = position.LeftBottom + offset;
                cells.Add(cell);
            }
        }
        return cells;
    }
}