using BlueSteelGenesis.Character;
using System.Collections.Generic;
using UnityEngine;

namespace BlueSteelGenesis.Modules
{
    public class ModuleManager
    {
        private List<GameModule> modules = new List<GameModule>();

        public void AddModule(GameModule module)
        {
            modules.Add(module);
            module.Initialize();
            Debug.Log($"Модуль {module.GetType().Name} добавлен");
        }

        public void RemoveModule(GameModule module)
        {
            modules.Remove(module);
            Debug.Log($"Модуль {module.GetType().Name} удален");
        }

        public void ExecuteAll()
        {
            foreach (var module in modules)
            {
                module.Execute();
            }
        }

        public void TriggerAll(BlueSteelGenesis.Character.Character user, Vector3Int pos)
        {
            foreach (var module in modules)
            {
                module.OnTrigger(user, pos);
            }
        }

        public void SetModuleActive<T>(bool active) where T : GameModule
        {
            foreach (var module in modules)
            {
                if (module is T)
                {

                    Debug.Log($"Модуль {typeof(T).Name} установлен в состояние: {active}");
                }
            }
        }
    }
}
