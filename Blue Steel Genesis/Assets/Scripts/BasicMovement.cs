using UnityEngine;





    /// <summary>
    /// Базовый модуль движения (BMM сокращение)
    /// </summary>
    public class BasicMovement : ActiveModule
    {
        public BasicMovement()
        {
            range = 3;
            energyCost = 1;
        }

        public override void Effect(Character user, Vector3Int pos)
        {
            user.move(pos);
            Debug.Log("BMM executed");
        }
    }
