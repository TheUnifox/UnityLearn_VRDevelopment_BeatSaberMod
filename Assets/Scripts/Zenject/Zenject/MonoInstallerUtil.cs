using System;
using ModestTree;
using UnityEngine;

namespace Zenject
{
    // Token: 0x02000220 RID: 544
    public static class MonoInstallerUtil
    {
        // Token: 0x06000BAA RID: 2986 RVA: 0x0001F290 File Offset: 0x0001D490
        public static string GetDefaultResourcePath<TInstaller>() where TInstaller : MonoInstallerBase
        {
            return "Installers/" + typeof(TInstaller).PrettyName();
        }

        // Token: 0x06000BAB RID: 2987 RVA: 0x0001F2AC File Offset: 0x0001D4AC
        public static TInstaller CreateInstaller<TInstaller>(string resourcePath, DiContainer container) where TInstaller : MonoInstallerBase
        {
            bool flag;
            GameObject gameObject = container.CreateAndParentPrefabResource(resourcePath, GameObjectCreationParameters.Default, null, out flag);
            if (flag && !container.IsValidating)
            {
                gameObject.SetActive(true);
            }
            TInstaller[] componentsInChildren = gameObject.GetComponentsInChildren<TInstaller>();
            ModestTree.Assert.That(componentsInChildren.Length == 1, "Could not find unique MonoInstaller with type '{0}' on prefab '{1}'", typeof(TInstaller), gameObject.name);
            return componentsInChildren[0];
        }
    }
}