// Decompiled with JetBrains decompiler
// Type: CallbacksInTime
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;

public class CallbacksInTime
{
  public LinkedListNode<BeatmapDataItem> lastProcessedNode;
  public readonly float aheadTime;
  public BeatmapEventData beatmapEventDataForCallbacksAfterNodeRemoval;
  protected readonly Dictionary<(System.Type BasicBeatmapEventType, int subtypeIdentifier), List<BeatmapDataCallbackWrapper>> _callbacksWithSubtypeIdentifier = new Dictionary<(System.Type, int), List<BeatmapDataCallbackWrapper>>();
  protected readonly Dictionary<System.Type, List<BeatmapDataCallbackWrapper>> _callbacks = new Dictionary<System.Type, List<BeatmapDataCallbackWrapper>>();

  public bool isEmpty => this._callbacksWithSubtypeIdentifier.Count == 0 && this._callbacks.Count == 0;

  public CallbacksInTime(float aheadTime) => this.aheadTime = aheadTime;

  public virtual void AddCallback(BeatmapDataCallbackWrapper callbackWrapper)
  {
    List<BeatmapDataCallbackWrapper> dataCallbackWrapperList = (List<BeatmapDataCallbackWrapper>) null;
    if (callbackWrapper.subtypeIdentifiers != null && callbackWrapper.subtypeIdentifiers.Length != 0)
    {
      foreach (int subtypeIdentifier in callbackWrapper.subtypeIdentifiers)
      {
        (System.Type, int) key = (callbackWrapper.BasicBeatmapEventType, subtypeIdentifier);
        if (!this._callbacksWithSubtypeIdentifier.TryGetValue(key, out dataCallbackWrapperList))
          dataCallbackWrapperList = this._callbacksWithSubtypeIdentifier[key] = new List<BeatmapDataCallbackWrapper>();
        dataCallbackWrapperList.Add(callbackWrapper);
      }
    }
    else
    {
      System.Type beatmapEventType = callbackWrapper.BasicBeatmapEventType;
      if (!this._callbacks.TryGetValue(beatmapEventType, out dataCallbackWrapperList))
        dataCallbackWrapperList = this._callbacks[beatmapEventType] = new List<BeatmapDataCallbackWrapper>();
      dataCallbackWrapperList.Add(callbackWrapper);
    }
  }

  public virtual void RemoveCallback(BeatmapDataCallbackWrapper callbackWrapper)
  {
    List<BeatmapDataCallbackWrapper> dataCallbackWrapperList = (List<BeatmapDataCallbackWrapper>) null;
    if (callbackWrapper.subtypeIdentifiers != null && callbackWrapper.subtypeIdentifiers.Length != 0)
    {
      foreach (int subtypeIdentifier in callbackWrapper.subtypeIdentifiers)
      {
        (System.Type, int) key = (callbackWrapper.BasicBeatmapEventType, subtypeIdentifier);
        if (!this._callbacksWithSubtypeIdentifier.TryGetValue(key, out dataCallbackWrapperList))
          break;
        dataCallbackWrapperList.Remove(callbackWrapper);
        if (dataCallbackWrapperList.Count == 0)
          this._callbacksWithSubtypeIdentifier.Remove(key);
      }
    }
    else
    {
      System.Type beatmapEventType = callbackWrapper.BasicBeatmapEventType;
      if (!this._callbacks.TryGetValue(beatmapEventType, out dataCallbackWrapperList))
        return;
      dataCallbackWrapperList.Remove(callbackWrapper);
      if (dataCallbackWrapperList.Count != 0)
        return;
      this._callbacks.Remove(beatmapEventType);
    }
  }

  public virtual void CallCallbacks(BeatmapDataItem beatmapDataItem)
  {
    System.Type type = beatmapDataItem.GetType();
    this.CallCallbacks(type, beatmapDataItem);
    System.Type baseType = type.BaseType;
    if (!(baseType != (System.Type) null))
      return;
    this.CallCallbacks(baseType, beatmapDataItem);
  }

  public virtual void CallCallbacks(System.Type beatmapEventDataType, BeatmapDataItem beatmapDataItem)
  {
    List<BeatmapDataCallbackWrapper> dataCallbackWrapperList;
    if (this._callbacksWithSubtypeIdentifier.TryGetValue((beatmapEventDataType, beatmapDataItem.subtypeIdentifier), out dataCallbackWrapperList))
    {
      foreach (BeatmapDataCallbackWrapper dataCallbackWrapper in dataCallbackWrapperList)
        dataCallbackWrapper.CallCallback(beatmapDataItem);
    }
    if (!this._callbacks.TryGetValue(beatmapEventDataType, out dataCallbackWrapperList))
      return;
    foreach (BeatmapDataCallbackWrapper dataCallbackWrapper in dataCallbackWrapperList)
      dataCallbackWrapper.CallCallback(beatmapDataItem);
  }
}
