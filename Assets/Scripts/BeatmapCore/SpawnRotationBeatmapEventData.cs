using System;

// Token: 0x02000017 RID: 23
public class SpawnRotationBeatmapEventData : BeatmapEventData
{
    // Token: 0x1700000F RID: 15
    // (get) Token: 0x06000053 RID: 83 RVA: 0x0000297D File Offset: 0x00000B7D
    public override int subtypeGroupIdentifier
    {
        get
        {
            return 0;
        }
    }

    // Token: 0x17000010 RID: 16
    // (get) Token: 0x06000054 RID: 84 RVA: 0x00002980 File Offset: 0x00000B80
    // (set) Token: 0x06000055 RID: 85 RVA: 0x00002988 File Offset: 0x00000B88
    public float rotation { get; private set; }

    // Token: 0x06000056 RID: 86 RVA: 0x00002991 File Offset: 0x00000B91
    public SpawnRotationBeatmapEventData(float time, SpawnRotationBeatmapEventData.SpawnRotationEventType spawnRotationEventType, float deltaRotation) : base(time, (spawnRotationEventType == SpawnRotationBeatmapEventData.SpawnRotationEventType.Early) ? -1000 : 1000, (int)spawnRotationEventType)
    {
        this.spawnRotationEventType = spawnRotationEventType;
        this._deltaRotation = deltaRotation;
    }

    // Token: 0x06000057 RID: 87 RVA: 0x000029B9 File Offset: 0x00000BB9
    public virtual void Mirror()
    {
        this.rotation = -this.rotation;
        this._deltaRotation = -this._deltaRotation;
    }

    // Token: 0x06000058 RID: 88 RVA: 0x000029D5 File Offset: 0x00000BD5
    public override BeatmapDataItem GetCopy()
    {
        return new SpawnRotationBeatmapEventData(base.time, this.spawnRotationEventType, this._deltaRotation);
    }

    // Token: 0x06000059 RID: 89 RVA: 0x000029EE File Offset: 0x00000BEE
    public virtual void RecalculateRotationFromPreviousEvent(SpawnRotationBeatmapEventData previousSpawnRotationBeatmapEventData)
    {
        this.rotation = previousSpawnRotationBeatmapEventData.rotation + this._deltaRotation;
    }

    // Token: 0x0600005A RID: 90 RVA: 0x00002A03 File Offset: 0x00000C03
    public virtual void SetFirstRotationEventRotation()
    {
        this.rotation = this._deltaRotation;
    }

    // Token: 0x0600005B RID: 91 RVA: 0x00002A11 File Offset: 0x00000C11
    protected override BeatmapEventData GetDefault()
    {
        return SpawnRotationBeatmapEventData._defaultCopy;
    }

    // Token: 0x04000085 RID: 133
    protected readonly SpawnRotationBeatmapEventData.SpawnRotationEventType spawnRotationEventType;

    // Token: 0x04000086 RID: 134
    protected float _deltaRotation;

    // Token: 0x04000087 RID: 135
    [DoesNotRequireDomainReloadInit]
    protected static readonly BeatmapEventData _defaultCopy = new SpawnRotationBeatmapEventData(0f, SpawnRotationBeatmapEventData.SpawnRotationEventType.Early, 0f);

    // Token: 0x02000018 RID: 24
    public enum SpawnRotationEventType
    {
        // Token: 0x04000089 RID: 137
        Early = 1,
        // Token: 0x0400008A RID: 138
        Late
    }
}
