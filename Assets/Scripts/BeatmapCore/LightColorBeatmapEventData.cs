using System;
using System.Collections.Generic;

// Token: 0x02000013 RID: 19
public class LightColorBeatmapEventData : BeatmapEventData
{
    // Token: 0x17000009 RID: 9
    // (get) Token: 0x06000034 RID: 52 RVA: 0x00002573 File Offset: 0x00000773
    // (set) Token: 0x06000035 RID: 53 RVA: 0x0000257B File Offset: 0x0000077B
    public EnvironmentColorType colorType { get; private set; }

    // Token: 0x1700000A RID: 10
    // (get) Token: 0x06000036 RID: 54 RVA: 0x00002584 File Offset: 0x00000784
    // (set) Token: 0x06000037 RID: 55 RVA: 0x0000258C File Offset: 0x0000078C
    public float brightness { get; private set; }

    // Token: 0x1700000B RID: 11
    // (get) Token: 0x06000038 RID: 56 RVA: 0x00002595 File Offset: 0x00000795
    // (set) Token: 0x06000039 RID: 57 RVA: 0x0000259D File Offset: 0x0000079D
    public int strobeBeatFrequency { get; private set; }

    // Token: 0x0600003A RID: 58 RVA: 0x000025A6 File Offset: 0x000007A6
    public LightColorBeatmapEventData(float time, int groupId, int elementId, BeatmapEventTransitionType transitionType, EnvironmentColorType colorType, float brightness, int strobeBeatFrequency) : base(time, -100, LightColorBeatmapEventData.SubtypeIdentifier(groupId, elementId))
    {
        this.groupId = groupId;
        this.elementId = elementId;
        this.transitionType = transitionType;
        this.colorType = colorType;
        this.brightness = brightness;
        this.strobeBeatFrequency = strobeBeatFrequency;
    }

    // Token: 0x0600003B RID: 59 RVA: 0x000025E6 File Offset: 0x000007E6
    public virtual void CopyColorDataFrom(LightColorBeatmapEventData lightColorBeatmapEventData)
    {
        this.colorType = lightColorBeatmapEventData.colorType;
        this.brightness = lightColorBeatmapEventData.brightness;
        this.strobeBeatFrequency = lightColorBeatmapEventData.strobeBeatFrequency;
    }

    // Token: 0x0600003C RID: 60 RVA: 0x0000260C File Offset: 0x0000080C
    public virtual void DisableStrobe()
    {
        this.strobeBeatFrequency = 0;
    }

    // Token: 0x0600003D RID: 61 RVA: 0x00002615 File Offset: 0x00000815
    public override BeatmapDataItem GetCopy()
    {
        return new LightColorBeatmapEventData(base.time, this.groupId, this.elementId, this.transitionType, this.colorType, this.brightness, this.strobeBeatFrequency);
    }

    // Token: 0x0600003E RID: 62 RVA: 0x00002646 File Offset: 0x00000846
    public static int SubtypeIdentifier(int groupId, int elementId)
    {
        return groupId * 10000 + elementId;
    }

    // Token: 0x0600003F RID: 63 RVA: 0x00002654 File Offset: 0x00000854
    protected override BeatmapEventData GetDefault()
    {
        LightColorBeatmapEventData result;
        if (LightColorBeatmapEventData._defaults.TryGetValue(LightColorBeatmapEventData.SubtypeIdentifier(this.groupId, this.elementId), out result))
        {
            return result;
        }
        return LightColorBeatmapEventData._defaults[LightColorBeatmapEventData.SubtypeIdentifier(this.groupId, this.elementId)] = new LightColorBeatmapEventData(0f, this.groupId, this.elementId, BeatmapEventTransitionType.Instant, EnvironmentColorType.Color0, 0f, 0);
    }

    // Token: 0x04000068 RID: 104
    public readonly int groupId;

    // Token: 0x04000069 RID: 105
    public readonly int elementId;

    // Token: 0x0400006A RID: 106
    public readonly BeatmapEventTransitionType transitionType;

    // Token: 0x0400006E RID: 110
    [DoesNotRequireDomainReloadInit]
    protected static readonly Dictionary<int, LightColorBeatmapEventData> _defaults = new Dictionary<int, LightColorBeatmapEventData>();
}
