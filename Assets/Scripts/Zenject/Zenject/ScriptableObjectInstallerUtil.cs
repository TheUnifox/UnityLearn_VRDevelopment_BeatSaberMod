using System;
using ModestTree;
using UnityEngine;

namespace Zenject
{
    // Token: 0x02000228 RID: 552
    public static class ScriptableObjectInstallerUtil
    {
        // Token: 0x06000BD0 RID: 3024 RVA: 0x0001F7CC File Offset: 0x0001D9CC
        public static string GetDefaultResourcePath<TInstaller>() where TInstaller : ScriptableObjectInstallerBase
        {
            return "Installers/" + typeof(TInstaller).PrettyName();
        }

        // Token: 0x06000BD1 RID: 3025 RVA: 0x0001F7E8 File Offset: 0x0001D9E8
        public static TInstaller CreateInstaller<TInstaller>(string resourcePath, DiContainer container) where TInstaller : ScriptableObjectInstallerBase
        {
            UnityEngine.Object[] array = Resources.LoadAll(resourcePath);
            ModestTree.Assert.That(array.Length == 1, "Could not find unique ScriptableObjectInstaller with type '{0}' at resource path '{1}'", typeof(TInstaller), resourcePath);
            UnityEngine.Object @object = array[0];
            ModestTree.Assert.That(@object is TInstaller, "Expected to find installer with type '{0}' at resource path '{1}'", typeof(TInstaller), resourcePath);
            return (TInstaller)((object)@object);
        }
    }
}
