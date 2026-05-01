using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class RevitalizingBite : ActiveModule    
{
    uint damage;
    uint regenValue;
    uint regenDuration;
    public RevitalizingBite() :base()
    {
        AddConstKeywords(new BossKeyword(), new LimitedPerBattleKeyword());
        range = 1;
        energyCost = 4;
        damage = 8;
        regenDuration = 4;
        regenValue = 3;
    }
    public override HashSet<ModuleKeyword> renewableKeywords()
    {
        var rk = base.renewableKeywords();
        rk.Add(new EnhanceKeyword<AutoRepairModule>(PossibleTargets.Self, regenValue, regenDuration));
        return rk;
    }
    public override async Task Effect(Character user, Vector3Int pos, ActionContext ctx)
    {
        await user.strike(pos, damage, MakeContext(user, pos));
        await user.apply(new AutoRepairModule(regenValue, regenDuration), MakeContext(user, user.Position.RightTop));
    }
    public override string Description()
    {
        return $"Deals {damage} damage to the adjacent creature.\n" + base.Description();
    }
}

