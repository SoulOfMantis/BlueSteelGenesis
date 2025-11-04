using UnityEngine;

/// <summary>
/// Ѕазовый модуль движени€ (только дл€ демонстрации)
/// </summary>
public class BasicMovement : Module
{
    public override void Execute()
    {
        Debug.Log("BasicMovement");
    }

    public override void Initialize()
    {
        Debug.Log("BasicMovement initialized");
    }
}