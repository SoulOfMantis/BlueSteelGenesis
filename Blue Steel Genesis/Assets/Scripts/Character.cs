using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace BlueSteelGenesis.Character_Modules
{
    public abstract class Character : MonoBehaviour
    {
        public Character(int maxHealth, int maxEnergy)
        {
            this.maxHealth = maxHealth;
            this.maxEnergy = maxEnergy;
        }



        public virtual void damage(int dmg)
        {
            currentHealth -= Math.Max(dmg, 1);
            triggerModules(TriggerType.OnDamage);
            if (currentHealth == 0)
                die();
        }
        public virtual void heal(int hp)
        {
            currentHealth += Math.Max(hp, 1);
            triggerModules(TriggerType.OnHeal);
        }
        abstract protected void die();



        public virtual void startTurn()
        {
            myTurn = true;
            currentEnergy = maxEnergy;
            triggerModules(TriggerType.OnTurnStart);
        }
        public virtual void endTurn()
        {
            triggerModules(TriggerType.OnTurnEnd);
            myTurn = false;
        }



        public void move(int x, int y, int z) => move(new Vector3Int(x, y, z));
        public void move(Vector3Int pos)
        {
            Position = pos;
            triggerModules(TriggerType.OnMove);
        }

        public void strike(int x, int y, int z, int dmg) => strike(new Vector3Int(x, y, z), dmg);
        public void strike(Vector3Int pos, int dmg)
        {
            Debug.Log($"Strike at {pos} for {dmg} damage");
            triggerModules(TriggerType.OnStrike);
        }

        public void apply(int x, int y, int z, StatusModule status) => apply(new Vector3Int(x, y, z), status);
        public void apply(Vector3Int pos, StatusModule status)
        {
            Debug.Log($"Apply {status.GetType().Name} at {pos}");
            triggerModules(TriggerType.OnApply);
        }



        public void addModule(GameModule module)
        {
            modules_.Add(module);
            module.Initialize();
        }
        public void addStatusModule(StatusModule status)
        {
            var module = status_modules_.Find(m => m.GetType() == status.GetType());
            if (module != null)
                module.Refresh(status);
            else
            {
                status_modules_.Add(status);
                status.Initialize();
                Debug.Log($"Status module {status.GetType().Name} added to {GetType().Name}");
            }
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
        protected void triggerModules(TriggerType triggerType)
        {
            modules_.ForEach(m => {
                if (m is PassiveModule pm && pm.triggerType == triggerType)
                    pm.Effect(this, Position);
            });
            processStatusModules(triggerType);
        }
        protected void processStatusModules(TriggerType triggerType)
        {
            status_modules_.ForEach(m => {
                if (triggerType == m.triggerType) m.Effect(this, Position);
            });
            status_modules_.RemoveAll(m => m.IsExpired());
        }
        protected bool trySpendEnergy(int amount)
        {
            if (amount > currentEnergy)
                return false;
            currentEnergy -= amount;
            return true;
        }



        public static InitiativeTracker Tracker;

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

        public Vector3Int Position {
            get => position_;
            protected set { 
                // TODO: adjust transform
                position_ = value;
            }
        }



        protected List<GameModule> modules_ = new();
        protected List<StatusModule> status_modules_ = new();

        private int current_health_;
        private int current_energy_;
        private Vector3Int position_;
    }
}
