using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Класс ключевых слов исключительно для внутреннего использования
/// </summary>
public abstract class InternalKeyword : ModuleKeyword
{
    public InternalKeyword() : base() { }
}
/// <summary>
/// Класс ключевых слов, показываемых игроку
/// </summary>
public class DefenseKeyword : InternalKeyword
{
    public DefenseKeyword() : base()
    {
        Name = "Defense";
        Description = "Intended to protect user of harm.";
    }
}
public class OffenseKeyword : InternalKeyword
{
    //TODO: finish the keyword
}
public class CommonKeyword : InternalKeyword
{
    //TODO: finish the keyword
}
public class BossKeyword : InternalKeyword
{
    //TODO: finish the keyword
}
//TODO: create more keywords
