using UnityEngine;

namespace BlueSteelGenesis.Character_Modules
{
    /// <summary>
    /// Пассивный модуль яда или поджога - наносит урон при начале хода
    /// </summary>
    public class PoisonModule : StatusModule
    {
        private int poisonDamage;

        public PoisonModule(int damage = 1, int duration = 3)
        {
            triggerType = TriggerType.OnTurnStart;
            poisonDamage = damage;
            turnsLeft = duration;
        }

        public override void Effect(Character user, Vector3Int pos)
        {
            if (turnsLeft > 0)
            {
                user.damage(poisonDamage);
                Debug.Log($"Poison dealt {poisonDamage} damage to {user.GetType().Name}");

                turnsLeft--;
                if (turnsLeft <= 0)
                {
                    Debug.Log($"Poison status expired on {user.GetType().Name}");
                }
            }
        }

        public override bool IsExpired()
        {
            return turnsLeft <= 0;
        }
    }
}