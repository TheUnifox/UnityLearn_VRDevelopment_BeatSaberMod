using System;
using System.Collections.Generic;
using static UnityEditor.IMGUI.Controls.CapsuleBoundsHandle;

// Token: 0x02000016 RID: 22
public class LightTranslationBeatmapEventData : BeatmapEventData
{
    // Token: 0x1700000D RID: 13
    // (get) Token: 0x06000049 RID: 73 RVA: 0x0000282F File Offset: 0x00000A2F
    // (set) Token: 0x0600004A RID: 74 RVA: 0x00002837 File Offset: 0x00000A37
    public float translation { get; private set; }

    // Token: 0x1700000E RID: 14
    // (get) Token: 0x0600004B RID: 75 RVA: 0x00002840 File Offset: 0x00000A40
    // (set) Token: 0x0600004C RID: 76 RVA: 0x00002848 File Offset: 0x00000A48
    public float distribution { get; private set; }

    // Token: 0x0600004D RID: 77 RVA: 0x00002854 File Offset: 0x00000A54
    public LightTranslationBeatmapEventData(float time, int groupId, int elementId, bool usePreviousEventValue, BeatmapSaveDataVersion3.BeatmapSaveData.EaseType easeType, LightAxis axis, float translation, float distribution) : base(time, -100, LightTranslationBeatmapEventData.SubtypeIdentifier(groupId, elementId, axis))
    {
        this.groupId = groupId;
        this.elementId = elementId;
        this.usePreviousEventValue = usePreviousEventValue;
        this.easeType = easeType;
        this.axis = axis;
        this.translation = translation;
        this.distribution = distribution;
    }

    // Token: 0x0600004E RID: 78 RVA: 0x000028A9 File Offset: 0x00000AA9
    public virtual void ChangeTranslation(float translation, float distribution)
    {
        this.translation = translation;
        this.distribution = distribution;
    }

    // Token: 0x0600004F RID: 79 RVA: 0x000028B9 File Offset: 0x00000AB9
    public override BeatmapDataItem GetCopy()
    {
        return new LightTranslationBeatmapEventData(base.time, this.groupId, this.elementId, this.usePreviousEventValue, this.easeType, this.axis, this.translation, this.distribution);
    }

    // Token: 0x06000050 RID: 80 RVA: 0x0000278C File Offset: 0x0000098C
    public static int SubtypeIdentifier(int groupId, int elementId, LightAxis axis)
    {
        return (int)(((int)axis * 100000) + groupId * 1000 + elementId);
    }

    // Token: 0x06000051 RID: 81 RVA: 0x000028F0 File Offset: 0x00000AF0
    protected override BeatmapEventData GetDefault()
    {
        LightTranslationBeatmapEventData result;
        if (LightTranslationBeatmapEventData._defaults.TryGetValue(LightTranslationBeatmapEventData.SubtypeIdentifier(this.groupId, this.elementId, this.axis), out result))
        {
            return result;
        }
        return LightTranslationBeatmapEventData._defaults[LightTranslationBeatmapEventData.SubtypeIdentifier(this.groupId, this.elementId, this.axis)] = new LightTranslationBeatmapEventData(0f, this.groupId, this.elementId, false, BeatmapSaveDataVersion3.BeatmapSaveData.EaseType.None, this.axis, this.translation, 0f);
    }

    // Token: 0x0400007C RID: 124
    public readonly int groupId;

    // Token: 0x0400007D RID: 125
    public readonly int elementId;

    // Token: 0x0400007E RID: 126
    public readonly bool usePreviousEventValue;

    // Token: 0x0400007F RID: 127
    public readonly BeatmapSaveDataVersion3.BeatmapSaveData.EaseType easeType;

    // Token: 0x04000080 RID: 128
    public readonly LightAxis axis;

    // Token: 0x04000083 RID: 131
    [DoesNotRequireDomainReloadInit]
    protected static readonly Dictionary<int, LightTranslationBeatmapEventData> _defaults = new Dictionary<int, LightTranslationBeatmapEventData>();
}
