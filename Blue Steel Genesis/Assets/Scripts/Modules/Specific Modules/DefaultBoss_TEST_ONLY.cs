using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class DefaultBoss : ActiveModule    
{
    public DefaultBoss():base()
    {
        AddConstKeywords(new BossKeyword());
    }
    public override async Task Effect(Character user, Vector3Int pos)
    {
        await Task.CompletedTask;
    }
    public override string Description()
    {
        return $"Default Boss module. Does nothing.\n" + base.Description();
    }
}

