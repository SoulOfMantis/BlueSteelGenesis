using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// ѕассивный модуль €да  - наносит урон при начале хода
/// </summary>
public class PoisonModule : StatusModule
{
    private int poisonDamage;

    public PoisonModule(int damage = 1, int duration = 3)
    {
        triggerType = TriggerType.OnTurnStart;
        poisonDamage = damage;
        turnsLeft = duration;
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