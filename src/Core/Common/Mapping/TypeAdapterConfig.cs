namespace GamaEdtech.Common.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class TypeAdapterConfig : Mapster.TypeAdapterConfig
    {
        public TypeAdapterConfig()
            : base()
        {
        }

        internal new IEnumerable<IRegister?> Scan(params Assembly[] assemblies)
        {
            var lst = assemblies.SelectMany(assembly => assembly.GetTypes()
                .Where(t => typeof(IRegister).GetTypeInfo().IsAssignableFrom(t.GetTypeInfo()) && t.GetTypeInfo().IsClass && !t.GetTypeInfo().IsAbstract))
                .Select(t => Activator.CreateInstance(t) as IRegister);
            foreach (var register in lst)
            {
                register?.Register(this);
            }

            return lst;
        }
    }
}
