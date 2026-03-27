using System.Threading.Tasks;
using UnityEngine;

public class RoboWolfSummoner : ActiveModule {
    public RoboWolfSummoner() {
        changeName("Summonning howl");
        range = 1;
        energyCost = 5;
    }
    public override string Description() =>
        "Summons a RoboWolf.\n" + base.Description();

    public override Task Effect(Character user, Vector3Int pos) {
        Entity.summon<AlliedPurpleDog>(new(pos));
        return Task.CompletedTask;
    }
    protected override bool checkFinalPosition(Vector3Int pos) {
        return !Entity.tracker.OutOfBounds(pos) && !Entity.tracker.IsOccupied(pos);
    }
}
