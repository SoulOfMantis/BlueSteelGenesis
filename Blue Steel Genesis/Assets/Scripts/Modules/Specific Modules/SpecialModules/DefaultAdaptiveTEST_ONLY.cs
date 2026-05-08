using System.Threading.Tasks;
using UnityEngine;

public class DefaultAdaptiveTEST_ONLY : PassiveModule
{
    public DefaultAdaptiveTEST_ONLY():base()
    {
        AddConstKeyword(new AdaptiveKeyword());
    }
    public override Task Effect(Character user, Vector3Int pos, ActionContext ctx)
    {
        return Task.CompletedTask;
    }

    public override string Description()
    {
        return $"Does nothing. For testing only." + base.Description();
    }
}
