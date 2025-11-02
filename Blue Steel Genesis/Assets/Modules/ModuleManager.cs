using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// менеджер модулей
/// </summary>
public class ModuleManager
{
    /// <summary>
    /// список все модулей
    /// </summary>
    private List<Module> modules = new List<Module>();

    
    /// <summary>
    /// добавление модул€
    /// </summary>
    /// <param name="module"></param>
    public void AddModule(Module module)
    {
        modules.Add(module);
        module.Initialize();
        Debug.Log($"ћодуль {module.GetType().Name} добавлен");
    }

    /// <summary>
    /// убрать модуль
    /// </summary>
    /// <param name="module"></param>
    public void RemoveModule(Module module)
    {
        modules.Remove(module);
        Debug.Log($"ћодуль {module.GetType().Name} удален");
    }

    
    /// <summary>
    /// выполнить действие всех модулей (потом будет изменЄн на добавлении всех активных модулей на панель)
    /// </summary>
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

    /// <summary>
    /// обновить все модули
    /// </summary>
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

    /// <summary>
    /// ¬ключить/выключить конкретный тип модулей
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="active"></param>
    public void SetModuleActive<T>(bool active) where T : Module
    {
        foreach (var module in modules)
        {
            if (module is T)
            {
                module.IsActive = active;
                Debug.Log($"ћодуль {typeof(T).Name} установлен в состо€ние: {active}");
            }
        }
    }
}