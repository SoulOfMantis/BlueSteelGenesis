using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Активный модуль ядовитого жала - наносит урон и накладывает отравление
/// </summary>
public class PoisonStinger : ActiveModule
{
    private int hitDamage;
    private int poisonDamage;
    private int duration;
    public PoisonStinger()
    {
        hitDamage = 1;
        poisonDamage = 1;
        duration = 3;
        changeName("PoisonStinger");
    }
    public PoisonStinger(int damage, int duration, int hitDamage) : this()
    {
        poisonDamage = damage;
        this.duration = duration;
        this.hitDamage = hitDamage;
    }

    public override async Task Effect(Character user, Vector3Int pos)
    {
        await user.strike(pos, hitDamage);
        PoisonModule poison = new PoisonModule(poisonDamage, duration);
        await user.apply(pos, poison);
    }
}
