using System.Collections.Generic;
using System.Linq;

public abstract class Enemy : NPC
{
    protected Enemy(uint maxHealth, uint maxEnergy, int initiative) : base(maxHealth, maxEnergy, initiative) {}

    protected override IEnumerable<Entity> getEnemies() =>
        tracker.Entities.Where(e => e is PlayerCharacter || (e is NPC npc && npc.IsAlliedToPlayer));
    protected override IEnumerable<Entity> getAllies() =>
        tracker.Entities.Where(e => (e is NPC npc) && npc.IsHostileToPlayer);
}

