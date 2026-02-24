using UnityEngine;
using BlueSteelGenesis.Character_Modules;

    public class Enemy : Character
    {
    public Enemy(int maxHealth, int maxEnergy, int initiative) : base(maxHealth, maxEnergy, initiative) {}

    protected override void die()
        {
        Debug.Log($"{name} умер");
        tracker.RemoveCharacter(this);
        Destroy(gameObject);
    }

    /// <summary>
    /// Определяет, можно ли использовать модуль с индексом moduleIndex,
    /// и возвращает подходящую цель.
    /// Наследники реализуют через switch-case.
    /// </summary>
    protected abstract bool TryGetTargetForModule(int moduleIndex, out Vector3Int target);

    /// <summary>
    /// Основная логика хода врага.
    /// Пытается использовать модули в порядке приоритета, пока есть энергия и доступные действия.
    /// </summary>
    public virtual void PerformTurn()
    {
        PlayerCharacter player = tracker.getPlayer();
        if (player == null) return;

        while (true)
        {
            bool actionDone = false;

            foreach (int idx in modulePriority)
            {
    
                if (!isActive(idx)) continue;

                ActiveModule module = getModule<ActiveModule>(idx);
                if (module == null) continue;

             
                if (!hasEnoughEnergy(module)) continue;


                if (TryGetTargetForModule(idx, out Vector3Int target))
                {

                    if (useActiveModule(idx, target))
                    {
                        actionDone = true;
                        break; 
                    }
                }
            }

            if (!actionDone)
                break; 
        }
    }
}
