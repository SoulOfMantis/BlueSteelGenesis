using UnityEngine;
using BlueSteelGenesis.Character;

public class PlayerCharacter : Character
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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

    //public override void damage(int dmg)
    //{
    //    base.damage(dmg);
    //    Debug.Log($"Игрок получил {dmg} урона!");
    //}

    //public override void heal(int hp)
    //{
    //    base.heal(hp);
    //    Debug.Log($"Игрок полечился на {hp}!");
    //}

    override protected void die()
    {
        //TODO: trigger modules on dying
        Debug.Log("Игрок умер!");
    }

}
