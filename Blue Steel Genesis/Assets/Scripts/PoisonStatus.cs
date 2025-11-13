using UnityEngine;
using System;

namespace BlueSteelGenesis.Character_Modules
{
    public class PoisonStatus : StatusEffect
    {
        private int damagePerTurn;

        public PoisonStatus(int damage = 1, int duration = 3) : base("Poison", duration)
        {
            damagePerTurn = damage;
        }

        public override void OnTurnStart(Character target)
        {
            target.damage(damagePerTurn);
            Debug.Log($"{target.GetType().Name} gets {damagePerTurn} poison damage!");
        }

        public override void Refresh(StatusEffect newStatus)
        {
            base.Refresh(newStatus);
            var poison = newStatus as PoisonStatus;
            if (poison != null)
            {
                damagePerTurn = Math.Max(damagePerTurn, poison.damagePerTurn);
            }
        }
    }
}
