using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DefaultCommon : PassiveModule
{
    public DefaultCommon():base()
    {
        AddConstKeyword(new CommonKeyword());
    }
    public override Task Effect(Character user, Vector3Int pos, ActionContext ctx)
    {
        return Task.CompletedTask;
    }
    public override string Description()
    {
        return $"Default Common module. Does nothing. For testing only." + base.Description();
    }
}

