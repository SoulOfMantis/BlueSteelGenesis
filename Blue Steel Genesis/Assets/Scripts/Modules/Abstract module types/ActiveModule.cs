using System.Collections.Generic;

public abstract class ActiveModule : GameModule
{
    public int energyCost { get; protected set; }
    public ActiveModule() : base()
    {
        AddConstKeyword(new ActiveKeyword());
    }
}