using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BlueSteelGenesis.Character_Modules
{
    public abstract class Character : MonoBehaviour
    {
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
            triggerModules(TriggerType.OnTurnStart, Vector3Int.zero);
        }
        public virtual void endTurn()
        {
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

        public void apply(int x, int y, int z, StatusModule status) => apply(new Vector3Int(x, y, z), status);
        public void apply(Vector3Int pos, StatusModule status)
        {
            Debug.Log($"Apply {status.GetType().Name} at {pos}");
            triggerModules(TriggerType.OnApply, pos);
        }



        public void addModule(GameModule module)
        {
            modules_.Add(module);
            module.Initialize();
        }
        public bool useActiveModule(int moduleIndex, Vector3Int pos)
        {
            if (modules_.ElementAtOrDefault(moduleIndex) is ActiveModule activeModule
                && trySpendEnergy(activeModule.energyCost))
            {
                activeModule.Effect(this, pos);
                return true;
            }
            return false;
        }
        public void AddStatusModule(StatusModule status)
        {
            var module = statusModules.Find(m => m.GetType() == status.GetType());
            if (module != null)
                module.Refresh(status);
            else
            {
                statusModules.Add(status);
                status.Initialize();
                Debug.Log($"Status module {status.GetType().Name} added to {GetType().Name}");
            }
        }
        protected void ProcessStatusModules(TriggerType triggerType, Vector3Int pos)
        {
            statusModules.ForEach(m => {
                if (triggerType == m.triggerType) m.Effect(this, pos);
            });
            statusModules.RemoveAll(m => m.IsExpired());
        }
        protected bool trySpendEnergy(int amount)
        {
            if (amount > currentEnergy)
                return false;
            currentEnergy -= amount;
            return true;
        }
        protected void triggerModules(TriggerType triggerType, Vector3Int pos)
        {
            modules_.ForEach(m => {
                if (m is PassiveModule pm && pm.triggerType == triggerType)
                    pm.Effect(this, pos);
            });
            ProcessStatusModules(triggerType, pos);
        }



        public static InitiativeTracker Tracker;
        private List<GameModule> modules_ = new();
        private List<StatusModule> statusModules = new();

        public int currentHealth
        {
            get => current_health_;
            protected set => current_health_ = Math.Clamp(value, 0, maxHealth);
        }
        public int maxHealth { get; protected set; }

        public int currentEnergy
        {
            get => current_energy_;
            protected set => current_energy_ = Math.Clamp(value, 0, maxEnergy);
        }
        public int maxEnergy { get; protected set; }

        public bool myTurn { get; protected set; }

        private int current_health_;
        private int current_energy_;
    }
}
