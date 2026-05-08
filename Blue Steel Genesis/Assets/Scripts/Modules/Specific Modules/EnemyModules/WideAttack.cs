using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class WideAttack : ActiveModule
{
    uint hitDamage = 8;
    public WideAttack() : base()
    {
        range = 1;
        energyCost = 4;
        Icon_name = "WideAttack";
        AddConstKeywords(new OffenseKeyword());
    }
    public override async Task Effect(Character user, Vector3Int pos, ActionContext ctx)
    {
        var attackPosition = user.Position.Where(x => Entity.tracker.GetNeighborTiles(x)
                    .Contains(pos)).First();
        var direction = attackPosition - pos;
        direction = new(direction.y, direction.x);

        bool flag = true;
        while (flag)
        {
            flag = !Entity.tracker.OutOfBounds(pos) && Entity.tracker.GetNeighborTiles(pos)
                .Any(n => user.Position.Contains(n));
            pos += direction;
        }
        direction *= -1;
        pos += direction;

        flag = true;
        while (flag)
        {
            await user.strike(pos, hitDamage, MakeContext(user, pos));
            pos += direction;
            flag = !Entity.tracker.OutOfBounds(pos) && Entity.tracker.GetNeighborTiles(pos)
                    .Any(n => user.Position.Contains(n));
        }
        await user.strike(pos, hitDamage, MakeContext(user, pos));
    }
    public override string Description()
    {
        return $"Deals {hitDamage} damage to each cell in front of the user.\n" + base.Description();
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

