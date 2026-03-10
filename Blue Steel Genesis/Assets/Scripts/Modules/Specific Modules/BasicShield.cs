using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


/// <summary>
/// Базовый модуль щита (BSM сокращение)
/// </summary>
public class BasicShield : ActiveModule
{
    private int shieldGiven;
    public BasicShield() : base()
    {
        shieldGiven = 3;
        energyCost = 1;
        range = 0;
        AddKeywords(new CommonKeyword(), new DefenseKeyword(), new ShieldKeyword());
    }
    public override string Description()
    {
        return $"Give {shieldGiven} shield to yourself.";
    }

    public BasicShield(int shield) : this()
    {
        shieldGiven = shield;
    }
    public override async Task Effect(Character user, Vector3Int pos)
    {
        await user.giveShield(shieldGiven);
        Debug.Log("BSM executed");
    }
}


