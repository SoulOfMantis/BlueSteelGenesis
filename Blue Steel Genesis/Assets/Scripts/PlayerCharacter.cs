
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



namespace BlueSteelGenesis.Character_Modules
{
    public class PlayerCharacter : Character
    {
        public List<Button> buttons;
        public List<int> modules; //Заглушка: 0 -- пассивный, 1 -- активный

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                if (modules[i] == 0)
                {
                    buttons[i].gameObject.SetActive(false);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

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
        }

        public override void heal(int hp)
        {
            base.heal(hp);
            Debug.Log($"Игрок полечился на {hp}!");
        }

        override protected void die()
        {
            Debug.Log("Игрок умер!");
        }

    }
}

