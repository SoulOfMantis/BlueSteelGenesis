using System.Threading.Tasks;
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
            range = 1;
        }

        public override async Task Effect(Character user, Vector3Int pos)
        {
             await user.strike(pos,1); //пока 1 урон
             Debug.Log("BAM executed");
        }

        public override bool checkPosition(Vector3Int pos, Character user)
        {
            return base.checkPosition(pos, user) && Character.tracker.IsOccupiedByCharacter(pos) && (pos != user.Position);
        }
    }
}

