using System;
using UnityEngine;

[Serializable]
class GameModuleSerializable {
    public GameModuleSerializable(GameModule module) {
        type = module.GetType().AssemblyQualifiedName;
        level = module.upgradeLevel;   
    }

    public GameModule create() {
        GameModule module = ModuleGenerator.CreateModuleByType(Type.GetType(type));
        module.SetUpgradeLevel(level);
        return module;
    }

    [SerializeField]
    private string type;
    [SerializeField]
    private int level = 0;
}