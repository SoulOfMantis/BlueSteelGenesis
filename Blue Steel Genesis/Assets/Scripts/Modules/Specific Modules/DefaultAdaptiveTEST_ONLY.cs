using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DefaultAdaptiveTEST_ONLY : PassiveModule    
{
    public DefaultAdaptiveTEST_ONLY():base()
    {
        AddKeywords(new List<string> { "Special", "Adaptive", "Useless" });
    }
    public override Task Effect(Character user, Vector3Int pos)
    {
        return Task.CompletedTask;
    }

    public override string Description()
    {
        return "DefaultAdaptiveTEST_ONLY";
    }
}

