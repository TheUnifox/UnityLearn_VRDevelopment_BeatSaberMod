using System;
using ModestTree;

namespace Zenject
{
    // Token: 0x020002B7 RID: 695
    public static class SubContainerCreatorUtil
    {
        // Token: 0x06000EF9 RID: 3833 RVA: 0x00029F88 File Offset: 0x00028188
        public static void ApplyBindSettings(SubContainerCreatorBindInfo subContainerBindInfo, DiContainer subContainer)
        {
            if (subContainerBindInfo.CreateKernel)
            {
                DiContainer diContainer = subContainer.ParentContainers.OnlyOrDefault<DiContainer>();
                ModestTree.Assert.IsNotNull(diContainer, "Could not find unique container when using WithKernel!");
                if (subContainerBindInfo.KernelType != null)
                {
                    diContainer.Bind(typeof(Kernel).Interfaces()).To(new Type[]
                    {
                        subContainerBindInfo.KernelType
                    }).FromSubContainerResolve().ByInstance(subContainer).AsCached();
                    subContainer.Bind(new Type[]
                    {
                        subContainerBindInfo.KernelType
                    }).AsCached();
                }
                else
                {
                    diContainer.BindInterfacesTo<Kernel>().FromSubContainerResolve().ByInstance(subContainer).AsCached();
                    subContainer.Bind<Kernel>().AsCached();
                }
                if (subContainerBindInfo.DefaultParentName != null)
                {
                    Installer<string, DefaultGameObjectParentInstaller>.Install(subContainer, subContainerBindInfo.DefaultParentName);
                }
            }
        }
    }
}