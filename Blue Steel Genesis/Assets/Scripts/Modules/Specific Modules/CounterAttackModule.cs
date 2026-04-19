using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class CounterAttack : PassiveModule
{
    public uint damage;
    public CounterAttack() : this(3) {}
    public CounterAttack(uint dmg)
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
            Vector3Int? target_pos = getCellsInRange(user.Position)
                .Intersect(target.Position)
                .Cast<Vector3Int?>().FirstOrDefault();
            if (target_pos.HasValue)
            {
                ActionContext action = new ActionContext(user, "counterattack", context, target);
                await user.strike(target_pos.Value, damage, action);                
            }
        }
    }
    public override string Description() {
        return $"Deals {damage} to the attacker";
    }
}

