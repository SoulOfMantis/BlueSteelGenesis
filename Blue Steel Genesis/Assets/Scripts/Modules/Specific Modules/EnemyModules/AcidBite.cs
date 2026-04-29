using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AcidBite : ActiveModule
{
    uint hitDamage = 3;
    uint acidDamage = 1;
    uint acidDuration = 1;
    public AcidBite() : base()
    {
        Icon_name = "AcidBiteModule";
        range = 1;
        energyCost = 2;
        AddConstKeywords(new OffenseKeyword());
    }
    public override HashSet<ModuleKeyword> renewableKeywords()
    {
        var res = base.renewableKeywords();
        res.Add(new InflictKeyword<AcidModule>(PossibleTargets.Target, acidDamage, acidDuration));
        return res;
    }
    public override async Task Effect(Character user, Vector3Int pos, ActionContext ctx)
    {
        await user.strike(pos, hitDamage, MakeContext(user, pos));
        await user.apply(pos, new AcidModule(acidDamage, acidDuration), MakeContext(user, pos));
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

