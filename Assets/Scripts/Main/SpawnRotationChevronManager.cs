// Decompiled with JetBrains decompiler
// Type: SpawnRotationChevronManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class SpawnRotationChevronManager : MonoBehaviour
{
  [SerializeField]
  protected float _fadeInTime = 0.3f;
  [SerializeField]
  protected float _fadeOutTime = 0.3f;
  [SerializeField]
  protected float _jumpStartOffsetTime = 0.3f;
  [SerializeField]
  protected float _cutOffsetTime = -0.3f;
  [SerializeField]
  protected AnimationCurve _fadeInLightAmountCurve;
  [SerializeField]
  protected AnimationCurve _fadeOutLightAmountCurve;
  [Inject]
  protected readonly SpawnRotationChevron.Pool _chevronPool;
  [Inject]
  protected readonly IAudioTimeSource _audioTimeSyncController;
  [Inject]
  protected readonly BeatmapObjectSpawnController _beatmapObjectSpawnController;
  [Inject]
  protected readonly BeatmapCallbacksController _beatmapCallbacksController;
  protected BeatmapDataCallbackWrapper _spawnRotationDataCallbackWrapper;
  protected BeatmapDataCallbackWrapper _beatmapObjectDataCallbackWrapper;
  protected Dictionary<int, SpawnRotationChevronManager.DirectionData> _directionToDataDictionary;
  protected HashSet<int> _activeDirections;
  protected List<int> _reusableDirectionsList;
  protected float _moveDuration;
  protected float _halfJumpDuration;
  protected float _currentSpawnRotation;

  public virtual void Start()
  {
    this._directionToDataDictionary = new Dictionary<int, SpawnRotationChevronManager.DirectionData>();
    this._activeDirections = new HashSet<int>();
    this._reusableDirectionsList = new List<int>();
    if (this._beatmapObjectSpawnController.isInitialized)
      this.HandleBeatmapObjectSpawnControllerDidInit();
    else
      this._beatmapObjectSpawnController.didInitEvent += new System.Action(this.HandleBeatmapObjectSpawnControllerDidInit);
  }

  public virtual void Update()
  {
    this._reusableDirectionsList.Clear();
    if (this._activeDirections.Count == 1 && (double) this._audioTimeSyncController.songTime < (double) this._audioTimeSyncController.songEndTime)
    {
      SpawnRotationChevronManager.DirectionData directionToData = this._directionToDataDictionary[this._activeDirections.First<int>()];
      directionToData.fadeInEndTime = Mathf.Min(directionToData.fadeInEndTime, this._audioTimeSyncController.songTime - this._fadeInTime + this._jumpStartOffsetTime);
      directionToData.fadeOutStartTime = Mathf.Max(directionToData.fadeOutStartTime, this._audioTimeSyncController.songTime + this._fadeOutTime + this._jumpStartOffsetTime);
    }
    foreach (int activeDirection in this._activeDirections)
    {
      SpawnRotationChevronManager.DirectionData directionToData = this._directionToDataDictionary[activeDirection];
      if ((double) directionToData.fadeInEndTime < (double) this._audioTimeSyncController.songTime && (double) directionToData.fadeOutStartTime > (double) this._audioTimeSyncController.songTime)
      {
        if (!directionToData.fullyLid)
        {
          directionToData.fullyLid = true;
          directionToData.chevron.SetLightAmount(1f);
        }
      }
      else if ((double) directionToData.fadeInEndTime > (double) this._audioTimeSyncController.songTime)
      {
        float time = Mathf.Clamp01((float) (1.0 - ((double) directionToData.fadeInEndTime - (double) this._audioTimeSyncController.songTime) / (double) this._fadeInTime));
        directionToData.fullyLid = false;
        directionToData.chevron.SetLightAmount(this._fadeInLightAmountCurve.Evaluate(time));
      }
      else if ((double) directionToData.fadeOutStartTime + (double) this._fadeOutTime < (double) this._audioTimeSyncController.songTime)
        this._reusableDirectionsList.Add(activeDirection);
      else if ((double) directionToData.fadeOutStartTime < (double) this._audioTimeSyncController.songTime)
      {
        float time = Mathf.Clamp01((this._audioTimeSyncController.songTime - directionToData.fadeOutStartTime) / this._fadeOutTime);
        directionToData.fullyLid = false;
        directionToData.chevron.SetLightAmount(this._fadeOutLightAmountCurve.Evaluate(time));
      }
    }
    foreach (int reusableDirections in this._reusableDirectionsList)
    {
      SpawnRotationChevronManager.DirectionData directionToData = this._directionToDataDictionary[reusableDirections];
      directionToData.fullyLid = false;
      this._chevronPool.Despawn(directionToData.chevron);
      directionToData.chevron = (SpawnRotationChevron) null;
      this._activeDirections.Remove(reusableDirections);
    }
  }

  public virtual void OnDestroy()
  {
    if (this._beatmapCallbacksController != null)
    {
      this._beatmapCallbacksController.RemoveBeatmapCallback(this._spawnRotationDataCallbackWrapper);
      this._beatmapCallbacksController.RemoveBeatmapCallback(this._beatmapObjectDataCallbackWrapper);
    }
    if (!((UnityEngine.Object) this._beatmapObjectSpawnController != (UnityEngine.Object) null))
      return;
    this._beatmapObjectSpawnController.didInitEvent -= new System.Action(this.HandleBeatmapObjectSpawnControllerDidInit);
  }

  public virtual void HandleBeatmapObjectCallback(BeatmapObjectData beatmapObjectData)
  {
    int key = Mathf.RoundToInt(this._currentSpawnRotation);
    SpawnRotationChevronManager.DirectionData directionData;
    if (this._activeDirections.Contains(key))
    {
      directionData = this._directionToDataDictionary[key];
    }
    else
    {
      SpawnRotationChevron spawnRotationChevron = this._chevronPool.Spawn();
      spawnRotationChevron.transform.rotation = Quaternion.Euler(0.0f, this._currentSpawnRotation, 0.0f);
      spawnRotationChevron.SetLightAmount(0.0f);
      if (!this._directionToDataDictionary.TryGetValue(key, out directionData))
      {
        directionData = this._directionToDataDictionary[key] = new SpawnRotationChevronManager.DirectionData();
        spawnRotationChevron.SetLightAmount(0.0f);
      }
      directionData.chevron = spawnRotationChevron;
      this._activeDirections.Add(key);
    }
    double num1 = (double) beatmapObjectData.time - (double) this._halfJumpDuration;
    float num2 = !(beatmapObjectData is ObstacleData obstacleData) ? beatmapObjectData.time : obstacleData.time + obstacleData.duration;
    double jumpStartOffsetTime = (double) this._jumpStartOffsetTime;
    float num3 = (float) (num1 + jumpStartOffsetTime);
    float b = num2 + this._cutOffsetTime;
    if ((double) num3 - (double) this._fadeInTime > (double) directionData.fadeOutStartTime + (double) this._fadeOutTime)
      directionData.fadeInEndTime = num3;
    directionData.fadeOutStartTime = Mathf.Max(directionData.fadeOutStartTime, b);
  }

  public virtual void HandleSpawnRotationBeatmapEvent(SpawnRotationBeatmapEventData beatmapEventData) => this._currentSpawnRotation = beatmapEventData.rotation;

  public virtual void HandleBeatmapObjectSpawnControllerDidInit()
  {
    float aheadTime = this.ComputeAheadTime();
    this._beatmapObjectDataCallbackWrapper = this._beatmapCallbacksController.AddBeatmapCallback<BeatmapObjectData>(aheadTime, new BeatmapDataCallback<BeatmapObjectData>(this.HandleBeatmapObjectCallback));
    this._spawnRotationDataCallbackWrapper = this._beatmapCallbacksController.AddBeatmapCallback<SpawnRotationBeatmapEventData>(aheadTime, new BeatmapDataCallback<SpawnRotationBeatmapEventData>(this.HandleSpawnRotationBeatmapEvent));
  }

  public virtual float ComputeAheadTime()
  {
    this._moveDuration = this._beatmapObjectSpawnController.moveDuration;
    this._halfJumpDuration = this._beatmapObjectSpawnController.jumpDuration * 0.5f;
    return this._moveDuration + this._halfJumpDuration + this._fadeInTime - this._jumpStartOffsetTime;
  }

  public class DirectionData
  {
    public SpawnRotationChevron chevron;
    public bool fullyLid;
    public float fadeOutStartTime;
    public float fadeInEndTime;
  }
}
