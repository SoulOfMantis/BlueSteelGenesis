using UnityEngine;

namespace BlueSteelGenesis.Character_Modules
{
    /// <summary>
    /// Активный модуль ядовитого жала - наносит урон и накладывает отравление
    /// </summary>
    public class PoisonStinger : BasicAttack
    {
        private int poisonDamage;
        private int hitDamage;
        private int duration;

        public PoisonStinger(int damage = 1, int duration = 3, int hitDamage = 3) : base()
        {
            poisonDamage = damage;
            this.hitDamage = hitDamage;
            this.duration = duration;
        }

        public override void Effect(Character user, Vector3Int pos)
        {
            base.Effect(user, pos);
            PoisonModule poison = new PoisonModule(poisonDamage, duration);
            user.apply(pos, poison);
        }

    }
}