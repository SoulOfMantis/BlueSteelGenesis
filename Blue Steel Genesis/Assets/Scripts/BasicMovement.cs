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

        public override void Effect(Character user, Vector3Int pos)
        {
            user.move(pos);
            Debug.Log("BMM executed");
        }

        public override bool TryGetTarget(Character user, out Vector3Int targetPos)
        {
            targetPos = user.Position;
            PlayerCharacter player = Character.tracker.getPlayer();
            if (player == null) return false;

            var moveRange = getCellsInRange(user.Position);
            int minDist = int.MaxValue;
            Vector3Int best = user.Position;

            foreach (var cell in moveRange)
            {
                if (cell == user.Position) continue;
                if (Character.tracker.OutOfBounds(cell) || Character.tracker.IsOccupied(cell)) continue;

                int dist = Mathf.Abs(player.Position.x - cell.x) + Mathf.Abs(player.Position.y - cell.y);
                if (dist < minDist)
                {
                    minDist = dist;
                    best = cell;
                }
            }

            if (best != user.Position)
            {
                targetPos = best;
                return true;
            }
            return false;
        }


    }
}