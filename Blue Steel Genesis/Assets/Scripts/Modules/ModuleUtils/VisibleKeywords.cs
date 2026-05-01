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
public class PoisonKeyword : VisibleKeyword
{
    public PoisonKeyword():base()
    {
        ChangeName("Poison");
        ChangeDescription("Some creatures may have resistance or immunity to this.");
    }
}
public class BurnKeyword : VisibleKeyword
{
    public BurnKeyword() : base()
    {
        ChangeName("Burn");
        ChangeDescription("Some creatures may have resistance or immunity to this.");
    }
}
public class FlightKeyword : VisibleKeyword
{
    public FlightKeyword()
    {
        ChangeName("Flight");
        ChangeDescription("Allows flying over obstacles.");
    }
}
public class AcidKeyword : VisibleKeyword
{
    public AcidKeyword() : base()
    {
        ChangeName("Acid");
        ChangeDescription("Some creatures may have resistance or immunity to this.");
    }
}
public enum PossibleTargets
{
    Self,
    Target,
    AllAdjacent,
    All
}
public abstract class TargetedVisibleKeyword : VisibleKeyword
{
    public PossibleTargets Target { get; }
    public TargetedVisibleKeyword(PossibleTargets target) : base()
    {
        Target = target;
    }
    public override bool Equals(object obj)
    {
        return base.Equals(obj) && obj is TargetedVisibleKeyword t && t.Target == Target;
    }
    public override int GetHashCode()
    {
        return (GetType(), Target).GetHashCode();
    }
    public static string TargetDescription(PossibleTargets target)
    {
        switch (target)
        {
            case PossibleTargets.Self:
                return "to yourself";
            case PossibleTargets.Target:
                return "to the target";
            case PossibleTargets.AllAdjacent:
                return "to all adjacent creatures";
            case PossibleTargets.All:
                return "to ALL creatures";
            default:
                return "";
        }
    }
}
public class ShieldKeyword : TargetedVisibleKeyword
{
    public ShieldKeyword(uint shield, PossibleTargets target) : base(target)
    {
        ChangeName($"Shield {shield}");
        ChangeDescription($"Protects from {shield} damage. Resets at the start of turn.");
    }
}
public abstract class TargetedStatusKeyword : TargetedVisibleKeyword
{
    public StatusModule Status { get; protected set; }
    public TargetedStatusKeyword(PossibleTargets target, Type statusType, params object[] args) : base(target)
    {
        if (!statusType.IsSubclassOf(typeof(StatusModule))) throw new ArgumentException();
        Status = ModuleGenerator.CreateModuleByType(statusType, args) as StatusModule;
    }
}
public class EnhanceKeyword<T> : TargetedStatusKeyword where T : PositiveStatusModule
{
    public EnhanceKeyword(PossibleTargets target, params object[] args) : base(target, typeof(T), args) 
    {
        ChangeName($"Enhance {Status.Name}");
        string desc = $"Apply status {Status.Name}. " + Status.Description();
        ChangeDescription(desc);
    }
}
public class InflictKeyword<T> : TargetedStatusKeyword where T:NegativeStatusModule
{
    public InflictKeyword(PossibleTargets target, params object[] args) : base(target, typeof(T), args)
    {
        ChangeName($"Inflict {Status.Name}");
        string desc = $"Apply status {Status.Name}. " + Status.Description();
        ChangeDescription(desc);
    }
}
//TODO: create more keywords
