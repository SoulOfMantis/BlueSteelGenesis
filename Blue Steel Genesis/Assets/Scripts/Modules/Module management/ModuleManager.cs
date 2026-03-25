using System;
using System.Collections.Generic;

public static class ModuleManager
{
    public static event Action ModulesChanged;
    public static uint MinEditableModuleIndex => 1;
    public static uint MaxEditableModuleIndex => Math.Min((uint)Modules.Count - 1, 4);
    public static IReadOnlyList<GameModule> Modules =>
        GameState.Run.Expedition.Player.modules.AsReadOnly();

    private static List<GameModule> EditableModules =>
        GameState.Run.Expedition.Player.modules;

    public static void SwapModules(uint idx1, uint idx2)
    {
        var modules = EditableModules;
        if (idx1 < MinEditableModuleIndex || idx2 < idx1 || idx2 > MaxEditableModuleIndex)
            return;
        (modules[(int)idx1], modules[(int)idx2]) = (modules[(int)idx2], modules[(int)idx1]);
        ModulesChanged?.Invoke();
    }

    public static void MoveModuleUp(uint idx) => SwapModules(idx - 1, idx);
    public static void MoveModuleDown(uint idx) => SwapModules(idx, idx + 1);

    public static bool AddModule(GameModule module)
    {
        if (module == null || MaxEditableModuleIndex >= 4) return false;
        EditableModules.Add(module);
        ModulesChanged?.Invoke();
        return true;
    }

    public static void RemoveModule(uint idx)
    {
        if (idx < MinEditableModuleIndex || idx > MaxEditableModuleIndex) return;
        EditableModules.RemoveAt((int)idx);
        ModulesChanged?.Invoke();
    }
    public static void SellModule(uint idx)
    {
        if (idx < MinEditableModuleIndex || idx > MaxEditableModuleIndex) return;
        GameState.Run.Expedition.Shop.Sell(EditableModules[(int)idx]);
        ModulesChanged?.Invoke();
    }
}