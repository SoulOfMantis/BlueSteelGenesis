using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class ElectrifiedBarrier : ActiveModule    
{
    uint shieldGiven;
    uint electrifiedDamage;
    uint electrifiedDuration;
    public ElectrifiedBarrier(uint shield, uint elecDamage, uint elecDuration)
    {
        energyCost = 3;
        range = 0;
        shieldGiven = shield;
        electrifiedDamage = elecDamage;
        electrifiedDuration = elecDuration;
        AddConstKeywords(new BossKeyword());
        //Icon_name = "ElectrifiedBarrier";
    }
    public override HashSet<ModuleKeyword> renewableKeywords()
    {
        var rk = base.renewableKeywords();
        rk.Add(new ShieldKeyword(shieldGiven, PossibleTargets.Self));
        rk.Add(new EnhanceKeyword<ElectrifiedModule>(PossibleTargets.Self, electrifiedDamage, electrifiedDuration));
        return rk;
    }
    public ElectrifiedBarrier() : this(9, 3, 3) { }
    public override async Task Effect(Character user, Vector3Int pos, ActionContext ctx)
    {
        await user.giveShield(shieldGiven, MakeContext(user, pos));
        await user.apply(new ElectrifiedModule(), MakeContext(user, user.Position.RightTop));
    }
}

