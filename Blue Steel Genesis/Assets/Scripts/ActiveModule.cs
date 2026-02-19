using UnityEngine;

namespace BlueSteelGenesis.Character_Modules
{
    public abstract class ActiveModule : GameModule
    {
        public int energyCost { get; protected set; }

        public virtual bool TryGetTarget(Character user, out Vector3Int targetPos)
        {
            targetPos = user.Position;
            return false;
        }
    }
}