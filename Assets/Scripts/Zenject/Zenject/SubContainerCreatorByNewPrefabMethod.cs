using System;
using System.Collections.Generic;
using ModestTree;

namespace Zenject
{
    // Token: 0x020002A4 RID: 676
    [NoReflectionBaking]
    public class SubContainerCreatorByNewPrefabMethod : SubContainerCreatorByNewPrefabDynamicContext
    {
        // Token: 0x06000EBD RID: 3773 RVA: 0x00028FA4 File Offset: 0x000271A4
        public SubContainerCreatorByNewPrefabMethod(DiContainer container, IPrefabProvider prefabProvider, GameObjectCreationParameters gameObjectBindInfo, Action<DiContainer> installerMethod) : base(container, prefabProvider, gameObjectBindInfo)
        {
            this._installerMethod = installerMethod;
        }

        // Token: 0x06000EBE RID: 3774 RVA: 0x00028FB8 File Offset: 0x000271B8
        protected override void AddInstallers(List<TypeValuePair> args, GameObjectContext context)
        {
            ModestTree.Assert.That(args.IsEmpty<TypeValuePair>());
            context.AddNormalInstaller(new ActionInstaller(this._installerMethod));
        }

        // Token: 0x0400048A RID: 1162
        private readonly Action<DiContainer> _installerMethod;
    }

    [NoReflectionBaking]
    public class SubContainerCreatorByNewPrefabMethod<TParam1> : SubContainerCreatorByNewPrefabDynamicContext
    {
        // Token: 0x06000EBF RID: 3775 RVA: 0x00028FD8 File Offset: 0x000271D8
        public SubContainerCreatorByNewPrefabMethod(DiContainer container, IPrefabProvider prefabProvider, GameObjectCreationParameters gameObjectBindInfo, Action<DiContainer, TParam1> installerMethod) : base(container, prefabProvider, gameObjectBindInfo)
        {
            this._installerMethod = installerMethod;
        }

        // Token: 0x06000EC0 RID: 3776 RVA: 0x00028FEC File Offset: 0x000271EC
        protected override void AddInstallers(List<TypeValuePair> args, GameObjectContext context)
        {
            ModestTree.Assert.IsEqual(args.Count, 1);
            ModestTree.Assert.That(args[0].Type.DerivesFromOrEqual<TParam1>());
            context.AddNormalInstaller(new ActionInstaller(delegate (DiContainer subContainer)
            {
                this._installerMethod(subContainer, (TParam1)((object)args[0].Value));
            }));
        }

        // Token: 0x0400048B RID: 1163
        private readonly Action<DiContainer, TParam1> _installerMethod;
    }

    [NoReflectionBaking]
    public class SubContainerCreatorByNewPrefabMethod<TParam1, TParam2> : SubContainerCreatorByNewPrefabDynamicContext
    {
        // Token: 0x06000EC5 RID: 3781 RVA: 0x000290F8 File Offset: 0x000272F8
        public SubContainerCreatorByNewPrefabMethod(DiContainer container, IPrefabProvider prefabProvider, GameObjectCreationParameters gameObjectBindInfo, Action<DiContainer, TParam1, TParam2> installerMethod) : base(container, prefabProvider, gameObjectBindInfo)
        {
            this._installerMethod = installerMethod;
        }

        // Token: 0x06000EC6 RID: 3782 RVA: 0x0002910C File Offset: 0x0002730C
        protected override void AddInstallers(List<TypeValuePair> args, GameObjectContext context)
        {
            ModestTree.Assert.IsEqual(args.Count, 2);
            ModestTree.Assert.That(args[0].Type.DerivesFromOrEqual<TParam1>());
            ModestTree.Assert.That(args[1].Type.DerivesFromOrEqual<TParam2>());
            context.AddNormalInstaller(new ActionInstaller(delegate (DiContainer subContainer)
            {
                this._installerMethod(subContainer, (TParam1)((object)args[0].Value), (TParam2)((object)args[1].Value));
            }));
        }

        // Token: 0x0400048E RID: 1166
        private readonly Action<DiContainer, TParam1, TParam2> _installerMethod;
    }

    [NoReflectionBaking]
    public class SubContainerCreatorByNewPrefabMethod<TParam1, TParam2, TParam3> : SubContainerCreatorByNewPrefabDynamicContext
    {
        // Token: 0x06000ECB RID: 3787 RVA: 0x00029248 File Offset: 0x00027448
        public SubContainerCreatorByNewPrefabMethod(DiContainer container, IPrefabProvider prefabProvider, GameObjectCreationParameters gameObjectBindInfo, Action<DiContainer, TParam1, TParam2, TParam3> installerMethod) : base(container, prefabProvider, gameObjectBindInfo)
        {
            this._installerMethod = installerMethod;
        }

        // Token: 0x06000ECC RID: 3788 RVA: 0x0002925C File Offset: 0x0002745C
        protected override void AddInstallers(List<TypeValuePair> args, GameObjectContext context)
        {
            ModestTree.Assert.IsEqual(args.Count, 3);
            ModestTree.Assert.That(args[0].Type.DerivesFromOrEqual<TParam1>());
            ModestTree.Assert.That(args[1].Type.DerivesFromOrEqual<TParam2>());
            ModestTree.Assert.That(args[2].Type.DerivesFromOrEqual<TParam3>());
            context.AddNormalInstaller(new ActionInstaller(delegate (DiContainer subContainer)
            {
                this._installerMethod(subContainer, (TParam1)((object)args[0].Value), (TParam2)((object)args[1].Value), (TParam3)((object)args[2].Value));
            }));
        }

        // Token: 0x04000491 RID: 1169
        private readonly Action<DiContainer, TParam1, TParam2, TParam3> _installerMethod;
    }

    [NoReflectionBaking]
    public class SubContainerCreatorByNewPrefabMethod<TParam1, TParam2, TParam3, TParam4> : SubContainerCreatorByNewPrefabDynamicContext
    {
        // Token: 0x06000ED1 RID: 3793 RVA: 0x000293D0 File Offset: 0x000275D0
        public SubContainerCreatorByNewPrefabMethod(DiContainer container, IPrefabProvider prefabProvider, GameObjectCreationParameters gameObjectBindInfo, Action<DiContainer, TParam1, TParam2, TParam3, TParam4> installerMethod) : base(container, prefabProvider, gameObjectBindInfo)
        {
            this._installerMethod = installerMethod;
        }

        // Token: 0x06000ED2 RID: 3794 RVA: 0x000293E4 File Offset: 0x000275E4
        protected override void AddInstallers(List<TypeValuePair> args, GameObjectContext context)
        {
            ModestTree.Assert.IsEqual(args.Count, 4);
            ModestTree.Assert.That(args[0].Type.DerivesFromOrEqual<TParam1>());
            ModestTree.Assert.That(args[1].Type.DerivesFromOrEqual<TParam2>());
            ModestTree.Assert.That(args[2].Type.DerivesFromOrEqual<TParam3>());
            ModestTree.Assert.That(args[3].Type.DerivesFromOrEqual<TParam4>());
            context.AddNormalInstaller(new ActionInstaller(delegate (DiContainer subContainer)
            {
                this._installerMethod(subContainer, (TParam1)((object)args[0].Value), (TParam2)((object)args[1].Value), (TParam3)((object)args[2].Value), (TParam4)((object)args[3].Value));
            }));
        }

        // Token: 0x04000494 RID: 1172
        private readonly Action<DiContainer, TParam1, TParam2, TParam3, TParam4> _installerMethod;
    }

    [NoReflectionBaking]
    public class SubContainerCreatorByNewPrefabMethod<TParam1, TParam2, TParam3, TParam4, TParam5> : SubContainerCreatorByNewPrefabDynamicContext
    {
        // Token: 0x06000ED7 RID: 3799 RVA: 0x0002958C File Offset: 0x0002778C
        public SubContainerCreatorByNewPrefabMethod(DiContainer container, IPrefabProvider prefabProvider, GameObjectCreationParameters gameObjectBindInfo, Action<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5> installerMethod) : base(container, prefabProvider, gameObjectBindInfo)
        {
            this._installerMethod = installerMethod;
        }

        // Token: 0x06000ED8 RID: 3800 RVA: 0x000295A0 File Offset: 0x000277A0
        protected override void AddInstallers(List<TypeValuePair> args, GameObjectContext context)
        {
            ModestTree.Assert.IsEqual(args.Count, 5);
            ModestTree.Assert.That(args[0].Type.DerivesFromOrEqual<TParam1>());
            ModestTree.Assert.That(args[1].Type.DerivesFromOrEqual<TParam2>());
            ModestTree.Assert.That(args[2].Type.DerivesFromOrEqual<TParam3>());
            ModestTree.Assert.That(args[3].Type.DerivesFromOrEqual<TParam4>());
            ModestTree.Assert.That(args[4].Type.DerivesFromOrEqual<TParam5>());
            context.AddNormalInstaller(new ActionInstaller(delegate (DiContainer subContainer)
            {
                this._installerMethod(subContainer, (TParam1)((object)args[0].Value), (TParam2)((object)args[1].Value), (TParam3)((object)args[2].Value), (TParam4)((object)args[3].Value), (TParam5)((object)args[4].Value));
            }));
        }

        // Token: 0x04000497 RID: 1175
        private readonly Action<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5> _installerMethod;
    }

    [NoReflectionBaking]
    public class SubContainerCreatorByNewPrefabMethod<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6> : SubContainerCreatorByNewPrefabDynamicContext
    {
        // Token: 0x06000EDD RID: 3805 RVA: 0x00029778 File Offset: 0x00027978
        public SubContainerCreatorByNewPrefabMethod(DiContainer container, IPrefabProvider prefabProvider, GameObjectCreationParameters gameObjectBindInfo, Action<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6> installerMethod) : base(container, prefabProvider, gameObjectBindInfo)
        {
            this._installerMethod = installerMethod;
        }

        // Token: 0x06000EDE RID: 3806 RVA: 0x0002978C File Offset: 0x0002798C
        protected override void AddInstallers(List<TypeValuePair> args, GameObjectContext context)
        {
            ModestTree.Assert.IsEqual(args.Count, 5);
            ModestTree.Assert.That(args[0].Type.DerivesFromOrEqual<TParam1>());
            ModestTree.Assert.That(args[1].Type.DerivesFromOrEqual<TParam2>());
            ModestTree.Assert.That(args[2].Type.DerivesFromOrEqual<TParam3>());
            ModestTree.Assert.That(args[3].Type.DerivesFromOrEqual<TParam4>());
            ModestTree.Assert.That(args[4].Type.DerivesFromOrEqual<TParam5>());
            ModestTree.Assert.That(args[5].Type.DerivesFromOrEqual<TParam6>());
            context.AddNormalInstaller(new ActionInstaller(delegate (DiContainer subContainer)
            {
                this._installerMethod(subContainer, (TParam1)((object)args[0].Value), (TParam2)((object)args[1].Value), (TParam3)((object)args[2].Value), (TParam4)((object)args[3].Value), (TParam5)((object)args[4].Value), (TParam6)((object)args[5].Value));
            }));
        }

        // Token: 0x0400049A RID: 1178
        private readonly Action<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6> _installerMethod;
    }

    [NoReflectionBaking] //Right from 6 to 10? Idk, I'm just decornstructing everything to code. Compiler must have gotten rid of the rest from being unused
    public class SubContainerCreatorByNewPrefabMethod<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10> : SubContainerCreatorByNewPrefabDynamicContext
    {
        // Token: 0x06000EE3 RID: 3811 RVA: 0x00029998 File Offset: 0x00027B98
        public SubContainerCreatorByNewPrefabMethod(DiContainer container, IPrefabProvider prefabProvider, GameObjectCreationParameters gameObjectBindInfo, Action<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10> installerMethod) : base(container, prefabProvider, gameObjectBindInfo)
        {
            this._installerMethod = installerMethod;
        }

        // Token: 0x06000EE4 RID: 3812 RVA: 0x000299AC File Offset: 0x00027BAC
        protected override void AddInstallers(List<TypeValuePair> args, GameObjectContext context)
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
            context.AddNormalInstaller(new ActionInstaller(delegate (DiContainer subContainer)
            {
                this._installerMethod(subContainer, (TParam1)((object)args[0].Value), (TParam2)((object)args[1].Value), (TParam3)((object)args[2].Value), (TParam4)((object)args[3].Value), (TParam5)((object)args[4].Value), (TParam6)((object)args[5].Value), (TParam7)((object)args[6].Value), (TParam8)((object)args[7].Value), (TParam9)((object)args[8].Value), (TParam10)((object)args[9].Value));
            }));
        }

        // Token: 0x0400049D RID: 1181
        private readonly Action<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10> _installerMethod;
    }
}

