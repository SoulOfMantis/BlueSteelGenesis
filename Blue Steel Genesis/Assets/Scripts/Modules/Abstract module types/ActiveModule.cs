public abstract class ActiveModule : GameModule
{
    public int energyCost { get; protected set; }
    public virtual bool CanBeUsed() => true;
}