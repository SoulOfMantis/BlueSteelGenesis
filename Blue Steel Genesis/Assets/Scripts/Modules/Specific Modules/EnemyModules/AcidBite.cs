using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AcidBite : ActiveModule
{
    uint hitDamage = 10;
    uint acidDamage = 3;
    uint acidDuration = 2;
    public AcidBite() : base()
    {
        Icon_name = "AcidBiteModule";
        range = 1;
        energyCost = 3;
        AddConstKeywords(new OffenseKeyword());
    }
    public override HashSet<ModuleKeyword> renewableKeywords()
    {
        var res = base.renewableKeywords();
        res.Add(new InflictKeyword<AcidModule>(PossibleTargets.Target, acidDamage, acidDuration));
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

