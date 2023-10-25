using System;
using System.Collections.Generic;
using ModestTree;

namespace Zenject
{
    // Token: 0x02000286 RID: 646
    [NoReflectionBaking]
    public class SubContainerCreatorByMethod : SubContainerCreatorByMethodBase
    {
        // Token: 0x06000E6F RID: 3695 RVA: 0x00027820 File Offset: 0x00025A20
        public SubContainerCreatorByMethod(DiContainer container, SubContainerCreatorBindInfo containerBindInfo, Action<DiContainer> installMethod) : base(container, containerBindInfo)
        {
            this._installMethod = installMethod;
        }

        // Token: 0x06000E70 RID: 3696 RVA: 0x00027834 File Offset: 0x00025A34
        public override DiContainer CreateSubContainer(List<TypeValuePair> args, InjectContext context)
        {
            ModestTree.Assert.IsEmpty<TypeValuePair>(args);
            DiContainer diContainer = base.CreateEmptySubContainer();
            this._installMethod(diContainer);
            diContainer.ResolveRoots();
            return diContainer;
        }

        // Token: 0x0400045E RID: 1118
        private readonly Action<DiContainer> _installMethod;
    }

    [NoReflectionBaking]
    public class SubContainerCreatorByMethod<TParam1> : SubContainerCreatorByMethodBase
    {
        // Token: 0x06000E71 RID: 3697 RVA: 0x00027864 File Offset: 0x00025A64
        public SubContainerCreatorByMethod(DiContainer container, SubContainerCreatorBindInfo containerBindInfo, Action<DiContainer, TParam1> installMethod) : base(container, containerBindInfo)
        {
            this._installMethod = installMethod;
        }

        // Token: 0x06000E72 RID: 3698 RVA: 0x00027878 File Offset: 0x00025A78
        public override DiContainer CreateSubContainer(List<TypeValuePair> args, InjectContext context)
        {
            ModestTree.Assert.IsEqual(args.Count, 1);
            ModestTree.Assert.That(args[0].Type.DerivesFromOrEqual<TParam1>());
            DiContainer diContainer = base.CreateEmptySubContainer();
            this._installMethod(diContainer, (TParam1)((object)args[0].Value));
            diContainer.ResolveRoots();
            return diContainer;
        }

        // Token: 0x0400045F RID: 1119
        private readonly Action<DiContainer, TParam1> _installMethod;
    }

    [NoReflectionBaking]
    public class SubContainerCreatorByMethod<TParam1, TParam2> : SubContainerCreatorByMethodBase
    {
        // Token: 0x06000E73 RID: 3699 RVA: 0x000278DC File Offset: 0x00025ADC
        public SubContainerCreatorByMethod(DiContainer container, SubContainerCreatorBindInfo containerBindInfo, Action<DiContainer, TParam1, TParam2> installMethod) : base(container, containerBindInfo)
        {
            this._installMethod = installMethod;
        }

        // Token: 0x06000E74 RID: 3700 RVA: 0x000278F0 File Offset: 0x00025AF0
        public override DiContainer CreateSubContainer(List<TypeValuePair> args, InjectContext context)
        {
            ModestTree.Assert.IsEqual(args.Count, 2);
            ModestTree.Assert.That(args[0].Type.DerivesFromOrEqual<TParam1>());
            ModestTree.Assert.That(args[1].Type.DerivesFromOrEqual<TParam2>());
            DiContainer diContainer = base.CreateEmptySubContainer();
            this._installMethod(diContainer, (TParam1)((object)args[0].Value), (TParam2)((object)args[1].Value));
            diContainer.ResolveRoots();
            return diContainer;
        }

        // Token: 0x04000460 RID: 1120
        private readonly Action<DiContainer, TParam1, TParam2> _installMethod;
    }

    [NoReflectionBaking]
    public class SubContainerCreatorByMethod<TParam1, TParam2, TParam3> : SubContainerCreatorByMethodBase
    {
        // Token: 0x06000E75 RID: 3701 RVA: 0x0002797C File Offset: 0x00025B7C
        public SubContainerCreatorByMethod(DiContainer container, SubContainerCreatorBindInfo containerBindInfo, Action<DiContainer, TParam1, TParam2, TParam3> installMethod) : base(container, containerBindInfo)
        {
            this._installMethod = installMethod;
        }

        // Token: 0x06000E76 RID: 3702 RVA: 0x00027990 File Offset: 0x00025B90
        public override DiContainer CreateSubContainer(List<TypeValuePair> args, InjectContext context)
        {
            ModestTree.Assert.IsEqual(args.Count, 3);
            ModestTree.Assert.That(args[0].Type.DerivesFromOrEqual<TParam1>());
            ModestTree.Assert.That(args[1].Type.DerivesFromOrEqual<TParam2>());
            ModestTree.Assert.That(args[2].Type.DerivesFromOrEqual<TParam3>());
            DiContainer diContainer = base.CreateEmptySubContainer();
            this._installMethod(diContainer, (TParam1)((object)args[0].Value), (TParam2)((object)args[1].Value), (TParam3)((object)args[2].Value));
            diContainer.ResolveRoots();
            return diContainer;
        }

        // Token: 0x04000461 RID: 1121
        private readonly Action<DiContainer, TParam1, TParam2, TParam3> _installMethod;
    }

    [NoReflectionBaking]
    public class SubContainerCreatorByMethod<TParam1, TParam2, TParam3, TParam4> : SubContainerCreatorByMethodBase
    {
        // Token: 0x06000E77 RID: 3703 RVA: 0x00027A44 File Offset: 0x00025C44
        public SubContainerCreatorByMethod(DiContainer container, SubContainerCreatorBindInfo containerBindInfo, Action<DiContainer, TParam1, TParam2, TParam3, TParam4> installMethod) : base(container, containerBindInfo)
        {
            this._installMethod = installMethod;
        }

        // Token: 0x06000E78 RID: 3704 RVA: 0x00027A58 File Offset: 0x00025C58
        public override DiContainer CreateSubContainer(List<TypeValuePair> args, InjectContext context)
        {
            ModestTree.Assert.IsEqual(args.Count, 4);
            ModestTree.Assert.That(args[0].Type.DerivesFromOrEqual<TParam1>());
            ModestTree.Assert.That(args[1].Type.DerivesFromOrEqual<TParam2>());
            ModestTree.Assert.That(args[2].Type.DerivesFromOrEqual<TParam3>());
            ModestTree.Assert.That(args[3].Type.DerivesFromOrEqual<TParam4>());
            DiContainer diContainer = base.CreateEmptySubContainer();
            this._installMethod(diContainer, (TParam1)((object)args[0].Value), (TParam2)((object)args[1].Value), (TParam3)((object)args[2].Value), (TParam4)((object)args[3].Value));
            diContainer.ResolveRoots();
            return diContainer;
        }

        // Token: 0x04000462 RID: 1122
        private readonly Action<DiContainer, TParam1, TParam2, TParam3, TParam4> _installMethod;
    }

    [NoReflectionBaking]
    public class SubContainerCreatorByMethod<TParam1, TParam2, TParam3, TParam4, TParam5> : SubContainerCreatorByMethodBase
    {
        // Token: 0x06000E79 RID: 3705 RVA: 0x00027B34 File Offset: 0x00025D34
        public SubContainerCreatorByMethod(DiContainer container, SubContainerCreatorBindInfo containerBindInfo, Action<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5> installMethod) : base(container, containerBindInfo)
        {
            this._installMethod = installMethod;
        }

        // Token: 0x06000E7A RID: 3706 RVA: 0x00027B48 File Offset: 0x00025D48
        public override DiContainer CreateSubContainer(List<TypeValuePair> args, InjectContext context)
        {
            ModestTree.Assert.IsEqual(args.Count, 5);
            ModestTree.Assert.That(args[0].Type.DerivesFromOrEqual<TParam1>());
            ModestTree.Assert.That(args[1].Type.DerivesFromOrEqual<TParam2>());
            ModestTree.Assert.That(args[2].Type.DerivesFromOrEqual<TParam3>());
            ModestTree.Assert.That(args[3].Type.DerivesFromOrEqual<TParam4>());
            ModestTree.Assert.That(args[4].Type.DerivesFromOrEqual<TParam5>());
            DiContainer diContainer = base.CreateEmptySubContainer();
            this._installMethod(diContainer, (TParam1)((object)args[0].Value), (TParam2)((object)args[1].Value), (TParam3)((object)args[2].Value), (TParam4)((object)args[3].Value), (TParam5)((object)args[4].Value));
            diContainer.ResolveRoots();
            return diContainer;
        }

        // Token: 0x04000463 RID: 1123
        private readonly Action<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5> _installMethod;
    }

    [NoReflectionBaking]
    public class SubContainerCreatorByMethod<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6> : SubContainerCreatorByMethodBase
    {
        // Token: 0x06000E7B RID: 3707 RVA: 0x00027C48 File Offset: 0x00025E48
        public SubContainerCreatorByMethod(DiContainer container, SubContainerCreatorBindInfo containerBindInfo, Action<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6> installMethod) : base(container, containerBindInfo)
        {
            this._installMethod = installMethod;
        }

        // Token: 0x06000E7C RID: 3708 RVA: 0x00027C5C File Offset: 0x00025E5C
        public override DiContainer CreateSubContainer(List<TypeValuePair> args, InjectContext context)
        {
            ModestTree.Assert.IsEqual(args.Count, 5);
            ModestTree.Assert.That(args[0].Type.DerivesFromOrEqual<TParam1>());
            ModestTree.Assert.That(args[1].Type.DerivesFromOrEqual<TParam2>());
            ModestTree.Assert.That(args[2].Type.DerivesFromOrEqual<TParam3>());
            ModestTree.Assert.That(args[3].Type.DerivesFromOrEqual<TParam4>());
            ModestTree.Assert.That(args[4].Type.DerivesFromOrEqual<TParam5>());
            ModestTree.Assert.That(args[5].Type.DerivesFromOrEqual<TParam6>());
            DiContainer diContainer = base.CreateEmptySubContainer();
            this._installMethod(diContainer, (TParam1)((object)args[0].Value), (TParam2)((object)args[1].Value), (TParam3)((object)args[2].Value), (TParam4)((object)args[3].Value), (TParam5)((object)args[4].Value), (TParam6)((object)args[5].Value));
            diContainer.ResolveRoots();
            return diContainer;
        }

        // Token: 0x04000464 RID: 1124
        private readonly Action<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6> _installMethod;
    }

    [NoReflectionBaking]
    public class SubContainerCreatorByMethod<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10> : SubContainerCreatorByMethodBase
    {
        // Token: 0x06000E7D RID: 3709 RVA: 0x00027D84 File Offset: 0x00025F84
        public SubContainerCreatorByMethod(DiContainer container, SubContainerCreatorBindInfo containerBindInfo, Action<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10> installMethod) : base(container, containerBindInfo)
        {
            this._installMethod = installMethod;
        }

        // Token: 0x06000E7E RID: 3710 RVA: 0x00027D98 File Offset: 0x00025F98
        public override DiContainer CreateSubContainer(List<TypeValuePair> args, InjectContext context)
        {
            ModestTree.Assert.IsEqual(args.Count, 10);
            ModestTree.Assert.That(args[0].Type.DerivesFromOrEqual<TParam1>());
            ModestTree.Assert.That(args[1].Type.DerivesFromOrEqual<TParam2>());
            ModestTree.Assert.That(args[2].Type.DerivesFromOrEqual<TParam3>());
            ModestTree.Assert.That(args[3].Type.DerivesFromOrEqual<TParam4>());
            ModestTree.Assert.That(args[4].Type.DerivesFromOrEqual<TParam5>());
            ModestTree.Assert.That(args[5].Type.DerivesFromOrEqual<TParam6>());
            ModestTree.Assert.That(args[6].Type.DerivesFromOrEqual<TParam7>());
            ModestTree.Assert.That(args[7].Type.DerivesFromOrEqual<TParam8>());
            ModestTree.Assert.That(args[8].Type.DerivesFromOrEqual<TParam9>());
            ModestTree.Assert.That(args[9].Type.DerivesFromOrEqual<TParam10>());
            DiContainer diContainer = base.CreateEmptySubContainer();
            this._installMethod(diContainer, (TParam1)((object)args[0].Value), (TParam2)((object)args[1].Value), (TParam3)((object)args[2].Value), (TParam4)((object)args[3].Value), (TParam5)((object)args[4].Value), (TParam6)((object)args[5].Value), (TParam7)((object)args[6].Value), (TParam8)((object)args[7].Value), (TParam9)((object)args[8].Value), (TParam10)((object)args[9].Value));
            diContainer.ResolveRoots();
            return diContainer;
        }

        // Token: 0x04000465 RID: 1125
        private readonly Action<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10> _installMethod;
    }
}