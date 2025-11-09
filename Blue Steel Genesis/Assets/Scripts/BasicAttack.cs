using UnityEngine;



namespace BlueSteelGenesis.Character_Modules
{
    /// <summary>
    /// Базовый модуль атаки (BAM сокращение)
    /// </summary>
    public class BasicAttack : ActiveModule
    {
        public BasicAttack()
        {
            energyCost = 1;
        }

        public override void Effect(Character user, Vector3Int pos)
        {
            if (CanActivate(user))
            {
                user.currentEnergy -= energyCost;
                user.strike(pos,1); //пока 1 урон
                Debug.Log("BAM executed");
            }
            else
            {
                Debug.Log("Not enough energy for attack!");
            }
        }
    }
}

