using UnityEngine;

namespace BlueSteelGenesis.Character_Modules
{
    public abstract class StatusModule : PassiveModule
    {
        protected int turnsLeft;

        protected abstract void OnTriggerEffect(Character user, Vector3Int pos);

        protected virtual void TurnTick(Character user)
        {
            if (turnsLeft > 0)
                turnsLeft--;
        }

        public override void Effect(Character user, Vector3Int pos)
        {

            OnTriggerEffect(user, pos);
            TurnTick(user);
        }
    }
}

