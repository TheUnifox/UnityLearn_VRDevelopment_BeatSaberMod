using System;
using ModestTree;
using UnityEngine;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x020001F3 RID: 499
    public class PrefabFactory<T> : IFactory<UnityEngine.Object, T>, IFactory
    {
        // Token: 0x17000092 RID: 146
        // (get) Token: 0x06000A4C RID: 2636 RVA: 0x0001B1B4 File Offset: 0x000193B4
        public DiContainer Container
        {
            get
            {
                return this._container;
            }
        }

        // Token: 0x06000A4D RID: 2637 RVA: 0x0001B1BC File Offset: 0x000193BC
        public virtual T Create(UnityEngine.Object prefab)
        {
            ModestTree.Assert.That(prefab != null, "Null prefab given to factory create method when instantiating object with type '{0}'.", typeof(T));
            return this._container.InstantiatePrefabForComponent<T>(prefab);
        }

        // Token: 0x06000A4F RID: 2639 RVA: 0x0001B1F0 File Offset: 0x000193F0
        private static object __zenCreate(object[] P_0)
        {
            return new PrefabFactory<T>();
        }

        // Token: 0x06000A50 RID: 2640 RVA: 0x0001B208 File Offset: 0x00019408
        private static void __zenFieldSetter0(object P_0, object P_1)
        {
            ((PrefabFactory<T>)P_0)._container = (DiContainer)P_1;
        }

        // Token: 0x06000A51 RID: 2641 RVA: 0x0001B228 File Offset: 0x00019428
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(PrefabFactory<T>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(PrefabFactory<T>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[]
            {
                new InjectTypeInfo.InjectMemberInfo(new ZenMemberSetterMethod(PrefabFactory<T>.__zenFieldSetter0), new InjectableInfo(false, null, "_container", typeof(DiContainer), null, InjectSources.Any))
            });
        }

        // Token: 0x0400030D RID: 781
        [Inject]
        private DiContainer _container;
    }

    public class PrefabFactory<P1, T> : IFactory<UnityEngine.Object, P1, T>, IFactory
    {
        // Token: 0x17000093 RID: 147
        // (get) Token: 0x06000A52 RID: 2642 RVA: 0x0001B2AC File Offset: 0x000194AC
        public DiContainer Container
        {
            get
            {
                return this._container;
            }
        }

        // Token: 0x06000A53 RID: 2643 RVA: 0x0001B2B4 File Offset: 0x000194B4
        public virtual T Create(UnityEngine.Object prefab, P1 param)
        {
            ModestTree.Assert.That(prefab != null, "Null prefab given to factory create method when instantiating object with type '{0}'.", typeof(T));
            return (T)((object)this._container.InstantiatePrefabForComponentExplicit(typeof(T), prefab, InjectUtil.CreateArgListExplicit<P1>(param)));
        }

        // Token: 0x06000A55 RID: 2645 RVA: 0x0001B2FC File Offset: 0x000194FC
        private static object __zenCreate(object[] P_0)
        {
            return new PrefabFactory<P1, T>();
        }

        // Token: 0x06000A56 RID: 2646 RVA: 0x0001B314 File Offset: 0x00019514
        private static void __zenFieldSetter0(object P_0, object P_1)
        {
            ((PrefabFactory<P1, T>)P_0)._container = (DiContainer)P_1;
        }

        // Token: 0x06000A57 RID: 2647 RVA: 0x0001B334 File Offset: 0x00019534
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(PrefabFactory<P1, T>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(PrefabFactory<P1, T>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[]
            {
                new InjectTypeInfo.InjectMemberInfo(new ZenMemberSetterMethod(PrefabFactory<P1, T>.__zenFieldSetter0), new InjectableInfo(false, null, "_container", typeof(DiContainer), null, InjectSources.Any))
            });
        }

        // Token: 0x0400030E RID: 782
        [Inject]
        private DiContainer _container;
    }

    public class PrefabFactory<P1, P2, T> : IFactory<UnityEngine.Object, P1, P2, T>, IFactory
    {
        // Token: 0x17000094 RID: 148
        // (get) Token: 0x06000A58 RID: 2648 RVA: 0x0001B3B8 File Offset: 0x000195B8
        public DiContainer Container
        {
            get
            {
                return this._container;
            }
        }

        // Token: 0x06000A59 RID: 2649 RVA: 0x0001B3C0 File Offset: 0x000195C0
        public virtual T Create(UnityEngine.Object prefab, P1 param, P2 param2)
        {
            ModestTree.Assert.That(prefab != null, "Null prefab given to factory create method when instantiating object with type '{0}'.", typeof(T));
            return (T)((object)this._container.InstantiatePrefabForComponentExplicit(typeof(T), prefab, InjectUtil.CreateArgListExplicit<P1, P2>(param, param2)));
        }

        // Token: 0x06000A5B RID: 2651 RVA: 0x0001B408 File Offset: 0x00019608
        private static object __zenCreate(object[] P_0)
        {
            return new PrefabFactory<P1, P2, T>();
        }

        // Token: 0x06000A5C RID: 2652 RVA: 0x0001B420 File Offset: 0x00019620
        private static void __zenFieldSetter0(object P_0, object P_1)
        {
            ((PrefabFactory<P1, P2, T>)P_0)._container = (DiContainer)P_1;
        }

        // Token: 0x06000A5D RID: 2653 RVA: 0x0001B440 File Offset: 0x00019640
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(PrefabFactory<P1, P2, T>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(PrefabFactory<P1, P2, T>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[]
            {
                new InjectTypeInfo.InjectMemberInfo(new ZenMemberSetterMethod(PrefabFactory<P1, P2, T>.__zenFieldSetter0), new InjectableInfo(false, null, "_container", typeof(DiContainer), null, InjectSources.Any))
            });
        }

        // Token: 0x0400030F RID: 783
        [Inject]
        private DiContainer _container;
    }

    public class PrefabFactory<P1, P2, P3, T> : IFactory<UnityEngine.Object, P1, P2, P3, T>, IFactory
    {
        // Token: 0x17000095 RID: 149
        // (get) Token: 0x06000A5E RID: 2654 RVA: 0x0001B4C4 File Offset: 0x000196C4
        public DiContainer Container
        {
            get
            {
                return this._container;
            }
        }

        // Token: 0x06000A5F RID: 2655 RVA: 0x0001B4CC File Offset: 0x000196CC
        public virtual T Create(UnityEngine.Object prefab, P1 param, P2 param2, P3 param3)
        {
            ModestTree.Assert.That(prefab != null, "Null prefab given to factory create method when instantiating object with type '{0}'.", typeof(T));
            return (T)((object)this._container.InstantiatePrefabForComponentExplicit(typeof(T), prefab, InjectUtil.CreateArgListExplicit<P1, P2, P3>(param, param2, param3)));
        }

        // Token: 0x06000A61 RID: 2657 RVA: 0x0001B520 File Offset: 0x00019720
        private static object __zenCreate(object[] P_0)
        {
            return new PrefabFactory<P1, P2, P3, T>();
        }

        // Token: 0x06000A62 RID: 2658 RVA: 0x0001B538 File Offset: 0x00019738
        private static void __zenFieldSetter0(object P_0, object P_1)
        {
            ((PrefabFactory<P1, P2, P3, T>)P_0)._container = (DiContainer)P_1;
        }

        // Token: 0x06000A63 RID: 2659 RVA: 0x0001B558 File Offset: 0x00019758
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(PrefabFactory<P1, P2, P3, T>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(PrefabFactory<P1, P2, P3, T>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[]
            {
                new InjectTypeInfo.InjectMemberInfo(new ZenMemberSetterMethod(PrefabFactory<P1, P2, P3, T>.__zenFieldSetter0), new InjectableInfo(false, null, "_container", typeof(DiContainer), null, InjectSources.Any))
            });
        }

        // Token: 0x04000310 RID: 784
        [Inject]
        private DiContainer _container;
    }

    public class PrefabFactory<P1, P2, P3, P4, T> : IFactory<UnityEngine.Object, P1, P2, P3, P4, T>, IFactory
    {
        // Token: 0x17000096 RID: 150
        // (get) Token: 0x06000A64 RID: 2660 RVA: 0x0001B5DC File Offset: 0x000197DC
        public DiContainer Container
        {
            get
            {
                return this._container;
            }
        }

        // Token: 0x06000A65 RID: 2661 RVA: 0x0001B5E4 File Offset: 0x000197E4
        public virtual T Create(UnityEngine.Object prefab, P1 param, P2 param2, P3 param3, P4 param4)
        {
            ModestTree.Assert.That(prefab != null, "Null prefab given to factory create method when instantiating object with type '{0}'.", typeof(T));
            return (T)((object)this._container.InstantiatePrefabForComponentExplicit(typeof(T), prefab, InjectUtil.CreateArgListExplicit<P1, P2, P3, P4>(param, param2, param3, param4)));
        }

        // Token: 0x06000A67 RID: 2663 RVA: 0x0001B63C File Offset: 0x0001983C
        private static object __zenCreate(object[] P_0)
        {
            return new PrefabFactory<P1, P2, P3, P4, T>();
        }

        // Token: 0x06000A68 RID: 2664 RVA: 0x0001B654 File Offset: 0x00019854
        private static void __zenFieldSetter0(object P_0, object P_1)
        {
            ((PrefabFactory<P1, P2, P3, P4, T>)P_0)._container = (DiContainer)P_1;
        }

        // Token: 0x06000A69 RID: 2665 RVA: 0x0001B674 File Offset: 0x00019874
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(PrefabFactory<P1, P2, P3, P4, T>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(PrefabFactory<P1, P2, P3, P4, T>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[]
            {
                new InjectTypeInfo.InjectMemberInfo(new ZenMemberSetterMethod(PrefabFactory<P1, P2, P3, P4, T>.__zenFieldSetter0), new InjectableInfo(false, null, "_container", typeof(DiContainer), null, InjectSources.Any))
            });
        }

        // Token: 0x04000311 RID: 785
        [Inject]
        private DiContainer _container;
    }
}
