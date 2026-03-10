using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DefaultBoss_TEST_ONLY : ActiveModule    
{
    uint damageDealt = 20;
    uint damageTaken = 3;
    bool usedThisBattle = false;
    public DefaultBoss_TEST_ONLY():base()
    {
        //AddKeywords(new BossKeyword(), new OffenseKeyword(), new OncePerBattle());
    }
    public override async Task Effect(Character user, Vector3Int pos)
    {
        await user.strike(pos, (int)damageDealt);
        await user.damage((int)3);
        usedThisBattle = true;
    }
    public override string Description()
    {
        return $"Deal {damageDealt} damage. Take {damageTaken} damage. Can only be used once per battle.";
    }
    public override bool CanBeUsed()
    {
        return usedThisBattle;
    }

}

