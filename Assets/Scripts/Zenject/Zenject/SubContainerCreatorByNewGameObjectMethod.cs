using System;
using System.Collections.Generic;
using ModestTree;

namespace Zenject
{
    // Token: 0x02000291 RID: 657
    [NoReflectionBaking]
    public class SubContainerCreatorByNewGameObjectMethod : SubContainerCreatorByNewGameObjectDynamicContext
    {
        // Token: 0x06000E87 RID: 3719 RVA: 0x000280B8 File Offset: 0x000262B8
        public SubContainerCreatorByNewGameObjectMethod(DiContainer container, GameObjectCreationParameters gameObjectBindInfo, Action<DiContainer> installerMethod) : base(container, gameObjectBindInfo)
        {
            this._installerMethod = installerMethod;
        }

        // Token: 0x06000E88 RID: 3720 RVA: 0x000280CC File Offset: 0x000262CC
        protected override void AddInstallers(List<TypeValuePair> args, GameObjectContext context)
        {
            ModestTree.Assert.That(args.IsEmpty<TypeValuePair>());
            context.AddNormalInstaller(new ActionInstaller(this._installerMethod));
        }

        // Token: 0x0400046B RID: 1131
        private readonly Action<DiContainer> _installerMethod;
    }

    [NoReflectionBaking]
    public class SubContainerCreatorByNewGameObjectMethod<TParam1> : SubContainerCreatorByNewGameObjectDynamicContext
    {
        // Token: 0x06000E89 RID: 3721 RVA: 0x000280EC File Offset: 0x000262EC
        public SubContainerCreatorByNewGameObjectMethod(DiContainer container, GameObjectCreationParameters gameObjectBindInfo, Action<DiContainer, TParam1> installerMethod) : base(container, gameObjectBindInfo)
        {
            this._installerMethod = installerMethod;
        }

        // Token: 0x06000E8A RID: 3722 RVA: 0x00028100 File Offset: 0x00026300
        protected override void AddInstallers(List<TypeValuePair> args, GameObjectContext context)
        {
            ModestTree.Assert.IsEqual(args.Count, 1);
            ModestTree.Assert.That(args[0].Type.DerivesFromOrEqual<TParam1>());
            context.AddNormalInstaller(new ActionInstaller(delegate (DiContainer subContainer)
            {
                this._installerMethod(subContainer, (TParam1)((object)args[0].Value));
            }));
        }

        // Token: 0x0400046C RID: 1132
        private readonly Action<DiContainer, TParam1> _installerMethod;
    }

    [NoReflectionBaking]
    public class SubContainerCreatorByNewGameObjectMethod<TParam1, TParam2> : SubContainerCreatorByNewGameObjectDynamicContext
    {
        // Token: 0x06000E8F RID: 3727 RVA: 0x0002820C File Offset: 0x0002640C
        public SubContainerCreatorByNewGameObjectMethod(DiContainer container, GameObjectCreationParameters gameObjectBindInfo, Action<DiContainer, TParam1, TParam2> installerMethod) : base(container, gameObjectBindInfo)
        {
            this._installerMethod = installerMethod;
        }

        // Token: 0x06000E90 RID: 3728 RVA: 0x00028220 File Offset: 0x00026420
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

        // Token: 0x0400046F RID: 1135
        private readonly Action<DiContainer, TParam1, TParam2> _installerMethod;
    }

    [NoReflectionBaking]
    public class SubContainerCreatorByNewGameObjectMethod<TParam1, TParam2, TParam3> : SubContainerCreatorByNewGameObjectDynamicContext
    {
        // Token: 0x06000E95 RID: 3733 RVA: 0x0002835C File Offset: 0x0002655C
        public SubContainerCreatorByNewGameObjectMethod(DiContainer container, GameObjectCreationParameters gameObjectBindInfo, Action<DiContainer, TParam1, TParam2, TParam3> installerMethod) : base(container, gameObjectBindInfo)
        {
            this._installerMethod = installerMethod;
        }

        // Token: 0x06000E96 RID: 3734 RVA: 0x00028370 File Offset: 0x00026570
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

        // Token: 0x04000472 RID: 1138
        private readonly Action<DiContainer, TParam1, TParam2, TParam3> _installerMethod;
    }

    [NoReflectionBaking]
    public class SubContainerCreatorByNewGameObjectMethod<TParam1, TParam2, TParam3, TParam4> : SubContainerCreatorByNewGameObjectDynamicContext
    {
        // Token: 0x06000E9B RID: 3739 RVA: 0x000284E4 File Offset: 0x000266E4
        public SubContainerCreatorByNewGameObjectMethod(DiContainer container, GameObjectCreationParameters gameObjectBindInfo, Action<DiContainer, TParam1, TParam2, TParam3, TParam4> installerMethod) : base(container, gameObjectBindInfo)
        {
            this._installerMethod = installerMethod;
        }

        // Token: 0x06000E9C RID: 3740 RVA: 0x000284F8 File Offset: 0x000266F8
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

        // Token: 0x04000475 RID: 1141
        private readonly Action<DiContainer, TParam1, TParam2, TParam3, TParam4> _installerMethod;
    }

    [NoReflectionBaking]
    public class SubContainerCreatorByNewGameObjectMethod<TParam1, TParam2, TParam3, TParam4, TParam5> : SubContainerCreatorByNewGameObjectDynamicContext
    {
        // Token: 0x06000EA1 RID: 3745 RVA: 0x000286A0 File Offset: 0x000268A0
        public SubContainerCreatorByNewGameObjectMethod(DiContainer container, GameObjectCreationParameters gameObjectBindInfo, Action<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5> installerMethod) : base(container, gameObjectBindInfo)
        {
            this._installerMethod = installerMethod;
        }

        // Token: 0x06000EA2 RID: 3746 RVA: 0x000286B4 File Offset: 0x000268B4
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

        // Token: 0x04000478 RID: 1144
        private readonly Action<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5> _installerMethod;
    }

    [NoReflectionBaking]
    public class SubContainerCreatorByNewGameObjectMethod<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6> : SubContainerCreatorByNewGameObjectDynamicContext
    {
        // Token: 0x06000EA7 RID: 3751 RVA: 0x0002888C File Offset: 0x00026A8C
        public SubContainerCreatorByNewGameObjectMethod(DiContainer container, GameObjectCreationParameters gameObjectBindInfo, Action<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6> installerMethod) : base(container, gameObjectBindInfo)
        {
            this._installerMethod = installerMethod;
        }

        // Token: 0x06000EA8 RID: 3752 RVA: 0x000288A0 File Offset: 0x00026AA0
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

        // Token: 0x0400047B RID: 1147
        private readonly Action<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6> _installerMethod;
    }

    [NoReflectionBaking]
    public class SubContainerCreatorByNewGameObjectMethod<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10> : SubContainerCreatorByNewGameObjectDynamicContext
    {
        // Token: 0x06000EAD RID: 3757 RVA: 0x00028AAC File Offset: 0x00026CAC
        public SubContainerCreatorByNewGameObjectMethod(DiContainer container, GameObjectCreationParameters gameObjectBindInfo, Action<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10> installerMethod) : base(container, gameObjectBindInfo)
        {
            this._installerMethod = installerMethod;
        }

        // Token: 0x06000EAE RID: 3758 RVA: 0x00028AC0 File Offset: 0x00026CC0
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

        // Token: 0x0400047E RID: 1150
        private readonly Action<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10> _installerMethod;
    }
}
