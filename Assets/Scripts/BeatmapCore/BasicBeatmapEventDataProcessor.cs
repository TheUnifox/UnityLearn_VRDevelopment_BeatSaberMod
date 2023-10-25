using System;
using System.Collections.Generic;

// Token: 0x02000019 RID: 25
public class BasicBeatmapEventDataProcessor : BeatmapEventDataProcessor<BasicBeatmapEventData>
{
    // Token: 0x0600005D RID: 93 RVA: 0x00002A30 File Offset: 0x00000C30
    protected override void ProcessInsertedEventDataInternal(LinkedListNode<BeatmapDataItem> insertedNode)
    {
        if (insertedNode.Previous == null)
        {
            ((BasicBeatmapEventData)insertedNode.Value).SetFirstSameTypeIndex();
        }
        else
        {
            ((BasicBeatmapEventData)insertedNode.Value).RecalculateSameTypeIndexFromPreviousEvent((BasicBeatmapEventData)insertedNode.Previous.Value);
        }
        BasicBeatmapEventData basicBeatmapEventData = (BasicBeatmapEventData)insertedNode.Value;
        for (LinkedListNode<BeatmapDataItem> next = insertedNode.Next; next != null; next = next.Next)
        {
            BasicBeatmapEventData basicBeatmapEventData2 = (BasicBeatmapEventData)next.Value;
            basicBeatmapEventData2.RecalculateSameTypeIndexFromPreviousEvent(basicBeatmapEventData);
            basicBeatmapEventData = basicBeatmapEventData2;
        }
    }

    // Token: 0x0600005E RID: 94 RVA: 0x00002AAC File Offset: 0x00000CAC
    protected override void ProcessBeforeDeleteEventDataInternal(LinkedListNode<BeatmapDataItem> nodeToDelete)
    {
        if (nodeToDelete.Next == null)
        {
            return;
        }
        if (nodeToDelete.Previous == null)
        {
            ((BasicBeatmapEventData)nodeToDelete.Next.Value).SetFirstSameTypeIndex();
        }
        else
        {
            ((BasicBeatmapEventData)nodeToDelete.Next.Value).RecalculateSameTypeIndexFromPreviousEvent((BasicBeatmapEventData)nodeToDelete.Previous.Value);
        }
        BasicBeatmapEventData basicBeatmapEventData = (BasicBeatmapEventData)nodeToDelete.Next.Value;
        for (LinkedListNode<BeatmapDataItem> next = nodeToDelete.Next.Next; next != null; next = next.Next)
        {
            BasicBeatmapEventData basicBeatmapEventData2 = (BasicBeatmapEventData)next.Value;
            basicBeatmapEventData2.RecalculateSameTypeIndexFromPreviousEvent(basicBeatmapEventData);
            basicBeatmapEventData = basicBeatmapEventData2;
        }
    }
}