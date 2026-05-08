using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


/// <summary>
/// ������� ������ ���� (BSM ����������)
/// </summary>
public class BasicShield : ActiveModule
{
    private uint shieldGiven;
    public BasicShield() : base()
    {
        price = 15;
        shieldGiven = 8;
        energyCost = 2;
        maxUpgradeLevel = 3;
        range = 0;
        AddConstKeywords(new CommonKeyword(), new DefenseKeyword());
    }
    public override string Description()
    {
        return $"Give {shieldGiven} shield to yourself.";
    }

    public BasicShield(uint shield) : this()
    {
        shieldGiven = shield;
    }
    public override async Task Effect(Character user, Vector3Int pos, ActionContext ctx)
    {
        await user.giveShield(shieldGiven, MakeContext(user, pos));
        Debug.Log("BSM executed");
    }
    public override HashSet<ModuleKeyword> renewableKeywords()
    {
        var rk = base.renewableKeywords();
        rk.Add(new ShieldKeyword(shieldGiven, PossibleTargets.Self));
        return rk;
    }
    public override void ApplyUpgrade()
    {
        base.ApplyUpgrade();
        shieldGiven += 2;
        price += 10;
    }

}


