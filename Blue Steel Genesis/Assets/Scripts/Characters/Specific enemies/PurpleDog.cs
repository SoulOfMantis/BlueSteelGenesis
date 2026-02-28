using System.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;

public class PurpleDog : Enemy
{
    public TMP_Text healthDisplay;


    public PurpleDog() : base(5, 3, 60)
    {

        addModule(new BasicAttack());
        addModule(new BasicMovement());

        SetPriorityModules();
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

    protected override bool TryGetTargetForZero(out Vector3Int targetPos)
    {
        targetPos = tracker.getPlayer().Position;
        return priorityModules[0].getCellsInRange(Position).Contains(targetPos);
    }
    protected override bool TryGetTargetForOne(out Vector3Int targetPos)
    {
        var playerPosition = tracker.getPlayer().Position;
        var moveRange = priorityModules[1].getCellsInRange(Position);
        int distance = Math.Abs(playerPosition.x - Position.x) + Math.Abs(playerPosition.y - Position.y);
        targetPos = Position;
        if (distance == 1) return false; //Нет смысла двигаться!

        foreach (var cell in moveRange)
        {
            int temp = Math.Abs(playerPosition.x - cell.x) + Math.Abs(playerPosition.y - cell.y);
            if (temp < distance && !tracker.IsOccupied(cell) && !tracker.OutOfBounds(cell))
            {
                distance = temp;
                targetPos = cell;
            }
        }
        return true;
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