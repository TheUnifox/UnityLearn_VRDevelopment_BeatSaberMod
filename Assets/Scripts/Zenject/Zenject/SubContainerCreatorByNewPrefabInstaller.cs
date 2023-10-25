using System;
using System.Collections.Generic;
using ModestTree;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x020002A2 RID: 674
    [NoReflectionBaking]
    public class SubContainerCreatorByNewPrefabInstaller : SubContainerCreatorByNewPrefabDynamicContext
    {
        // Token: 0x06000EB7 RID: 3767 RVA: 0x00028E78 File Offset: 0x00027078
        public SubContainerCreatorByNewPrefabInstaller(DiContainer container, IPrefabProvider prefabProvider, GameObjectCreationParameters gameObjectBindInfo, Type installerType, List<TypeValuePair> extraArgs) : base(container, prefabProvider, gameObjectBindInfo)
        {
            this._installerType = installerType;
            this._extraArgs = extraArgs;
            ModestTree.Assert.That(installerType.DerivesFrom<InstallerBase>(), "Invalid installer type given during bind command.  Expected type '{0}' to derive from 'Installer<>'", installerType);
        }

        // Token: 0x06000EB8 RID: 3768 RVA: 0x00028EA8 File Offset: 0x000270A8
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

        // Token: 0x04000486 RID: 1158
        private readonly Type _installerType;

        // Token: 0x04000487 RID: 1159
        private readonly List<TypeValuePair> _extraArgs;
    }
}
