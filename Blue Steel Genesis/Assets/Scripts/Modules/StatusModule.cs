using BlueSteelGenesis.Character;
using UnityEngine;

namespace BlueSteelGenesis.Modules
{
    public abstract class StatusModule : PassiveModule
    {
        protected int turnsLeft;

        protected virtual void turnTick(BlueSteelGenesis.Character.Character user)
        {
            if (turnsLeft > 0)
                turnsLeft--;
        }

        public override void OnTrigger(BlueSteelGenesis.Character.Character user, Vector3Int pos)
        {
            turnTick(user);
            base.OnTrigger(user, pos);
        }
    }
}