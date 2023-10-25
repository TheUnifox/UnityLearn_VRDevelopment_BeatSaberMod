using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x02000282 RID: 642
    [NoReflectionBaking]
    public class SubContainerCreatorByInstaller : ISubContainerCreator
    {
        // Token: 0x06000E65 RID: 3685 RVA: 0x000276E0 File Offset: 0x000258E0
        public SubContainerCreatorByInstaller(DiContainer container, SubContainerCreatorBindInfo containerBindInfo, Type installerType, IEnumerable<TypeValuePair> extraArgs)
        {
            this._installerType = installerType;
            this._container = container;
            this._extraArgs = extraArgs.ToList<TypeValuePair>();
            this._containerBindInfo = containerBindInfo;
            ModestTree.Assert.That(installerType.DerivesFrom<InstallerBase>(), "Invalid installer type given during bind command.  Expected type '{0}' to derive from 'Installer<>'", installerType);
        }

        // Token: 0x06000E66 RID: 3686 RVA: 0x0002771C File Offset: 0x0002591C
        public SubContainerCreatorByInstaller(DiContainer container, SubContainerCreatorBindInfo containerBindInfo, Type installerType) : this(container, containerBindInfo, installerType, new List<TypeValuePair>())
        {
        }

        // Token: 0x06000E67 RID: 3687 RVA: 0x0002772C File Offset: 0x0002592C
        public DiContainer CreateSubContainer(List<TypeValuePair> args, InjectContext context)
        {
            DiContainer diContainer = this._container.CreateSubContainer();
            SubContainerCreatorUtil.ApplyBindSettings(this._containerBindInfo, diContainer);
            List<TypeValuePair> list = Zenject.Internal.ZenPools.SpawnList<TypeValuePair>();
            list.AllocFreeAddRange(this._extraArgs);
            list.AllocFreeAddRange(args);
            InstallerBase installerBase = (InstallerBase)diContainer.InstantiateExplicit(this._installerType, list);
            Zenject.Internal.ZenPools.DespawnList<TypeValuePair>(list);
            installerBase.InstallBindings();
            diContainer.ResolveRoots();
            return diContainer;
        }

        // Token: 0x04000456 RID: 1110
        private readonly Type _installerType;

        // Token: 0x04000457 RID: 1111
        private readonly DiContainer _container;

        // Token: 0x04000458 RID: 1112
        private readonly List<TypeValuePair> _extraArgs;

        // Token: 0x04000459 RID: 1113
        private readonly SubContainerCreatorBindInfo _containerBindInfo;
    }
}