using System.Collections.Generic;

public abstract class StatusModule : PassiveModule
{
    protected int turnsLeft;

    public StatusModule() : base()
    {
        AddConstKeyword(new StatusKeyword());
    }
    protected void turnTick()
    {
        turnsLeft--;
    }

    public abstract void Refresh(StatusModule other);

    public abstract bool IsExpired();

}
public abstract class NegativeStatusModule : StatusModule
{
    public NegativeStatusModule() :base()
    {
        AddConstKeyword(new NegativeStatusKeyword());
    }
}
public abstract class PositiveStatusModule : StatusModule
{
    public PositiveStatusModule() : base()
    {
        AddConstKeyword(new PositiveStatusKeyword());
    }
}

