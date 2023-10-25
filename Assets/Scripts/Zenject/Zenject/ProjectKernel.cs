using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x020002D3 RID: 723
    public class ProjectKernel : MonoKernel
    {
        // Token: 0x06000F7F RID: 3967 RVA: 0x0002BBE8 File Offset: 0x00029DE8
        public void OnApplicationQuit()
        {
            if (this._settings.EnsureDeterministicDestructionOrderOnApplicationQuit)
            {
                this.DestroyEverythingInOrder();
            }
        }

        // Token: 0x06000F80 RID: 3968 RVA: 0x0002BC00 File Offset: 0x00029E00
        public void DestroyEverythingInOrder()
        {
            this.ForceUnloadAllScenes(true);
            ModestTree.Assert.That(!base.IsDestroyed);
            UnityEngine.Object.DestroyImmediate(base.gameObject);
            ModestTree.Assert.That(base.IsDestroyed);
        }

        // Token: 0x06000F81 RID: 3969 RVA: 0x0002BC30 File Offset: 0x00029E30
        public void ForceUnloadAllScenes(bool immediate = false)
        {
            // OnApplicationQuit should always be called before OnDestroy
            // (Unless it is destroyed manually)
            Assert.That(!IsDestroyed);

            var sceneOrder = new List<Scene>();

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                sceneOrder.Add(SceneManager.GetSceneAt(i));
            }

            // Destroy the scene contexts from bottom to top
            // Since this is the reverse order that they were loaded in
            foreach (var sceneContext in _contextRegistry.SceneContexts.OrderByDescending(x => sceneOrder.IndexOf(x.gameObject.scene)).ToList())
            {
                if (immediate)
                {
                    DestroyImmediate(sceneContext.gameObject);
                }
                else
                {
                    Destroy(sceneContext.gameObject);
                }
            }
        }

        // Token: 0x06000F83 RID: 3971 RVA: 0x0002BD10 File Offset: 0x00029F10
        private static void __zenFieldSetter0(object P_0, object P_1)
        {
            ((ProjectKernel)P_0)._settings = (ZenjectSettings)P_1;
        }

        // Token: 0x06000F84 RID: 3972 RVA: 0x0002BD30 File Offset: 0x00029F30
        private static void __zenFieldSetter1(object P_0, object P_1)
        {
            ((ProjectKernel)P_0)._contextRegistry = (SceneContextRegistry)P_1;
        }

        // Token: 0x06000F85 RID: 3973 RVA: 0x0002BD50 File Offset: 0x00029F50
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(ProjectKernel), new InjectTypeInfo.InjectConstructorInfo(null, new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[]
            {
                new InjectTypeInfo.InjectMemberInfo(new ZenMemberSetterMethod(ProjectKernel.__zenFieldSetter0), new InjectableInfo(false, null, "_settings", typeof(ZenjectSettings), null, InjectSources.Any)),
                new InjectTypeInfo.InjectMemberInfo(new ZenMemberSetterMethod(ProjectKernel.__zenFieldSetter1), new InjectableInfo(false, null, "_contextRegistry", typeof(SceneContextRegistry), null, InjectSources.Any))
            });
        }

        // Token: 0x040004E2 RID: 1250
        [Inject]
        private ZenjectSettings _settings;

        // Token: 0x040004E3 RID: 1251
        [Inject]
        private SceneContextRegistry _contextRegistry;
    }
}
