using System.Threading.Tasks;
using UnityEngine;

public class SwarmMandibles : ActiveModule
{
    uint biteDamage;
    uint attackCount;

    public SwarmMandibles() : base()
    {
        range = 1;
        energyCost = 2;
        biteDamage = 1;        
        attackCount = 4;
        AddConstKeywords(new OffenseKeyword());
    }
    public SwarmMandibles(uint biteDamage, uint attackCount) : this()
    {
        this.biteDamage = biteDamage;
        this.attackCount = attackCount;
    }

    public override string Description() {
        return $"Bites {attackCount} times, dealing {biteDamage} damage per bite.\n" + base.Description();
    }

    protected override bool checkFinalPosition(Vector3Int pos) {
        return Entity.tracker.IsOccupiedByCharacter(pos);
    }

    public override async Task Effect(Character user, Vector3Int pos, ActionContext ctx) {
        for (uint i = 0; i < attackCount; ++i)
            await user.strike(pos, biteDamage, MakeContext(user, pos));
    }
}
