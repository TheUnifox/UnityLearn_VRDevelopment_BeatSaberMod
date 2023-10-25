using System;

namespace JetBrains.Annotations
{
    // Token: 0x02000041 RID: 65
    [AttributeUsage(AttributeTargets.Parameter)]
    internal sealed class AspMvcAreaAttribute : Attribute
    {
        // Token: 0x0600009A RID: 154 RVA: 0x00002579 File Offset: 0x00000779
        public AspMvcAreaAttribute()
        {
        }

        // Token: 0x0600009B RID: 155 RVA: 0x00002581 File Offset: 0x00000781
        public AspMvcAreaAttribute([NotNull] string anonymousProperty)
        {
            this.AnonymousProperty = anonymousProperty;
        }

        // Token: 0x1700001E RID: 30
        // (get) Token: 0x0600009C RID: 156 RVA: 0x00002590 File Offset: 0x00000790
        // (set) Token: 0x0600009D RID: 157 RVA: 0x00002598 File Offset: 0x00000798
        [CanBeNull]
        public string AnonymousProperty { get; private set; }
    }
}
