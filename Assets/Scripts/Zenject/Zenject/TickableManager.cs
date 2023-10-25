using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using ModestTree.Util;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x020002E4 RID: 740
    public class TickableManager
    {
        // Token: 0x06000FD9 RID: 4057 RVA: 0x0002CC24 File Offset: 0x0002AE24
        [Inject]
        public TickableManager()
        {
        }

        // Token: 0x1700015B RID: 347
        // (get) Token: 0x06000FDA RID: 4058 RVA: 0x0002CC50 File Offset: 0x0002AE50
        public IEnumerable<ITickable> Tickables
        {
            get
            {
                return this._tickables;
            }
        }

        // Token: 0x1700015C RID: 348
        // (get) Token: 0x06000FDB RID: 4059 RVA: 0x0002CC58 File Offset: 0x0002AE58
        // (set) Token: 0x06000FDC RID: 4060 RVA: 0x0002CC60 File Offset: 0x0002AE60
        public bool IsPaused
        {
            get
            {
                return this._isPaused;
            }
            set
            {
                this._isPaused = value;
            }
        }

        // Token: 0x06000FDD RID: 4061 RVA: 0x0002CC6C File Offset: 0x0002AE6C
        [Inject]
        public void Initialize()
        {
            this.InitTickables();
            this.InitFixedTickables();
            this.InitLateTickables();
        }

        // Token: 0x06000FDE RID: 4062 RVA: 0x0002CC80 File Offset: 0x0002AE80
        private void InitFixedTickables()
        {
            foreach (Type type in from x in this._fixedPriorities
                                  select x.First)
            {
                ModestTree.Assert.That(type.DerivesFrom<IFixedTickable>(), "Expected type '{0}' to drive from IFixedTickable while checking priorities in TickableHandler", type);
            }
            using (List<IFixedTickable>.Enumerator enumerator2 = this._fixedTickables.GetEnumerator())
            {
                while (enumerator2.MoveNext())
                {
                    IFixedTickable tickable = enumerator2.Current;
                    List<int> list = (from x in this._fixedPriorities
                                      where tickable.GetType().DerivesFromOrEqual(x.First)
                                      select x.Second).ToList<int>();
                    int priority = list.IsEmpty<int>() ? 0 : list.Distinct<int>().Single<int>();
                    this._fixedUpdater.AddTask(tickable, priority);
                }
            }
        }

        // Token: 0x06000FDF RID: 4063 RVA: 0x0002CDB8 File Offset: 0x0002AFB8
        private void InitTickables()
        {
            foreach (Type type in from x in this._priorities
                                  select x.First)
            {
                ModestTree.Assert.That(type.DerivesFrom<ITickable>(), "Expected type '{0}' to drive from ITickable while checking priorities in TickableHandler", type);
            }
            using (List<ITickable>.Enumerator enumerator2 = this._tickables.GetEnumerator())
            {
                while (enumerator2.MoveNext())
                {
                    ITickable tickable = enumerator2.Current;
                    List<int> list = (from x in this._priorities
                                      where tickable.GetType().DerivesFromOrEqual(x.First)
                                      select x.Second).ToList<int>();
                    int priority = list.IsEmpty<int>() ? 0 : list.Distinct<int>().Single<int>();
                    this._updater.AddTask(tickable, priority);
                }
            }
        }

        // Token: 0x06000FE0 RID: 4064 RVA: 0x0002CEF0 File Offset: 0x0002B0F0
        private void InitLateTickables()
        {
            foreach (Type type in from x in this._latePriorities
                                  select x.First)
            {
                ModestTree.Assert.That(type.DerivesFrom<ILateTickable>(), "Expected type '{0}' to drive from ILateTickable while checking priorities in TickableHandler", type);
            }
            using (List<ILateTickable>.Enumerator enumerator2 = this._lateTickables.GetEnumerator())
            {
                while (enumerator2.MoveNext())
                {
                    ILateTickable tickable = enumerator2.Current;
                    List<int> list = (from x in this._latePriorities
                                      where tickable.GetType().DerivesFromOrEqual(x.First)
                                      select x.Second).ToList<int>();
                    int priority = list.IsEmpty<int>() ? 0 : list.Distinct<int>().Single<int>();
                    this._lateUpdater.AddTask(tickable, priority);
                }
            }
        }

        // Token: 0x06000FE1 RID: 4065 RVA: 0x0002D028 File Offset: 0x0002B228
        public void Add(ITickable tickable, int priority)
        {
            this._updater.AddTask(tickable, priority);
        }

        // Token: 0x06000FE2 RID: 4066 RVA: 0x0002D038 File Offset: 0x0002B238
        public void Add(ITickable tickable)
        {
            this.Add(tickable, 0);
        }

        // Token: 0x06000FE3 RID: 4067 RVA: 0x0002D044 File Offset: 0x0002B244
        public void AddLate(ILateTickable tickable, int priority)
        {
            this._lateUpdater.AddTask(tickable, priority);
        }

        // Token: 0x06000FE4 RID: 4068 RVA: 0x0002D054 File Offset: 0x0002B254
        public void AddLate(ILateTickable tickable)
        {
            this.AddLate(tickable, 0);
        }

        // Token: 0x06000FE5 RID: 4069 RVA: 0x0002D060 File Offset: 0x0002B260
        public void AddFixed(IFixedTickable tickable, int priority)
        {
            this._fixedUpdater.AddTask(tickable, priority);
        }

        // Token: 0x06000FE6 RID: 4070 RVA: 0x0002D070 File Offset: 0x0002B270
        public void AddFixed(IFixedTickable tickable)
        {
            this._fixedUpdater.AddTask(tickable, 0);
        }

        // Token: 0x06000FE7 RID: 4071 RVA: 0x0002D080 File Offset: 0x0002B280
        public void Remove(ITickable tickable)
        {
            this._updater.RemoveTask(tickable);
        }

        // Token: 0x06000FE8 RID: 4072 RVA: 0x0002D090 File Offset: 0x0002B290
        public void RemoveLate(ILateTickable tickable)
        {
            this._lateUpdater.RemoveTask(tickable);
        }

        // Token: 0x06000FE9 RID: 4073 RVA: 0x0002D0A0 File Offset: 0x0002B2A0
        public void RemoveFixed(IFixedTickable tickable)
        {
            this._fixedUpdater.RemoveTask(tickable);
        }

        // Token: 0x06000FEA RID: 4074 RVA: 0x0002D0B0 File Offset: 0x0002B2B0
        public void Update()
        {
            if (this.IsPaused)
            {
                return;
            }
            this._updater.OnFrameStart();
            this._updater.UpdateAll();
        }

        // Token: 0x06000FEB RID: 4075 RVA: 0x0002D0D4 File Offset: 0x0002B2D4
        public void FixedUpdate()
        {
            if (this.IsPaused)
            {
                return;
            }
            this._fixedUpdater.OnFrameStart();
            this._fixedUpdater.UpdateAll();
        }

        // Token: 0x06000FEC RID: 4076 RVA: 0x0002D0F8 File Offset: 0x0002B2F8
        public void LateUpdate()
        {
            if (this.IsPaused)
            {
                return;
            }
            this._lateUpdater.OnFrameStart();
            this._lateUpdater.UpdateAll();
        }

        // Token: 0x06000FED RID: 4077 RVA: 0x0002D11C File Offset: 0x0002B31C
        private static object __zenCreate(object[] P_0)
        {
            return new TickableManager();
        }

        // Token: 0x06000FEE RID: 4078 RVA: 0x0002D134 File Offset: 0x0002B334
        private static void __zenFieldSetter0(object P_0, object P_1)
        {
            ((TickableManager)P_0)._tickables = (List<ITickable>)P_1;
        }

        // Token: 0x06000FEF RID: 4079 RVA: 0x0002D154 File Offset: 0x0002B354
        private static void __zenFieldSetter1(object P_0, object P_1)
        {
            ((TickableManager)P_0)._fixedTickables = (List<IFixedTickable>)P_1;
        }

        // Token: 0x06000FF0 RID: 4080 RVA: 0x0002D174 File Offset: 0x0002B374
        private static void __zenFieldSetter2(object P_0, object P_1)
        {
            ((TickableManager)P_0)._lateTickables = (List<ILateTickable>)P_1;
        }

        // Token: 0x06000FF1 RID: 4081 RVA: 0x0002D194 File Offset: 0x0002B394
        private static void __zenFieldSetter3(object P_0, object P_1)
        {
            ((TickableManager)P_0)._priorities = (List<ModestTree.Util.ValuePair<Type, int>>)P_1;
        }

        // Token: 0x06000FF2 RID: 4082 RVA: 0x0002D1B4 File Offset: 0x0002B3B4
        private static void __zenFieldSetter4(object P_0, object P_1)
        {
            ((TickableManager)P_0)._fixedPriorities = (List<ModestTree.Util.ValuePair<Type, int>>)P_1;
        }

        // Token: 0x06000FF3 RID: 4083 RVA: 0x0002D1D4 File Offset: 0x0002B3D4
        private static void __zenFieldSetter5(object P_0, object P_1)
        {
            ((TickableManager)P_0)._latePriorities = (List<ModestTree.Util.ValuePair<Type, int>>)P_1;
        }

        // Token: 0x06000FF4 RID: 4084 RVA: 0x0002D1F4 File Offset: 0x0002B3F4
        private static void __zenInjectMethod0(object P_0, object[] P_1)
        {
            ((TickableManager)P_0).Initialize();
        }

        // Token: 0x06000FF5 RID: 4085 RVA: 0x0002D204 File Offset: 0x0002B404
        [Zenject.Internal.Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(TickableManager), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(TickableManager.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[]
            {
                new InjectTypeInfo.InjectMethodInfo(new ZenInjectMethod(TickableManager.__zenInjectMethod0), new InjectableInfo[0], "Initialize")
            }, new InjectTypeInfo.InjectMemberInfo[]
            {
                new InjectTypeInfo.InjectMemberInfo(new ZenMemberSetterMethod(TickableManager.__zenFieldSetter0), new InjectableInfo(true, null, "_tickables", typeof(List<ITickable>), null, InjectSources.Local)),
                new InjectTypeInfo.InjectMemberInfo(new ZenMemberSetterMethod(TickableManager.__zenFieldSetter1), new InjectableInfo(true, null, "_fixedTickables", typeof(List<IFixedTickable>), null, InjectSources.Local)),
                new InjectTypeInfo.InjectMemberInfo(new ZenMemberSetterMethod(TickableManager.__zenFieldSetter2), new InjectableInfo(true, null, "_lateTickables", typeof(List<ILateTickable>), null, InjectSources.Local)),
                new InjectTypeInfo.InjectMemberInfo(new ZenMemberSetterMethod(TickableManager.__zenFieldSetter3), new InjectableInfo(true, null, "_priorities", typeof(List<ModestTree.Util.ValuePair<Type, int>>), null, InjectSources.Local)),
                new InjectTypeInfo.InjectMemberInfo(new ZenMemberSetterMethod(TickableManager.__zenFieldSetter4), new InjectableInfo(true, "Fixed", "_fixedPriorities", typeof(List<ModestTree.Util.ValuePair<Type, int>>), null, InjectSources.Local)),
                new InjectTypeInfo.InjectMemberInfo(new ZenMemberSetterMethod(TickableManager.__zenFieldSetter5), new InjectableInfo(true, "Late", "_latePriorities", typeof(List<ModestTree.Util.ValuePair<Type, int>>), null, InjectSources.Local))
            });
        }

        // Token: 0x040004FC RID: 1276
        [Inject(Optional = true, Source = InjectSources.Local)]
        private readonly List<ITickable> _tickables;

        // Token: 0x040004FD RID: 1277
        [Inject(Optional = true, Source = InjectSources.Local)]
        private readonly List<IFixedTickable> _fixedTickables;

        // Token: 0x040004FE RID: 1278
        [Inject(Optional = true, Source = InjectSources.Local)]
        private readonly List<ILateTickable> _lateTickables;

        // Token: 0x040004FF RID: 1279
        [Inject(Optional = true, Source = InjectSources.Local)]
        private readonly List<ModestTree.Util.ValuePair<Type, int>> _priorities;

        // Token: 0x04000500 RID: 1280
        [Inject(Optional = true, Id = "Fixed", Source = InjectSources.Local)]
        private readonly List<ModestTree.Util.ValuePair<Type, int>> _fixedPriorities;

        // Token: 0x04000501 RID: 1281
        [Inject(Optional = true, Id = "Late", Source = InjectSources.Local)]
        private readonly List<ModestTree.Util.ValuePair<Type, int>> _latePriorities;

        // Token: 0x04000502 RID: 1282
        private readonly TickablesTaskUpdater _updater = new TickablesTaskUpdater();

        // Token: 0x04000503 RID: 1283
        private readonly FixedTickablesTaskUpdater _fixedUpdater = new FixedTickablesTaskUpdater();

        // Token: 0x04000504 RID: 1284
        private readonly LateTickablesTaskUpdater _lateUpdater = new LateTickablesTaskUpdater();

        // Token: 0x04000505 RID: 1285
        private bool _isPaused;
    }
}
