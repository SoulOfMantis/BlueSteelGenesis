public abstract class ActiveModule : GameModule
{
    public uint energyCost { get; protected set; }
    public virtual bool CanBeUsed() => true;
}