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
        await user.strike(pos, (int)damageDealt);
        await user.damage((int)damageTaken);
        SpendUse();
    }
    public override string Description()
    {
        var keyword = constKeywords.First(k => k.GetType() == typeof(LimitedPerBattleKeyword)) as LimitedPerBattleKeyword;
        return $"Deal {damageDealt} damage. Take {damageTaken} damage. {keyword.Name} {keyword.MaxUses}.";
    }
    
}

