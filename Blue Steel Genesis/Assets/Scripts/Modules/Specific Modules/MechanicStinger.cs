using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Активный модуль ядовитого жала - наносит урон и накладывает отравление
/// </summary>
public class MechanicStinger : ActiveModule
{
    private uint hitDamage;
    private uint poisonDamage;
    private uint duration;

    public MechanicStinger() : base()
    {
        range = 1;
        hitDamage = 1;
        duration = 3;
        poisonDamage = 1;
        Icon_name = "Module_mechanical_sting2";
        AddConstKeywords(new OffenseKeyword(), new CommonKeyword());
    }
    public MechanicStinger(uint damage, uint duration, uint hitDamage) : this()
    {
        this.hitDamage = hitDamage;
        poisonDamage = damage;
        this.duration = duration;
    }
    public override HashSet<ModuleKeyword> renewableKeywords()
    {
        var rk = base.renewableKeywords();
        rk.Add(new InflictKeyword<PoisonModule>(PossibleTargets.Target, poisonDamage, duration));
        return rk;
    }
    public override string Description()
    {
        return $"Deals {hitDamage} damage to the adjacent creature.\n" + base.Description();
    }
    public override async Task Effect(Character user, Vector3Int pos)
    {
        await user.strike(pos, hitDamage);
        PoisonModule poison = new PoisonModule(poisonDamage, duration);
        await user.apply(pos, poison);
    }

    protected override bool checkFinalPosition(Vector3Int pos)
    {
        return Character.tracker.IsOccupiedByCharacter(pos);
    }
    public override bool checkPosition(Character user, Vector3Int pos)
    {
        return base.checkPosition(user, pos) && (pos != user.Position);
    }

}
