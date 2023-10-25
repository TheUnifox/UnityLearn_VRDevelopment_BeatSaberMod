using System;
using System.Collections.Generic;
using ModestTree;
using UnityEngine.SceneManagement;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x020002DB RID: 731
    public class SceneContextRegistry
    {
        // Token: 0x17000158 RID: 344
        // (get) Token: 0x06000FA2 RID: 4002 RVA: 0x0002C2E4 File Offset: 0x0002A4E4
        public IEnumerable<SceneContext> SceneContexts
        {
            get
            {
                return this._map.Values;
            }
        }

        // Token: 0x06000FA3 RID: 4003 RVA: 0x0002C2F4 File Offset: 0x0002A4F4
        public void Add(SceneContext context)
        {
            ModestTree.Assert.That(!this._map.ContainsKey(context.gameObject.scene));
            this._map.Add(context.gameObject.scene, context);
        }

        // Token: 0x06000FA4 RID: 4004 RVA: 0x0002C32C File Offset: 0x0002A52C
        public SceneContext GetSceneContextForScene(string name)
        {
            Scene sceneByName = SceneManager.GetSceneByName(name);
            ModestTree.Assert.That(sceneByName.IsValid(), "Could not find scene with name '{0}'", name);
            return this.GetSceneContextForScene(sceneByName);
        }

        // Token: 0x06000FA5 RID: 4005 RVA: 0x0002C35C File Offset: 0x0002A55C
        public SceneContext GetSceneContextForScene(Scene scene)
        {
            return this._map[scene];
        }

        // Token: 0x06000FA6 RID: 4006 RVA: 0x0002C36C File Offset: 0x0002A56C
        public SceneContext TryGetSceneContextForScene(string name)
        {
            Scene sceneByName = SceneManager.GetSceneByName(name);
            ModestTree.Assert.That(sceneByName.IsValid(), "Could not find scene with name '{0}'", name);
            return this.TryGetSceneContextForScene(sceneByName);
        }

        // Token: 0x06000FA7 RID: 4007 RVA: 0x0002C39C File Offset: 0x0002A59C
        public SceneContext TryGetSceneContextForScene(Scene scene)
        {
            SceneContext result;
            if (this._map.TryGetValue(scene, out result))
            {
                return result;
            }
            return null;
        }

        // Token: 0x06000FA8 RID: 4008 RVA: 0x0002C3BC File Offset: 0x0002A5BC
        public DiContainer GetContainerForScene(Scene scene)
        {
            DiContainer diContainer = this.TryGetContainerForScene(scene);
            if (diContainer != null)
            {
                return diContainer;
            }
            throw ModestTree.Assert.CreateException("Unable to find DiContainer for scene '{0}'", new object[]
            {
                scene.name
            });
        }

        // Token: 0x06000FA9 RID: 4009 RVA: 0x0002C3F0 File Offset: 0x0002A5F0
        public DiContainer TryGetContainerForScene(Scene scene)
        {
            if (scene == ProjectContext.Instance.gameObject.scene)
            {
                return ProjectContext.Instance.Container;
            }
            SceneContext sceneContext = this.TryGetSceneContextForScene(scene);
            if (sceneContext != null)
            {
                return sceneContext.Container;
            }
            return null;
        }

        // Token: 0x06000FAA RID: 4010 RVA: 0x0002C438 File Offset: 0x0002A638
        public void Remove(SceneContext context)
        {
            if (!this._map.Remove(context.gameObject.scene))
            {
                ModestTree.Log.Warn("Failed to remove SceneContext from SceneContextRegistry", Array.Empty<object>());
            }
        }

        // Token: 0x06000FAC RID: 4012 RVA: 0x0002C478 File Offset: 0x0002A678
        private static object __zenCreate(object[] P_0)
        {
            return new SceneContextRegistry();
        }

        // Token: 0x06000FAD RID: 4013 RVA: 0x0002C490 File Offset: 0x0002A690
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(SceneContextRegistry), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(SceneContextRegistry.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x040004F1 RID: 1265
        private readonly Dictionary<Scene, SceneContext> _map = new Dictionary<Scene, SceneContext>();
    }
}
