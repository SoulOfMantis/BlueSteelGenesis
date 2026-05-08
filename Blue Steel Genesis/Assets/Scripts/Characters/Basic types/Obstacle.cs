using System.Threading.Tasks;
using UnityEngine;

public abstract class Obstacle : Entity {
    [SerializeField] protected ObstacleSFX sfx;

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

    public override Task damage(uint dmg, ActionContext ctx) {
        if (sfx != null)
            sfx.play(TriggerType.OnDamage);
        return base.damage(dmg, ctx);
    }

    public override Task loseHealth(uint hp, ActionContext ctx) {
        if (sfx != null)
            sfx.play(TriggerType.OnHealthLost);
        return base.loseHealth(hp, ctx);
    }

    public override Task heal(uint hp, ActionContext ctx) {
        if (sfx != null)
            sfx.play(TriggerType.OnHeal);
        return base.heal(hp, ctx);
    }

    protected override Task die()
    {
        if (sfx != null)
            sfx.play(TriggerType.OnDeath);
        tracker.RemoveObstacle(this);
        Destroy(gameObject);
        return Task.CompletedTask;
    }
}
