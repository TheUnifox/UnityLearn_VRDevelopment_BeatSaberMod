// Decompiled with JetBrains decompiler
// Type: BeatmapEventDataBoxGroup
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;

public class BeatmapEventDataBoxGroup : IComparable<BeatmapEventDataBoxGroup>
{
  protected readonly float beat;
  protected readonly Dictionary<(int elementId, System.Type boxType, int subtypeIdentifier), BeatmapEventDataBoxGroup.ElementData> _elementDataDict = new Dictionary<(int, System.Type, int), BeatmapEventDataBoxGroup.ElementData>();
  protected readonly List<BeatmapEventData> _unpackedBeatmapEventData = new List<BeatmapEventData>();
  protected readonly IReadOnlyCollection<BeatmapEventDataBox> _beatmapEventDataBoxList;

  public IReadOnlyDictionary<(int elementId, System.Type boxType, int subtypeIdentifier), BeatmapEventDataBoxGroup.ElementData> elementDataDict => (IReadOnlyDictionary<(int, System.Type, int), BeatmapEventDataBoxGroup.ElementData>) this._elementDataDict;

  public BeatmapEventDataBoxGroup(
    float beat,
    IReadOnlyCollection<BeatmapEventDataBox> beatmapEventDataBoxList)
  {
    this.beat = beat;
    this._beatmapEventDataBoxList = beatmapEventDataBoxList;
    foreach (BeatmapEventDataBox beatmapEventDataBox in (IEnumerable<BeatmapEventDataBox>) this._beatmapEventDataBoxList)
    {
      int subtypeIdentifier = beatmapEventDataBox.subtypeIdentifier;
      System.Type type = beatmapEventDataBox.GetType();
      float beatStep = beatmapEventDataBox.beatStep;
      foreach ((int num1, int num2, int num3) in beatmapEventDataBox.indexFilter)
      {
        (int, System.Type, int) key = (num1, type, subtypeIdentifier);
        if (!this._elementDataDict.ContainsKey(key))
          this._elementDataDict[key] = new BeatmapEventDataBoxGroup.ElementData(this, beatmapEventDataBox, num1, num2, num3, beat + beatStep * (float) num2);
      }
    }
  }

  public virtual BeatmapEventDataBoxGroup GetCopyWithNewBeat(float newBeat) => new BeatmapEventDataBoxGroup(newBeat, this._beatmapEventDataBoxList);

  public virtual void RemoveBeatmapEventDataFromBeatmapData(BeatmapData beatmapData)
  {
    foreach (BeatmapEventData beatmapEventData in this._unpackedBeatmapEventData)
      beatmapData.RemoveBeatmapEventData(beatmapEventData);
    this._unpackedBeatmapEventData.Clear();
  }

  public virtual void SyncWithBeatmapData(
    int groupId,
    BeatmapData beatmapData,
    IBeatToTimeConvertor beatToTimeConvertor)
  {
    this.RemoveBeatmapEventDataFromBeatmapData(beatmapData);
    foreach (KeyValuePair<(int elementId, System.Type boxType, int subtypeIdentifier), BeatmapEventDataBoxGroup.ElementData> keyValuePair in this._elementDataDict)
    {
      BeatmapEventDataBoxGroup.ElementData elementData = keyValuePair.Value;
      BeatmapEventDataBoxGroup.ElementData next = elementData.next;
      float maxBeat = next != null ? next.startBeat : float.MaxValue;
      elementData.eventBox.Unpack(this.beat, groupId, elementData.elementId, elementData.durationOrderIndex, elementData.distributionOrderIndex, maxBeat, beatToTimeConvertor, this._unpackedBeatmapEventData);
    }
    foreach (BeatmapEventData beatmapEventData in this._unpackedBeatmapEventData)
      beatmapData.InsertBeatmapEventData(beatmapEventData);
  }

  public virtual int CompareTo(BeatmapEventDataBoxGroup b)
  {
    if ((double) this.beat < (double) b.beat)
      return -1;
    return (double) this.beat > (double) b.beat ? 1 : 0;
  }

  public class ElementData
  {
    public readonly float startBeat;
    public readonly int elementId;
    public readonly int durationOrderIndex;
    public readonly int distributionOrderIndex;
    public readonly System.Type eventBoxType;
    public readonly int eventBoxSubtypeIdentifier;
    public readonly BeatmapEventDataBox eventBox;
    public readonly BeatmapEventDataBoxGroup boxGroup;
    protected BeatmapEventDataBoxGroup.ElementData _next;
    protected BeatmapEventDataBoxGroup.ElementData _previous;

    public BeatmapEventDataBoxGroup.ElementData next => this._next;

    public BeatmapEventDataBoxGroup.ElementData previous => this._previous;

    public ElementData(
      BeatmapEventDataBoxGroup boxGroup,
      BeatmapEventDataBox eventBox,
      int elementId,
      int durationOrderIndex,
      int distributionOrderIndex,
      float startBeat)
    {
      this.eventBox = eventBox;
      this.boxGroup = boxGroup;
      this.eventBoxType = eventBox.GetType();
      this.eventBoxSubtypeIdentifier = eventBox.subtypeIdentifier;
      this.elementId = elementId;
      this.startBeat = startBeat;
      this.durationOrderIndex = durationOrderIndex;
      this.distributionOrderIndex = distributionOrderIndex;
    }

    public virtual void ResetConnections()
    {
      this._next = (BeatmapEventDataBoxGroup.ElementData) null;
      this._previous = (BeatmapEventDataBoxGroup.ElementData) null;
    }

    public virtual void ConnectWithPrevious(
      BeatmapEventDataBoxGroup.ElementData prevElementData)
    {
      this._previous = prevElementData;
      if (this._previous == null)
        return;
      this._previous._next = this;
    }

    public virtual void ConnectWithNext(
      BeatmapEventDataBoxGroup.ElementData nextElementData)
    {
      this._next = nextElementData;
      if (this._next == null)
        return;
      this._next._previous = this;
    }
  }
}
