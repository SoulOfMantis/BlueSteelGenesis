using System;
using System.Collections.Generic;
using System.Text;
public class ModuleGenerator
{
    Random gen;
    public ModuleGenerator(int seed) => UpdateSeed(seed);
    public GameModule GetNextModule()
    {
        //gen.Next();
        //TODO: get name of the module by id, preferably without switchcase with all possible names/array with all possible names
        //return Activator.CreateInstance(Type.GetType("")) as GameModule;
        return null;
    }
    public void UpdateSeed(int seed) => gen = new(seed);
}

