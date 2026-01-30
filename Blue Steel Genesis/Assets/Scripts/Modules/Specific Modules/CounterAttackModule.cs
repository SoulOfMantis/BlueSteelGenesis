using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CounterAttack : PassiveModule
{
    public int damage;
    public CounterAttack(int dmg)
    {
        damage = dmg;
        range = 1;
        triggerType = TriggerType.OnDamage;
    }
    public override async Task Effect(Character user, Vector3Int pos)
    {
        if (context == null) throw new ArgumentNullException();
        if (context.prevActionContext == null) return;
        if (context.prevActionContext.actionName == "strike" && Character.tracker.isAlive(context.prevActionContext.acting))
        {
            var target = context.prevActionContext.acting;
            if (getCellsInRange(user.Position).Contains(target.Position))
            {
                await user.strike(target.Position, damage, context);
            }
        }
    }
}

