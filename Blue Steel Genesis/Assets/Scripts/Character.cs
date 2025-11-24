using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BlueSteelGenesis.Character_Modules
{
    public abstract class Character : MonoBehaviour
    {
        private List<GameModule> modules_ = new List<GameModule>();
        private List<StatusModule> statusModules = new List<StatusModule>();


        public void AddStatusModule(StatusModule status)
        {
            if (!statusModules.Exists(x => x.GetType() == status.GetType()))
            {
                statusModules.Add(status);
                status.Initialize();
                Debug.Log($"Status module {status.GetType().Name} added to {GetType().Name}");
            }
            else
            {
                statusModules.Find(x => x == status).Refresh(status);
            }
        }


        public void RemoveStatusModule(StatusModule status)
        {
            if (statusModules.Remove(status))
            {
                Debug.Log($"Status module {status.GetType().Name} removed from {GetType().Name}");
            }
        }

        protected void ProcessStatusModules(TriggerType triggerType, Vector3Int pos)
        {
            List<StatusModule> expiredStatuses = new List<StatusModule>();

            foreach (var status in statusModules)
            {
                if (status.triggerType == triggerType)
                {
                    status.Effect(this, pos);
                    if (status.IsExpired())
                    {
                        expiredStatuses.Add(status);
                    }
                }
            }

            foreach (var expiredStatus in expiredStatuses)
            {
                RemoveStatusModule(expiredStatus);
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
            ProcessStatusModules(triggerType, pos);
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

public abstract class Character : MonoBehaviour {
  public bool myTurn = false;
  public static InitiativeTracker Tracker;
  public virtual void damage(int dmg) {
    currentHealth -= Math.Max(dmg, 1);
    if (current_health_ == 0) die();
  }
  public virtual void heal(int hp) {
    currentHealth += Math.Max(hp, 1);
  }
  abstract protected void die();

 public virtual void startTurn() {

        myTurn = true;
        currentEnergy = maxEnergy;
        // TODO: trigger modules
  }
  public virtual void endTurn() {

        myTurn = false;
        Tracker.StartNextTurn();

    // TODO: trigger modules
  }



  public void move(int x, int y, int z) => move(new Vector3Int(x, y, z));
  public void move(Vector3Int pos) {
    // TODO
  }
  public void strike(int x, int y, int z, int dmg) => strike(new Vector3Int(x, y, z), dmg);
  public void strike(Vector3Int pos, int dmg) {
    // TODO
  }

  // TODO: public void addModule(/* smth */)
  // TODO: protected void triggerModule(/* smth */)
  // TODO: protected void triggerModules(TriggerType)



  public int currentHealth {
    get => current_health_;
    protected set =>
      current_health_ = Math.Clamp(value, 0, maxHealth);
  }
  public int maxHealth { get; protected set; }

  public int currentEnergy {
    get => current_energy_;
    protected set =>
      current_energy_ = Math.Clamp(value, 0, maxEnergy);
  }
  public int maxEnergy { get; protected set; }


   // protected bool myTurn { get; private set; }


    private int current_health_;
  private int current_energy_;
  // private List<Module> modules_;
}

