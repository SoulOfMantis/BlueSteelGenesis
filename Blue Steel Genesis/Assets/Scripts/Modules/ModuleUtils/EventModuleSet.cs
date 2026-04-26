using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// ‘иктивный модуль, объедин€ющий несколько GameModule дл€ отображени€ в одной подсказке.
/// </summary>
public class GameModuleSet : GameModule
{
    private List<GameModule> modules = new();

    public GameModuleSet(IEnumerable<GameModule> modules)
    {
        this.modules.AddRange(modules);
        changeName($"{this.modules.Count} module(s)");
    }

    public override string Description()
    {
        StringBuilder sb = new();
        foreach (var mod in modules)
        {
            sb.AppendLine($"<b>{mod.Name}</b>");
            sb.AppendLine(mod.Description());
            sb.AppendLine();
        }
        return sb.ToString().TrimEnd();
    }

    public override Task Effect(Character user, Vector3Int pos) => Task.CompletedTask;
    protected override bool checkIntermediatePosition(Vector3Int pos) => false;
}