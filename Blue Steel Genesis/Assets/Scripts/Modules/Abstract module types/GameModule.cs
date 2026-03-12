using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
/// <summary>
/// класс модуля
/// </summary>
public abstract class GameModule
{
    public string Name { get; protected set; }
    private string icon_name = "default_default.png";
    public string Icon_name { get => icon_name; protected set => icon_name = value; }
    public int range = 0;
    public abstract string Description();
    public GameModule()
    {
        changeName(GetType().Name);
        constKeywords = new();
        tempKeywords = new();
    }
    protected List<Vector3Int> getAvailableCells(int n, Vector3Int start)
    {
        var res = new HashSet<Vector3Int>();
        HashSet<Vector3Int> toAdd = new HashSet<Vector3Int>();
        res.Add(start);
        for (int i = 1; i <= n; i++)
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
    public virtual List<Vector3Int> getCellsInRange(Vector3Int start) => getAvailableCells(range, start);

    public void changeName(string newName) => Name = newName;
    public abstract Task Effect(Character user, Vector3Int pos);
    public Task Use(Character user, Vector3Int pos)
    {
        if (!CanBeUsed()) return Task.CompletedTask;
        SpendUse();
        return Effect(user, pos);
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
    public void AddTemporaryKeyword(ModuleKeyword keyword) =>
        tempKeywords.Add(keyword);
    public void AddTemporaryKeywords(params ModuleKeyword[] keywords)
    {
        foreach (var k in keywords)
            AddTemporaryKeyword(k);
    }
    public void ClearTemporaryKeywords() => tempKeywords.Clear();
    public HashSet<ModuleKeyword> constKeywords { get; private set; }
    public void AddConstKeyword(ModuleKeyword keyword) =>
        constKeywords.Add(keyword);
    public void AddConstKeywords(params ModuleKeyword[] keywords)
    {
        foreach (var k in keywords)
            AddConstKeyword(k);
    }
    public HashSet<ModuleKeyword> GetKeywords()
    {
        var res = constKeywords;
        res.UnionWith(renewableKeywords());
        res.UnionWith(tempKeywords);
        return res;
    }
    public bool HasKeywords(params ModuleKeyword[] keywords) => keywords.All(k => GetKeywords().Any(kw => kw.GetType() == k.GetType()));
    private HashSet<FrequencyLimiterKeyword> GetFrequencyLimiterKeywords() =>
        constKeywords.Union(tempKeywords).Where(k => k is FrequencyLimiterKeyword).Select(k => k as FrequencyLimiterKeyword).ToHashSet();
    public virtual bool CanBeUsed() => GetFrequencyLimiterKeywords().All(k => k.CanBeUsed());
    public void SpendUse()
    {
        foreach (var freq in GetFrequencyLimiterKeywords())
            freq.SpendUseLeft();
    }
    public void Recharge(TriggerType trigger)
    {
        foreach (var freq in GetFrequencyLimiterKeywords().Where(f => f.rechargeTime == trigger))
            freq.Recharge();
    }

}

