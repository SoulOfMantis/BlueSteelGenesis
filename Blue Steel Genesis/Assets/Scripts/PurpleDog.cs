using UnityEngine;
using BlueSteelGenesis.Character_Modules;
using System;
using System.Linq;

public class PurpleDog : Enemy 
{
    // Purple Dog enemy constructor
    public PurpleDog() : base(5, 3, 60)
    {
        addModule(new BasicAttack());
        addModule(new BasicMovement());
    }

    // Begins PDs turn
    public override void startTurn()
    {
        base.startTurn();

        MainLogic();

        endTurn();
    }

    

    // Check if PD can attack player
    private bool CanAttack(Vector3Int playerPosition)
    {
        BasicAttack attack = getModule<BasicAttack>(0);
        
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
}