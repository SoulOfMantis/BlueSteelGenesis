using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class ExplodeWithSlime : PassiveModule
{
    uint summonSize;
    uint acidDamage;
    uint acidDuration;
    public ExplodeWithSlime() : base()
    {
        Icon_name = "AcidExplosionModule";
        acidDamage = 1;
        acidDuration = 2;
        triggerType = TriggerType.OnDeath;
        range = 10;
        summonSize = 1;
        AddConstKeywords(new OffenseKeyword());
    }
    public ExplodeWithSlime(uint summonSize) : this()
    {
        this.summonSize = summonSize;
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
                    if (!Entity.summon<SmallAcidSlime>(new(getCellsInRange(user).First(), (int)summonSize)))
                        Debug.Log("Couldn't summon a small slime.");
                        break;                    
                case 2:
                    if(!Entity.summon<BigAcidSlime>(new(getCellsInRange(user).First(), (int)summonSize)))
                        Debug.Log("Couldn't summon a big slime.");
                    break;
                default:
                    break;
            }
    }
    protected override bool checkFinalPosition(Vector3Int pos)
    {
        return new PositionCollection(pos, (int)summonSize).All(p => !Entity.tracker.IsOccupied(p));
    }
    public override bool checkPosition(Character user, Vector3Int pos)
    {
        return true;
    }
}

