using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CounterAttack : PassiveModule
{
    public uint damage;
    public CounterAttack(uint dmg = 3)
    {
        damage = dmg;
        range = 1;
        triggerType = TriggerType.OnDamage;
    }
    public override async Task Effect(Character user, Vector3Int pos)
    {
        if (context == null) throw new ArgumentNullException();
        if (context.prevActionContext == null) return;
        if (context.prevActionContext.actionName != "strike" || context.prevActionContext.prevActionContext != null) return;
        if (Character.tracker.isAlive(context.prevActionContext.acting))
        {
            var target = context.prevActionContext.acting;
            if (getCellsInRange(user.Position).Contains(target.Position))
            {
                ActionContext action = new ActionContext(user, "counterattack", context, target);
                await user.strike(target.Position, damage, action);                
            }
        }
    }
    public override string Description() {
        return $"Deals {damage} to the attacker";
    }
}

