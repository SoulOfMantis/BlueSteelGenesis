using System.Collections.Generic;
using System.Threading.Tasks;
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
            range = 3;
            energyCost = 1;
        }

        public override async Task Effect(Character user, Vector3Int pos)
        {
            await user.move(pos, getCellsInRange(user.Position));
            Debug.Log("BMM executed");
        }
        public override List<Vector3Int> getCellsInRange(Vector3Int start)
        {
            return Navigation.Dijkstra.listReachable(start, p => start.ManhattanDistance(p) <= range && !Character.tracker.IsOccupied(p), range);
        }

        public override bool checkPosition(Vector3Int pos, Character user)
        {
            return base.checkPosition(pos, user) && !Character.tracker.IsOccupied(pos);
        }
    }
}