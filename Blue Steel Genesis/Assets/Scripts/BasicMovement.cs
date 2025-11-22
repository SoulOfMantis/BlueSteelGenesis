using UnityEngine;





namespace BlueSteelGenesis.Character_Modules
{
    /// <summary>
    /// Базовый модуль движения (BMM сокращение)
    /// </summary>
    public class BasicMovement : ActiveModule
    {
        public BasicMovement()
        {
            energyCost = 1;
        }

        public override void Effect(Character user, Vector3Int pos)
        {
            if (CanActivate(user))
            {
                user.currentEnergy -= energyCost;
                user.move(pos);
                Debug.Log("BMM executed");
            }
            else
            {
                Debug.Log("Not enough energy for movement!");
            }
        }
    }
}