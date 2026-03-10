using System;
using System.Collections.Generic;
using System.Text;
 
public abstract class ModuleKeyword
{
    public string Name { get; protected set; }
    public string Description { get; protected set; }
    public ModuleKeyword()
    {
        changeName(GetType().Name);
        Description = "Default_Keyword_Description";
    }
    protected void changeName(string value) => Name = value;
}