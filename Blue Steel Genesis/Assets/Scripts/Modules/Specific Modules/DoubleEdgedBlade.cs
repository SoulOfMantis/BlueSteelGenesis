using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class DoubleEdgedBlade : ActiveModule
{
    uint damageDealt = 20;
    uint damageTaken = 3;
    public DoubleEdgedBlade():base()
    {
        AddConstKeywords(new BossKeyword(), new OffenseKeyword(), new LimitedPerBattleKeyword());
        range = 2;
    }
    public override async Task Effect(Character user, Vector3Int pos, ActionContext ctx)
    {
        await user.strike(pos, damageDealt, MakeContext(user, pos));
        await user.damage(damageTaken, MakeContext(user, pos));
    }
    public override string Description()
    {
        return $"Deal {damageDealt} damage. Take {damageTaken} damage.\n" + base.Description();
    }
    protected override bool checkFinalPosition(Vector3Int pos)
    {
        return Character.tracker.IsOccupiedByCharacter(pos);
    }
    public override bool checkPosition(Character user, Vector3Int pos)
    {
        return base.checkPosition(user, pos) && !user.Position.Contains(pos);
    }
}

