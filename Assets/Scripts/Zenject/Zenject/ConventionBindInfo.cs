using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Zenject
{
    // Token: 0x0200005E RID: 94
    [NoReflectionBaking]
    public class ConventionBindInfo
    {
        // Token: 0x06000263 RID: 611 RVA: 0x00007B90 File Offset: 0x00005D90
        public void AddAssemblyFilter(Func<Assembly, bool> predicate)
        {
            this._assemblyFilters.Add(predicate);
        }

        // Token: 0x06000264 RID: 612 RVA: 0x00007BA0 File Offset: 0x00005DA0
        public void AddTypeFilter(Func<Type, bool> predicate)
        {
            this._typeFilters.Add(predicate);
        }

        // Token: 0x06000265 RID: 613 RVA: 0x00007BB0 File Offset: 0x00005DB0
        private IEnumerable<Assembly> GetAllAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }

        // Token: 0x06000266 RID: 614 RVA: 0x00007BBC File Offset: 0x00005DBC
        private bool ShouldIncludeAssembly(Assembly assembly)
        {
            return this._assemblyFilters.All((Func<Assembly, bool> predicate) => predicate(assembly));
        }

        // Token: 0x06000267 RID: 615 RVA: 0x00007BF0 File Offset: 0x00005DF0
        private bool ShouldIncludeType(Type type)
        {
            return this._typeFilters.All((Func<Type, bool> predicate) => predicate(type));
        }

        // Token: 0x06000268 RID: 616 RVA: 0x00007C24 File Offset: 0x00005E24
        private Type[] GetTypes(Assembly assembly)
        {
            Type[] types;
            if (!ConventionBindInfo._assemblyTypeCache.TryGetValue(assembly, out types))
            {
                types = assembly.GetTypes();
                ConventionBindInfo._assemblyTypeCache[assembly] = types;
            }
            return types;
        }

        // Token: 0x06000269 RID: 617 RVA: 0x00007C54 File Offset: 0x00005E54
        public List<Type> ResolveTypes()
        {
            return this.GetAllAssemblies().Where(new Func<Assembly, bool>(this.ShouldIncludeAssembly)).SelectMany((Assembly assembly) => this.GetTypes(assembly)).Where(new Func<Type, bool>(this.ShouldIncludeType)).ToList<Type>();
        }

        // Token: 0x040000D8 RID: 216
        private readonly List<Func<Type, bool>> _typeFilters = new List<Func<Type, bool>>();

        // Token: 0x040000D9 RID: 217
        private readonly List<Func<Assembly, bool>> _assemblyFilters = new List<Func<Assembly, bool>>();

        // Token: 0x040000DA RID: 218
        private static readonly Dictionary<Assembly, Type[]> _assemblyTypeCache = new Dictionary<Assembly, Type[]>();
    }
}
