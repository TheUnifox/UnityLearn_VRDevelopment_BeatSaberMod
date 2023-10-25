using System;
using System.Collections.Generic;
using ModestTree;

namespace Zenject
{
    // Token: 0x02000237 RID: 567
    [NoReflectionBaking]
    public class LazyInstanceInjector
    {
        // Token: 0x06000D21 RID: 3361 RVA: 0x0002377C File Offset: 0x0002197C
        public LazyInstanceInjector(DiContainer container)
        {
            this._container = container;
        }

        // Token: 0x170000EB RID: 235
        // (get) Token: 0x06000D22 RID: 3362 RVA: 0x00023798 File Offset: 0x00021998
        public IEnumerable<object> Instances
        {
            get
            {
                return this._instancesToInject;
            }
        }

        // Token: 0x06000D23 RID: 3363 RVA: 0x000237A0 File Offset: 0x000219A0
        public void AddInstance(object instance)
        {
            this._instancesToInject.Add(instance);
        }

        // Token: 0x06000D24 RID: 3364 RVA: 0x000237B0 File Offset: 0x000219B0
        public void AddInstances(IEnumerable<object> instances)
        {
            this._instancesToInject.UnionWith(instances);
        }

        // Token: 0x06000D25 RID: 3365 RVA: 0x000237C0 File Offset: 0x000219C0
        public void LazyInject(object instance)
        {
            if (this._instancesToInject.Remove(instance))
            {
                this._container.Inject(instance);
            }
        }

        // Token: 0x06000D26 RID: 3366 RVA: 0x000237DC File Offset: 0x000219DC
        public void LazyInjectAll()
        {
            List<object> list = new List<object>();
            while (!this._instancesToInject.IsEmpty<object>())
            {
                list.Clear();
                list.AddRange(this._instancesToInject);
                foreach (object instance in list)
                {
                    this.LazyInject(instance);
                }
            }
        }

        // Token: 0x040003B8 RID: 952
        private readonly DiContainer _container;

        // Token: 0x040003B9 RID: 953
        private readonly HashSet<object> _instancesToInject = new HashSet<object>();
    }
}
