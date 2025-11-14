using UnityEngine;

namespace BlueSteelGenesis.Character_Modules
{
    public abstract class StatusModule : PassiveModule
    {
        protected int turnsLeft;

        public abstract bool IsExpired();
    }
}