
namespace BlueSteelGenesis.Character_Modules
{
    /// <summary>
    /// Базовый класс для всех статус эффектов
    /// </summary>
    public abstract class StatusEffect
    {
        public string statusName;
        public int duration;
        public int maxDuration;

        public StatusEffect(string name, int duration)
        {
            statusName = name;
            this.duration = duration;
            this.maxDuration = duration;
        }

        /// <summary>
        /// Вызывается при применении статуса
        /// </summary>
        public virtual void OnApply(Character target) { }

        /// <summary>
        /// Вызывается при удалении статуса
        /// </summary>
        
        public virtual void OnRemove(Character target) { }
        /// <summary>
        /// Вызывается при применении ударе
        /// </summary>
        public virtual void OnStrike(Character target) { }

        /// <summary>
        /// Вызывается при применении получении урона
        /// </summary>
        public virtual void OnDamage(Character target) { }

        /// <summary>
        /// Вызывается при применении смерти
        /// </summary>
        public virtual void OnDeath(Character target) { }

        /// <summary>
        /// Вызывается при применении лечении
        /// </summary>
        public virtual void OnHeal(Character target) { }

        /// <summary>
        /// Вызывается при движении
        /// </summary>
        public virtual void OnMove(Character target) { }

        /// <summary>
        /// Вызывается в начале хода персонажа
        /// </summary>
        public virtual void OnTurnStart(Character target) { }

        /// <summary>
        /// Вызывается в конце хода персонажа
        /// </summary>
        public virtual void OnTurnEnd(Character target) { }

        /// <summary>
        /// Уменьшает длительность и возвращает true, если статус закончился
        /// </summary>
        public virtual bool TickDuration()
        {
            duration--;
            return duration <= 0;
        }

        /// <summary>
        /// Обновляет статус при повторном применении
        /// </summary>
        public virtual void Refresh(StatusEffect newStatus)
        {
            duration = newStatus.duration;
        }
    }
}