using System;
using System.Collections.Generic;

// Token: 0x0200001D RID: 29
public class LightRotationBeatmapEventDataProcessor : BeatmapEventDataProcessor<LightRotationBeatmapEventData>
{
    // Token: 0x0600006B RID: 107 RVA: 0x00002D50 File Offset: 0x00000F50
    protected override void ProcessInsertedEventDataInternal(LinkedListNode<BeatmapDataItem> insertedNode)
    {
        LightRotationBeatmapEventData lightRotationBeatmapEventData = (LightRotationBeatmapEventData)insertedNode.Value;
        if (lightRotationBeatmapEventData.usePreviousEventValue)
        {
            LightRotationBeatmapEventData lightRotationBeatmapEventData2 = (LightRotationBeatmapEventData)lightRotationBeatmapEventData.previousSameTypeEventData;
            if (lightRotationBeatmapEventData2 == null)
            {
                return;
            }
            lightRotationBeatmapEventData.ChangeRotation(lightRotationBeatmapEventData2.rotation);
        }
        LightRotationBeatmapEventData lightRotationBeatmapEventData3 = (LightRotationBeatmapEventData)lightRotationBeatmapEventData.nextSameTypeEventData;
        if (lightRotationBeatmapEventData3 == null)
        {
            return;
        }
        if (lightRotationBeatmapEventData3.usePreviousEventValue)
        {
            lightRotationBeatmapEventData3.ChangeRotation(lightRotationBeatmapEventData.rotation);
        }
    }

    // Token: 0x0600006C RID: 108 RVA: 0x00002DB4 File Offset: 0x00000FB4
    protected override void ProcessBeforeDeleteEventDataInternal(LinkedListNode<BeatmapDataItem> nodeToDelete)
    {
        LightRotationBeatmapEventData lightRotationBeatmapEventData = (LightRotationBeatmapEventData)nodeToDelete.Value;
        LightRotationBeatmapEventData lightRotationBeatmapEventData2 = (LightRotationBeatmapEventData)lightRotationBeatmapEventData.nextSameTypeEventData;
        LightRotationBeatmapEventData lightRotationBeatmapEventData3 = (LightRotationBeatmapEventData)lightRotationBeatmapEventData.previousSameTypeEventData;
        if (lightRotationBeatmapEventData2 == null || lightRotationBeatmapEventData3 == null)
        {
            return;
        }
        if (!lightRotationBeatmapEventData2.usePreviousEventValue)
        {
            return;
        }
        lightRotationBeatmapEventData2.ChangeRotation(lightRotationBeatmapEventData3.rotation);
    }
}