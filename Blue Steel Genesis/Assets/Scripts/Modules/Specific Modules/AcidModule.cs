using System;
using System.Threading.Tasks;
using UnityEngine;

public class AcidModule : NegativeStatusModule
{
    uint damage;
    public AcidModule(uint dmg, uint dur) : base()
    {
        damage = dmg;
        turnsLeft.Value = dur;
        AddConstKeyword(new AcidKeyword());
        triggerType = TriggerType.OnTurnEnd;
    }
    public override string Description()
    {
        return $"Take {damage} damage at the end of next {turnsLeft} turns.\n" + base.Description();
    }
    public override bool IsExpired() => turnsLeft == 0;
    public override void Refresh(StatusModule other)
    {
        if (other is AcidModule a)
        {
            damage += a.damage;
            turnsLeft.Value = Math.Min(turnsLeft, a.turnsLeft);
            UpdateTooltipIfCurrent();
        }
    }
    public override async Task Effect(Character user, Vector3Int pos)
    {
        await user.damage(damage);
        turnTick();
    }
}
