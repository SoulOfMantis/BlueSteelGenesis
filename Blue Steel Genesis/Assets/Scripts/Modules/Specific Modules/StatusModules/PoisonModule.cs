using System;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// ��������� ������ ���  - ������� ���� ��� ������ ����
/// </summary>
public class PoisonModule : NegativeStatusModule
{
    protected uint poisonDamage;
    public PoisonModule(uint damage, uint duration)
    {
        triggerType = TriggerType.OnTurnStart;
        poisonDamage = damage;
        turnsLeft.Value = duration;
        AddConstKeyword(new PoisonKeyword());
        Icon_name = "Module_poison";
    }
    public PoisonModule() : this(1, 3) {}
    public override string Description()
    {
        return $"Deals {poisonDamage} damage to the target at the start next {turnsLeft} turns." + base.Description();
    }
    public override async Task Effect(Character user, Vector3Int pos, ActionContext ctx)
    {
        await user.damage(poisonDamage, MakeContext(user, pos));
        Debug.Log($"Poison dealt {poisonDamage} damage to {user.GetType().Name}");
        turnTick();
    }

    public override void Refresh(StatusModule other)
    {
        if (other is PoisonModule p)
        {
            turnsLeft += p.turnsLeft;
            poisonDamage = Math.Max(poisonDamage, turnsLeft / 5) + p.poisonDamage/2;
            UpdateTooltipIfCurrent();
        }
    }

    public override bool IsExpired()
    {
        return turnsLeft <= 0;
    }
}