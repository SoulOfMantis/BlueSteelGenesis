public abstract class PassiveModule : GameModule
{
    public TriggerType triggerType;
    protected ActionContext context { get; private set; }
    public void loadContext(ActionContext c)
    {
        context = c;
    }
}
