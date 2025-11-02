using UnityEngine;

/// <summary>
/// класс модулей
/// </summary>
public abstract class Module
{
    /// <summary>
    /// поле активен ли модуль или нет
    /// </summary>
    [SerializeField] protected bool isActive = true; ///Исходим что модули пока активные.Небольшое уточнение - Атрибут [SerializeField] в Unity позволяет сделать приватное или защищённое поле видимым в инспекторе, но при этом защитить его от внешнего изменения в коде.


    ///доступ к состоянию модуля
    public bool IsActive
    {
        get => isActive;
        set => isActive = value;
    }

    /// <summary>
    /// сделать действие
    /// </summary>
    public abstract void Execute();

    /// <summary>
    /// обновить модуль
    /// </summary>
    public virtual void Update() { }

    /// <summary>
    /// метод для инициализации
    /// </summary>
    public virtual void Initialize() { }
}