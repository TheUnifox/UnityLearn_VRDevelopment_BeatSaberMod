// Decompiled with JetBrains decompiler
// Type: PlayerHeightDetector
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class PlayerHeightDetector : MonoBehaviour
{
  [Inject]
  protected readonly PlayerTransforms _playerTransforms;
  [Inject]
  protected readonly BeatmapCallbacksController _beatmapCallbacksController;
  [Inject]
  protected readonly IAudioTimeSource _audioTimeSyncController;
  [Inject]
  protected readonly PlayerHeightDetector.InitData _initData;
  protected BeatmapDataCallbackWrapper _beatmapObjectCallbackWrapper;
  protected float _noTopObstaclesStartTime;
  protected float _computedPlayerHeight;
  protected float _changeWeight;
  protected float _lastReportedHeight;

  public event System.Action<float> playerHeightDidChangeEvent;

  public float playerHeight => this._lastReportedHeight;

  public virtual void Start()
  {
    this._computedPlayerHeight = this._initData.startPlayerHeight;
    this._lastReportedHeight = this._computedPlayerHeight;
    this._beatmapObjectCallbackWrapper = this._beatmapCallbacksController.AddBeatmapCallback<ObstacleData>(0.5f, new BeatmapDataCallback<ObstacleData>(this.BeatmapObjectSpawnCallback));
    System.Action<float> heightDidChangeEvent = this.playerHeightDidChangeEvent;
    if (heightDidChangeEvent == null)
      return;
    heightDidChangeEvent(this._lastReportedHeight);
  }

  public virtual void OnDestroy()
  {
    if (this._beatmapCallbacksController == null)
      return;
    this._beatmapCallbacksController.RemoveBeatmapCallback(this._beatmapObjectCallbackWrapper);
  }

  public virtual void LateUpdate()
  {
    float songTime = this._audioTimeSyncController.songTime;
    if ((double) songTime < (double) this._noTopObstaclesStartTime)
      return;
    float b = this._playerTransforms.headPseudoLocalPos.y + this._initData.headPosToPlayerHeightOffset;
    float num = b - this._computedPlayerHeight;
    this._changeWeight += (double) num > 0.0 ? (float) ((double) num * (double) Time.deltaTime * 16.0) : (float) ((double) num * (double) Time.deltaTime * 2.0);
    this._computedPlayerHeight = Mathf.Lerp(this._computedPlayerHeight, b, (float) ((double) Time.deltaTime * (double) Mathf.Abs(this._changeWeight) * 4.0 / ((double) songTime * 0.10000000149011612 + 1.0)));
    this._changeWeight *= 0.9f;
    if ((double) Mathf.Abs(this._lastReportedHeight - this._computedPlayerHeight) <= (double) Mathf.Epsilon)
      return;
    this._lastReportedHeight = this._computedPlayerHeight;
    System.Action<float> heightDidChangeEvent = this.playerHeightDidChangeEvent;
    if (heightDidChangeEvent == null)
      return;
    heightDidChangeEvent(this._computedPlayerHeight);
  }

  public virtual void BeatmapObjectSpawnCallback(ObstacleData obstacleData)
  {
    if (obstacleData.lineLayer != NoteLineLayer.Top)
      return;
    this._noTopObstaclesStartTime = Mathf.Max(this._noTopObstaclesStartTime, (float) ((double) obstacleData.time + (double) obstacleData.duration + 0.5));
  }

  public class InitData
  {
    public readonly float headPosToPlayerHeightOffset;
    public readonly float startPlayerHeight;

    public InitData(float headPosToPlayerHeightOffset, float startPlayerHeight)
    {
      this.startPlayerHeight = startPlayerHeight;
      this.headPosToPlayerHeightOffset = headPosToPlayerHeightOffset;
    }
  }
}
