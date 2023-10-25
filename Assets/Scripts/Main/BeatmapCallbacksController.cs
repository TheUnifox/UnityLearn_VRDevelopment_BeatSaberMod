// Decompiled with JetBrains decompiler
// Type: BeatmapCallbacksController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using UnityEngine.Assertions;
using Zenject;

public class BeatmapCallbacksController : IDisposable
{
  protected readonly Dictionary<float, CallbacksInTime> _callbacksInTimes = new Dictionary<float, CallbacksInTime>();
  protected readonly IReadonlyBeatmapData _beatmapData;
  protected readonly BeatmapCallbacksController.ICallCallbacksBehavior _callCallbacksBehavior;
  protected readonly float _startFilterTime;
  protected float _prevSongTime = float.MinValue;
  protected float _songTime;
  protected bool _sendCallbacksOnBeatmapDataChangeChange;
  protected bool _processingCallbacks;

  public bool sendCallbacksOnBeatmapDataChange
  {
    get => this._sendCallbacksOnBeatmapDataChangeChange;
    set => this._sendCallbacksOnBeatmapDataChangeChange = value;
  }

  public float songTime => this._songTime;

  public event System.Action didProcessAllCallbacksThisFrameEvent;

  [Inject]
  public BeatmapCallbacksController(BeatmapCallbacksController.InitData initData)
  {
    this._beatmapData = initData.beatmapData;
    this._startFilterTime = initData.startFilterTime;
    this._beatmapData.beatmapEventDataWasInsertedEvent += new System.Action<BeatmapEventData, LinkedListNode<BeatmapDataItem>>(this.HandleBeatmapEventDataWasInserted);
    this._beatmapData.beatmapEventDataWillBeRemovedEvent += new System.Action<BeatmapEventData, LinkedListNode<BeatmapDataItem>>(this.HandleBeatmapEventDataWillBeRemoved);
    this._beatmapData.beatmapEventDataWasRemovedEvent += new System.Action<BeatmapEventData>(this.HandleBeatmapEventDataWasRemoved);
    this._callCallbacksBehavior = initData.shouldKeepReplayState ? (BeatmapCallbacksController.ICallCallbacksBehavior) new BeatmapCallbacksController.CallCallbacksBehaviorWithLastState() : (BeatmapCallbacksController.ICallCallbacksBehavior) new BeatmapCallbacksController.CallCallbacksBehavior();
  }

  public virtual void Dispose()
  {
    if (this._beatmapData == null)
      return;
    this._beatmapData.beatmapEventDataWasInsertedEvent -= new System.Action<BeatmapEventData, LinkedListNode<BeatmapDataItem>>(this.HandleBeatmapEventDataWasInserted);
    this._beatmapData.beatmapEventDataWillBeRemovedEvent -= new System.Action<BeatmapEventData, LinkedListNode<BeatmapDataItem>>(this.HandleBeatmapEventDataWillBeRemoved);
    this._beatmapData.beatmapEventDataWasRemovedEvent -= new System.Action<BeatmapEventData>(this.HandleBeatmapEventDataWasRemoved);
  }

  public virtual void ReplayState() => this._callCallbacksBehavior.Replay(this._callbacksInTimes);

  public virtual void ManualUpdate(float songTime)
  {
    if ((double) songTime == (double) this._prevSongTime)
      return;
    this._songTime = songTime;
    this._processingCallbacks = true;
    if ((double) songTime > (double) this._prevSongTime)
    {
      foreach (KeyValuePair<float, CallbacksInTime> callbacksInTime1 in this._callbacksInTimes)
      {
        CallbacksInTime callbacksInTime2 = callbacksInTime1.Value;
        for (LinkedListNode<BeatmapDataItem> linkedListNode = callbacksInTime2.lastProcessedNode != null ? callbacksInTime2.lastProcessedNode.Next : this._beatmapData.allBeatmapDataItems.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
        {
          BeatmapDataItem beatmapDataItem = linkedListNode.Value;
          if ((double) beatmapDataItem.time - (double) callbacksInTime2.aheadTime <= (double) songTime)
          {
            if (beatmapDataItem.type == BeatmapDataItem.BeatmapDataItemType.BeatmapEvent || beatmapDataItem.type == BeatmapDataItem.BeatmapDataItemType.BeatmapObject && (double) beatmapDataItem.time >= (double) this._startFilterTime)
              this._callCallbacksBehavior.CallCallbacks(callbacksInTime2, beatmapDataItem);
            callbacksInTime2.lastProcessedNode = linkedListNode;
          }
          else
            break;
        }
      }
    }
    else
    {
      foreach (KeyValuePair<float, CallbacksInTime> callbacksInTime3 in this._callbacksInTimes)
      {
        CallbacksInTime callbacksInTime4 = callbacksInTime3.Value;
        LinkedListNode<BeatmapDataItem> linkedListNode = callbacksInTime4.lastProcessedNode;
        while (linkedListNode != null)
        {
          BeatmapDataItem beatmapDataItem = linkedListNode.Value;
          if ((double) beatmapDataItem.time - (double) callbacksInTime4.aheadTime > (double) songTime)
          {
            if (beatmapDataItem.type == BeatmapDataItem.BeatmapDataItemType.BeatmapEvent)
            {
              BeatmapEventData nextData = (BeatmapEventData) beatmapDataItem;
              if (nextData.previousSameTypeEventData != null)
              {
                this._callCallbacksBehavior.CallCallbacks(callbacksInTime4, (BeatmapDataItem) nextData.previousSameTypeEventData);
              }
              else
              {
                BeatmapEventData beatmapEventData = nextData.GetDefault(nextData);
                if (beatmapEventData != null)
                  this._callCallbacksBehavior.CallCallbacks(callbacksInTime4, (BeatmapDataItem) beatmapEventData);
              }
            }
            linkedListNode = linkedListNode.Previous;
            callbacksInTime4.lastProcessedNode = linkedListNode;
          }
          else
            break;
        }
      }
    }
    this._prevSongTime = songTime;
    this._processingCallbacks = false;
    System.Action callbacksThisFrameEvent = this.didProcessAllCallbacksThisFrameEvent;
    if (callbacksThisFrameEvent == null)
      return;
    callbacksThisFrameEvent();
  }

  public virtual BeatmapDataCallbackWrapper AddBeatmapCallback<T>(
    float aheadTime,
    BeatmapDataCallback<T> callback)
    where T : BeatmapDataItem
  {
    return this.AddBeatmapCallback<T>(aheadTime, callback, (int[]) null);
  }

  public virtual BeatmapDataCallbackWrapper AddBeatmapCallback<T>(BeatmapDataCallback<T> callback) where T : BeatmapDataItem => this.AddBeatmapCallback<T>(0.0f, callback, (int[]) null);

  public virtual BeatmapDataCallbackWrapper AddBeatmapCallback<T>(
    BeatmapDataCallback<T> callback,
    params int[] beatmapDataSubtypeIdentifiers)
    where T : BeatmapDataItem
  {
    return this.AddBeatmapCallback<T>(0.0f, callback, beatmapDataSubtypeIdentifiers);
  }

  public virtual BeatmapDataCallbackWrapper AddBeatmapCallback<T>(
    float aheadTime,
    BeatmapDataCallback<T> callback,
    params int[] beatmapDataSubtypeIdentifiers)
    where T : BeatmapDataItem
  {
    Assert.IsFalse(this._processingCallbacks);
    CallbacksInTime callbacksInTime;
    if (!this._callbacksInTimes.TryGetValue(aheadTime, out callbacksInTime))
    {
      callbacksInTime = new CallbacksInTime(aheadTime);
      this._callbacksInTimes[aheadTime] = callbacksInTime;
    }
    BeatmapDataCallbackWrapper<T> callbackWrapper = new BeatmapDataCallbackWrapper<T>(callback, aheadTime, beatmapDataSubtypeIdentifiers);
    callbacksInTime.AddCallback((BeatmapDataCallbackWrapper) callbackWrapper);
    return (BeatmapDataCallbackWrapper) callbackWrapper;
  }

  public virtual void RemoveBeatmapCallback(BeatmapDataCallbackWrapper callbackWrapper)
  {
    Assert.IsFalse(this._processingCallbacks);
    if (callbackWrapper == null)
      return;
    float aheadTime = callbackWrapper.aheadTime;
    CallbacksInTime callbacksInTime;
    if (!this._callbacksInTimes.TryGetValue(aheadTime, out callbacksInTime))
      return;
    callbacksInTime.RemoveCallback(callbackWrapper);
    if (!callbacksInTime.isEmpty)
      return;
    this._callbacksInTimes.Remove(aheadTime);
  }

  public virtual void TriggerBeatmapEvent(BeatmapEventData beatmapEventData)
  {
    foreach (KeyValuePair<float, CallbacksInTime> callbacksInTime in this._callbacksInTimes)
      callbacksInTime.Value.CallCallbacks((BeatmapDataItem) beatmapEventData);
  }

  public virtual void HandleBeatmapEventDataWasInserted(
    BeatmapEventData beatmapEventData,
    LinkedListNode<BeatmapDataItem> node)
  {
    Assert.IsFalse(this._processingCallbacks);
    if (!this._sendCallbacksOnBeatmapDataChangeChange)
      return;
    foreach (KeyValuePair<float, CallbacksInTime> callbacksInTime1 in this._callbacksInTimes)
    {
      CallbacksInTime callbacksInTime2 = callbacksInTime1.Value;
      if ((double) beatmapEventData.time - (double) callbacksInTime2.aheadTime <= (double) this._prevSongTime && (beatmapEventData.nextSameTypeEventData == null || (double) beatmapEventData.nextSameTypeEventData.time - (double) callbacksInTime2.aheadTime > (double) this._prevSongTime))
      {
        this._callCallbacksBehavior.CallCallbacks(callbacksInTime2, (BeatmapDataItem) beatmapEventData);
        if (callbacksInTime2.lastProcessedNode == null)
          callbacksInTime2.lastProcessedNode = this._beatmapData.allBeatmapDataItems.First;
        else if (callbacksInTime2.lastProcessedNode.Next != null && callbacksInTime2.lastProcessedNode.Next.Value == beatmapEventData)
          callbacksInTime2.lastProcessedNode = callbacksInTime2.lastProcessedNode.Next;
      }
      else if ((double) beatmapEventData.time - (double) callbacksInTime2.aheadTime > (double) this._prevSongTime && (beatmapEventData.previousSameTypeEventData == null || (double) beatmapEventData.previousSameTypeEventData.time - (double) callbacksInTime2.aheadTime <= (double) this._prevSongTime))
      {
        this._callCallbacksBehavior.CallCallbacks(callbacksInTime2, (BeatmapDataItem) (beatmapEventData.previousSameTypeEventData ?? beatmapEventData.GetDefault(beatmapEventData)));
        if (callbacksInTime2.lastProcessedNode == null)
        {
          float? time = this._beatmapData.allBeatmapDataItems.First?.Value.time;
          float prevSongTime = this._prevSongTime;
          if ((double) time.GetValueOrDefault() < (double) prevSongTime & time.HasValue)
            callbacksInTime2.lastProcessedNode = this._beatmapData.allBeatmapDataItems.First;
        }
      }
    }
  }

  public virtual void HandleBeatmapEventDataWillBeRemoved(
    BeatmapEventData beatmapEventDataToRemove,
    LinkedListNode<BeatmapDataItem> nodeToRemove)
  {
    Assert.IsFalse(this._processingCallbacks);
    foreach (KeyValuePair<float, CallbacksInTime> callbacksInTime1 in this._callbacksInTimes)
    {
      CallbacksInTime callbacksInTime2 = callbacksInTime1.Value;
      callbacksInTime2.beatmapEventDataForCallbacksAfterNodeRemoval = (BeatmapEventData) null;
      if (callbacksInTime2.lastProcessedNode == nodeToRemove)
        callbacksInTime2.lastProcessedNode = nodeToRemove.Previous;
      if ((((double) beatmapEventDataToRemove.time - (double) callbacksInTime2.aheadTime > (double) this._prevSongTime ? 0 : (beatmapEventDataToRemove.nextSameTypeEventData == null ? 1 : ((double) beatmapEventDataToRemove.nextSameTypeEventData.time - (double) callbacksInTime2.aheadTime > (double) this._prevSongTime ? 1 : 0))) | ((double) beatmapEventDataToRemove.time - (double) callbacksInTime2.aheadTime <= (double) this._prevSongTime ? (false ? 1 : 0) : (beatmapEventDataToRemove.previousSameTypeEventData == null ? (true ? 1 : 0) : ((double) beatmapEventDataToRemove.previousSameTypeEventData.time - (double) callbacksInTime2.aheadTime <= (double) this._prevSongTime ? 1 : 0)))) != 0)
        callbacksInTime2.beatmapEventDataForCallbacksAfterNodeRemoval = beatmapEventDataToRemove.previousSameTypeEventData ?? beatmapEventDataToRemove.GetDefault(beatmapEventDataToRemove);
    }
  }

  public virtual void HandleBeatmapEventDataWasRemoved(BeatmapEventData beatmapEventData)
  {
    Assert.IsFalse(this._processingCallbacks);
    foreach (KeyValuePair<float, CallbacksInTime> callbacksInTime1 in this._callbacksInTimes)
    {
      CallbacksInTime callbacksInTime2 = callbacksInTime1.Value;
      if (callbacksInTime2.beatmapEventDataForCallbacksAfterNodeRemoval != null)
        this._callCallbacksBehavior.CallCallbacks(callbacksInTime2, (BeatmapDataItem) callbacksInTime2.beatmapEventDataForCallbacksAfterNodeRemoval);
    }
  }

  public class InitData
  {
    public readonly IReadonlyBeatmapData beatmapData;
    public readonly float startFilterTime;
    public readonly bool shouldKeepReplayState;

    public InitData(
      IReadonlyBeatmapData beatmapData,
      float startFilterTime,
      bool shouldKeepReplayState)
    {
      this.beatmapData = beatmapData;
      this.startFilterTime = startFilterTime;
      this.shouldKeepReplayState = shouldKeepReplayState;
    }
  }

  public interface ICallCallbacksBehavior
  {
    void CallCallbacks(CallbacksInTime callbacksInTime, BeatmapDataItem beatmapDataItem);

    void Replay(
      Dictionary<float, CallbacksInTime> callbacksInTimes);
  }

  public class CallCallbacksBehavior : BeatmapCallbacksController.ICallCallbacksBehavior
  {
    public virtual void CallCallbacks(
      CallbacksInTime callbacksInTime,
      BeatmapDataItem beatmapDataItem)
    {
      callbacksInTime.CallCallbacks(beatmapDataItem);
    }

    public virtual void Replay(
      Dictionary<float, CallbacksInTime> callbacksInTimes)
    {
    }
  }

  public class CallCallbacksBehaviorWithLastState : BeatmapCallbacksController.ICallCallbacksBehavior
  {
    protected readonly Dictionary<(System.Type, int), BeatmapDataItem> _replayState = new Dictionary<(System.Type, int), BeatmapDataItem>();

    public virtual void CallCallbacks(
      CallbacksInTime callbacksInTime,
      BeatmapDataItem beatmapDataItem)
    {
      callbacksInTime.CallCallbacks(beatmapDataItem);
      this._replayState[(beatmapDataItem.GetType(), beatmapDataItem.subtypeIdentifier)] = beatmapDataItem;
    }

    public virtual void Replay(
      Dictionary<float, CallbacksInTime> callbacksInTimes)
    {
      foreach (KeyValuePair<(System.Type, int), BeatmapDataItem> keyValuePair in this._replayState)
      {
        foreach (KeyValuePair<float, CallbacksInTime> callbacksInTime in callbacksInTimes)
          callbacksInTime.Value.CallCallbacks(keyValuePair.Value);
      }
    }
  }
}
