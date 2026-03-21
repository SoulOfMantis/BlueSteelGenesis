public abstract class FrequencyLimiterKeyword : VisibleKeyword
{
    public uint MaxUses { get; }
    public uint UsesLeft { get; private set; }
    public void Recharge() => UsesLeft = MaxUses;
    public FrequencyLimiterKeyword(TriggerType recharge, uint max = 1) : base()
    {
        MaxUses = max;
        UsesLeft = 0;
        rechargeTime = recharge;
    }
    public bool CanBeUsed() => UsesLeft >= 1;
    public void SpendUseLeft() => UsesLeft -= 1;
    public TriggerType rechargeTime { get; }
}
/// <summary>
/// TODO: pick a cooler sounding name
/// </summary>
public class LimitedPerBattleKeyword : FrequencyLimiterKeyword
{
    public LimitedPerBattleKeyword(uint max = 1) : base(TriggerType.OnBattleStart, max)
    {
        ChangeName($"LimitedPerBattle {MaxUses}");
        ChangeDescription($"Can only be used up to {MaxUses} times per battle. {UsesLeft} uses left.");
    }
}
/// <summary>
/// TODO: pick a cooler sounding name
/// </summary>
public class LimitedPerTurnKeyword : FrequencyLimiterKeyword
{
    public LimitedPerTurnKeyword(uint max = 1) : base(TriggerType.OnTurnStart, max)
    {
        ChangeName($"LimitedPerTurn {MaxUses}");
        ChangeDescription($"Can only be used up to {MaxUses} times per turn. {UsesLeft} uses left.");
    }
}
