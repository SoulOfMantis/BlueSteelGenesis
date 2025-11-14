using UnityEngine;
using System.Collections.Generic;

namespace BlueSteelGenesis.Character_Modules
{
    /// <summary>
    /// Активный модуль ядовитого жала - наносит урон и накладывает отравление
    /// </summary>
    public class PoisonStinger : ActiveModule
    {
        private int poisonDamage;
        private int hitDamage;
        private int duration;

        public PoisonStinger(int damage = 1, int duration = 3, int hitDamage = 3)
        {
            poisonDamage = damage;
            this.hitDamage = hitDamage;
            this.duration = duration;
        }

        public override void Effect(Character user, Vector3Int pos)
        {
            user.damage(hitDamage);
            Debug.Log($"Poison Stinger dealt {hitDamage} damage to {user.GetType().Name}");
            PoisonStatus poisonStatus = new PoisonStatus(poisonDamage, duration);
            user.AddStatusEffect(poisonStatus);

            Debug.Log($"Poison status applied to {user.GetType().Name} for {duration} turns");
        }


        private class PoisonStatus : StatusModule
        {
            private int poisonDamage;

            public PoisonStatus(int damage, int duration)
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
}