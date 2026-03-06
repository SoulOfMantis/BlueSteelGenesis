using System;
using System.Linq;
using System.Collections.Generic;
public class ModuleGenerator
{
    Random gen;
    static List<Type> commonModuleTypes = null;
    static List<Type> bossModuleTypes = null;
    public ModuleGenerator(int seed) => UpdateSeed(seed);
    public GameModule GetNextCommonModule(List<GameModule> forbidden = null) => GetNextCommonModule(GetForbiddenTypes(forbidden));
    public GameModule GetNextCommonModule(List<Type> forbidden = null)
    {
        if (commonModuleTypes == null) getModuleTypes();
        int id = gen.Next(commonModuleTypes.Count);
        Type moduleType = commonModuleTypes[id];
        if (forbidden != null)
            while (forbidden.Contains(moduleType))
            {
                id = gen.Next(commonModuleTypes.Count);
                moduleType = commonModuleTypes[id];
            }
        return Activator.CreateInstance(moduleType) as GameModule;
    }
    public GameModule GetNextBossModule(List<GameModule> forbidden = null) => GetNextBossModule(GetForbiddenTypes(forbidden));
    public GameModule GetNextBossModule(List<Type> forbidden = null)
    {
        if (bossModuleTypes == null) getModuleTypes();
        int id = gen.Next(bossModuleTypes.Count);
        Type moduleType = bossModuleTypes[id];
        if (forbidden != null)
            while (forbidden.Contains(moduleType))
            {
                id = gen.Next(bossModuleTypes.Count);
                moduleType = bossModuleTypes[id];
            }
        return Activator.CreateInstance(moduleType) as GameModule;
    }
    static List<Type> GetForbiddenTypes(List<GameModule> forbidden) => forbidden?.Select(m => m.GetType()).Distinct().ToList();
    static bool isCommon(Type moduleType) => hasKeywords(moduleType, "Common");
    static bool isCommon(GameModule module) => hasKeywords(module, "Common");
    static bool isBoss(Type moduleType) => hasKeywords(moduleType, "Boss");
    static bool isBoss(GameModule module) => hasKeywords(module, "Boss");
    static bool hasKeywords(Type moduleType, string keyword) => hasKeywords(moduleType, new List<string> { keyword });
    static bool hasKeywords(Type moduleType, List<string> keywords) =>!moduleType.IsAbstract && hasKeywords((Activator.CreateInstance(moduleType) as GameModule), keywords);
    static bool hasKeywords(GameModule module, List<string> keywords) =>  keywords.All(k => module.Keywords.Contains(k));
    static bool hasKeywords(GameModule module, string keyword) => hasKeywords(module, new List<string> { keyword });

    static void getModuleTypes()
    {
        commonModuleTypes = typeof(GameModule).Assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(GameModule)) && isCommon(type)).ToList();
        bossModuleTypes = typeof(GameModule).Assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(GameModule)) && isBoss(type)).ToList();
    }
    public void UpdateSeed(int seed) => gen = new(seed);
}

