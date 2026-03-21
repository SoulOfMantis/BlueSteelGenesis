public abstract class Obstacle : Entity {
    public override URangeValue currentHealth { get; protected set; } = new();
    public override uint maxHealth {
        get => currentHealth.Max;
        protected set => currentHealth.Max = value;
    }
}
