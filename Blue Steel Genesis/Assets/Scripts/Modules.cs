using UnityEngine;

public abstract class Module
{
    [SerializeField] protected bool isActive = true; ///»сходим что модули пока активные.Ќебольшое уточнение - јтрибут [SerializeField] в Unity позвол€ет сделать приватное или защищЄнное поле видимым в инспекторе, но при этом защитить его от внешнего изменени€ в коде.


    public bool IsActive
    {
        get => isActive;
        set => isActive = value;
    }

    
    public abstract void Execute();

    
    public virtual void Update() { }

    
    public virtual void Initialize() { }
}