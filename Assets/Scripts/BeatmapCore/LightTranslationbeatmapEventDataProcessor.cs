using System;
using System.Collections.Generic;

// Token: 0x0200001E RID: 30
public class LightTranslationBeatmapEventDataProcessor : BeatmapEventDataProcessor<LightTranslationBeatmapEventData>
{
    // Token: 0x0600006E RID: 110 RVA: 0x00002E08 File Offset: 0x00001008
    protected override void ProcessInsertedEventDataInternal(LinkedListNode<BeatmapDataItem> insertedNode)
    {
        LightTranslationBeatmapEventData lightTranslationBeatmapEventData = (LightTranslationBeatmapEventData)insertedNode.Value;
        if (lightTranslationBeatmapEventData.usePreviousEventValue)
        {
            LightTranslationBeatmapEventData lightTranslationBeatmapEventData2 = (LightTranslationBeatmapEventData)lightTranslationBeatmapEventData.previousSameTypeEventData;
            if (lightTranslationBeatmapEventData2 == null)
            {
                return;
            }
            lightTranslationBeatmapEventData.ChangeTranslation(lightTranslationBeatmapEventData2.translation, lightTranslationBeatmapEventData2.distribution);
        }
        LightTranslationBeatmapEventData lightTranslationBeatmapEventData3 = (LightTranslationBeatmapEventData)lightTranslationBeatmapEventData.nextSameTypeEventData;
        if (lightTranslationBeatmapEventData3 == null)
        {
            return;
        }
        if (lightTranslationBeatmapEventData3.usePreviousEventValue)
        {
            lightTranslationBeatmapEventData3.ChangeTranslation(lightTranslationBeatmapEventData.translation, lightTranslationBeatmapEventData.distribution);
        }
    }

    // Token: 0x0600006F RID: 111 RVA: 0x00002E78 File Offset: 0x00001078
    protected override void ProcessBeforeDeleteEventDataInternal(LinkedListNode<BeatmapDataItem> nodeToDelete)
    {
        LightTranslationBeatmapEventData lightTranslationBeatmapEventData = (LightTranslationBeatmapEventData)nodeToDelete.Value;
        LightTranslationBeatmapEventData lightTranslationBeatmapEventData2 = (LightTranslationBeatmapEventData)lightTranslationBeatmapEventData.nextSameTypeEventData;
        LightTranslationBeatmapEventData lightTranslationBeatmapEventData3 = (LightTranslationBeatmapEventData)lightTranslationBeatmapEventData.previousSameTypeEventData;
        if (lightTranslationBeatmapEventData2 == null || lightTranslationBeatmapEventData3 == null)
        {
            return;
        }
        if (!lightTranslationBeatmapEventData2.usePreviousEventValue)
        {
            return;
        }
        lightTranslationBeatmapEventData2.ChangeTranslation(lightTranslationBeatmapEventData3.translation, lightTranslationBeatmapEventData3.distribution);
    }
}
