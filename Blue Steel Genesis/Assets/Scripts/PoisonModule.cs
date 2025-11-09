using UnityEngine;



namespace BlueSteelGenesis.Character_Modules
{
    /// <summary>
    /// Пассивный модуль яда или поджога - наносит урон при начале хода
    /// </summary>
    public class PoisonModule : PassiveModule
    {
        private int poisonDamage;

        public PoisonModule(int damage = 1, int duration = 3)
        {
            triggerType = TriggerType.OnTurnStart;
            poisonDamage = damage;
            turnsLeft = duration;
        }

        
        protected override void TurnTick(Character user)
        {
            if (turnsLeft > 0)
            {
                turnsLeft--;
                if (turnsLeft == 0)
                {
                    Debug.Log("Poison effect ended");
                }
            }
        }

        protected override void OnTriggerEffect(Character user, Vector3Int pos)
        {
            if (turnsLeft > 0)
            {
                user.damage(poisonDamage);
                Debug.Log($"Poison dealt {poisonDamage} damage to {user.GetType().Name}");
            }
        }
    }
}

