using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


/// <summary>
/// ������� ������ ���� (BSM ����������)
/// </summary>
public class BasicShieldModule : ActiveModule
{
    private uint shieldGiven;
    public BasicShieldModule() : base()
    {
        price = 15;
        shieldGiven = 8;
        energyCost = 2;
        range = 0;
        AddConstKeywords(new CommonKeyword(), new DefenseKeyword());
        Icon_name = "BasicShieldModule";
    }
    public override string Description()
    {
        return $"Give {shieldGiven} shield to yourself.";
    }

    public BasicShieldModule(uint shield) : this()
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
}


