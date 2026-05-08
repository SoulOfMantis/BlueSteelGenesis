using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ������� ������ ����� (BAM ����������)
/// </summary>
public class BasicAttackModule : ActiveModule
{
    protected uint hitDamage;

    public BasicAttackModule() : base()
    {
        price = 15;
        hitDamage = 2;
        energyCost = 1;
        range = 1;
        Icon_name = "BasicAttackModule";
        AddConstKeywords(new CommonKeyword(), new OffenseKeyword());
    }
    public override string Description()
    {
        return $"Deal {hitDamage} damage to the adjacent creature.\n" + base.Description();
    }

    public BasicAttackModule(uint hitDamage) : this()
    {
        this.hitDamage = hitDamage;
    }
    public override async Task Effect(Character user, Vector3Int pos, ActionContext ctx)
    {
        await user.strike(pos, hitDamage, MakeContext(user, pos));
        Debug.Log("BAM executed");
    }

    protected override bool checkFinalPosition(Vector3Int pos)
    {
        return Entity.tracker.IsOccupied(pos);
    }
    public override bool checkPosition(Character user, Vector3Int pos)
    {
        return base.checkPosition(user, pos) && !user.Position.Contains(pos);
    }
}


