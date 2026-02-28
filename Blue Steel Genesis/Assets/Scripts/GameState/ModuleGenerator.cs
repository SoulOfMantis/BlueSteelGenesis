using System;
using System.Linq;
using System.Collections.Generic;
public class ModuleGenerator
{
    Random gen;
    static List<Type> normalModuleTypes = null;
    public ModuleGenerator(int seed) => UpdateSeed(seed);
    public GameModule GetNextModule(List<Type> forbidden = null)
    {
        if (normalModuleTypes == null) getModuleTypes();
        int id = gen.Next(normalModuleTypes.Count);
        Type moduleType = normalModuleTypes[id];
        if (forbidden != null)
        {
            while (forbidden.Contains(moduleType))
            {
                id = gen.Next(normalModuleTypes.Count);
                moduleType = normalModuleTypes[id];
            }
        } 
        return Activator.CreateInstance(moduleType) as GameModule;
    }
    static void getModuleTypes()
    {
        normalModuleTypes = typeof(GameModule).Assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(GameModule)) && !type.IsAbstract 
        && !type.IsSubclassOf(typeof(StatusModule))).ToList();
    }
    public void UpdateSeed(int seed) => gen = new(seed);
}

