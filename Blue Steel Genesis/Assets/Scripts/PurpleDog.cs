using UnityEngine;
using BlueSteelGenesis.Character_Modules;
using System.Threading;
using System;

public class PurpleDog : Enemy 
{
    // Purple Dog enemy constructor
    public PurpleDog() : base(5, 3)
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
    private bool CanAttack(Vector3Int playerPosition, BasicAttack attack)
    {
        // Check if PD has enough energy
        if (!hasEnoughEnergy(attack)) return false;

        // Available cells
        var attackRange = attack.getCellsInRange(Position);
        
        return attackRange.Contains(playerPosition);
    }

    // Finds best position to get to player
    private Vector3Int FindBestPosition(Vector3Int playerPosition, BasicMovement move)
    {   
        // Available cells
        var moveRange = move.getCellsInRange(Position);

        int distance = 1000;
        Vector3Int bestPosition = new Vector3Int();

        // Finding closest available cell
        foreach (var i in moveRange)
        {
            int temp = Math.Abs(playerPosition.x - i.x) + Math.Abs(playerPosition.y - i.y);
            if (temp < distance && temp != 0)
            {
                distance = temp;
                bestPosition = i;
            }
        }

        return bestPosition;
    }

    // Function of main logic
    private void MainLogic()
    {
        // Object of player
        PlayerCharacter player = this.GetComponent<PlayerCharacter>();

        // Postion of player
        Vector3Int playerPosition = player.Position;

        // Get modules
        BasicAttack attack = getModule<BasicAttack>(0);
        BasicMovement move = getModule<BasicMovement>(1);

        while (true)
        {
            //  Try attacking player
            if (CanAttack(playerPosition, attack) && useActiveModule(0, playerPosition))
                Debug.Log("PD attacks the player");

            // Get closer to player
            else if (!CanAttack(playerPosition, attack) && useActiveModule(1, FindBestPosition(playerPosition, move)))
                Debug.Log("PD moves closer to player");

            // No energy
            else
            {
                Debug.Log("Ran out of energy");
                break;
            }
        }
    }
}