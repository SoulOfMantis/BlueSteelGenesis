using System.Threading.Tasks;
using UnityEngine;

public class RoboWolfSummonerPlayer : ActiveModule 

{
    public RoboWolfSummonerPlayer() 
    {
        changeName("Summonning howl");
        range = 1;
        energyCost = 5;
        AddConstKeywords(new LimitedPerBattleKeyword(2));
        Icon_name = "RoboWolfSummoner";
    }
    public override string Description() => "Summons an allied RoboWolf.\n" + base.Description();

    public override Task Effect(Character user, Vector3Int pos, ActionContext ctx) {
        Entity.summon<AlliedRoboWolf>(new(pos));
        return Task.CompletedTask;
    }

    protected override bool checkFinalPosition(Vector3Int pos) {
        return !Entity.tracker.OutOfBounds(pos) && !Entity.tracker.IsOccupied(pos);
    }
}
