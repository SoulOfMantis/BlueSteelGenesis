
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;


namespace BlueSteelGenesis.Character_Modules
{
    public class PlayerCharacter : Character
    {
        //public List<Button> buttons;
        public TMP_Text energyDisplay;
        public TMP_Text healthDisplay;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
           
        }

        public void EndBattle()
        {
            statusModules = new List<StatusModule>();
        }

        // Update is called once per frame
        void Update()
        {

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
                updateEnergy();
            return flag;
        }

        public override void startTurn()
        {
            base.startTurn();
            //TODO: Включить кнопки для игрока
        }

        public override void damage(int dmg)
        {
            base.damage(dmg);
            Debug.Log($"Игрок получил {dmg} урона!");
            updateHealth();
        }

        public override void heal(int hp)
        {
            base.heal(hp);
            Debug.Log($"Игрок полечился на {hp}!");
            updateHealth();
        }

        override protected void die()
        {
            Debug.Log("Игрок умер!");
        }

    }
}

