public abstract class StatusModule : PassiveModule
{
    protected URangeValue turnsLeft = new();

    protected void turnTick()
    {
        turnsLeft--;
    }

    public abstract void Refresh(StatusModule other);

    public abstract bool IsExpired();
}
