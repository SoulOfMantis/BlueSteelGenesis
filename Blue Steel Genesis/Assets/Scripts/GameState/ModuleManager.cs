using System;
using System.Collections.Generic;
using System.Text;

internal class ModuleManager
{
    public static event Action ModulesChanged;

    public static IReadOnlyList<GameModule> Modules =>
        GameState.Run.Expedition.Player.modules.AsReadOnly();

    private static List<GameModule> EditableModules =>
        GameState.Run.Expedition.Player.modules;

    public static void SwapModules(int idx1, int idx2)
    {
        var modules = EditableModules;
        if (idx1 < 0 || idx2 < 0 || idx1 >= modules.Count || idx2 >= modules.Count)
            return;
        (modules[idx1], modules[idx2]) = (modules[idx2], modules[idx1]);
        ModulesChanged?.Invoke();
    }

    public static void MoveModuleUp(int idx) => SwapModules(idx, idx - 1);
    public static void MoveModuleDown(int idx) => SwapModules(idx, idx + 1);

    public static void AddModule(GameModule module)
    {
        if (module == null) return;
        EditableModules.Add(module);
        ModulesChanged?.Invoke();
    }

    public static void RemoveModule(int idx)
    {
        if (idx < 0 || idx >= EditableModules.Count) return;
        EditableModules.RemoveAt(idx);
        ModulesChanged?.Invoke();
    }
}