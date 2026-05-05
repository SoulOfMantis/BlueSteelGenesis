using System;
using UnityEngine;

[Serializable]
class GameModuleSerializable {
    public GameModuleSerializable(GameModule module) {
        type = module.GetType().AssemblyQualifiedName;
        //TODO: save level
    }

    public GameModule create() {
        GameModule module = ModuleGenerator.CreateModuleByType(Type.GetType(type));
        //TODO: apply level
        return module;
    }

    [SerializeField]
    private string type;
    [SerializeField]
    private int level = 0;
}