using System;
using System.Collections.Generic;

// Token: 0x0200001C RID: 28
public class LightColorBeatmapEventDataProcessor : BeatmapEventDataProcessor<LightColorBeatmapEventData>
{
    // Token: 0x06000068 RID: 104 RVA: 0x00002CA4 File Offset: 0x00000EA4
    protected override void ProcessInsertedEventDataInternal(LinkedListNode<BeatmapDataItem> insertedNode)
    {
        LightColorBeatmapEventData lightColorBeatmapEventData = (LightColorBeatmapEventData)insertedNode.Value;
        if (lightColorBeatmapEventData.transitionType == BeatmapEventTransitionType.Extend)
        {
            LightColorBeatmapEventData lightColorBeatmapEventData2 = (LightColorBeatmapEventData)lightColorBeatmapEventData.previousSameTypeEventData;
            if (lightColorBeatmapEventData2 == null)
            {
                return;
            }
            lightColorBeatmapEventData.CopyColorDataFrom(lightColorBeatmapEventData2);
        }
        LightColorBeatmapEventData lightColorBeatmapEventData3 = (LightColorBeatmapEventData)lightColorBeatmapEventData.nextSameTypeEventData;
        if (lightColorBeatmapEventData3 == null)
        {
            return;
        }
        if (lightColorBeatmapEventData3.transitionType == BeatmapEventTransitionType.Extend)
        {
            lightColorBeatmapEventData3.CopyColorDataFrom(lightColorBeatmapEventData);
        }
    }

    // Token: 0x06000069 RID: 105 RVA: 0x00002D00 File Offset: 0x00000F00
    protected override void ProcessBeforeDeleteEventDataInternal(LinkedListNode<BeatmapDataItem> nodeToDelete)
    {
        LightColorBeatmapEventData lightColorBeatmapEventData = (LightColorBeatmapEventData)nodeToDelete.Value;
        LightColorBeatmapEventData lightColorBeatmapEventData2 = (LightColorBeatmapEventData)lightColorBeatmapEventData.nextSameTypeEventData;
        LightColorBeatmapEventData lightColorBeatmapEventData3 = (LightColorBeatmapEventData)lightColorBeatmapEventData.previousSameTypeEventData;
        if (lightColorBeatmapEventData2 == null || lightColorBeatmapEventData3 == null)
        {
            return;
        }
        if (lightColorBeatmapEventData2.transitionType != BeatmapEventTransitionType.Extend)
        {
            return;
        }
        lightColorBeatmapEventData2.CopyColorDataFrom(lightColorBeatmapEventData3);
    }
}
