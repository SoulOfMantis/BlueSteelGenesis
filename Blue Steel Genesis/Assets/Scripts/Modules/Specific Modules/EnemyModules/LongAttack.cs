using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LongAttack : ActiveModule
{
    uint hitDamage = 3;

    public LongAttack() : base()
    {
        range = 1;
        energyCost = 2;
        AddConstKeywords(new OffenseKeyword());
    }
    public override HashSet<ModuleKeyword> renewableKeywords()
    {
        var res = base.renewableKeywords();
        res.Add(new InflictKeyword<AcidModule>(PossibleTargets.Target));
        return res;
    }
    public override async Task Effect(Character user, Vector3Int pos)
    {
        await user.strike(pos, hitDamage);
        await user.apply(pos, new AcidModule(acidDamage, acidDuration));
    }
    public override string Description()
    {
        return $"Deals {hitDamage} damage to the adjacent cell.\n" + base.Description();
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

