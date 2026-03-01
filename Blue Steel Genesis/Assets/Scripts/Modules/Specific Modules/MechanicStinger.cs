using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Активный модуль ядовитого жала - наносит урон и накладывает отравление
/// </summary>
public class MechanicStinger : ActiveModule
{
    private int hitDamage;
    private int poisonDamage;
    private int duration;

    public MechanicStinger()
    {
        hitDamage = 1;
        duration = 3;
        poisonDamage = 1;
        changeName("MechanicStinger");
    }
    public MechanicStinger(int damage, int duration, int hitDamage) : this()
    {
        this.hitDamage = hitDamage;
        poisonDamage = damage;
        this.duration = duration;
    }
    public override string Description()
    {
        return $"A mechanic weapon modeled after scorpion's stinger." +
            $"Deals {hitDamage} damage to adjacent creature and inflicts poison " +
            $"that deals {poisonDamage} damage at the start of it's turn for {duration} turns.";
    }
    public override async Task Effect(Character user, Vector3Int pos)
    {
        await user.strike(pos, hitDamage);
        PoisonModule poison = new PoisonModule(poisonDamage, duration);
        await user.apply(pos, poison);
    }
}
