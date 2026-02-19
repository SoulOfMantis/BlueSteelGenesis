using BlueSteelGenesis.Character_Modules;
using System.Collections.Generic;
using UnityEngine;

    public class Enemy : Character
    {
    public Enemy(int maxHealth, int maxEnergy, int initiative) : base(maxHealth, maxEnergy, initiative) {}

    protected override void die()
        {
        Debug.Log($"{name} умер");
        tracker.RemoveCharacter(this);
        Destroy(gameObject);
    }

    protected List<int> modulePriority;


    protected virtual bool TryGetTargetForModule(int moduleIndex, out Vector3Int target)
    {
        target = Position;
        if (tracker == null) return false;
        if (tracker.getPlayer() == null) return false;
        return false;
    }

    /// <summary>
    /// есть ли энергия для любого модуля
    /// </summary>
    /// <returns></returns>
    protected bool HasEnergyForAnyModule()
    {
        foreach (int idx in modulePriority)
        {
            if (!isActive(idx)) continue;
            ActiveModule module = getModule<ActiveModule>(idx);
            if (module != null && hasEnoughEnergy(module))
                return true;
        }
        return false;
    }

    /// <summary>
    /// Основная логика хода врага.
    /// Пытается использовать модули в порядке приоритета, пока есть энергия и доступные действия.
    /// </summary>
    public virtual void PerformTurn()
    {
        PlayerCharacter player = tracker.getPlayer();
        if (player == null) return;

        while (HasEnergyForAnyModule())
        {
            bool actionDone = false;
            foreach (int idx in modulePriority)
            {
                if (!isActive(idx)) continue;
                ActiveModule module = getModule<ActiveModule>(idx);
                if (module == null || !hasEnoughEnergy(module)) continue;

                if (module.TryGetTarget(this, out Vector3Int targetPos))
                {
                    if (useActiveModule(idx, targetPos))
                    {
                        actionDone = true;
                        break;
                    }
                }
            }
            if (!actionDone) break;
        }
    }
}
