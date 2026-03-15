using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// ��������� ������ ���  - ������� ���� ��� ������ ����
/// </summary>
public class PoisonModule : NegativeStatusModule
{
    protected uint poisonDamage;
    public PoisonModule() :base()
    {
        triggerType = TriggerType.OnTurnStart;
        poisonDamage = 1;
        turnsLeft = 3;
        AddConstKeyword(new PoisonKeyword());
        Icon_name = "Module_poison";
    }
    public PoisonModule(uint damage = 1, uint duration = 3) :this()
    {
        poisonDamage = damage;
        turnsLeft = duration;
    }
    public override string Description()
    {
        return $"Target takes {poisonDamage} damage at the start of the turn for another {turnsLeft} turns." + base.Description();
    }
    public override async Task Effect(Character user, Vector3Int pos)
    {
        await user.damage(poisonDamage);
        Debug.Log($"Poison dealt {poisonDamage} damage to {user.GetType().Name}");
        turnTick();
    }

    public override void Refresh(StatusModule other)
    {
        if (other is PoisonModule p) turnsLeft += p.turnsLeft;
    }

    public override bool IsExpired()
    {
        return turnsLeft <= 0;
    }
}