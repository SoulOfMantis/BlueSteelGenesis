using System.Threading.Tasks;
using UnityEngine;

public class DogSummoner_TEST_ONLY : ActiveModule {
    public DogSummoner_TEST_ONLY() {
        changeName("Dog summoner");
        range = 1;
        energyCost = 2;
        AddConstKeyword(new LimitedPerBattleKeyword(2));
    }
    public override string Description() =>
        "Summons an allied dog. Can be used only 2 times per battle";

    public override Task Effect(Character user, Vector3Int pos, ActionContext ctx) {
        Entity.summon<AlliedPurpleDog>(new(pos));
        return Task.CompletedTask;
    }
    protected override bool checkFinalPosition(Vector3Int pos) {
        return !Entity.tracker.OutOfBounds(pos) && !Entity.tracker.IsOccupied(pos);
    }
}
