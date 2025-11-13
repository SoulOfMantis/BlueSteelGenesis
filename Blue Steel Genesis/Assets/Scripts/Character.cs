using System;
using System.Collections.Generic;
using UnityEngine;


namespace BlueSteelGenesis.Character_Modules
{
    public abstract class Character : MonoBehaviour
    {
        private List<GameModule> modules_ = new List<GameModule>();
        private List<StatusEffect> activeStatuses = new List<StatusEffect>();

        /// <summary>
        /// Добавляет статус персонажу
        /// </summary>
        public void AddStatus(StatusEffect status)
        {
            var existingStatus = activeStatuses.Find(s => s.GetType() == status.GetType());
            if (existingStatus != null)
            {
                existingStatus.Refresh(status);
                Debug.Log($"Status {status.GetType().Name} refreshed");
            }
            else
            {
                activeStatuses.Add(status);
                status.OnApply(this);
                Debug.Log($"Status {status.GetType().Name} applied");
            }
        }

        /// <summary>
        /// Удаляет статус у персонажа
        /// </summary>
        public void RemoveStatus(StatusEffect status)
        {
            if (activeStatuses.Remove(status))
            {
                status.OnRemove(this);
                Debug.Log($"Status {status.GetType().Name} removed");
            }
        }

        /// <summary>
        /// Удаляет статус по типу
        /// </summary>
        public void RemoveStatus<T>() where T : StatusEffect
        {
            var status = activeStatuses.Find(s => s is T);
            if (status != null)
            {
                RemoveStatus(status);
            }
        }

        /// <summary>
        /// Проверяет, есть ли статус у персонажа
        /// </summary>
        public bool HasStatus<T>() where T : StatusEffect
        {
            return activeStatuses.Exists(s => s is T);
        }

        /// <summary>
        /// Получает статус по типу
        /// </summary>
        public T GetStatus<T>() where T : StatusEffect
        {
            return (T)activeStatuses.Find(s => s is T);
        }

        /// <summary>
        /// Обрабатывает статусы в начале хода
        /// </summary>
        private void ProcessStartOfTurnStatuses()
        {
            for (int i = activeStatuses.Count - 1; i >= 0; i--)
            {
                var status = activeStatuses[i];
                status.OnTurnStart(this);
                if (status.TickDuration())
                {
                    RemoveStatus(status);
                }
            }
        }

        /// <summary>
        /// Обрабатывает статусы в конце хода
        /// </summary>
        private void ProcessEndOfTurnStatuses()
        {
            foreach (var status in activeStatuses)
            {
                status.OnTurnEnd(this);
            }
        }

        public virtual void damage(int dmg)
        {
            currentHealth -= Math.Max(dmg, 1);
            triggerModules(TriggerType.OnDamage, Vector3Int.zero);
            if (currentHealth == 0) 
                die();
        }

        public virtual void heal(int hp)
        {
            currentHealth += Math.Max(hp, 1);
            triggerModules(TriggerType.OnHeal, Vector3Int.zero);
        }

        abstract protected void die();

        public virtual void startTurn()
        {
            myTurn = true;
            currentEnergy = maxEnergy;
            ProcessStartOfTurnStatuses();

            triggerModules(TriggerType.OnTurnStart, Vector3Int.zero);
        }

        public virtual void endTurn()
        {
            ProcessEndOfTurnStatuses();
            triggerModules(TriggerType.OnTurnEnd, Vector3Int.zero);
            myTurn = false;
        }

        public void move(int x, int y, int z) => move(new Vector3Int(x, y, z));
        public void move(Vector3Int pos)
        {
            transform.position = pos; 
            triggerModules(TriggerType.OnMove, pos);
        }

        public void strike(int x, int y, int z, int dmg) => strike(new Vector3Int(x, y, z), dmg);
        public void strike(Vector3Int pos, int dmg)
        {
            Debug.Log($"Strike at {pos} for {dmg} damage");
            triggerModules(TriggerType.OnStrike, pos);
        }

        public void addModule(GameModule module)
        {
            modules_.Add(module);
            module.Initialize();
        }

     
        public bool useActiveModule(int moduleIndex, Vector3Int pos)
        {
            if (moduleIndex < 0 || moduleIndex >= modules_.Count)
                return false;

            var module = modules_[moduleIndex];
            if (module is ActiveModule activeModule)
            {
                activeModule.Effect(this, pos);
                return true;
            }
            return false;
        }

       protected void triggerModules(TriggerType triggerType, Vector3Int pos)
        {
            foreach (var module in modules_)
            {
                if (module is PassiveModule passiveModule &&
                    passiveModule.triggerType == triggerType)
                {
                    passiveModule.Effect(this, pos);
                }
            }
        }

        public int currentHealth
        {
            get => current_health_;
            protected set => current_health_ = Math.Clamp(value, 0, maxHealth);
        }
        public int maxHealth { get; protected set; } = 100;


        public int currentEnergy
        {
            get => current_energy_;
            set => current_energy_ = Math.Clamp(value, 0, maxEnergy);
        }
        public int maxEnergy { get; protected set; } = 5;

        public bool myTurn { get; protected set; }

        private int current_health_ = 100;
        private int current_energy_;
    }
}