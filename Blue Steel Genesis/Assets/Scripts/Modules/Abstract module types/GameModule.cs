using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
/// <summary>
/// ����� ������
/// </summary>
public abstract class GameModule
{
    public string Name { get; protected set; }
    private string icon_name = "NoModuleIcon";
    public string Icon_name { get => icon_name; protected set => icon_name = value; }
    public uint price;
    public uint range = 0;

    public virtual string Description()
    {
        string res = "";
        foreach (var k in GetVisibleKeywords())
        {
            res += $"{k.Name}";
            if (k is TargetedVisibleKeyword t)
                res += $" {TargetedVisibleKeyword.TargetDescription(t.Target)}";
            res += ".\n";
        }
        return res;
    }
    public GameModule()
    {
        changeName(GetType().Name);
        constKeywords = new();
        tempKeywords = new();
    }
    protected List<Vector3Int> getAvailableCells(uint n, IEnumerable<Vector3Int> start)
    {
        var res = start.ToHashSet();
        HashSet<Vector3Int> toAdd = new();
        for (uint i = 1; i <= n; i++)
        {
            foreach (var cell in res)
            {
                toAdd.Add(new Vector3Int(cell.x + 1, cell.y));
                toAdd.Add(new Vector3Int(cell.x - 1, cell.y));
                toAdd.Add(new Vector3Int(cell.x, cell.y + 1));
                toAdd.Add(new Vector3Int(cell.x, cell.y - 1));
            }
            foreach (var cell in toAdd)
            {
                if (checkIntermediatePosition(cell))
                    res.Add(cell);
            }
            toAdd.Clear();
        }
        return res.Where(c => checkFinalPosition(c)).ToList();
    }
    public virtual List<Vector3Int> getCellsInRange(Character user) => getCellsInRange(user.Position);
    public virtual List<Vector3Int> getCellsInRange(PositionCollection start)
    {
        return getAvailableCells(range, start);
    }
    public void changeName(string newName)
    {
        Name = newName;
        UpdateTooltipIfCurrent();
    }
    public abstract Task Effect(Character user, Vector3Int pos);
    public async Task Use(Character user, Vector3Int pos)
    {
        await Awaitable.WaitForSecondsAsync(.1f);
        if (CanBeUsed()) 
        {        SpendUse();
            await Effect(user, pos);
        }
    }

    public virtual void Initialize()
    {
        Debug.Log($"Module {Name} initialized");
    }
    protected virtual bool checkFinalPosition(Vector3Int pos) => true;
    protected virtual bool checkIntermediatePosition(Vector3Int pos) => !Character.tracker.OutOfBounds(pos);
    public virtual bool checkPosition(Character user, Vector3Int pos) => getCellsInRange(user).Contains(pos);
    public virtual HashSet<ModuleKeyword> renewableKeywords() => new();
    public HashSet<ModuleKeyword> tempKeywords { get; private set; }
    public void AddTemporaryKeyword(ModuleKeyword keyword)
    {
        tempKeywords.Add(keyword);
        UpdateTooltipIfCurrent();
    }
    public void AddTemporaryKeywords(params ModuleKeyword[] keywords)
    {
        foreach (var k in keywords)
            AddTemporaryKeyword(k);
    }
    public void ClearTemporaryKeywords() => tempKeywords.Clear();
    public HashSet<ModuleKeyword> constKeywords { get; private set; }
    public void AddConstKeyword(ModuleKeyword keyword)
    {
        constKeywords.Add(keyword);
        UpdateTooltipIfCurrent();
    }
    public void RemoveConstKeyword(ModuleKeyword keyword)
    {
        constKeywords.Remove(keyword);
        UpdateTooltipIfCurrent();
    }
    public void AddConstKeywords(params ModuleKeyword[] keywords)
    {
        foreach (var k in keywords)
            AddConstKeyword(k);
    }
    public void ReplaceConstKeyword(ModuleKeyword toDelete, ModuleKeyword replacement)
    {
        RemoveConstKeyword(toDelete);
        AddConstKeyword(replacement);
    }
    public HashSet<ModuleKeyword> GetKeywords()
    {
        var res = constKeywords;
        res.UnionWith(renewableKeywords());
        res.UnionWith(tempKeywords);
        return res;
    }
    public HashSet<VisibleKeyword> GetVisibleKeywords() => GetKeywords().Where(k => k is VisibleKeyword).Select(k => k as VisibleKeyword).ToHashSet();
    public bool HasAllKeywords(params ModuleKeyword[] keywords) => HasAllKeywords(keywords.ToHashSet());
    public bool HasAllKeywords(IEnumerable<ModuleKeyword> keywords) => keywords?.All(k => GetKeywords().Any(kw => kw.Equals(k))) ?? true;
    public bool HasAnyKeywords(params ModuleKeyword[] keywords) => HasAnyKeywords(keywords.ToHashSet());
    public bool HasAnyKeywords(IEnumerable<ModuleKeyword> keywords) => keywords?.Any(k => GetKeywords().Any(kw => kw.Equals(k))) ?? true;
    private HashSet<FrequencyLimiterKeyword> GetFrequencyLimiterKeywords() =>
        constKeywords.Union(tempKeywords).Where(k => k is FrequencyLimiterKeyword).Select(k => k as FrequencyLimiterKeyword).ToHashSet();
    public virtual bool CanBeUsed() => GetFrequencyLimiterKeywords().All(k => k.CanBeUsed());
    public void SpendUse()
    {
        foreach (var freq in GetFrequencyLimiterKeywords())
            freq.SpendUseLeft();
        UpdateTooltipIfCurrent();
    }
    public void Recharge(TriggerType trigger)
    {
        foreach (var freq in GetFrequencyLimiterKeywords().Where(f => f.rechargeTime == trigger))
            freq.Recharge();
        UpdateTooltipIfCurrent();
    }
    protected void UpdateTooltipIfCurrent()
    {
        if (TooltipSystem.IsCurrent(this))
            TooltipSystem.Update(TooltipSystem.TooltipType.moduleTooltip);
    }
}

