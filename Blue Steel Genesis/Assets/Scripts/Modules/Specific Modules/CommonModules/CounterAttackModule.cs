using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class CounterAttack : PassiveModule
{
    public uint damage;
    public CounterAttack() : this(3) {}
    public CounterAttack(uint dmg)
    {
        price = 30;
        damage = dmg;
        range = 1;
        triggerType = TriggerType.OnDamage;
        AddConstKeyword(new CounterattackKeyword());
    }
    public override async Task Effect(Character user, Vector3Int pos, ActionContext ctx)
    {
        ctx.ThrowIfIncomplete();
        if (user == ctx.actor ||
            ctx.module.HasAnyKeywords(new CounterattackKeyword()) ||
            !Entity.tracker.isAlive(ctx.actor))
            return;

        var target = ctx.actor;
        Vector3Int? target_pos = getCellsInRange(user.Position)
            .Intersect(target.Position)
            .Cast<Vector3Int?>().FirstOrDefault();
        if (target_pos.HasValue)
            await user.strike(target_pos.Value, damage, MakeContext(user, pos));
    }
    public override string Description() {
        return $"Deals {damage} to the attacker." + base.Description();
    }
}

