using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// �������� ������ ��������� ���� - ������� ���� � ����������� ����������
/// </summary>
public class MechanicStinger : ActiveModule
{
    private int hitDamage;
    private int poisonDamage;
    private int duration;

    public MechanicStinger() : base()
    {
        energyCost = 1;
        range = 1;
        hitDamage = 1;
        duration = 3;
        AddKeywords(new List<string> { "Offense", "Common" });
        poisonDamage = 1;
        Icon_name = "Module_mechanical_sting2";
        AddConstKeywords(new OffenseKeyword(), new CommonKeyword());
    }
    public MechanicStinger(int damage, int duration, int hitDamage) : this()
    {
        this.hitDamage = hitDamage;
        poisonDamage = damage;
        this.duration = duration;
    }
    public override HashSet<ModuleKeyword> renewableKeywords()
    {
        var rk = base.renewableKeywords();
        rk.Add(new InflictKeyword<PoisonModule>(poisonDamage, duration));
        return rk;
    }
    public override string Description()
    {
        return $"A mechanic weapon modeled after scorpion's stinger. " +
            $"Deals {hitDamage} damage to adjacent creature and inflicts poison " +
            $"({poisonDamage} damage for {duration} turns).";
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
