using BlueSteelGenesis.Character_Modules;
using System;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class PurpleDog : Enemy 
{
    public TMP_Text healthDisplay;

    // Purple Dog enemy constructor
    public PurpleDog() : base(5, 3, 60)
    {
        addModule(new BasicAttack());
        addModule(new BasicMovement());
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
    public override async Task startTurn()
    {
        await base.startTurn();
        
        if (currentHealth > 0)
            await MainLogic();

        await endTurn();
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
    private async Task MainLogic()
    {
        // Object of player
        PlayerCharacter player = tracker.getPlayer();

        // Get modules

        while (modules_.Any(m => hasEnoughEnergy((ActiveModule)m)))
        {
            //  Try attacking player
            if (CanAttack(player.Position) && await useActiveModule(0, player.Position))
                Debug.Log("PD attacks the player");

            // Get closer to player
            else if (await useActiveModule(1, FindBestPosition(player.Position)))
                Debug.Log("PD moves closer to player");

            // No available modules
            else
            {
                Debug.Log("Ran out of energy");
                break;
            }
        }
    }

    public override async Task damage(int dmg)
    {
        Debug.Log($"Собака получила {dmg} урона!");
        await base.damage(dmg);
        updateHealth();
        //play taking damage animation
    }

    public override async Task heal(int hp)
    {
        Debug.Log($"Собака восстановила {hp} здоровья!");
        await base.heal(hp);
        updateHealth();
        //play healing animation
    }
    public override async Task startBattle()
    {
        await base.startBattle();
        updateHealth();
    }

    }