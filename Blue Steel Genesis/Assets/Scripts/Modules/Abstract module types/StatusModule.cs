public abstract class StatusModule : PassiveModule
{
    protected int turnsLeft;

    protected void turnTick()
    {
        turnsLeft--;
    }

    public abstract void Refresh(StatusModule other);

    public abstract bool IsExpired();
}
public abstract class PositiveStatus : StatusModule
{

}

public abstract class NegativeStatus : StatusModule
{

}