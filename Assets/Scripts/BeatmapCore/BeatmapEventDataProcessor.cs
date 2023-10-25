using System;
using System.Collections.Generic;

// Token: 0x0200001A RID: 26
public abstract class BeatmapEventDataProcessor<T> : ISortedListItemProcessor<BeatmapDataItem> where T : BeatmapEventData
{
    // Token: 0x06000060 RID: 96 RVA: 0x00002B4A File Offset: 0x00000D4A
    public void ProcessInsertedData(LinkedListNode<BeatmapDataItem> insertedNode)
    {
        BeatmapEventDataProcessor < T >.ProcessInsertedEventDataCommon(insertedNode);
        this.ProcessInsertedEventDataInternal(insertedNode);
    }

    // Token: 0x06000061 RID: 97 RVA: 0x00002B59 File Offset: 0x00000D59
    public void ProcessBeforeDeleteData(LinkedListNode<BeatmapDataItem> nodeToDelete)
    {
        this.ProcessBeforeDeleteEventDataInternal(nodeToDelete);
        BeatmapEventDataProcessor < T >.ProcessBeforeDeleteEventDataCommon(nodeToDelete);
    }

    // Token: 0x06000062 RID: 98 RVA: 0x00002B68 File Offset: 0x00000D68
    private static void ProcessBeforeDeleteEventDataCommon(LinkedListNode<BeatmapDataItem> nodeToDelete)
    {
        T t = (T)((object)nodeToDelete.Value);
        if (t.previousSameTypeEventData != null)
        {
            t.previousSameTypeEventData.__ConnectWithNextSameTypeEventData(t.nextSameTypeEventData);
        }
        if (t.nextSameTypeEventData != null)
        {
            t.nextSameTypeEventData.__ConnectWithPreviousSameTypeEventData(t.previousSameTypeEventData);
        }
    }

    // Token: 0x06000063 RID: 99 RVA: 0x00002BD4 File Offset: 0x00000DD4
    private static void ProcessInsertedEventDataCommon(LinkedListNode<BeatmapDataItem> insertedNode)
    {
        T t = (T)((object)insertedNode.Value);
        t.__ResetConnections();
        bool flag = false;
        for (LinkedListNode<BeatmapDataItem> linkedListNode = insertedNode.Previous; linkedListNode != null; linkedListNode = linkedListNode.Previous)
        {
            BeatmapEventData beatmapEventData = (T)((object)linkedListNode.Value);
            if (beatmapEventData.subtypeIdentifier == t.subtypeIdentifier)
            {
                t.__ConnectWithNextSameTypeEventData(beatmapEventData.nextSameTypeEventData);
                beatmapEventData.__ConnectWithNextSameTypeEventData(t);
                flag = true;
                break;
            }
        }
        if (!flag)
        {
            for (LinkedListNode<BeatmapDataItem> linkedListNode = insertedNode.Next; linkedListNode != null; linkedListNode = linkedListNode.Next)
            {
                BeatmapEventData beatmapEventData = (T)((object)linkedListNode.Value);
                if (beatmapEventData.subtypeIdentifier == t.subtypeIdentifier)
                {
                    t.__ConnectWithNextSameTypeEventData(beatmapEventData);
                    return;
                }
            }
        }
    }

    // Token: 0x06000064 RID: 100 RVA: 0x00002C99 File Offset: 0x00000E99
    protected virtual void ProcessInsertedEventDataInternal(LinkedListNode<BeatmapDataItem> insertedNode)
    {
    }

    // Token: 0x06000065 RID: 101 RVA: 0x00002C99 File Offset: 0x00000E99
    protected virtual void ProcessBeforeDeleteEventDataInternal(LinkedListNode<BeatmapDataItem> nodeToDelete)
    {
    }
}
