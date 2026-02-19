using BlueSteelGenesis.Character_Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PurpleDog : Enemy 
{
    public TMP_Text healthDisplay;

    private const int ATTACK_INDEX = 0;
    private const int MOVE_INDEX = 1;

    // Purple Dog enemy constructor
    public PurpleDog() : base(5, 3, 60)
    {
        addModule(new BasicAttack());
        addModule(new BasicMovement());
        modulePriority = new List<int> { ATTACK_INDEX, MOVE_INDEX };
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


    // Begins PDs turn
    public override void startTurn()
    {
        base.startTurn();   
        PerformTurn();     
        endTurn();          
    }



    // Check if PD can attack player
    private bool CanAttack(Vector3Int playerPosition)
    {

        BasicAttack attack = getModule<BasicAttack>(0);
        Debug.Log($"Cost {attack.energyCost} {currentEnergy}");
        // Available cells
        var attackRange = attack.getCellsInRange(Position);


        return attackRange.Contains(playerPosition);
    }

    // Finds best position to get to player
    private Vector3Int FindBestPosition(Vector3Int playerPosition)
    {   

        BasicMovement move = getModule<BasicMovement>(1);

        // Available cells
        var moveRange = move.getCellsInRange(Position);

        int distance = Math.Abs(playerPosition.x - Position.x) + Math.Abs(playerPosition.y - Position.y);
        Vector3Int bestPosition = Position;
        
        if (distance == 1)
            return bestPosition;

        // Finding closest available cell
        foreach (var cell in moveRange)
        {
            int temp = Math.Abs(playerPosition.x - cell.x) + Math.Abs(playerPosition.y - cell.y);
            if (temp < distance && !tracker.IsOccupied(cell) && !tracker.OutOfBounds(cell))
            {
                distance = temp;
                bestPosition = cell;
            }
        }
        
        return bestPosition;
    }

    // Function of main logic
    private void MainLogic()
    {
        // Object of player
        PlayerCharacter player = tracker.getPlayer();

        // Get modules

        while (modules_.Any(m => hasEnoughEnergy((ActiveModule)m)))
        {
            //  Try attacking player
            if (CanAttack(player.Position) && useActiveModule(0, player.Position))
                Debug.Log("PD attacks the player");

            // Get closer to player
            else if (useActiveModule(1, FindBestPosition(player.Position)))
                Debug.Log("PD moves closer to player");

            // No available modules
            else
            {
                Debug.Log("Ran out of energy");
                break;
            }
        }
    }

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


    protected override bool TryGetTargetForModule(int moduleIndex, out Vector3Int target)
    {
        target = Position;
        PlayerCharacter player = tracker.getPlayer();
        if (player == null) return false;

        switch (moduleIndex)
        {
            case 0: 
                BasicAttack attack = getModule<BasicAttack>(0);
                if (attack == null) return false;
                var attackRange = attack.getCellsInRange(Position);
                if (attackRange.Contains(player.Position))
                {
                    target = player.Position;
                    return true;
                }
                break;

            case 1: 
                BasicMovement move = getModule<BasicMovement>(1);
                if (move == null) return false;
                var moveRange = move.getCellsInRange(Position);
                int minDist = int.MaxValue;
                Vector3Int best = Position;
                foreach (var cell in moveRange)
                {
                    if (cell == Position) continue;
                    if (tracker.OutOfBounds(cell) || tracker.IsOccupied(cell)) continue;
                    int dist = Mathf.Abs(player.Position.x - cell.x) + Mathf.Abs(player.Position.y - cell.y);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        best = cell;
                    }
                }
                if (best != Position)
                {
                    target = best;
                    return true;
                }
                break;
        }
        return false;
    }

}