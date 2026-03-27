using System.Threading.Tasks;
public abstract class Obstacle : Entity {
    public override URangeValue currentHealth { get; protected set; } = new();
    public override uint maxHealth {
        get => currentHealth.Max;
        protected set => currentHealth.Max = value;
    }
    protected override Task die()
    {
        if (TooltipSystem.IsCurrent(this))
        {
            TooltipSystem.Unlock(TooltipSystem.TooltipType.entityTooltip);
            TooltipSystem.Hide(TooltipSystem.TooltipType.entityTooltip);
        }
        return Task.CompletedTask;
    }
}
