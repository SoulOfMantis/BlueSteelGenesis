using System;
using System.Collections.Generic;
using System.Text;
/// <summary>
/// Класс ключевых слов, показываемых игроку
/// </summary>
public abstract class VisibleKeyword : ModuleKeyword
{
    public string Name { get; private set; }
    protected void ChangeName(string value) => Name = value;
    public VisibleKeyword() : base() 
    {
        ChangeName("Default");
        ChangeDescription("Default description. If you see this, something went wrong.");
    }
    public string Description { get; private set; }
    protected void ChangeDescription(string value) => Description = value;
}
public class ShieldKeyword : VisibleKeyword
{
    public ShieldKeyword(int? shield = null) : base()
    {
        ChangeName($"Shield {shield}");
        ChangeDescription($"Protects from {shield.ToString() ?? "#error"} damage. Resets at the start of turn.");
    }
}
//public class 
public class InflictKeyword<T> : VisibleKeyword where T:StatusModule
{
    public T Status { get; }
    public InflictKeyword(params object[] args) : base()
    {
        Status = ModuleGenerator.CreateModuleByType(typeof(T), args) as T;
        ChangeName($"Inflict {Status.Name}");
        ChangeDescription($"Apply status {Status.Name} to the target.");
    }
}
//TODO: create more keywords
