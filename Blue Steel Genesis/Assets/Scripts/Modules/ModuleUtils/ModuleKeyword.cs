using System;
using System.Collections.Generic;
using System.Text;
 
public abstract class ModuleKeyword
{
    public string Name { get; protected set; }
    public string Description { get; protected set; }
    public ModuleKeyword()
    {
        changeName(GetType().Name);
        Description = "Default_Keyword_Description";
    }
    protected void changeName(string value) => Name = value;
}
public class DefenseKeyword : ModuleKeyword
{
    public DefenseKeyword() : base()
    {
        Name = "Defense";
        Description = "Intended to protect user of harm.";
    }
}
public class OffenseKeyword : ModuleKeyword
{
    //TODO: finish the keyword
}
public class CommonKeyword : ModuleKeyword
{
    //TODO: finish the keyword
}
public class BossKeyword : ModuleKeyword
{
    //TODO: finish the keyword
}
//TODO: create more keywords

/// <summary>
/// Класс ключевых слов, показываемых игроку
/// </summary>
public abstract class VisibleKeyword : ModuleKeyword { }
public class ShieldKeyword : VisibleKeyword
{
    //TODO: finish the keyword
}