using System.Collections.Generic;
using UnityEngine;

public class ModuleManager
{
    private List<Module> modules = new List<Module>();

    
    public void AddModule(Module module)
    {
        modules.Add(module);
        module.Initialize();
        Debug.Log($"Модуль {module.GetType().Name} добавлен");
    }

    
    public void RemoveModule(Module module)
    {
        modules.Remove(module);
        Debug.Log($"Модуль {module.GetType().Name} удален");
    }

    
    public void ExecuteAll()
    {
        foreach (var module in modules)
        {
            if (module.IsActive)
            {
                module.Execute();
            }
        }
    }

    
    public void UpdateAll()
    {
        foreach (var module in modules)
        {
            if (module.IsActive)
            {
                module.Update();
            }
        }
    }

    
    public void SetModuleActive<T>(bool active) where T : Module
    {
        foreach (var module in modules)
        {
            if (module is T)
            {
                module.IsActive = active;
                Debug.Log($"Модуль {typeof(T).Name} установлен в состояние: {active}");
            }
        }
    }
}