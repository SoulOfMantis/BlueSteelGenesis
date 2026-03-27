using System;
using System.Threading.Tasks;
using UnityEngine;

public class BurnModule : NegativeStatusModule
{
    uint damage;
    public BurnModule(uint dmg, uint dur) : base()
    {
        damage = dmg;
        turnsLeft.Value = dur;
        //To do add BurnKeyword
        //AddConstKeyword(new Keyword()); 
        triggerType = TriggerType.OnMove;
        Icon_name = "BurnModule";
    }
    public override string Description()
    {
        return $"Take {damage} damage when you move next {turnsLeft} times.\n" + base.Description();
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
    public override async Task Effect(Character user, Vector3Int pos)
    {
        await user.damage(damage);
        turnTick();
    }
}
