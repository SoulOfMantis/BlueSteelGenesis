using System.Threading.Tasks;
using UnityEngine;


/// <summary>
/// Базовый модуль атаки (BAM сокращение)
/// </summary>
public class BasicAttack : ActiveModule
{
    protected uint hitDamage;

    public BasicAttack()
    {
        hitDamage = 1;
        energyCost = 1;
        range = 1;
        changeName("BasicAttack");
        //Icon_name = "...";
    }
    public override string Description()
    {
        return $"Deal {hitDamage} to the adjacent creature.";
    }

    public BasicAttack(uint hitDamage) : this()
    {
        this.hitDamage = hitDamage;
    }
    public override async Task Effect(Character user, Vector3Int pos)
    {
        await user.strike(pos, hitDamage);
        Debug.Log("BAM executed");
    }

    protected override bool checkFinalPosition(Vector3Int pos)
    {
        return Character.tracker.IsOccupiedByCharacter(pos);
    }
    public override bool checkPosition(Character user, Vector3Int pos)
    {
        return base.checkPosition(user, pos) && (pos != user.Position);
    }

}


