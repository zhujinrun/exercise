using System;
using System.Collections.Generic;
using System.Text;

namespace TestAbstractFactory
{
    public abstract class TypeMapperBase:Dictionary<Type,Type>
    {
    }

    public class TypeMapperDictionary:Dictionary<Type, TypeMapperBase> { }
}
