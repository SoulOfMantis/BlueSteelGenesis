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

    // Check if PD can attack player
    private bool CanAttack(Vector3Int playerPosition, BasicAttack attack)
    {
        // Check if PD has enough energy
        if (!hasEnoughEnergy(attack)) return false;

        // Available cells
        var attackRange = attack.getCellsInRange(Position);
        
        return attackRange.Contains(playerPosition);
    }

    private Vector3Int FindBestPosition(Vector3Int playerPosition, BasicMovement move)
    {
        return new Vector3Int(3, 4, 5);
    }

    // Function of main logic
    private void MainLogic()
    {
        // Object of player
        PlayerCharacter player = this.GetComponent<PlayerCharacter>();

        // Postions of characters
        Vector3Int playerPosition = player.Position;
        Vector3Int dogPosition = this.Position;

        // Get modules
        BasicAttack attack = getModule<BasicAttack>(0);
        BasicMovement move = getModule<BasicMovement>(1);

        while (currentEnergy > 0)
        {
            //  Try attacking player
            if (CanAttack(playerPosition, attack))
            {
                attack.Effect(this, playerPosition);
                currentEnergy -= attack.energyCost;
                Debug.Log("PD attacks the player");
            }
            else
            {
                
            }

        }
    }

}