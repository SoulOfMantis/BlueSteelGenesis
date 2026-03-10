public abstract class PassiveModule : GameModule
{
    public TriggerType triggerType = TriggerType.Never;
    public PassiveModule() : base()
    {
        //AddKeyword(new PassiveKeyword());
    }
}



