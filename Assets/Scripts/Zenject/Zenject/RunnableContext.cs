using System;
using ModestTree;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x02000209 RID: 521
    public abstract class RunnableContext : Context
    {
        // Token: 0x170000BE RID: 190
        // (get) Token: 0x06000B24 RID: 2852 RVA: 0x0001DCD8 File Offset: 0x0001BED8
        // (set) Token: 0x06000B25 RID: 2853 RVA: 0x0001DCE0 File Offset: 0x0001BEE0
        public bool Initialized { get; private set; }

        // Token: 0x06000B26 RID: 2854 RVA: 0x0001DCEC File Offset: 0x0001BEEC
        protected void Initialize()
        {
            if (RunnableContext._staticAutoRun && this._autoRun)
            {
                this.Run();
                return;
            }
            RunnableContext._staticAutoRun = true;
        }

        // Token: 0x06000B27 RID: 2855 RVA: 0x0001DD0C File Offset: 0x0001BF0C
        public void Run()
        {
            ModestTree.Assert.That(!this.Initialized, "The context already has been initialized!");
            this.RunInternal();
            this.Initialized = true;
        }

        // Token: 0x06000B28 RID: 2856
        protected abstract void RunInternal();

        // Token: 0x06000B29 RID: 2857 RVA: 0x0001DD30 File Offset: 0x0001BF30
        public static T CreateComponent<T>(GameObject gameObject) where T : RunnableContext
        {
            RunnableContext._staticAutoRun = false;
            T result = gameObject.AddComponent<T>();
            ModestTree.Assert.That(RunnableContext._staticAutoRun);
            return result;
        }

        // Token: 0x06000B2C RID: 2860 RVA: 0x0001DD60 File Offset: 0x0001BF60
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(RunnableContext), new InjectTypeInfo.InjectConstructorInfo(null, new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x04000352 RID: 850
        [Tooltip("When false, wait until run method is explicitly called. Otherwise run on initialize")]
        [SerializeField]
        private bool _autoRun = true;

        // Token: 0x04000353 RID: 851
        private static bool _staticAutoRun = true;
    }
}
