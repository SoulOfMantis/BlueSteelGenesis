using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class ElectrifiedModule : PositiveStatusModule
{
    uint damage;
    public ElectrifiedModule() : this(1, 1) {    }
    public ElectrifiedModule(uint dmg, uint dur)
    {
        damage = dmg;
        turnsLeft.Value = dur;
        AddConstKeywords(new LightningKeyword(), new CounterattackKeyword());
        triggerType = TriggerType.OnDamage;
    }
    public override string Description()
    {
        return $"Deals {damage} damage to the attacker after next {turnsLeft} attacks.\n" + base.Description();
    }
    public override bool IsExpired() => turnsLeft == 0;
    public override void Refresh(StatusModule other)
    {
        if (other is ElectrifiedModule e)
        {
            damage += e.damage;
            UpdateTooltipIfCurrent();
        }
    }
    public override async Task Effect(Character user, Vector3Int pos, ActionContext ctx)
    {
        ctx.ThrowIfIncomplete();
        if (user == ctx.actor ||
            ctx.module.HasAnyKeywords(new CounterattackKeyword()) ||
            !Entity.tracker.isAlive(ctx.actor))
            return;

        await ctx.actor.damage(damage, MakeContext(user, ctx.actor.Position.RightTop));
        turnTick();
    }
}
