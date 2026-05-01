using System;
using System.Threading.Tasks;
using UnityEngine;

public class BurnModule : NegativeStatusModule
{
    uint damage;
    public BurnModule() : base()
    {
        damage = 1;
        turnsLeft.Value = 3;
        AddConstKeyword(new BurnKeyword());
        triggerType = TriggerType.OnMove;
        Icon_name = "BurnModule";
    }
    public BurnModule(uint dmg, uint dur) : this()
    {
        damage = dmg;
        turnsLeft.Value = dur;
    }
    public override string Description()
    {
        return $"Deals {damage} damage when next {turnsLeft} times when target moves.\n" + base.Description();
    }
    public override bool IsExpired() => turnsLeft == 0;
    public override void Refresh(StatusModule other)
    {
        if (other is BurnModule b)
        {
            damage += b.damage;
            turnsLeft.Value = Math.Max(turnsLeft-1, 1);
        }
    }
    public override async Task Effect(Character user, Vector3Int pos, ActionContext ctx)
    {
        await user.damage(damage, MakeContext(user, pos));
        turnTick();
    }
}
