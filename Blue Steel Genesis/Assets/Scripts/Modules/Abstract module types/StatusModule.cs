public abstract class StatusModule : PassiveModule
{
    protected int turnsLeft;

    public StatusModule() : base()
    {
        AddKeyword("Status");
    }
    protected void turnTick()
    {
        turnsLeft--;
    }

    public abstract void Refresh(StatusModule other);

    public abstract bool IsExpired();
}
