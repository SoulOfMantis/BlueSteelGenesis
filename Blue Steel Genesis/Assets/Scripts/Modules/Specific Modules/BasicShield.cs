using System.Threading.Tasks;
using UnityEngine;


/// <summary>
/// Базовый модуль щита (BSM сокращение)
/// </summary>
public class BasicShield : ActiveModule
{
    private int shieldGiven;

    public BasicShield(int shield = 1)
    {
        this.shieldGiven = shield;
        energyCost = 1;
        range = 0;
        Name = "BasicShield";
        Description = "The most basic defense there is.";

    }

    public override async Task Effect(Character user, Vector3Int pos)
    {
        await user.giveShield(shieldGiven);
        Debug.Log("BSM executed");
    }
}


