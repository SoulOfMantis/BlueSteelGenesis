using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// �������� ������ ��������� ���� - ������� ���� � ����������� ����������
/// </summary>
public class MechanicStinger : ActiveModule
{
    private uint hitDamage;
    private uint poisonDamage;
    private uint duration;

    public MechanicStinger() : base()
    {
        price = 20;
        energyCost = 3;
        range = 1;
        hitDamage = 2;
        duration = 2;
        poisonDamage = 1;
        Icon_name = "MechanicStinger";
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
    public override async Task Effect(Character user, Vector3Int pos, ActionContext ctx)
    {
        await user.strike(pos, hitDamage, MakeContext(user, pos));
        PoisonModule poison = new PoisonModule(poisonDamage, duration);
        await user.apply(pos, poison, MakeContext(user, pos));
    }

    protected override bool checkFinalPosition(Vector3Int pos)
    {
        return Entity.tracker.IsOccupied(pos);
    }
    public override bool checkPosition(Character user, Vector3Int pos)
    {
        return base.checkPosition(user, pos) && !user.Position.Contains(pos);
    }

}
