using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using ModestTree.Util;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Zenject.Internal
{
    // Token: 0x0200031A RID: 794
    public static class ZenUtilInternal
    {
        // Token: 0x0600110B RID: 4363 RVA: 0x0002FF70 File Offset: 0x0002E170
        public static bool IsNull(object obj)
        {
            return obj == null || obj.Equals(null);
        }

        // Token: 0x0600110C RID: 4364 RVA: 0x0002FF80 File Offset: 0x0002E180
        public static bool AreFunctionsEqual(Delegate left, Delegate right)
        {
            return left.Target == right.Target && left.Method() == right.Method();
        }

        // Token: 0x0600110D RID: 4365 RVA: 0x0002FFA4 File Offset: 0x0002E1A4
        public static int GetInheritanceDelta(Type derived, Type parent)
        {
            ModestTree.Assert.That(derived.DerivesFromOrEqual(parent));
            if (parent.IsInterface())
            {
                return 1;
            }
            if (derived == parent)
            {
                return 0;
            }
            int num = 1;
            Type type = derived;
            while ((type = type.BaseType()) != parent)
            {
                num++;
            }
            return num;
        }

        // Token: 0x0600110E RID: 4366 RVA: 0x0002FFF0 File Offset: 0x0002E1F0
        public static IEnumerable<SceneContext> GetAllSceneContexts()
        {
            foreach (Scene scene in ModestTree.Util.UnityUtil.AllLoadedScenes)
            {
                List<SceneContext> list = scene.GetRootGameObjects().SelectMany((GameObject root) => root.GetComponentsInChildren<SceneContext>()).ToList<SceneContext>();
                if (!list.IsEmpty<SceneContext>())
                {
                    ModestTree.Assert.That(list.Count == 1, "Found multiple scene contexts in scene '{0}'", scene.name);
                    yield return list[0];
                }
            }
            IEnumerator<Scene> enumerator = null;
            yield break;
            yield break;
        }

        // Token: 0x0600110F RID: 4367 RVA: 0x0002FFFC File Offset: 0x0002E1FC
        public static void AddStateMachineBehaviourAutoInjectersInScene(Scene scene)
        {
            foreach (GameObject gameObject in ZenUtilInternal.GetRootGameObjects(scene))
            {
                if (gameObject != null)
                {
                    ZenUtilInternal.AddStateMachineBehaviourAutoInjectersUnderGameObject(gameObject);
                }
            }
        }

        // Token: 0x06001110 RID: 4368 RVA: 0x00030054 File Offset: 0x0002E254
        public static void AddStateMachineBehaviourAutoInjectersUnderGameObject(GameObject root)
        {
            foreach (Animator animator in root.GetComponentsInChildren<Animator>(true))
            {
                if (animator.gameObject.GetComponent<ZenjectStateMachineBehaviourAutoInjecter>() == null)
                {
                    animator.gameObject.AddComponent<ZenjectStateMachineBehaviourAutoInjecter>();
                }
            }
        }

        // Token: 0x06001111 RID: 4369 RVA: 0x0003009C File Offset: 0x0002E29C
        public static void GetInjectableMonoBehavioursInScene(Scene scene, List<MonoBehaviour> monoBehaviours)
        {
            foreach (GameObject gameObject in ZenUtilInternal.GetRootGameObjects(scene))
            {
                if (gameObject != null)
                {
                    ZenUtilInternal.GetInjectableMonoBehavioursUnderGameObjectInternal(gameObject, monoBehaviours);
                }
            }
        }

        // Token: 0x06001112 RID: 4370 RVA: 0x000300F4 File Offset: 0x0002E2F4
        public static void GetInjectableMonoBehavioursUnderGameObject(GameObject gameObject, List<MonoBehaviour> injectableComponents)
        {
            ZenUtilInternal.GetInjectableMonoBehavioursUnderGameObjectInternal(gameObject, injectableComponents);
        }

        // Token: 0x06001113 RID: 4371 RVA: 0x00030100 File Offset: 0x0002E300
        private static void GetInjectableMonoBehavioursUnderGameObjectInternal(GameObject gameObject, List<MonoBehaviour> injectableComponents)
        {
            if (gameObject == null)
            {
                return;
            }
            foreach (MonoBehaviour monoBehaviour in gameObject.GetComponents<MonoBehaviour>())
            {
                if (monoBehaviour != null && monoBehaviour.GetType().DerivesFromOrEqual<GameObjectContext>())
                {
                    injectableComponents.Add(monoBehaviour);
                    return;
                }
            }
            for (int j = 0; j < gameObject.transform.childCount; j++)
            {
                Transform child = gameObject.transform.GetChild(j);
                if (child != null)
                {
                    ZenUtilInternal.GetInjectableMonoBehavioursUnderGameObjectInternal(child.gameObject, injectableComponents);
                }
            }
            MonoBehaviour[] components;
            foreach (MonoBehaviour monoBehaviour2 in components)
            {
                if (monoBehaviour2 != null && ZenUtilInternal.IsInjectableMonoBehaviourType(monoBehaviour2.GetType()))
                {
                    injectableComponents.Add(monoBehaviour2);
                }
            }
        }

        // Token: 0x06001114 RID: 4372 RVA: 0x000301C4 File Offset: 0x0002E3C4
        public static bool IsInjectableMonoBehaviourType(Type type)
        {
            return type != null && !type.DerivesFrom<MonoInstaller>() && TypeAnalyzer.HasInfo(type);
        }

        // Token: 0x06001115 RID: 4373 RVA: 0x000301E0 File Offset: 0x0002E3E0
        public static IEnumerable<GameObject> GetRootGameObjects(Scene scene)
        {
            if (scene.isLoaded)
            {
                return from x in scene.GetRootGameObjects()
                       where x.GetComponent<ProjectContext>() == null
                       select x;
            }
            return from x in Resources.FindObjectsOfTypeAll<GameObject>()
                   where x.transform.parent == null && x.GetComponent<ProjectContext>() == null && x.scene == scene
                   select x;
        }
    }
}
