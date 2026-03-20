using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class DefaultBoss_TEST_ONLY : ActiveModule    
{
    uint damageDealt = 20;
    uint damageTaken = 3;
    public DefaultBoss_TEST_ONLY():base()
    {
        AddConstKeywords(new BossKeyword(), new OffenseKeyword(), new LimitedPerBattleKeyword());
    }
    public override async Task Effect(Character user, Vector3Int pos)
    {
        await user.strike(pos, damageDealt);
        await user.damage(damageTaken);
        SpendUse();
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
        return base.checkPosition(user, pos) && (pos != user.Position);
    }
}

