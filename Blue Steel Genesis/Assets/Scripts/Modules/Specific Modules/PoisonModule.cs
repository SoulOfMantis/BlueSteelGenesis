using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// ��������� ������ ���  - ������� ���� ��� ������ ����
/// </summary>
public class PoisonModule : StatusModule
{
    private int poisonDamage;
    public PoisonModule() :base()
    {
        triggerType = TriggerType.OnTurnStart;
        poisonDamage = 1;
        turnsLeft = 3;
        AddKeywords(new List<string> { "Poison", "Negative" });
    }
    public PoisonModule(int damage, int duration) : this()
    {
        poisonDamage = damage;
        turnsLeft = duration;
        Icon_name = "Module_poison";
    }
    public override string Description()
    {
        return $"One of the most infamous ways to kill. " +
            $"You will take {poisonDamage} damage at the start of your turn for another {turnsLeft} turns.";
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