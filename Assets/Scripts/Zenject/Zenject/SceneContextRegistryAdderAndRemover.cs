using System;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x020002DC RID: 732
    public class SceneContextRegistryAdderAndRemover : IInitializable, IDisposable
    {
        // Token: 0x06000FAE RID: 4014 RVA: 0x0002C4E0 File Offset: 0x0002A6E0
        public SceneContextRegistryAdderAndRemover(SceneContext sceneContext, SceneContextRegistry registry)
        {
            this._registry = registry;
            this._sceneContext = sceneContext;
        }

        // Token: 0x06000FAF RID: 4015 RVA: 0x0002C4F8 File Offset: 0x0002A6F8
        public void Initialize()
        {
            this._registry.Add(this._sceneContext);
        }

        // Token: 0x06000FB0 RID: 4016 RVA: 0x0002C50C File Offset: 0x0002A70C
        public void Dispose()
        {
            this._registry.Remove(this._sceneContext);
        }

        // Token: 0x06000FB1 RID: 4017 RVA: 0x0002C520 File Offset: 0x0002A720
        private static object __zenCreate(object[] P_0)
        {
            return new SceneContextRegistryAdderAndRemover((SceneContext)P_0[0], (SceneContextRegistry)P_0[1]);
        }

        // Token: 0x06000FB2 RID: 4018 RVA: 0x0002C550 File Offset: 0x0002A750
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(SceneContextRegistryAdderAndRemover), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(SceneContextRegistryAdderAndRemover.__zenCreate), new InjectableInfo[]
            {
                new InjectableInfo(false, null, "sceneContext", typeof(SceneContext), null, InjectSources.Any),
                new InjectableInfo(false, null, "registry", typeof(SceneContextRegistry), null, InjectSources.Any)
            }), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x040004F2 RID: 1266
        private readonly SceneContextRegistry _registry;

        // Token: 0x040004F3 RID: 1267
        private readonly SceneContext _sceneContext;
    }
}
