using System.Threading.Tasks;
using UnityEngine;


/// <summary>
/// Базовый модуль щита (BSM сокращение)
/// </summary>
public class BasicShield : ActiveModule
{
    private uint shieldGiven;
    public BasicShield()
    {
        changeName("BasicShield");
        shieldGiven = 3;
        energyCost = 1;
        range = 0;
        Name = "BasicShield";
    }
    public override string Description()
    {
        return $"Give {shieldGiven} shield to yourself.";
    }

    public BasicShield(uint shield) : this()
    {
        shieldGiven = shield;
    }
    public override async Task Effect(Character user, Vector3Int pos)
    {
        await user.giveShield(shieldGiven);
        Debug.Log("BSM executed");
    }
}


