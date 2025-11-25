
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;


namespace BlueSteelGenesis.Character_Modules
{
    public class PlayerCharacter : Character
    {
        public TMP_Text energyDisplay;
        public TMP_Text healthDisplay;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
           
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void EndBattle()
        {
            statusModules = new List<StatusModule>();
        }

        void updateHealth()
        {
            healthDisplay.text = $"{currentHealth}/{maxHealth}";
        }

        void updateEnergy()
        {
            energyDisplay.text = $"{currentEnergy}/{maxEnergy}";
        }

        public override bool useActiveModule(int moduleIndex, Vector3Int pos)
        {
            var flag = base.useActiveModule(moduleIndex, pos);
            if (flag)
            {
                updateEnergy();
                //play animation of module use success
            }
            else
            {
                //play animation of module use fail
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

        public override void startTurn()
        {
            base.startTurn();
            //play start turn animation
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
            Debug.Log($"Игрок полечился на {hp}!");
            updateHealth();
            //play healing animation
        }

        override protected void die()
        {
            Debug.Log("Игрок умер!");
        }

    }
}

