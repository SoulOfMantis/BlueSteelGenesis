using UnityEngine;


namespace BlueSteelGenesis.Character_Modules
{
    public abstract class PassiveModule : GameModule, ImmediateModule
    {
        public TriggerType triggerType;

        public abstract void Effect(Character user, Vector3Int pos);
    }
}
    

