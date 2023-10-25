using System;
using System.Collections.Generic;
using ModestTree;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x0200028F RID: 655
    [NoReflectionBaking]
    public class SubContainerCreatorByNewGameObjectInstaller : SubContainerCreatorByNewGameObjectDynamicContext
    {
        // Token: 0x06000E81 RID: 3713 RVA: 0x00027F90 File Offset: 0x00026190
        public SubContainerCreatorByNewGameObjectInstaller(DiContainer container, GameObjectCreationParameters gameObjectBindInfo, Type installerType, List<TypeValuePair> extraArgs) : base(container, gameObjectBindInfo)
        {
            this._installerType = installerType;
            this._extraArgs = extraArgs;
            ModestTree.Assert.That(installerType.DerivesFrom<InstallerBase>(), "Invalid installer type given during bind command.  Expected type '{0}' to derive from 'Installer<>'", installerType);
        }

        // Token: 0x06000E82 RID: 3714 RVA: 0x00027FBC File Offset: 0x000261BC
        protected override void AddInstallers(List<TypeValuePair> args, GameObjectContext context)
        {
            context.AddNormalInstaller(new ActionInstaller(delegate (DiContainer subContainer)
            {
                List<TypeValuePair> list = Zenject.Internal.ZenPools.SpawnList<TypeValuePair>();
                list.AllocFreeAddRange(this._extraArgs);
                list.AllocFreeAddRange(args);
                InstallerBase installerBase = (InstallerBase)subContainer.InstantiateExplicit(this._installerType, list);
                Zenject.Internal.ZenPools.DespawnList<TypeValuePair>(list);
                installerBase.InstallBindings();
            }));
        }

        // Token: 0x04000467 RID: 1127
        private readonly Type _installerType;

        // Token: 0x04000468 RID: 1128
        private readonly List<TypeValuePair> _extraArgs;
    }
}
