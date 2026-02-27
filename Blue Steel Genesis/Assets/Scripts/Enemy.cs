using BlueSteelGenesis.Character_Modules;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Enemy : Character
{
    private List<ActiveModule> priorityModules;

    public Enemy(int maxHealth, int maxEnergy, int initiative) : base(maxHealth, maxEnergy, initiative) { }


    /// <summary> устанавливает порядок модулей </summary>
    protected void SetPriorityModules(List<GameModule> allModules)
    {
        priorityModules = allModules.OfType<ActiveModule>().ToList();
    }


    /// <summary> метод выполнения хода </summary>
    protected void ExecuteTurn()
    {
        base.startTurn(); 

        bool actionTaken;
        do
        {
            actionTaken = false;

            foreach (var module in priorityModules)
            {
                if (currentEnergy >= module.energyCost)
                {
                    if (TryGetTargetForModule(module, out Vector3Int target))
                    {
                        int index = GetModuleIndex(module);
                        if (index != -1 && useActiveModule(index, target))
                        {
                            actionTaken = true;
                            break; 
                        }
                    }
                }
            }
        } while (actionTaken && HasEnergyForAnyModule());

        base.endTurn(); 
    }



    protected virtual bool TryGetTargetForModule(ActiveModule module, out Vector3Int target)
    {
        target = default;

        if (module is BasicAttack attack)
            return TryGetAttackTarget(attack, out target);
        if (module is BasicMovement movement)
            return TryGetMovementTarget(movement, out target);

        return false;
    }

 
    protected virtual bool TryGetAttackTarget(BasicAttack attack, out Vector3Int target)
    {
        target = default;
        PlayerCharacter player = tracker.getPlayer();
        var attackRange = attack.getCellsInRange(Position);
        if (attackRange.Contains(player.Position))
        {
            target = player.Position;
            return true;
        }
        return false;
    }


    protected virtual bool TryGetMovementTarget(BasicMovement movement, out Vector3Int target)
    {
        target = default;
        PlayerCharacter player = tracker.getPlayer();
        var moveRange = movement.getCellsInRange(Position);
        int currentDist = Mathf.Abs(player.Position.x - Position.x) + Mathf.Abs(player.Position.y - Position.y);
        Vector3Int best = Position;
        if (currentDist <= 1) return false; 
        foreach (var cell in moveRange)
        {
            int dist = Mathf.Abs(player.Position.x - cell.x) + Mathf.Abs(player.Position.y - cell.y);
            if (dist < currentDist && !tracker.IsOccupied(cell) && !tracker.OutOfBounds(cell))
            {
                currentDist = dist;
                best = cell;
            }
        }
        if (best != Position)
        {
            target = best;
            return true;
        }
        return false;
    }


    private int GetModuleIndex(ActiveModule module)
    {
        for (int i = 0; i < modules_.Count; i++)
            if (modules_[i] == module) return i;
        return -1;
    }


    private bool HasEnergyForAnyModule()
    {
        foreach (var m in modules_)
            if (m is ActiveModule module && currentEnergy >= module.energyCost)
                return true;
        return false;
    }

    protected override void die()
    {
        Debug.Log($"{name} умер");
        tracker.RemoveCharacter(this);
        Destroy(gameObject);
    }
}