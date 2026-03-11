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
        ChangeName("Shield");
        ChangeDescription($"Protects from {shield.ToString() ?? "#error"} damage. Resets at the start of turn.");
    }
}
public class InflictKeyword<T> : VisibleKeyword where T:StatusModule
{
    public InflictKeyword(params object[] args) : base()
    {
        var st = ModuleGenerator.CreateModuleByType(typeof(T), args);
        ChangeName($"Inflict {st.Name}");
        ChangeDescription($"Apply status {st.Name} to the target.");
    }
}
//TODO: create more keywords
