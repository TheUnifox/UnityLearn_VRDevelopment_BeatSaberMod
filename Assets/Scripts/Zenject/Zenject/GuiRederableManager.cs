using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using ModestTree.Util;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x020002C7 RID: 711
    public class GuiRenderableManager
    {
        // Token: 0x06000F3C RID: 3900 RVA: 0x0002AE64 File Offset: 0x00029064
        public GuiRenderableManager([Inject(Optional = true, Source = InjectSources.Local)] List<IGuiRenderable> renderables, [Inject(Optional = true, Source = InjectSources.Local)] List<ModestTree.Util.ValuePair<Type, int>> priorities)
        {
            this._renderables = new List<GuiRenderableManager.RenderableInfo>();
            using (List<IGuiRenderable>.Enumerator enumerator = renderables.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    IGuiRenderable renderable = enumerator.Current;
                    List<int> list = (from x in priorities
                                      where renderable.GetType().DerivesFromOrEqual(x.First)
                                      select x.Second).ToList<int>();
                    int priority = list.IsEmpty<int>() ? 0 : list.Distinct<int>().Single<int>();
                    this._renderables.Add(new GuiRenderableManager.RenderableInfo(renderable, priority));
                }
            }
            this._renderables = (from x in this._renderables
                                 orderby x.Priority
                                 select x).ToList<GuiRenderableManager.RenderableInfo>();
        }

        // Token: 0x06000F3D RID: 3901 RVA: 0x0002AF6C File Offset: 0x0002916C
        public void OnGui()
        {
            foreach (GuiRenderableManager.RenderableInfo renderableInfo in this._renderables)
            {
                try
                {
                    renderableInfo.Renderable.GuiRender();
                }
                catch (Exception innerException)
                {
                    throw ModestTree.Assert.CreateException(innerException, "Error occurred while calling {0}.GuiRender", new object[]
                    {
                        renderableInfo.Renderable.GetType()
                    });
                }
            }
        }

        // Token: 0x06000F3E RID: 3902 RVA: 0x0002AFF0 File Offset: 0x000291F0
        private static object __zenCreate(object[] P_0)
        {
            return new GuiRenderableManager((List<IGuiRenderable>)P_0[0], (List<ModestTree.Util.ValuePair<Type, int>>)P_0[1]);
        }

        // Token: 0x06000F3F RID: 3903 RVA: 0x0002B020 File Offset: 0x00029220
        [Zenject.Internal.Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(GuiRenderableManager), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(GuiRenderableManager.__zenCreate), new InjectableInfo[]
            {
                new InjectableInfo(true, null, "renderables", typeof(List<IGuiRenderable>), null, InjectSources.Local),
                new InjectableInfo(true, null, "priorities", typeof(List<ModestTree.Util.ValuePair<Type, int>>), null, InjectSources.Local)
            }), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x040004CA RID: 1226
        private List<GuiRenderableManager.RenderableInfo> _renderables;

        // Token: 0x020002C8 RID: 712
        private class RenderableInfo
        {
            // Token: 0x06000F40 RID: 3904 RVA: 0x0002B0B4 File Offset: 0x000292B4
            public RenderableInfo(IGuiRenderable renderable, int priority)
            {
                this.Renderable = renderable;
                this.Priority = priority;
            }

            // Token: 0x06000F41 RID: 3905 RVA: 0x0002B0CC File Offset: 0x000292CC
            private static object __zenCreate(object[] P_0)
            {
                return new GuiRenderableManager.RenderableInfo((IGuiRenderable)P_0[0], (int)P_0[1]);
            }

            // Token: 0x06000F42 RID: 3906 RVA: 0x0002B0FC File Offset: 0x000292FC
            [Zenject.Internal.Preserve]
            private static InjectTypeInfo __zenCreateInjectTypeInfo()
            {
                return new InjectTypeInfo(typeof(GuiRenderableManager.RenderableInfo), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(GuiRenderableManager.RenderableInfo.__zenCreate), new InjectableInfo[]
                {
                    new InjectableInfo(false, null, "renderable", typeof(IGuiRenderable), null, InjectSources.Any),
                    new InjectableInfo(false, null, "priority", typeof(int), null, InjectSources.Any)
                }), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
            }

            // Token: 0x040004CB RID: 1227
            public IGuiRenderable Renderable;

            // Token: 0x040004CC RID: 1228
            public int Priority;
        }
    }
}
