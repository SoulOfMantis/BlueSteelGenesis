using System.Collections.Generic;

public abstract class ActiveModule : GameModule
{
    public ActiveModule() : base()
    {
        AddConstKeyword(new ActiveKeyword());
    }
    public uint energyCost { get; protected set; }
}