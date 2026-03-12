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
