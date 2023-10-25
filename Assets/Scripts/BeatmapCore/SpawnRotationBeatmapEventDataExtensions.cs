using System;
using System.Collections.Generic;

// Token: 0x0200001F RID: 31
public class SpawnRotationBeatmapEventDataProcessor : BeatmapEventDataProcessor<SpawnRotationBeatmapEventData>
{
    // Token: 0x06000071 RID: 113 RVA: 0x00002ED4 File Offset: 0x000010D4
    protected override void ProcessInsertedEventDataInternal(LinkedListNode<BeatmapDataItem> insertedNode)
    {
        if (insertedNode.Previous == null)
        {
            ((SpawnRotationBeatmapEventData)insertedNode.Value).SetFirstRotationEventRotation();
        }
        else
        {
            ((SpawnRotationBeatmapEventData)insertedNode.Value).RecalculateRotationFromPreviousEvent((SpawnRotationBeatmapEventData)insertedNode.Previous.Value);
        }
        SpawnRotationBeatmapEventData previousSpawnRotationBeatmapEventData = (SpawnRotationBeatmapEventData)insertedNode.Value;
        for (LinkedListNode<BeatmapDataItem> next = insertedNode.Next; next != null; next = next.Next)
        {
            SpawnRotationBeatmapEventData spawnRotationBeatmapEventData = (SpawnRotationBeatmapEventData)next.Value;
            spawnRotationBeatmapEventData.RecalculateRotationFromPreviousEvent(previousSpawnRotationBeatmapEventData);
            previousSpawnRotationBeatmapEventData = spawnRotationBeatmapEventData;
        }
    }

    // Token: 0x06000072 RID: 114 RVA: 0x00002F50 File Offset: 0x00001150
    protected override void ProcessBeforeDeleteEventDataInternal(LinkedListNode<BeatmapDataItem> nodeToDelete)
    {
        if (nodeToDelete.Next == null)
        {
            return;
        }
        if (nodeToDelete.Previous == null)
        {
            ((SpawnRotationBeatmapEventData)nodeToDelete.Next.Value).SetFirstRotationEventRotation();
        }
        else
        {
            ((SpawnRotationBeatmapEventData)nodeToDelete.Next.Value).RecalculateRotationFromPreviousEvent((SpawnRotationBeatmapEventData)nodeToDelete.Previous.Value);
        }
        SpawnRotationBeatmapEventData previousSpawnRotationBeatmapEventData = (SpawnRotationBeatmapEventData)nodeToDelete.Next.Value;
        for (LinkedListNode<BeatmapDataItem> next = nodeToDelete.Next.Next; next != null; next = next.Next)
        {
            SpawnRotationBeatmapEventData spawnRotationBeatmapEventData = (SpawnRotationBeatmapEventData)next.Value;
            spawnRotationBeatmapEventData.RecalculateRotationFromPreviousEvent(previousSpawnRotationBeatmapEventData);
            previousSpawnRotationBeatmapEventData = spawnRotationBeatmapEventData;
        }
    }
}
