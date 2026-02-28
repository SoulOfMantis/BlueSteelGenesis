using BlueSteelGenesis.Character_Modules;
using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class PurpleDog : Enemy
{
    public TMP_Text healthDisplay;


    public PurpleDog() : base(5, 3, 60)
    {

        addModule(new BasicAttack());
        addModule(new BasicMovement());

        SetPriorityModules(modules_);
    }

    void updateHealth()
    {
        healthDisplay.text = $"{currentHealth}/{maxHealth}";
    }

    void Start()
    {
        if (tracker != null) tracker.AddCharacter(this);
        Debug.Log("Dog added");
    }

    public override void startTurn()
    {

        ExecuteTurn();
    }


    //private bool CanAttack(Vector3Int playerPosition)
    //{
    //    BasicAttack attack = getModule<BasicAttack>(0);
    //    Debug.Log($"Cost {attack.energyCost} {currentEnergy}");
    //    var attackRange = attack.getCellsInRange(Position);
    //    return attackRange.Contains(playerPosition);
    //}

    //private Vector3Int FindBestPosition(Vector3Int playerPosition)
    //{
    //    BasicMovement move = getModule<BasicMovement>(1);
    //    var moveRange = move.getCellsInRange(Position);
    //    int distance = Math.Abs(playerPosition.x - Position.x) + Math.Abs(playerPosition.y - Position.y);
    //    Vector3Int bestPosition = Position;

    //    if (distance == 1)
    //        return bestPosition;

    //    foreach (var cell in moveRange)
    //    {
    //        int temp = Math.Abs(playerPosition.x - cell.x) + Math.Abs(playerPosition.y - cell.y);
    //        if (temp < distance && !tracker.IsOccupied(cell) && !tracker.OutOfBounds(cell))
    //        {
    //            distance = temp;
    //            bestPosition = cell;
    //        }
    //    }

    //    return bestPosition;
    //}

    //private void MainLogic()
    //{
    //    PlayerCharacter player = tracker.getPlayer();
    //    while (modules_.Any(m => hasEnoughEnergy((ActiveModule)m)))
    //    {
    //        if (CanAttack(player.Position) && useActiveModule(0, player.Position))
    //            Debug.Log("PD attacks the player");
    //        else if (useActiveModule(1, FindBestPosition(player.Position)))
    //            Debug.Log("PD moves closer to player");
    //        else
    //        {
    //            Debug.Log("Ran out of energy");
    //            break;
    //        }
    //    }
    //}


    public override void damage(int dmg)
    {
        Debug.Log($"Собака получила {dmg} урона!");
        base.damage(dmg);
        updateHealth();
        //play taking damage animation
    }

    public override void heal(int hp)
    {
        Debug.Log($"Собака восстановила {hp} здоровья!");
        base.heal(hp);
        updateHealth();
        //play healing animation
    }

    public override void startBattle()
    {
        base.startBattle();
        updateHealth();
    }
}