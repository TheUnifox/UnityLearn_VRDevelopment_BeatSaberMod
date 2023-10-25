using System;
using System.Collections.Generic;

public class BasicBeatmapEventData : BeatmapEventData
{
    
    
    public new BasicBeatmapEventData previousSameTypeEventData
    {
        get
        {
            return (BasicBeatmapEventData)base.previousSameTypeEventData;
        }
    }

    
    
    public new BasicBeatmapEventData nextSameTypeEventData
    {
        get
        {
            return (BasicBeatmapEventData)base.nextSameTypeEventData;
        }
    }

    
    
    
    public int sameTypeIndex { get; private set; }

    
    public BasicBeatmapEventData(float time, BasicBeatmapEventType basicBeatmapEventType, int value, float floatValue) : base(time, -100, BasicBeatmapEventData.SubtypeIdentifier(basicBeatmapEventType))
    {
        this.basicBeatmapEventType = basicBeatmapEventType;
        this.value = value;
        this.floatValue = floatValue;
    }

    
    public override BeatmapDataItem GetCopy()
    {
        return new BasicBeatmapEventData(base.time, this.basicBeatmapEventType, this.value, this.floatValue);
    }

    
    public static int SubtypeIdentifier(BasicBeatmapEventType type)
    {
        return (int)type;
    }

    
    public virtual void SetFirstSameTypeIndex()
    {
        this.sameTypeIndex = 1;
    }

    
    public virtual void RecalculateSameTypeIndexFromPreviousEvent(BasicBeatmapEventData basicBeatmapEventData)
    {
        this.sameTypeIndex = basicBeatmapEventData.sameTypeIndex + 1;
    }

    
    protected override BeatmapEventData GetDefault()
    {
        BasicBeatmapEventData result;
        if (BasicBeatmapEventData._defaultsForType.TryGetValue(this.basicBeatmapEventType, out result))
        {
            return result;
        }
        return BasicBeatmapEventData._defaultsForType[this.basicBeatmapEventType] = new BasicBeatmapEventData(0f, this.basicBeatmapEventType, 0, 0f);
    }

    
    public readonly BasicBeatmapEventType basicBeatmapEventType;

    
    public readonly int value;

    
    public readonly float floatValue;


    [DoesNotRequireDomainReloadInit]
    protected static readonly Dictionary<BasicBeatmapEventType, BasicBeatmapEventData> _defaultsForType = new Dictionary<BasicBeatmapEventType, BasicBeatmapEventData>();
}