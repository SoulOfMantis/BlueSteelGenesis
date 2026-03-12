using System;
using System.Linq;
using System.Collections.Generic;
public class ModuleGenerator
{
    Random gen;
    static List<Type> commonModuleTypes = null;
    static List<Type> bossModuleTypes = null;
    public ModuleGenerator(int seed) => UpdateSeed(seed);
    public static GameModule CreateModuleByType(Type type)
    {
        if (!type.IsSubclassOf(typeof(GameModule)) || type.IsAbstract) throw new ArgumentException();
        return Activator.CreateInstance(type) as GameModule;
    }
    public static GameModule CreateModuleByType(Type type, params Object[] parameters)
    {
        if (!type.IsSubclassOf(typeof(GameModule)) || type.IsAbstract) throw new ArgumentException();
        return Activator.CreateInstance(type, parameters) as GameModule;
    }
    public GameModule GetNextCommonModule() => GetNextCommonModule((IEnumerable<Type>)null);
    public GameModule GetNextCommonModule(IEnumerable<GameModule> forbidden = null) => GetNextCommonModule(GetForbiddenTypes(forbidden));
    public GameModule GetNextCommonModule(IEnumerable<Type> forbidden = null)
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
        return CreateModuleByType(moduleType);
    }
    public GameModule GetNextBossModule() => GetNextBossModule((IEnumerable<Type>)null);
    public GameModule GetNextBossModule(IEnumerable<GameModule> forbidden = null) => GetNextBossModule(GetForbiddenTypes(forbidden));
    public GameModule GetNextBossModule(IEnumerable<Type> forbidden = null)
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
        return CreateModuleByType(moduleType);
    }
    static List<Type> GetForbiddenTypes(IEnumerable<GameModule> forbidden) => forbidden?.Select(m => m.GetType()).Distinct().ToList();
    static bool isCommon(Type moduleType) => !moduleType.IsAbstract && isCommon(CreateModuleByType(moduleType));
    static bool isCommon(GameModule module) => hasKeywords(module, new CommonKeyword());
    static bool isBoss(Type moduleType) => !moduleType.IsAbstract && isBoss(CreateModuleByType(moduleType));
    static bool isBoss(GameModule module) => hasKeywords(module, new BossKeyword());
    static bool hasKeywords(Type moduleType, params ModuleKeyword[] keywords) =>!moduleType.IsAbstract && hasKeywords(CreateModuleByType(moduleType), keywords);
    static bool hasKeywords(GameModule module, params ModuleKeyword[] keywords) =>  module.HasKeywords(keywords);
    static void getModuleTypes()
    {
        commonModuleTypes = typeof(GameModule).Assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(GameModule)) && isCommon(type)).ToList();
        bossModuleTypes = typeof(GameModule).Assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(GameModule)) && isBoss(type)).ToList();
    }
    public void UpdateSeed(int seed) => gen = new(seed);
}

