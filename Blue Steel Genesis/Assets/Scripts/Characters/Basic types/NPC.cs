using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public abstract class NPC : Character
{
    public NPC(uint maxHealth, uint maxEnergy, int initiative)
    {
        Name = "Default npc name";
        Description = "Default npc description. If you see this, something went wrong.";
        this.maxHealth = maxHealth;
        this.maxEnergy = maxEnergy;
        currentHealth.Value = maxHealth;
        currentEnergy.Value = maxEnergy;
        Initiative = initiative;
    }

    protected List<ActiveModule> priorityModules;
    protected void SetPriorityModules() => priorityModules = listModules<ActiveModule>().ToList();

    /// <summary> метод выполнения хода </summary>
    public override async Task startTurn()
    {
        await base.startTurn();
        if (currentHealth == 0)
            return;
        await TurnLogic();
        await endTurn();
    }
    protected async Task TurnLogic()
    {
        if (priorityModules == null) SetPriorityModules();
        bool actionTaken = true;
        while (actionTaken && canUseAnyModule() && tracker.IsPlayerAlive())
        {
            actionTaken = false;
            foreach (var module in priorityModules)
                if (currentEnergy >= module.energyCost && module.CanBeUsed())
                {
                    int index = priorityModules.FindIndex(m => m == module);
                    if (TryGetTargetForModule(index, out Vector3Int target))
                    {
                        index = modules_.FindIndex(m => m == module);
                        if (await useActiveModule(index, target))
                        {
                            actionTaken = true;
                            break;
                        }
                    }
                }
        }
    }

    protected bool TryGetTargetForModule(int index, out Vector3Int targetPos)
    {
        targetPos = default;
        return index switch
        {
            0 => TryGetTargetForZero(out targetPos),
            1 => TryGetTargetForOne(out targetPos),
            2 => TryGetTargetForTwo(out targetPos),
            3 => TryGetTargetForThree(out targetPos),
            _ => false,
        };
    }

    protected abstract bool TryGetTargetForZero(out Vector3Int targetPos);
    protected abstract bool TryGetTargetForOne(out Vector3Int targetPos);
    protected virtual bool TryGetTargetForTwo(out Vector3Int targetPos)
    {
        targetPos = default;
        return false;
    }
    protected virtual bool TryGetTargetForThree(out Vector3Int targetPos)
    {
        targetPos = default;
        return false;
    }

    protected override async Task die()
    {
        if (myTurn)
            await endTurn();
        await processTrigger(TriggerType.OnDeath, null);
        if (TooltipSystem.IsCurrent(this))
            TooltipSystem.Hide(TooltipSystem.TooltipType.entityTooltip);
        if (sfx != null)
            sfx.play(TriggerType.OnDeath);
        if (visualHandler != null)
            await visualHandler.PlayDeathAnimation();

        Debug.Log($"{name} умер");
        tracker.RemoveCharacter(this);
        Destroy(gameObject);
    }

    protected bool GetDirectApproachTarget(out Vector3Int targetPos, ActiveModule module, IEnumerable<Vector3Int> targets) {
        targetPos = Position.LeftBottom;
        var moveRange = module.getCellsInRange(Position).Concat(Position).ToHashSet();
        var path = Navigation.Dijkstra.getPath(Position, targets,
            p => !tracker.IsOccupied(p) && !tracker.OutOfBounds(p)) ?? new();

        Vector3Int offset = new();
        foreach (var move in path)
            if ((Position + offset + move).All(p => moveRange.Contains(p)))
                offset += move;
            else break;
        targetPos = Position.LeftBottom + offset;
        return offset != Vector3Int.zero;
    }
    protected bool GetDirectApproachTarget(out Vector3Int targetPos, ActiveModule module, IEnumerable<Entity> targets) =>
        GetDirectApproachTarget(out targetPos, module, targets.SelectMany(e => e.Position.NeighborPositions()));

    protected bool GetApproachTarget(out Vector3Int targetPos, ActiveModule module, IEnumerable<Vector3Int> targets) {
        targetPos = Position.LeftBottom;
        var moveRange = module.getCellsInRange(Position).Concat(Position).ToHashSet();
        var path = Navigation.Dijkstra.getPath(Position, targets,
            p => !tracker.IsOccupiedByObstacle(p) && !tracker.OutOfBounds(p)) ?? new();

        Vector3Int offset = new();
        foreach (var move in path)
            if ((Position + offset + move).All(p => moveRange.Contains(p)))
                offset += move;
            else break;
        targetPos = Position.LeftBottom + offset;
        return offset != Vector3Int.zero;
    }
    protected bool GetApproachTarget(out Vector3Int targetPos, ActiveModule module, IEnumerable<Entity> targets) =>
        GetApproachTarget(out targetPos, module, targets.SelectMany(e => e.Position.NeighborPositions()));



    protected abstract IEnumerable<Entity> getEnemies();
    protected abstract IEnumerable<Entity> getAllies();

    public bool IsHostileToPlayer => this is Enemy;
    public bool IsAlliedToPlayer => this is Ally;


    public override URangeValue currentHealth { get; protected set; } = new();
    public override uint maxHealth {
        get => currentHealth.Max;
        protected set => currentHealth.Max = value;
    }
    public override uint maxEnergy {
        get => currentEnergy.Max;
        protected set => currentEnergy.Max = value;
    }
    protected override List<GameModule> modules_ { get; set; } = new();
}
