using System;
using System.Collections.Generic;
using System.Text;
 
public abstract class ModuleKeyword
{
    public override int GetHashCode()
    {
        return base.GetHashCode() & GetType().GetHashCode();
    }
    public override bool Equals(object obj)
    {
        return obj.GetType() == GetType();
    }
}