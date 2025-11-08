using UnityEngine;
using BlueSteelGenesis.Character;

namespace BlueSteelGenesis.Modules
{
    /// <summary>
    /// Базовый модуль атаки (BAM сокращение)
    /// </summary>
    public class BasicAttack : ActiveModule
    {
        public override void Effect(BlueSteelGenesis.Character.Character user, Vector3Int pos)
        {
            user.strike(pos, 1);
            Debug.Log("BAM attack executed");
        }

        public override void Execute()
        {
            Debug.Log("BAM executed");
        }
    }
}
