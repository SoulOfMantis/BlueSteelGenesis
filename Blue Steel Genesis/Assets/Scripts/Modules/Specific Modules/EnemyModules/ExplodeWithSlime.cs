using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class ExplodeWithSlime : PassiveModule
{
    uint summonSize;
    uint acidDamage;
    uint acidDuration;
    public ExplodeWithSlime(uint summonSize) : base()
    {
        acidDamage = 1;
        acidDuration = 2;
        triggerType = TriggerType.OnDeath;
        range = 10;
        this.summonSize = summonSize;
        AddConstKeywords(new OffenseKeyword());
    }
    public override HashSet<ModuleKeyword> renewableKeywords()
    {
        var res = base.renewableKeywords();
        res.Add(new InflictKeyword<AcidModule>(PossibleTargets.All, acidDamage, acidDuration));
        return res;
    }
    public override string Description()
    {
        return "When dies:\n" + base.Description() + $"Summons 2 slimes of size {summonSize}x{summonSize}.";
    }
    public override async Task Effect(Character user, Vector3Int pos)
    {
        foreach (var e in Entity.tracker.Entities.Where(e => e == user))
            await user.apply(e.Position.LeftBottom, new AcidModule(acidDamage, acidDuration));
        for (int i = 0; i < 2; i++)
            switch (summonSize)
            {
                case 1:
                    Entity.summon<SmallAcidSlime>(new(getCellsInRange(user).First(), (int)summonSize));
                    break;
                case 2:
                    Entity.summon<BigAcidSlime>(new(getCellsInRange(user).First(), (int)summonSize));
                    break;
                default:
                    break;
        }
    }
    protected override bool checkFinalPosition(Vector3Int pos)
    {
        return new PositionCollection(pos, (int)summonSize).All(p => !Entity.tracker.IsOccupied(p));
    }
}

