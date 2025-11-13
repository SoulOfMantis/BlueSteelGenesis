using UnityEngine;

namespace BlueSteelGenesis.Character_Modules
{
    public abstract class StatusModule : PassiveModule
    {
        protected int turnsLeft;

        protected virtual void TurnTick(Character user)
        {
            if (turnsLeft > 0)
                turnsLeft--;
        }
    }
}

