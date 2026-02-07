using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Активный модуль ядовитого жала - наносит урон и накладывает отравление
/// </summary>
public class MechanicStinger : BasicAttack
{
    protected int poisonDamage;
    protected int duration;

    public MechanicStinger(int damage = 1, int duration = 3, int hitDamage = 1) : base(hitDamage)
    {
        poisonDamage = damage;
        this.duration = duration;
        Name = "MechanicStinger";
    }
    public override string Description()
    {
        return $"A mechanic weapon modeled after scorpion's stinger." +
            $"Deals {hitDamage} damage to adjacent creature and inflicts poison " +
            $"that deals {poisonDamage} damage at the start of it's turn for {duration} turns.";
    }
    public override async Task Effect(Character user, Vector3Int pos)
    {
        await base.Effect(user, pos);
        PoisonModule poison = new PoisonModule(poisonDamage, duration);
        await user.apply(pos, poison);
    }
}
