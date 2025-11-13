using UnityEngine;

namespace BlueSteelGenesis.Character_Modules
{
    /// <summary>
    /// модуль что наносит урон и травит
    /// </summary>
    public class PoisonStinger : StatusModule
    {
        private int poisonDamage;
        private int hitDamage;
        private int duration;
        public PoisonStinger(int damage = 1, int duration = 3, int hitDamage = 3)
        {
            triggerType = TriggerType.OnTurnStart;
            poisonDamage = damage;
            this.hitDamage = hitDamage;
            turnsLeft = duration;
            this.duration = duration;
        }

        public override void Effect(Character user, Vector3Int pos)
        {
            if (turnsLeft > 0)
            {
                if (turnsLeft == duration) //это  разовый урон на момент нанесения
                {
                    user.damage(hitDamage);
                    Debug.Log($"Hit dealt {hitDamage} damage to {user.GetType().Name}");
                }
                user.damage(poisonDamage);
                Debug.Log($"Poison dealt {poisonDamage} damage to {user.GetType().Name}");
            }
            TurnTick(user);
        }
    }
}

