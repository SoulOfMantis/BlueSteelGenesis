using UnityEngine;

/// <summary>
/// Ѕазовый модуль атаки (только дл€ демонстрации)
/// </summary>
public class BasicAttack : Module
{
    public override void Execute()
    {
        Debug.Log("BasicAttack");
    }

    public override void Initialize()
    {
        Debug.Log("BasicAttack initialized");
    }
}