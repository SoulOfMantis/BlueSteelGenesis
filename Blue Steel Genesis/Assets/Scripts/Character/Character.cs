
using System;
using System.Collections.Generic;
using UnityEngine;


namespace BlueSteelGenesis.Character
{
    public abstract class Character : MonoBehaviour
    {
        private List<GameModule> modules_ = new List<GameModule>();

        public virtual void damage(int dmg)
        {
            currentHealth -= Math.Max(dmg, 1);
            triggerModules(TriggerType.OnDamage, Vector3Int.zero);
            if (current_health_ == 0) die();
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
            myTurn = false;
            triggerModules(TriggerType.OnTurnEnd, Vector3Int.zero);
        }

        public void move(int x, int y, int z) => move(new Vector3Int(x, y, z));
        public void move(Vector3Int pos)
        {
            triggerModules(TriggerType.OnMove, pos);
        }

        public void strike(int x, int y, int z, int dmg) => strike(new Vector3Int(x, y, z), dmg);
        public void strike(Vector3Int pos, int dmg)
        {
            triggerModules(TriggerType.OnStrike, pos);
        }

        public void addModule(GameModule module)
        {
            modules_.Add(module);
            module.Initialize();
        }

        protected void triggerModule(GameModule module, Character user, Vector3Int pos)
        {
            if (module != null)
            {
                module.OnTrigger(user, pos);
            }
        }

        protected void triggerModules(TriggerType triggerType, Vector3Int pos)
        {
            foreach (var module in modules_)
            {
                if (module.triggerType == triggerType)
                {
                    module.OnTrigger(this, pos);
                }
            }
        }

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

        protected bool myTurn { get; private set; }

        private int current_health_;
        private int current_energy_;
    }
}