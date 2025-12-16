using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

    public abstract class Character : MonoBehaviour
    {
        public Character(int maxHealth, int maxEnergy, int initiative)
        {
            this.maxHealth = maxHealth;
            this.maxEnergy = maxEnergy;
            currentHealth = maxHealth;
            currentEnergy = maxEnergy;
            Initiative = initiative;
        }


        public virtual void damage(int dmg, ActionContext prevAction = null)
        {
            currentHealth -= Math.Max(dmg, 1);
        ActionContext action = new ActionContext(null, "takeDamage", prevAction, this);
            triggerModules(TriggerType.OnDamage, action);
            if (currentHealth == 0)
                die();
        }
        public virtual void heal(int hp, ActionContext prevAction = null)
        {
            currentHealth += Math.Max(hp, 1);
        ActionContext action = new ActionContext(null, "heal", prevAction, this);
        triggerModules(TriggerType.OnHeal, action);
    }
    abstract protected void die();

        public virtual void drainEnergy(int amount, ActionContext prevAction = null)
        {
            currentEnergy -= Math.Max(amount, 1);
            ActionContext action = new ActionContext(null, "drainEnergy", prevAction, this);
            triggerModules(TriggerType.OnEnergyDrain, action);
        }
        public virtual void restoreEnergy(int amount, ActionContext prevAction = null)
        {
            currentEnergy += Math.Max(amount, 1);
            ActionContext action = new ActionContext(null, "restoreEnergy", prevAction, this);
            triggerModules(TriggerType.OnEnergyRestore, action);
    }


        public virtual void startBattle()
        {
            Position = tracker.WorldToCell(transform.position);
            triggerModules(TriggerType.OnBattleStart);
        }
        public virtual void endBattle()
        {
            status_modules_.Clear();
            triggerModules(TriggerType.OnBattleEnd);
        }
        public virtual void startTurn()
        {
            Debug.Log($"turn started");
            myTurn = true;
            restoreEnergy(maxEnergy);
            triggerModules(TriggerType.OnTurnStart);
        }
        public virtual void endTurn()
        {
            triggerModules(TriggerType.OnTurnEnd);
            myTurn = false;
            tracker.NextTurn();
        }


        public void move(Vector3Int pos, ActionContext prevAction = null)
        {
            if (tracker.OutOfBounds(pos) || tracker.IsOccupied(pos))
                return;
            Position = pos;
        var action = new ActionContext(this, "move", prevAction);
            triggerModules(TriggerType.OnMove, pos, action);
        }

        public void strike(Vector3Int pos, int dmg, ActionContext prevAction = null)
        {
            Character target = tracker.FindCharacterAtPosition(pos);
            if (target == null)
                return;
         var action = new ActionContext(this, "strike", prevAction, target);
            target.damage(dmg, action);
        triggerModules(TriggerType.OnStrike, pos, action);
        Debug.Log($"Strike at {pos} for {dmg} damage");
        }

        public void apply(Vector3Int pos, StatusModule status, ActionContext prevAction = null)
        {
            Character target = tracker.FindCharacterAtPosition(pos);
            if (target == null)
                return;
        var action = new ActionContext(this, "apply", prevAction, target);
        target.addStatusModule(status, action);
            triggerModules(TriggerType.OnApply, pos, action);
            Debug.Log($"Apply {status.GetType().Name} at {pos}");
        }


        public void addModule(GameModule module)
        {
            modules_.Add(module);
            module.Initialize();
        }
        public void addStatusModule(StatusModule status, ActionContext prevAction = null)
        {
            var module = status_modules_.Find(m => m.GetType() == status.GetType());
            if (module != null)
                module.Refresh(status);
            else
            {
                status_modules_.Add(status);
            var action = new ActionContext(null, "addStatusModule", prevAction, this);
            if (status is NegativeStatus) 
                triggerModules(TriggerType.OnNegativeStatusApplied, action);
            else if (status is PositiveStatus) 
                triggerModules(TriggerType.OnPositiveStatusApplied, action);
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
        protected void triggerModules(TriggerType triggerType, ActionContext context = null) => triggerModules(triggerType, Position, context);
        protected void triggerModules(TriggerType triggerType, Vector3Int pos, ActionContext context = null)
        {
            foreach (var pm in listModules<PassiveModule>().Where(pm => pm.triggerType == triggerType))
               {
            pm.loadContext(context);
                usePassiveModule_internal(pm, pos); 
                 }
            processStatusModules(triggerType, context);
        }
        protected void processStatusModules(TriggerType triggerType, ActionContext context = null)
        {
        foreach (var st in status_modules_.Where(m => triggerType == m.triggerType))
        {
            st.loadContext(context);
            useStatusModule_internal(st);
        }
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
            return module as ModuleT;
        }


        public bool isPassive(int module_index) => getModule<PassiveModule>(module_index) != null;
        public bool isActive(int module_index) => getModule<ActiveModule>(module_index) != null;
        public bool doesModuleExist(int module_index) => getModule<GameModule>(module_index) != null;
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
        public int Initiative { get; protected set; }

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

