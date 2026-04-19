using System.Collections.Generic;

public abstract class PassiveModule : GameModule
{
    public TriggerType triggerType = TriggerType.Never;
    public PassiveModule() : base()
    {
        AddConstKeyword(new PassiveKeyword());
    }

    protected ActionContext context { get; private set; }
    public void loadContext(ActionContext c)
    {
        context = c;
    }
}
