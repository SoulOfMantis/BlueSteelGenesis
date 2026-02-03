using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CounterAttack : PassiveModule
{
    public int damage;
    public CounterAttack(int dmg = 3)
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

}

