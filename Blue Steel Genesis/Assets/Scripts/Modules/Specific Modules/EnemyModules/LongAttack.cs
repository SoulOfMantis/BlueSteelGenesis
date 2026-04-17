using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class LongAttack : ActiveModule
{
    uint hitDamage = 4;
    uint waveDamage = 3;

    public LongAttack() : base()
    {
        range = 1;
        energyCost = 3;
        Icon_name = "LongAttack";
        AddConstKeywords(new OffenseKeyword());
    }
    public override async Task Effect(Character user, Vector3Int pos)
    {
        await user.strike(pos, hitDamage);
        var attackPosition = user.Position.Where(x => Entity.tracker.GetNeighborTiles(x)
                            .Contains(pos)).First();
        var direction = pos - attackPosition;
        direction.Clamp(new(-1, -1), new(1, 1));

        bool flag = true;

        while (flag)
        {
            if (Entity.tracker.FindEntityAtPosition(pos) is Entity e)
                await e.damage(waveDamage);
            flag = !Entity.tracker.IsOccupied(pos);
            pos += direction;
            flag = flag && !Entity.tracker.OutOfBounds(pos);
        }
    }
    public override string Description()
    {
        return $"Deals {hitDamage} damage in a line until it hits an obstacle.\n" + base.Description();
    }
    protected override bool checkFinalPosition(Vector3Int pos)
    {
        return Entity.tracker.IsOccupied(pos);
    }
    public override bool checkPosition(Character user, Vector3Int pos)
    {
        return base.checkPosition(user, pos) && !user.Position.Contains(pos);
    }
}

