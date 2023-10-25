// Decompiled with JetBrains decompiler
// Type: BeatmapEventDataBoxGroupProcessor
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;

public class BeatmapEventDataBoxGroupProcessor : ISortedListItemProcessor<BeatmapEventDataBoxGroup>
{
  protected readonly HashSet<BeatmapEventDataBoxGroup> _dirtyBoxGroups = new HashSet<BeatmapEventDataBoxGroup>();

  public IReadOnlyCollection<BeatmapEventDataBoxGroup> dirtyBoxGroups => (IReadOnlyCollection<BeatmapEventDataBoxGroup>) this._dirtyBoxGroups;

  public virtual void ProcessInsertedData(
    LinkedListNode<BeatmapEventDataBoxGroup> insertedNode)
  {
    foreach (KeyValuePair<(int elementId, System.Type boxType, int subtypeIdentifier), BeatmapEventDataBoxGroup.ElementData> keyValuePair in (IEnumerable<KeyValuePair<(int elementId, System.Type boxType, int subtypeIdentifier), BeatmapEventDataBoxGroup.ElementData>>) insertedNode.Value.elementDataDict)
    {
      BeatmapEventDataBoxGroup.ElementData nextElementData1 = keyValuePair.Value;
      nextElementData1.ResetConnections();
      (int, System.Type, int) key = (nextElementData1.elementId, nextElementData1.eventBoxType, nextElementData1.eventBoxSubtypeIdentifier);
      bool flag = false;
      for (LinkedListNode<BeatmapEventDataBoxGroup> previous = insertedNode.Previous; previous != null; previous = previous.Previous)
      {
        BeatmapEventDataBoxGroup.ElementData elementData;
        if (previous.Value.elementDataDict.TryGetValue(key, out elementData))
        {
          nextElementData1.ConnectWithNext(elementData.next);
          elementData.ConnectWithNext(nextElementData1);
          this._dirtyBoxGroups.Add(elementData.boxGroup);
          flag = true;
          break;
        }
      }
      if (!flag)
      {
        for (LinkedListNode<BeatmapEventDataBoxGroup> next = insertedNode.Next; next != null; next = next.Next)
        {
          BeatmapEventDataBoxGroup.ElementData nextElementData2;
          if (next.Value.elementDataDict.TryGetValue(key, out nextElementData2))
          {
            nextElementData1.ConnectWithNext(nextElementData2);
            break;
          }
        }
      }
    }
  }

  public virtual void ProcessBeforeDeleteData(
    LinkedListNode<BeatmapEventDataBoxGroup> nodeToDelete)
  {
    foreach (KeyValuePair<(int elementId, System.Type boxType, int subtypeIdentifier), BeatmapEventDataBoxGroup.ElementData> keyValuePair in (IEnumerable<KeyValuePair<(int elementId, System.Type boxType, int subtypeIdentifier), BeatmapEventDataBoxGroup.ElementData>>) nodeToDelete.Value.elementDataDict)
    {
      BeatmapEventDataBoxGroup.ElementData elementData = keyValuePair.Value;
      if (elementData.previous != null)
      {
        elementData.previous.ConnectWithNext(elementData.next);
        this._dirtyBoxGroups.Add(elementData.previous.boxGroup);
      }
      else if (elementData.next != null)
        elementData.next.ConnectWithPrevious(elementData.previous);
    }
  }

  public virtual void ClearDirtyData() => this._dirtyBoxGroups.Clear();
}
