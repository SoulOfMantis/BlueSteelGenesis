using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ModuleGenerator
{
    [SerializeField]
    Unity.Mathematics.Random gen;
    static List<Type> commonModuleTypes = null;
    static readonly Type defaultCommonModuleType = typeof(DefaultCommon);
    static List<Type> bossModuleTypes = null;
    static readonly Type defaultBossModuleType = typeof(DefaultBoss);
    public ModuleGenerator(int seed) => UpdateSeed(seed);
    public static GameModule CreateModuleByType(Type type)
    {
        if (!type.IsSubclassOf(typeof(GameModule)) || type.IsAbstract) throw new ArgumentException();
        return Activator.CreateInstance(type) as GameModule;
    }
    public static GameModule CreateModuleByType(Type type, params System.Object[] parameters)
    {
        if (!type.IsSubclassOf(typeof(GameModule)) || type.IsAbstract) throw new ArgumentException();
        return Activator.CreateInstance(type, parameters) as GameModule;
    }
    public GameModule GetNextCommonModule(IEnumerable<GameModule> forbidden, IEnumerable<ModuleKeyword> requiredKeywords = null, IEnumerable<ModuleKeyword> forbiddenKeywords = null) =>
        GetNextCommonModule(GetForbiddenTypes(forbidden), requiredKeywords, forbiddenKeywords);
    public GameModule GetNextCommonModule(IEnumerable<Type> forbidden = null, IEnumerable<ModuleKeyword> requiredKeywords = null, IEnumerable<ModuleKeyword> forbiddenKeywords = null)
    {
        if (commonModuleTypes == null) getModuleTypes();
        var types = commonModuleTypes.Except(forbidden ?? new List<Type>()).Where(t => hasAllKeywords(t, requiredKeywords)).ToList(); ;
        if (forbiddenKeywords != null)
            types = commonModuleTypes.Where(t => !hasAnyKeywords(t, forbiddenKeywords)).ToList();
        if (types.Count == 0) return CreateModuleByType(defaultCommonModuleType);
        int id = gen.NextInt(types.Count);
        Type moduleType = types[id];
        return CreateModuleByType(moduleType);
    }
    public GameModule GetNextBossModule(IEnumerable<GameModule> forbidden, IEnumerable<ModuleKeyword> requiredKeywords = null, IEnumerable<ModuleKeyword> forbiddenKeywords = null) => 
        GetNextBossModule(GetForbiddenTypes(forbidden), requiredKeywords, forbiddenKeywords);
    public GameModule GetNextBossModule(IEnumerable<Type> forbidden = null, IEnumerable<ModuleKeyword> requiredKeywords = null, IEnumerable<ModuleKeyword> forbiddenKeywords = null)
    {
        if (bossModuleTypes == null) getModuleTypes();
        var types = bossModuleTypes.Except(forbidden ?? new List<Type>()).Where(t => hasAllKeywords(t, requiredKeywords)).ToList(); ;
        if (forbiddenKeywords != null)
            types = commonModuleTypes.Where(t => !hasAnyKeywords(t, forbiddenKeywords)).ToList();
        if (types.Count == 0) return CreateModuleByType(defaultBossModuleType);
        int id = gen.NextInt(types.Count);
        Type moduleType = types[id];
        return CreateModuleByType(moduleType);
    }
    static List<Type> GetForbiddenTypes(IEnumerable<GameModule> forbidden) => forbidden?.Where(m => m != null)?.Select(m => m.GetType())?.Distinct()?.ToList();
    public static bool isCommon(Type moduleType) => !moduleType.IsAbstract && isCommon(CreateModuleByType(moduleType));
    public static bool isCommon(GameModule module) => hasAllKeywords(module, new CommonKeyword());
    public static bool isBoss(Type moduleType) => !moduleType.IsAbstract && isBoss(CreateModuleByType(moduleType));
    public static bool isBoss(GameModule module) => hasAllKeywords(module, new BossKeyword());
    
    public static bool hasAllKeywords(Type moduleType, IEnumerable<ModuleKeyword> keywords) => !moduleType.IsAbstract && hasAllKeywords(CreateModuleByType(moduleType), keywords);
    public static bool hasAllKeywords(Type moduleType, params ModuleKeyword[] keywords) => !moduleType.IsAbstract && hasAllKeywords(CreateModuleByType(moduleType), keywords);
    public static bool hasAllKeywords(GameModule module, IEnumerable<ModuleKeyword> keywords) => module.HasAllKeywords(keywords);
    public static bool hasAllKeywords(GameModule module, params ModuleKeyword[] keywords) => module.HasAllKeywords(keywords);
    
    public static bool hasAnyKeywords(Type moduleType, IEnumerable<ModuleKeyword> keywords) => !moduleType.IsAbstract && hasAnyKeywords(CreateModuleByType(moduleType), keywords);
    public static bool hasAnyKeywords(Type moduleType, params ModuleKeyword[] keywords) => !moduleType.IsAbstract && hasAnyKeywords(CreateModuleByType(moduleType), keywords);
    public static bool hasAnyKeywords(GameModule module, IEnumerable<ModuleKeyword> keywords) => module.HasAnyKeywords(keywords);
    public static bool hasAnyKeywords(GameModule module, params ModuleKeyword[] keywords) => module.HasAnyKeywords(keywords);

    static void getModuleTypes()
    {
        commonModuleTypes = typeof(GameModule).Assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(GameModule)) && isCommon(type)).ToList();
        commonModuleTypes.Remove(defaultCommonModuleType);
        bossModuleTypes = typeof(GameModule).Assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(GameModule)) && isBoss(type)).ToList();
        bossModuleTypes.Remove(defaultBossModuleType);
    }
    public void UpdateSeed(int seed) => gen = new((uint)seed);
}

