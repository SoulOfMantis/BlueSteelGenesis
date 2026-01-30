using System.Threading.Tasks;
using UnityEngine;


/// <summary>
/// Базовый модуль атаки (BAM сокращение)
/// </summary>
public class BasicAttack : ActiveModule
{
    private int hitDamage;

    public BasicAttack(int hitDamage = 1)
    {
        this.hitDamage = hitDamage;
        energyCost = 1;
        range = 1;
    }

    public override async Task Effect(Character user, Vector3Int pos)
    {
        await user.strike(pos, hitDamage); //пока 1 урон
        Debug.Log("BAM executed");
    }

    protected override bool checkFinalPosition(Vector3Int pos)
    {
        return base.checkFinalPosition(pos) && Character.tracker.IsOccupiedByCharacter(pos);
    }
    public override bool checkPosition(Character user, Vector3Int pos)
    {
        return base.checkPosition(user, pos) && (pos != user.Position);
    }

}


