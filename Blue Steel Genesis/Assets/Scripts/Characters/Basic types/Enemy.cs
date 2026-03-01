using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Enemy : Character
{
    public Enemy(int maxHealth, int maxEnergy, int initiative)
    {
        this.maxHealth = maxHealth;
        this.maxEnergy = maxEnergy;
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
        Initiative = initiative;
    }
    protected List<ActiveModule> priorityModules;
    protected void SetPriorityModules() => priorityModules = listModules<ActiveModule>().ToList();
    /// <summary> метод выполнения хода </summary>
    public override async Task startTurn()
    {
        await base.startTurn();
        await TurnLogic();
        await endTurn();
    }
    protected async Task TurnLogic()
    {
        if (priorityModules == null) SetPriorityModules();
        bool actionTaken = false;
        while (actionTaken && CanUseAnyPriorityModule())
        {
            actionTaken = false;
            foreach (var module in priorityModules)
                if (currentEnergy >= module.energyCost)
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
            0 => TryGetTargetForOne(out targetPos),
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

    private bool CanUseAnyPriorityModule()
    {
        return priorityModules.Any(m => currentEnergy >= m.energyCost && m.CanBeUsed());
    }

    protected override Task die()
    {
        Debug.Log($"{name} умер");
        tracker.RemoveCharacter(this);
        Destroy(gameObject);
        return Task.CompletedTask;
    }

    public override int currentHealth { get; protected set; }
    public override int maxHealth { get; protected set; }
    public override int maxEnergy { get; protected set; }
    protected override List<GameModule> modules_ { get; set; } = new();
}

