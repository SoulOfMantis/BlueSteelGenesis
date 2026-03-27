using System.Threading.Tasks;
public abstract class Obstacle : Entity {
    public override URangeValue currentHealth { get; protected set; } = new();
    public override uint maxHealth {
        get => currentHealth.Max;
        protected set => currentHealth.Max = value;
    }
    public Obstacle(uint maxHealth)
    {
        this.maxHealth = maxHealth;
        currentHealth.Max = maxHealth;
        currentHealth.Value = maxHealth;
    }

    protected override Task die()
    {
        tracker.RemoveObstacle(this);
        Destroy(gameObject);
        return Task.CompletedTask;
    }
}
