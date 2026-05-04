using System;
using System.Threading.Tasks;
using UnityEngine;

public class EmergencyShield : PassiveModule
{
    uint shieldPercent;

    public EmergencyShield(uint shieldPercent) {
        this.shieldPercent = shieldPercent;
        range = 0;
        triggerType = TriggerType.OnHealthDamage;
        AddConstKeywords(new CommonKeyword(), new DefenseKeyword(), new LimitedPerTurnKeyword());
    }
    public EmergencyShield() : this(20) {}

    public override async Task Effect(Character user, Vector3Int pos, ActionContext ctx) {
        ctx.ThrowIfIncomplete();
        ctx.ThrowIfNoActionData<uint>();

        uint shield_amount = Math.Min(
            (user.maxHealth - user.currentHealth) * shieldPercent / 100,
            ctx.GetActionData<uint>().Value * 2
        );
        await user.giveShield(shield_amount, MakeContext(user, pos));
    }
    public override string Description() =>
        $"When health is lost due to damage, grants a shield equal to {shieldPercent}% of health lost, up to twice the received damage.\n" + base.Description();
}
