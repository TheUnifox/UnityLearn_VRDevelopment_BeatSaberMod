using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Zenject
{
    // Token: 0x0200005B RID: 91
    [NoReflectionBaking]
    public class ConventionAssemblySelectionBinder
    {
        // Token: 0x0600024E RID: 590 RVA: 0x0000798C File Offset: 0x00005B8C
        public ConventionAssemblySelectionBinder(ConventionBindInfo bindInfo)
        {
            this.BindInfo = bindInfo;
        }

        // Token: 0x1700003A RID: 58
        // (get) Token: 0x0600024F RID: 591 RVA: 0x0000799C File Offset: 0x00005B9C
        // (set) Token: 0x06000250 RID: 592 RVA: 0x000079A4 File Offset: 0x00005BA4
        protected ConventionBindInfo BindInfo { get; private set; }

        // Token: 0x06000251 RID: 593 RVA: 0x000079B0 File Offset: 0x00005BB0
        public void FromAllAssemblies()
        {
        }

        // Token: 0x06000252 RID: 594 RVA: 0x000079B4 File Offset: 0x00005BB4
        public void FromAssemblyContaining<T>()
        {
            this.FromAssembliesContaining(new Type[]
            {
                typeof(T)
            });
        }

        // Token: 0x06000253 RID: 595 RVA: 0x000079D0 File Offset: 0x00005BD0
        public void FromAssembliesContaining(params Type[] types)
        {
            this.FromAssembliesContaining(types);
        }

        // Token: 0x06000254 RID: 596 RVA: 0x000079DC File Offset: 0x00005BDC
        public void FromAssembliesContaining(IEnumerable<Type> types)
        {
            this.FromAssemblies((from t in types
                                 select t.Assembly).Distinct<Assembly>());
        }

        // Token: 0x06000255 RID: 597 RVA: 0x00007A10 File Offset: 0x00005C10
        public void FromThisAssembly()
        {
            this.FromAssemblies(new Assembly[]
            {
                Assembly.GetCallingAssembly()
            });
        }

        // Token: 0x06000256 RID: 598 RVA: 0x00007A28 File Offset: 0x00005C28
        public void FromAssembly(Assembly assembly)
        {
            this.FromAssemblies(new Assembly[]
            {
                assembly
            });
        }

        // Token: 0x06000257 RID: 599 RVA: 0x00007A3C File Offset: 0x00005C3C
        public void FromAssemblies(params Assembly[] assemblies)
        {
            this.FromAssemblies(assemblies);
        }

        // Token: 0x06000258 RID: 600 RVA: 0x00007A48 File Offset: 0x00005C48
        public void FromAssemblies(IEnumerable<Assembly> assemblies)
        {
            this.BindInfo.AddAssemblyFilter((Assembly assembly) => assemblies.Contains(assembly));
        }

        // Token: 0x06000259 RID: 601 RVA: 0x00007A7C File Offset: 0x00005C7C
        public void FromAssembliesWhere(Func<Assembly, bool> predicate)
        {
            this.BindInfo.AddAssemblyFilter(predicate);
        }
    }
}
