using System;
using System.Collections.Generic;
using System.Text;

namespace TestAbstractFactory
{
    public class AssemblyMechanism
    {
        private static TypeMapperDictionary dictionary = new TypeMapperDictionary();

        static AssemblyMechanism()
        {
            dictionary.Add(typeof(ConcreteFactoryX), new ConcreteXTypeMapper());
            dictionary.Add(typeof(ConcreteFactoryY), new ConcreteYTypeMapper());
        }

        public static void Assembly(IAbstractFactoryWithTypeMapper factory)
        {
            TypeMapperBase mapper = dictionary[factory.GetType()];
            factory.Mapper = mapper;
        }
    }
}
