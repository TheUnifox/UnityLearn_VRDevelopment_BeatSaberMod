using System;
using ModestTree;
using UnityEngine;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x020001F8 RID: 504
    public class PrefabResourceFactory<T> : IFactory<string, T>, IFactory
    {
        // Token: 0x17000097 RID: 151
        // (get) Token: 0x06000A6A RID: 2666 RVA: 0x0001B6F8 File Offset: 0x000198F8
        public DiContainer Container
        {
            get
            {
                return this._container;
            }
        }

        // Token: 0x06000A6B RID: 2667 RVA: 0x0001B700 File Offset: 0x00019900
        public virtual T Create(string prefabResourceName)
        {
            ModestTree.Assert.That(!string.IsNullOrEmpty(prefabResourceName), "Null or empty prefab resource name given to factory create method when instantiating object with type '{0}'.", typeof(T));
            GameObject prefab = (GameObject)Resources.Load(prefabResourceName);
            return this._container.InstantiatePrefabForComponent<T>(prefab);
        }

        // Token: 0x06000A6D RID: 2669 RVA: 0x0001B74C File Offset: 0x0001994C
        private static object __zenCreate(object[] P_0)
        {
            return new PrefabResourceFactory<T>();
        }

        // Token: 0x06000A6E RID: 2670 RVA: 0x0001B764 File Offset: 0x00019964
        private static void __zenFieldSetter0(object P_0, object P_1)
        {
            ((PrefabResourceFactory<T>)P_0)._container = (DiContainer)P_1;
        }

        // Token: 0x06000A6F RID: 2671 RVA: 0x0001B784 File Offset: 0x00019984
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(PrefabResourceFactory<T>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(PrefabResourceFactory<T>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[]
            {
                new InjectTypeInfo.InjectMemberInfo(new ZenMemberSetterMethod(PrefabResourceFactory<T>.__zenFieldSetter0), new InjectableInfo(false, null, "_container", typeof(DiContainer), null, InjectSources.Any))
            });
        }

        // Token: 0x04000312 RID: 786
        [Inject]
        private DiContainer _container;
    }

    public class PrefabResourceFactory<P1, T> : IFactory<string, P1, T>, IFactory
    {
        // Token: 0x17000098 RID: 152
        // (get) Token: 0x06000A70 RID: 2672 RVA: 0x0001B808 File Offset: 0x00019A08
        public DiContainer Container
        {
            get
            {
                return this._container;
            }
        }

        // Token: 0x06000A71 RID: 2673 RVA: 0x0001B810 File Offset: 0x00019A10
        public virtual T Create(string prefabResourceName, P1 param)
        {
            ModestTree.Assert.That(!string.IsNullOrEmpty(prefabResourceName), "Null or empty prefab resource name given to factory create method when instantiating object with type '{0}'.", typeof(T));
            GameObject prefab = (GameObject)Resources.Load(prefabResourceName);
            return (T)((object)this._container.InstantiatePrefabForComponentExplicit(typeof(T), prefab, InjectUtil.CreateArgListExplicit<P1>(param)));
        }

        // Token: 0x06000A73 RID: 2675 RVA: 0x0001B870 File Offset: 0x00019A70
        private static object __zenCreate(object[] P_0)
        {
            return new PrefabResourceFactory<P1, T>();
        }

        // Token: 0x06000A74 RID: 2676 RVA: 0x0001B888 File Offset: 0x00019A88
        private static void __zenFieldSetter0(object P_0, object P_1)
        {
            ((PrefabResourceFactory<P1, T>)P_0)._container = (DiContainer)P_1;
        }

        // Token: 0x06000A75 RID: 2677 RVA: 0x0001B8A8 File Offset: 0x00019AA8
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(PrefabResourceFactory<P1, T>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(PrefabResourceFactory<P1, T>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[]
            {
                new InjectTypeInfo.InjectMemberInfo(new ZenMemberSetterMethod(PrefabResourceFactory<P1, T>.__zenFieldSetter0), new InjectableInfo(false, null, "_container", typeof(DiContainer), null, InjectSources.Any))
            });
        }

        // Token: 0x04000313 RID: 787
        [Inject]
        private DiContainer _container;
    }

    public class PrefabResourceFactory<P1, P2, T> : IFactory<string, P1, P2, T>, IFactory
    {
        // Token: 0x17000099 RID: 153
        // (get) Token: 0x06000A76 RID: 2678 RVA: 0x0001B92C File Offset: 0x00019B2C
        public DiContainer Container
        {
            get
            {
                return this._container;
            }
        }

        // Token: 0x06000A77 RID: 2679 RVA: 0x0001B934 File Offset: 0x00019B34
        public virtual T Create(string prefabResourceName, P1 param, P2 param2)
        {
            ModestTree.Assert.That(!string.IsNullOrEmpty(prefabResourceName), "Null or empty prefab resource name given to factory create method when instantiating object with type '{0}'.", typeof(T));
            GameObject prefab = (GameObject)Resources.Load(prefabResourceName);
            return (T)((object)this._container.InstantiatePrefabForComponentExplicit(typeof(T), prefab, InjectUtil.CreateArgListExplicit<P1, P2>(param, param2)));
        }

        // Token: 0x06000A79 RID: 2681 RVA: 0x0001B994 File Offset: 0x00019B94
        private static object __zenCreate(object[] P_0)
        {
            return new PrefabResourceFactory<P1, P2, T>();
        }

        // Token: 0x06000A7A RID: 2682 RVA: 0x0001B9AC File Offset: 0x00019BAC
        private static void __zenFieldSetter0(object P_0, object P_1)
        {
            ((PrefabResourceFactory<P1, P2, T>)P_0)._container = (DiContainer)P_1;
        }

        // Token: 0x06000A7B RID: 2683 RVA: 0x0001B9CC File Offset: 0x00019BCC
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(PrefabResourceFactory<P1, P2, T>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(PrefabResourceFactory<P1, P2, T>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[]
            {
                new InjectTypeInfo.InjectMemberInfo(new ZenMemberSetterMethod(PrefabResourceFactory<P1, P2, T>.__zenFieldSetter0), new InjectableInfo(false, null, "_container", typeof(DiContainer), null, InjectSources.Any))
            });
        }

        // Token: 0x04000314 RID: 788
        [Inject]
        private DiContainer _container;
    }

    public class PrefabResourceFactory<P1, P2, P3, T> : IFactory<string, P1, P2, P3, T>, IFactory
    {
        // Token: 0x1700009A RID: 154
        // (get) Token: 0x06000A7C RID: 2684 RVA: 0x0001BA50 File Offset: 0x00019C50
        public DiContainer Container
        {
            get
            {
                return this._container;
            }
        }

        // Token: 0x06000A7D RID: 2685 RVA: 0x0001BA58 File Offset: 0x00019C58
        public virtual T Create(string prefabResourceName, P1 param, P2 param2, P3 param3)
        {
            ModestTree.Assert.That(!string.IsNullOrEmpty(prefabResourceName), "Null or empty prefab resource name given to factory create method when instantiating object with type '{0}'.", typeof(T));
            GameObject prefab = (GameObject)Resources.Load(prefabResourceName);
            return (T)((object)this._container.InstantiatePrefabForComponentExplicit(typeof(T), prefab, InjectUtil.CreateArgListExplicit<P1, P2, P3>(param, param2, param3)));
        }

        // Token: 0x06000A7F RID: 2687 RVA: 0x0001BABC File Offset: 0x00019CBC
        private static object __zenCreate(object[] P_0)
        {
            return new PrefabResourceFactory<P1, P2, P3, T>();
        }

        // Token: 0x06000A80 RID: 2688 RVA: 0x0001BAD4 File Offset: 0x00019CD4
        private static void __zenFieldSetter0(object P_0, object P_1)
        {
            ((PrefabResourceFactory<P1, P2, P3, T>)P_0)._container = (DiContainer)P_1;
        }

        // Token: 0x06000A81 RID: 2689 RVA: 0x0001BAF4 File Offset: 0x00019CF4
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(PrefabResourceFactory<P1, P2, P3, T>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(PrefabResourceFactory<P1, P2, P3, T>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[]
            {
                new InjectTypeInfo.InjectMemberInfo(new ZenMemberSetterMethod(PrefabResourceFactory<P1, P2, P3, T>.__zenFieldSetter0), new InjectableInfo(false, null, "_container", typeof(DiContainer), null, InjectSources.Any))
            });
        }

        // Token: 0x04000315 RID: 789
        [Inject]
        private DiContainer _container;
    }

    public class PrefabResourceFactory<P1, P2, P3, P4, T> : IFactory<string, P1, P2, P3, P4, T>, IFactory
    {
        // Token: 0x1700009B RID: 155
        // (get) Token: 0x06000A82 RID: 2690 RVA: 0x0001BB78 File Offset: 0x00019D78
        public DiContainer Container
        {
            get
            {
                return this._container;
            }
        }

        // Token: 0x06000A83 RID: 2691 RVA: 0x0001BB80 File Offset: 0x00019D80
        public virtual T Create(string prefabResourceName, P1 param, P2 param2, P3 param3, P4 param4)
        {
            ModestTree.Assert.That(!string.IsNullOrEmpty(prefabResourceName), "Null or empty prefab resource name given to factory create method when instantiating object with type '{0}'.", typeof(T));
            GameObject prefab = (GameObject)Resources.Load(prefabResourceName);
            return (T)((object)this._container.InstantiatePrefabForComponentExplicit(typeof(T), prefab, InjectUtil.CreateArgListExplicit<P1, P2, P3, P4>(param, param2, param3, param4)));
        }

        // Token: 0x06000A85 RID: 2693 RVA: 0x0001BBE4 File Offset: 0x00019DE4
        private static object __zenCreate(object[] P_0)
        {
            return new PrefabResourceFactory<P1, P2, P3, P4, T>();
        }

        // Token: 0x06000A86 RID: 2694 RVA: 0x0001BBFC File Offset: 0x00019DFC
        private static void __zenFieldSetter0(object P_0, object P_1)
        {
            ((PrefabResourceFactory<P1, P2, P3, P4, T>)P_0)._container = (DiContainer)P_1;
        }

        // Token: 0x06000A87 RID: 2695 RVA: 0x0001BC1C File Offset: 0x00019E1C
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(PrefabResourceFactory<P1, P2, P3, P4, T>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(PrefabResourceFactory<P1, P2, P3, P4, T>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[]
            {
                new InjectTypeInfo.InjectMemberInfo(new ZenMemberSetterMethod(PrefabResourceFactory<P1, P2, P3, P4, T>.__zenFieldSetter0), new InjectableInfo(false, null, "_container", typeof(DiContainer), null, InjectSources.Any))
            });
        }

        // Token: 0x04000316 RID: 790
        [Inject]
        private DiContainer _container;
    }
}