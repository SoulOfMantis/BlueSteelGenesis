using System.Threading.Tasks;
using UnityEngine;

public class RoboWolfSummonerModule : ActiveModule {

    public RoboWolfSummonerModule() {
        changeName("Summonning howl");
        range = 1;
        energyCost = 5;
        Icon_name = "RoboWolfsSummoner";
    }

    public override string Description() =>
        "Summons a RoboWolf.\n" + base.Description();

    public override Task Effect(Character user, Vector3Int pos, ActionContext ctx) {
        user.summon<RoboWolf>(new(pos));
        return Task.CompletedTask;
    }
    protected override bool checkFinalPosition(Vector3Int pos) {
        return !Entity.tracker.OutOfBounds(pos) && !Entity.tracker.IsOccupied(pos);
    }
}
