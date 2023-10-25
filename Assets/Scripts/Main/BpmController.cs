// Decompiled with JetBrains decompiler
// Type: BpmController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using Zenject;

public class BpmController : IDisposable, IBpmController
{
  protected readonly BeatmapCallbacksController _beatmapCallbacksController;
  protected readonly BeatmapDataCallbackWrapper _beatmapDataCallback;
  protected float _currentBpm;

  public float currentBpm => this._currentBpm;

  [Inject]
  public BpmController(
    BpmController.InitData initData,
    BeatmapCallbacksController beatmapCallbacksController)
  {
    this._beatmapCallbacksController = beatmapCallbacksController;
    this._beatmapDataCallback = this._beatmapCallbacksController.AddBeatmapCallback<BPMChangeBeatmapEventData>(new BeatmapDataCallback<BPMChangeBeatmapEventData>(this.HandleBpmChangeBeatmapEvent));
    this._currentBpm = initData.startBpm;
  }

  public virtual void Dispose()
  {
    if (this._beatmapCallbacksController == null)
      return;
    this._beatmapCallbacksController.RemoveBeatmapCallback(this._beatmapDataCallback);
  }

  public virtual void HandleBpmChangeBeatmapEvent(
    BPMChangeBeatmapEventData bpmChangeBeatmapEventData)
  {
    this._currentBpm = bpmChangeBeatmapEventData.bpm;
  }

  public class InitData
  {
    public readonly float startBpm;

    public InitData(float startBpm) => this.startBpm = startBpm;
  }
}
