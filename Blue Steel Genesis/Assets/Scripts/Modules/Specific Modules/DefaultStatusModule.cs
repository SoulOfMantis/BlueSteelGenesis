using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DefaultStatusModule : StatusModule
{
    public override Task Effect(Character user, Vector3Int pos, ActionContext ctx)
    {
        return Task.CompletedTask;
    }
    public override string Description()
    {
        return $"Default Status module. Does nothing." + base.Description();
    }
    public override bool IsExpired()
    {
        throw new System.NotImplementedException();
    }
    public override void Refresh(StatusModule other)
    {
        throw new System.NotImplementedException();
    }
}

