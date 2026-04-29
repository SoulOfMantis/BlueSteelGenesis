using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;

public class BasicStrongAttack : ActiveModule
{
    protected uint hitDamage;

    public BasicStrongAttack() : base()
    {
        hitDamage = 5;
        energyCost = 3;
        range = 1; 
        AddConstKeywords(new CommonKeyword(), new OffenseKeyword());
        //Icon_name = "...";
    }
    public override string Description()
    {
        return $"Deal {hitDamage} to the adjacent creature.";
    }

    public BasicStrongAttack(uint hitDamage) : this()
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
        return Character.tracker.IsOccupiedByCharacter(pos);
    }
    public override bool checkPosition(Character user, Vector3Int pos)
    {
        return base.checkPosition(user, pos) && !user.Position.Contains(pos);
    }

}


