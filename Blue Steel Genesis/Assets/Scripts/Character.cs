using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Profiling.Editor;
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
            currentHealth = maxHealth;
            currentEnergy = maxEnergy;
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

        public virtual void drainEnergy(int amount)
        {
            currentEnergy -= Math.Max(amount, 1);
            triggerModules(TriggerType.OnEnergyDrain);
        }
        public virtual void restoreEnergy(int amount)
        {
            currentEnergy += Math.Max(amount, 1);
            triggerModules(TriggerType.OnEnergyRestore);
        }


        public virtual void startBattle()
        {
            // TODO: adjust position
            triggerModules(TriggerType.OnBattleStart);
        }
        public virtual void endBattle()
        {
            status_modules_.Clear();
            triggerModules(TriggerType.OnBattleEnd);
        }
        public virtual void startTurn()
        {
            myTurn = true;
            restoreEnergy(maxEnergy);
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
            if (tracker.OutOfBounds(pos) || tracker.IsOccupied(pos))
                return;
            Position = pos;
            triggerModules(TriggerType.OnMove, pos);
        }

        public void strike(int x, int y, int z, int dmg) => strike(new Vector3Int(x, y, z), dmg);
        public void strike(Vector3Int pos, int dmg)
        {
            Character target = tracker.FindCharacterAtPosition(pos);
            if (target == null)
                return;
            target.damage(dmg);
            triggerModules(TriggerType.OnStrike, pos);
            Debug.Log($"Strike at {pos} for {dmg} damage");
        }

        public void apply(int x, int y, int z, StatusModule status) => apply(new Vector3Int(x, y, z), status);
        public void apply(Vector3Int pos, StatusModule status)
        {
            Character target = tracker.FindCharacterAtPosition(pos);
            if (target == null)
                return;
            target.addStatusModule(status);
            triggerModules(TriggerType.OnApply, pos);
            Debug.Log($"Apply {status.GetType().Name} at {pos}");
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
            var activeModule = getModule<ActiveModule>(moduleIndex);
            if (hasEnoughEnergy(activeModule) && isCorrectPosition(activeModule, pos))
            {
                useActiveModule_internal(activeModule, pos);
                drainEnergy(activeModule.energyCost);
                return true;
            }
            return false;
        }
        protected void triggerModules(TriggerType triggerType) => triggerModules(triggerType, Position);
        protected void triggerModules(TriggerType triggerType, Vector3Int pos)
        {
            foreach (var pm in listModules<PassiveModule>().Where(pm => pm.triggerType == triggerType))
                usePassiveModule_internal(pm, pos);
            processStatusModules(triggerType);
        }
        protected void processStatusModules(TriggerType triggerType)
        {
            foreach (var st in status_modules_.Where(m => triggerType == m.triggerType))
                useStatusModule_internal(st);
            status_modules_.RemoveAll(m => m.IsExpired());
        }
        

        protected IEnumerable<ModuleT> listModules<ModuleT>()
            where ModuleT: GameModule
        {
            return modules_.Where(m => m is ModuleT).Select(m => m as ModuleT);
        }
        protected ModuleT getModule<ModuleT>(int module_index)
            where ModuleT: GameModule
        {
            var module = modules_.ElementAtOrDefault(module_index);
            if (module is ModuleT res)
                return res;
            return null;
        }


        protected bool isPassive(int module_index) => getModule<PassiveModule>(module_index) != null;
        protected bool isActive(int module_index) => getModule<ActiveModule>(module_index) != null;
        protected bool doesModuleExist(int module_index) => getModule<GameModule>(module_index) != null;
        protected virtual bool isCorrectPosition(ActiveModule module, Vector3Int pos) => true;
        protected virtual bool hasEnoughEnergy(ActiveModule module) => module != null && currentEnergy >= module.energyCost;
        protected virtual void useActiveModule_internal(ActiveModule m, Vector3Int pos) => m.Effect(this, pos);
        protected virtual void usePassiveModule_internal(PassiveModule m, Vector3Int pos) => m.Effect(this, pos);
        protected virtual void useStatusModule_internal(StatusModule m) => m.Effect(this, Position);


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
        public int initiative { get; protected set; }

        public bool myTurn { get; protected set; }

        public Vector3Int Position {
            get => position_;
            protected set { 
                transform.position = tracker.CellToWorld(value);
                position_ = value;
            }
        }


        public static SceneTracker tracker;

        protected List<GameModule> modules_ = new();
        protected List<StatusModule> status_modules_ = new();

        private int current_health_;
        private int current_energy_;
        private Vector3Int position_;
    }
}
