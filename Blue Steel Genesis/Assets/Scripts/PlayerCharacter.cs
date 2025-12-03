
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;


namespace BlueSteelGenesis.Character_Modules
{
    public class PlayerCharacter : Character
    {
        public static List<ModuleButton> activeModuleButtons = new();
        public TMP_Text energyDisplay;
        public TMP_Text healthDisplay;
        PlayerCharacter() : base(10, 3, 10)
        {
            //modules hardcoded for now
            addModule(new PoisonStinger());
            addModule(new BasicMovement());
            modules_.ForEach(m => m.changeName(m.GetType().Name));
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (tracker != null) tracker.AddCharacter(this);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public List<Vector3Int> GetModulePositions(int n) => modules_[n].getCellsInRange(Position);

        void updateHealth()
        {
            healthDisplay.text = $"{currentHealth}/{maxHealth}";
        }

        void updateEnergy()
        {
            energyDisplay.text = $"{currentEnergy}/{maxEnergy}";
            activeModuleButtons.ForEach(mb => mb.buttonInteractableManaging());
        }

        public string getmoduleName(int index)
        {
            if (!doesModuleExist(index)) return null;
            return modules_[index].Name;
        }
        public string getmoduleDescription(int index)
        {
            if (!doesModuleExist(index)) return null;
            return modules_[index].Description;
        }
        protected override void useActiveModule_internal(ActiveModule m, Vector3Int pos)
        {
            //play using module animation
            base.useActiveModule_internal(m, pos);
        }
        protected override void usePassiveModule_internal(PassiveModule m, Vector3Int pos)
        {
            //play using module animation
            base.usePassiveModule_internal(m, pos);
        }
        protected override void useStatusModule_internal(StatusModule m)
        {
            //play using module animation
            base.useStatusModule_internal(m);
    }

        protected override bool isCorrectPosition(ActiveModule module, Vector3Int pos)
        {
            var flag = base.isCorrectPosition(module, pos);
            if (!flag)
            {
                //play animation of incorrect position for module
            }
            return flag;
        }

        protected override bool hasEnoughEnergy(ActiveModule module)
        {
            var flag = base.hasEnoughEnergy(module);
            if (!flag)
            {                
                //play animation of not enough energy for module
            }
            return flag;
        }

        public bool canUseModule(int module_index)
        {
            return myTurn && hasEnoughEnergy(getModule<ActiveModule>(module_index));
        }

        public override void startBattle()
        {
            base.startBattle();
            //play starting battle animation
        }

        public override void endBattle()
        {
            base.endBattle();
            //play ending battle animation
        }

        public override void startTurn()
        {
            base.startTurn();
            //play start turn animation
        }

        public override void endTurn()
        {
            base.endTurn();
            updateEnergy();
            //play turn end animation
        }

        public override void damage(int dmg)
        {
            base.damage(dmg);
            Debug.Log($"Игрок получил {dmg} урона!");
            updateHealth();
            //play taking damage animation
        }

        public override void heal(int hp)
        {
            base.heal(hp);
            Debug.Log($"Игрок восстановил {hp} здоровья!");
            updateHealth();
            //play healing animation
        }

        public override void drainEnergy(int amount)
        {
            base.drainEnergy(amount);
            updateEnergy();
            //play losing energy animation
        }
        public override void restoreEnergy(int amount)
        {
            base.restoreEnergy(amount);
            updateEnergy();
            //play restoring energy animation
        }

        override protected void die()
        {
            Debug.Log("Игрок умер!");
            tracker.RemoveCharacter(this);
            //TODO: player loss
            //play dying animation
        }

        public static void Victory() 
        {

        }
    }
}

