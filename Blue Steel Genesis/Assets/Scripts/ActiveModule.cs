using UnityEngine;


namespace BlueSteelGenesis.Character_Modules
{
    public abstract class ActiveModule : GameModule
    {
        public int energyCost { get; protected set; }
    }
}