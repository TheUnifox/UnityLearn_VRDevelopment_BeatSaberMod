using System;
using UnityEngine;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x020002CB RID: 715
    public class GuiRenderer : MonoBehaviour
    {
        // Token: 0x06000F4D RID: 3917 RVA: 0x0002B2A4 File Offset: 0x000294A4
        [Inject]
        private void Construct(GuiRenderableManager renderableManager)
        {
            this._renderableManager = renderableManager;
        }

        // Token: 0x06000F4E RID: 3918 RVA: 0x0002B2B0 File Offset: 0x000294B0
        public void OnGUI()
        {
            this._renderableManager.OnGui();
        }

        // Token: 0x06000F50 RID: 3920 RVA: 0x0002B2C8 File Offset: 0x000294C8
        private static void __zenInjectMethod0(object P_0, object[] P_1)
        {
            ((GuiRenderer)P_0).Construct((GuiRenderableManager)P_1[0]);
        }

        // Token: 0x06000F51 RID: 3921 RVA: 0x0002B2E4 File Offset: 0x000294E4
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(GuiRenderer), new InjectTypeInfo.InjectConstructorInfo(null, new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[]
            {
                new InjectTypeInfo.InjectMethodInfo(new ZenInjectMethod(GuiRenderer.__zenInjectMethod0), new InjectableInfo[]
                {
                    new InjectableInfo(false, null, "renderableManager", typeof(GuiRenderableManager), null, InjectSources.Any)
                }, "Construct")
            }, new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x040004D1 RID: 1233
        private GuiRenderableManager _renderableManager;
    }
}
