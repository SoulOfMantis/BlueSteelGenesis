using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class SwarmScatter : PassiveModule
{
    uint shieldAmount;

    public SwarmScatter() : base()
    {
        range = 1;
        shieldAmount = 3;
        triggerType = TriggerType.OnTurnEnd;
        AddConstKeywords(new DefenseKeyword(), new ShieldKeyword(shieldAmount, PossibleTargets.Self));
    }
    public SwarmScatter(uint shieldAmount) : this()
    {
        this.shieldAmount = shieldAmount;
    }

    public override string Description() {
        return "At the end of their turn, the bugs scatter, making them harder to hit.\n" + base.Description();
    }

    public override async Task Effect(Character user, Vector3Int pos) {
        await user.giveShield(shieldAmount);
    }
}
