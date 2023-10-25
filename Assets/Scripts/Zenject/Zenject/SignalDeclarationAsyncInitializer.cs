using System;
using System.Collections.Generic;
using ModestTree;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x02000043 RID: 67
    public class SignalDeclarationAsyncInitializer : IInitializable
    {
        // Token: 0x060001DB RID: 475 RVA: 0x00006860 File Offset: 0x00004A60
        public SignalDeclarationAsyncInitializer([Inject(Source = InjectSources.Local)] List<SignalDeclaration> declarations, [Inject(Optional = true, Source = InjectSources.Local)] LazyInject<TickableManager> tickManager)
        {
            this._declarations = declarations;
            this._tickManager = tickManager;
        }

        // Token: 0x060001DC RID: 476 RVA: 0x00006878 File Offset: 0x00004A78
        public void Initialize()
        {
            for (int i = 0; i < this._declarations.Count; i++)
            {
                SignalDeclaration signalDeclaration = this._declarations[i];
                if (signalDeclaration.IsAsync)
                {
                    ModestTree.Assert.IsNotNull(this._tickManager.Value, "TickableManager is required when using asynchronous signals");
                    this._tickManager.Value.Add(signalDeclaration, signalDeclaration.TickPriority);
                }
            }
        }

        // Token: 0x060001DD RID: 477 RVA: 0x000068DC File Offset: 0x00004ADC
        private static object __zenCreate(object[] P_0)
        {
            return new SignalDeclarationAsyncInitializer((List<SignalDeclaration>)P_0[0], (LazyInject<TickableManager>)P_0[1]);
        }

        // Token: 0x060001DE RID: 478 RVA: 0x0000690C File Offset: 0x00004B0C
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(SignalDeclarationAsyncInitializer), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(SignalDeclarationAsyncInitializer.__zenCreate), new InjectableInfo[]
            {
                new InjectableInfo(false, null, "declarations", typeof(List<SignalDeclaration>), null, InjectSources.Local),
                new InjectableInfo(true, null, "tickManager", typeof(LazyInject<TickableManager>), null, InjectSources.Local)
            }), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x04000094 RID: 148
        private readonly LazyInject<TickableManager> _tickManager;

        // Token: 0x04000095 RID: 149
        private readonly List<SignalDeclaration> _declarations;
    }
}
