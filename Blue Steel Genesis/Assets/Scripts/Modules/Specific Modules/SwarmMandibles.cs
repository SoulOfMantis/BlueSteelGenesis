using System.Threading.Tasks;
using UnityEngine;

public class SwarmMandibles : ActiveModule
{
    uint biteDamage;
    uint attackCount;

    public SwarmMandibles(uint biteDamage, uint attackCount) {
        range = 1;
        energyCost = 1;
        this.biteDamage = biteDamage;
        this.attackCount = attackCount;
        AddConstKeywords(new OffenseKeyword());
    }

    public override string Description() {
        return $"Bites {attackCount} times, dealing {biteDamage} damage per bite.\n" + base.Description();
    }

    protected override bool checkFinalPosition(Vector3Int pos) {
        return Entity.tracker.IsOccupiedByCharacter(pos);
    }

    public override async Task Effect(Character user, Vector3Int pos) {
        for (uint i = 0; i < attackCount; ++i)
            await user.strike(pos, biteDamage);
    }
}
