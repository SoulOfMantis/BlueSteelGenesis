using UnityEngine;


namespace BlueSteelGenesis.Character_Modules
{
    public abstract class ActiveModule : GameModule
    {

        public int energyCost;

        public virtual bool CanActivate(Character user)
        {
            return user.currentEnergy >= energyCost && user.myTurn;
        }
    }
}