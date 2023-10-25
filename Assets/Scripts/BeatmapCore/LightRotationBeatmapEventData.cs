using System;
using System.Collections.Generic;
using static UnityEditor.IMGUI.Controls.CapsuleBoundsHandle;

// Token: 0x02000014 RID: 20
public class LightRotationBeatmapEventData : BeatmapEventData
{
    // Token: 0x1700000C RID: 12
    // (get) Token: 0x06000041 RID: 65 RVA: 0x000026CA File Offset: 0x000008CA
    // (set) Token: 0x06000042 RID: 66 RVA: 0x000026D2 File Offset: 0x000008D2
    public float rotation { get; private set; }

    // Token: 0x06000043 RID: 67 RVA: 0x000026DC File Offset: 0x000008DC
    public LightRotationBeatmapEventData(float time, int groupId, int elementId, bool usePreviousEventValue, BeatmapSaveDataVersion3.BeatmapSaveData.EaseType easeType, LightAxis axis, float rotation, int loopCount, LightRotationDirection rotationDirection) : base(time, -100, LightRotationBeatmapEventData.SubtypeIdentifier(groupId, elementId, axis))
    {
        this.groupId = groupId;
        this.elementId = elementId;
        this.usePreviousEventValue = usePreviousEventValue;
        this.easeType = easeType;
        this.axis = axis;
        this.loopCount = loopCount;
        this.rotation = rotation;
        this.rotationDirection = rotationDirection;
    }

    // Token: 0x06000044 RID: 68 RVA: 0x00002739 File Offset: 0x00000939
    public virtual void ChangeRotation(float rotation)
    {
        this.rotation = rotation;
    }

    // Token: 0x06000045 RID: 69 RVA: 0x00002744 File Offset: 0x00000944
    public override BeatmapDataItem GetCopy()
    {
        return new LightRotationBeatmapEventData(base.time, this.groupId, this.elementId, this.usePreviousEventValue, this.easeType, this.axis, this.rotation, this.loopCount, this.rotationDirection);
    }

    // Token: 0x06000046 RID: 70 RVA: 0x0000278C File Offset: 0x0000098C
    public static int SubtypeIdentifier(int groupId, int elementId, LightAxis axis)
    {
        return (int)((int)axis * 100000 + groupId * 1000 + elementId);
    }

    // Token: 0x06000047 RID: 71 RVA: 0x000027A0 File Offset: 0x000009A0
    protected override BeatmapEventData GetDefault()
    {
        LightRotationBeatmapEventData result;
        if (LightRotationBeatmapEventData._defaults.TryGetValue(LightRotationBeatmapEventData.SubtypeIdentifier(this.groupId, this.elementId, this.axis), out result))
        {
            return result;
        }
        return LightRotationBeatmapEventData._defaults[LightRotationBeatmapEventData.SubtypeIdentifier(this.groupId, this.elementId, this.axis)] = new LightRotationBeatmapEventData(0f, this.groupId, this.elementId, false, BeatmapSaveDataVersion3.BeatmapSaveData.EaseType.None, this.axis, this.rotation, this.loopCount, LightRotationDirection.Automatic);
    }

    // Token: 0x0400006F RID: 111
    public readonly int groupId;

    // Token: 0x04000070 RID: 112
    public readonly int elementId;

    // Token: 0x04000071 RID: 113
    public readonly bool usePreviousEventValue;

    // Token: 0x04000072 RID: 114
    public readonly BeatmapSaveDataVersion3.BeatmapSaveData.EaseType easeType;

    // Token: 0x04000073 RID: 115
    public readonly LightAxis axis;

    // Token: 0x04000074 RID: 116
    public readonly int loopCount;

    // Token: 0x04000075 RID: 117
    public readonly LightRotationDirection rotationDirection;

    // Token: 0x04000077 RID: 119
    [DoesNotRequireDomainReloadInit]
    protected static readonly Dictionary<int, LightRotationBeatmapEventData> _defaults = new Dictionary<int, LightRotationBeatmapEventData>();
}
