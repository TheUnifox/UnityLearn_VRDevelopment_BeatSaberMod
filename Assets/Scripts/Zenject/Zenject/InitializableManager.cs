using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using ModestTree.Util;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x020002CC RID: 716
    public class InitializableManager
    {
        // Token: 0x06000F52 RID: 3922 RVA: 0x0002B374 File Offset: 0x00029574
        [Inject]
        public InitializableManager([Inject(Optional = true, Source = InjectSources.Local)] List<IInitializable> initializables, [Inject(Optional = true, Source = InjectSources.Local)] List<ModestTree.Util.ValuePair<Type, int>> priorities)
        {
            this._initializables = new List<InitializableManager.InitializableInfo>();
            for (int i = 0; i < initializables.Count; i++)
            {
                IInitializable initializable = initializables[i];
                List<int> list = (from x in priorities
                                  where initializable.GetType().DerivesFromOrEqual(x.First)
                                  select x.Second).ToList<int>();
                int priority = list.IsEmpty<int>() ? 0 : list.Distinct<int>().Single<int>();
                this._initializables.Add(new InitializableManager.InitializableInfo(initializable, priority));
            }
        }

        // Token: 0x06000F53 RID: 3923 RVA: 0x0002B428 File Offset: 0x00029628
        public void Add(IInitializable initializable)
        {
            this.Add(initializable, 0);
        }

        // Token: 0x06000F54 RID: 3924 RVA: 0x0002B434 File Offset: 0x00029634
        public void Add(IInitializable initializable, int priority)
        {
            ModestTree.Assert.That(!this._hasInitialized);
            this._initializables.Add(new InitializableManager.InitializableInfo(initializable, priority));
        }

        // Token: 0x06000F55 RID: 3925 RVA: 0x0002B458 File Offset: 0x00029658
        public void Initialize()
        {
            ModestTree.Assert.That(!this._hasInitialized);
            this._hasInitialized = true;
            this._initializables = (from x in this._initializables
                                    orderby x.Priority
                                    select x).ToList<InitializableManager.InitializableInfo>();
            foreach (InitializableManager.InitializableInfo initializableInfo in this._initializables)
            {
                try
                {
                    initializableInfo.Initializable.Initialize();
                }
                catch (Exception innerException)
                {
                    throw ModestTree.Assert.CreateException(innerException, "Error occurred while initializing IInitializable with type '{0}'", new object[]
                    {
                        initializableInfo.Initializable.GetType()
                    });
                }
            }
        }

        // Token: 0x06000F56 RID: 3926 RVA: 0x0002B528 File Offset: 0x00029728
        private static object __zenCreate(object[] P_0)
        {
            return new InitializableManager((List<IInitializable>)P_0[0], (List<ModestTree.Util.ValuePair<Type, int>>)P_0[1]);
        }

        // Token: 0x06000F57 RID: 3927 RVA: 0x0002B558 File Offset: 0x00029758
        [Zenject.Internal.Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(InitializableManager), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(InitializableManager.__zenCreate), new InjectableInfo[]
            {
                new InjectableInfo(true, null, "initializables", typeof(List<IInitializable>), null, InjectSources.Local),
                new InjectableInfo(true, null, "priorities", typeof(List<ModestTree.Util.ValuePair<Type, int>>), null, InjectSources.Local)
            }), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x040004D2 RID: 1234
        private List<InitializableManager.InitializableInfo> _initializables;

        // Token: 0x040004D3 RID: 1235
        private bool _hasInitialized;

        // Token: 0x020002CD RID: 717
        private class InitializableInfo
        {
            // Token: 0x06000F58 RID: 3928 RVA: 0x0002B5EC File Offset: 0x000297EC
            public InitializableInfo(IInitializable initializable, int priority)
            {
                this.Initializable = initializable;
                this.Priority = priority;
            }

            // Token: 0x06000F59 RID: 3929 RVA: 0x0002B604 File Offset: 0x00029804
            private static object __zenCreate(object[] P_0)
            {
                return new InitializableManager.InitializableInfo((IInitializable)P_0[0], (int)P_0[1]);
            }

            // Token: 0x06000F5A RID: 3930 RVA: 0x0002B634 File Offset: 0x00029834
            [Zenject.Internal.Preserve]
            private static InjectTypeInfo __zenCreateInjectTypeInfo()
            {
                return new InjectTypeInfo(typeof(InitializableManager.InitializableInfo), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(InitializableManager.InitializableInfo.__zenCreate), new InjectableInfo[]
                {
                    new InjectableInfo(false, null, "initializable", typeof(IInitializable), null, InjectSources.Any),
                    new InjectableInfo(false, null, "priority", typeof(int), null, InjectSources.Any)
                }), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
            }

            // Token: 0x040004D4 RID: 1236
            public IInitializable Initializable;

            // Token: 0x040004D5 RID: 1237
            public int Priority;
        }
    }
}
