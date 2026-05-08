using System.Threading.Tasks;
using UnityEngine;

public class FollowUpAttack : PassiveModule
{
    uint damage;
    uint hits;

    public FollowUpAttack(uint damage, uint hits) {
        price = 50;
        this.damage = damage;
        this.hits = hits;
        maxUpgradeLevel = 2;
        range = 1;
        triggerType = TriggerType.OnStrike;
        AddConstKeywords(new CommonKeyword(), new OffenseKeyword(), new FollowUpKeyword());
    }
    public FollowUpAttack() : this(1, 1) {}

    public override async Task Effect(Character user, Vector3Int pos, ActionContext ctx) {
        ctx.ThrowIfIncomplete();
        if (ctx.module.HasAnyKeywords(new CounterattackKeyword(), new FollowUpKeyword()) ||
                !Entity.tracker.isAlive(ctx.targetEntity))
            return;

        await user.strike(ctx.targetPosition, damage, MakeContext(user, ctx.targetPosition));
    }
    public override string Description() =>
        $"On every strike, triggers a follow-up attack that deals {damage} damage.\n" + base.Description();
    public override void ApplyUpgrade()
    {
        base.ApplyUpgrade();
        hits += 1;
        price += 50;
    }

}
