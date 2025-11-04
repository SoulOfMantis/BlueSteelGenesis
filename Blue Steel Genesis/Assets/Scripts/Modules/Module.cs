using UnityEngine;

/// <summary>
/// Абстрактный базовый класс для модулей
/// </summary>
public abstract class Module
{
    [SerializeField] protected bool isActive = true;

    public bool IsActive
    {
        get => isActive;
        set => isActive = value;
    }

    public abstract void Execute();
    public virtual void Initialize() { }
}