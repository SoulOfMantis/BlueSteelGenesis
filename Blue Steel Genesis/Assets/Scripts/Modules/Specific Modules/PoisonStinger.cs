using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Активный модуль ядовитого жала - наносит урон и накладывает отравление
/// </summary>
public class PoisonStinger : BasicAttack
{
    private int poisonDamage;
    private int duration;

    public PoisonStinger(int damage = 1, int duration = 3, int hitDamage = 1) : base(hitDamage)
    {
        poisonDamage = damage;
        this.duration = duration;
    }

    public override async Task Effect(Character user, Vector3Int pos)
    {
        await base.Effect(user, pos);
        PoisonModule poison = new PoisonModule(poisonDamage, duration);
        await user.apply(pos, poison);
    }
}
