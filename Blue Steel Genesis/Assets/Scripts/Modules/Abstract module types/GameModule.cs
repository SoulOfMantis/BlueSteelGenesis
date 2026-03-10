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
    public HashSet<ModuleKeyword> Keywords { get; private set; }
    public string Name { get; protected set; }
    private string icon_name = "default_default.png";
    public string Icon_name { get => icon_name; protected set => icon_name = value; }
    public int range = 0;
    public abstract string Description(); 
    public GameModule()
    {
        changeName(GetType().Name);
        Keywords = new();
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

    public virtual void Initialize()
    {
        Debug.Log($"Module {Name} initialized");
    }
    protected virtual bool checkFinalPosition(Vector3Int pos) => true;
    protected virtual bool checkIntermediatePosition(Vector3Int pos) => !Character.tracker.OutOfBounds(pos);
    public virtual bool checkPosition(Character user, Vector3Int pos) => getCellsInRange(user).Contains(pos);
    public void AddKeyword(ModuleKeyword keyword) =>
        Keywords.Add(keyword);
    public void AddKeywords(params ModuleKeyword[] keywords)
    {
        foreach (var k in keywords)
            AddKeyword(k);
    }
    public bool HasKeywords(params ModuleKeyword[] keywords) => keywords.All(k => Keywords.Any(kw => kw.GetType() == k.GetType()));

}

