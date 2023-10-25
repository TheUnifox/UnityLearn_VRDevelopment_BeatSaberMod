// Decompiled with JetBrains decompiler
// Type: WaypointsTestGameplayManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class WaypointsTestGameplayManager : MonoBehaviour
{
  [SerializeField]
  protected Vector3 _outerCapsuleStart;
  [SerializeField]
  protected Vector3 _outerCapsuleEnd;
  [SerializeField]
  protected float _outerCapsuleRadius = 0.22f;
  [Space]
  [SerializeField]
  protected Vector3 _innerSphereOffset;
  [SerializeField]
  protected float _innerSphereRadius = 0.1f;
  [Space]
  [SerializeField]
  protected LayerMask _layersToColliderWith;
  [Inject]
  protected readonly BTSCharacterSpawnController _characterSpawnController;
  [Inject]
  protected readonly IAudioTimeSource _audioTimeSource;
  [Inject]
  protected readonly StandardGameplaySceneSetupData _standardSceneSetupData;
  [Inject]
  protected readonly GameplayCoreSceneSetupData _levelSceneSetupData;
  protected Transform _target;
  protected bool _firstPosSaved;
  protected bool _speedSaved;
  protected Vector3 _lastFramePos;
  protected float _lastFrameSpeed;
  protected float _biggestFrameSpeed;
  protected float _biggestFrameSpeedSongTime;
  protected float _biggestAcceleration;
  protected float _biggestAccelerationSongTime;

  public virtual void Start()
  {
    this._target = this._characterSpawnController.transform;
    Time.captureFramerate = 15;
  }

  public virtual void LateUpdate()
  {
    Vector3 position = this._target.position;
    Quaternion rotation = this._target.rotation;
    if (this._characterSpawnController.isSpawned)
    {
      Vector3 start = rotation * this._outerCapsuleStart + position;
      Vector3 end = rotation * this._outerCapsuleEnd + position;
      if (!this._characterSpawnController.isCharacterVisible)
        end = start;
      if (Physics.CheckSphere(rotation * this._innerSphereOffset + position, this._innerSphereRadius, (int) this._layersToColliderWith))
        Debug.LogError((object) string.Format("Character went trough obstacle or note ({0}-{1}-{2}), bar {3}, ({4}s)", (object) this._levelSceneSetupData.difficultyBeatmap.level.songName, (object) this._levelSceneSetupData.difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic.serializedName, (object) this._levelSceneSetupData.difficultyBeatmap.difficulty, (object) this.GetBar(this._audioTimeSource.songTime), (object) this._audioTimeSource.songTime));
      else if (Physics.CheckCapsule(start, end, this._outerCapsuleRadius, (int) this._layersToColliderWith))
        Debug.LogWarning((object) string.Format("Character touched obstacle or note ({0}-{1}-{2}), bar {3}, ({4}s)", (object) this._levelSceneSetupData.difficultyBeatmap.level.songName, (object) this._levelSceneSetupData.difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic.serializedName, (object) this._levelSceneSetupData.difficultyBeatmap.difficulty, (object) this.GetBar(this._audioTimeSource.songTime), (object) this._audioTimeSource.songTime));
    }
    if (this._firstPosSaved)
    {
      float num1 = (this._lastFramePos - position).magnitude / Time.deltaTime;
      if (this._characterSpawnController.isSpawned && (double) num1 > (double) this._biggestFrameSpeed)
      {
        this._biggestFrameSpeed = num1;
        this._biggestFrameSpeedSongTime = this._audioTimeSource.songTime;
      }
      if (this._speedSaved)
      {
        float num2 = Mathf.Abs(this._lastFrameSpeed - num1) / Time.deltaTime;
        if (this._characterSpawnController.isSpawned && (double) num2 > 10.0)
          Debug.LogError((object) string.Format("({0}-{1}-{2}) Too high acceleration {3}m/s^2, bar {4}, ({5}s)", (object) this._levelSceneSetupData.difficultyBeatmap.level.songName, (object) this._levelSceneSetupData.difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic.serializedName, (object) this._levelSceneSetupData.difficultyBeatmap.difficulty, (object) num2, (object) this.GetBar(this._audioTimeSource.songTime), (object) this._audioTimeSource.songTime));
        if (this._characterSpawnController.isSpawned && (double) num2 > (double) this._biggestAcceleration)
        {
          this._biggestAcceleration = num2;
          this._biggestAccelerationSongTime = this._audioTimeSource.songTime;
        }
      }
      this._speedSaved = true;
      this._lastFrameSpeed = num1;
    }
    this._lastFramePos = position;
    this._firstPosSaved = true;
  }

  public virtual void OnDestroy()
  {
    if (!this._firstPosSaved)
      return;
    Debug.Log((object) string.Format("FINISHED: ({0}-{1}-{2}), max velocity: {3}m/s (bar {4}, {5}s), max acceleration: {6}m/s^2 (bar {7}, {8}s)", (object) this._levelSceneSetupData.difficultyBeatmap.level.songName, (object) this._levelSceneSetupData.difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic.serializedName, (object) this._levelSceneSetupData.difficultyBeatmap.difficulty, (object) this._biggestFrameSpeed, (object) this.GetBar(this._biggestFrameSpeedSongTime), (object) this._biggestFrameSpeedSongTime, (object) this._biggestAcceleration, (object) this.GetBar(this._biggestAccelerationSongTime), (object) this._biggestAccelerationSongTime));
  }

  public virtual string GetBar(float songTime)
  {
    double minutes = (double) songTime.SecondsToMinutes();
    return string.Format("{0}-{1}", (object) Mathf.FloorToInt((float) (minutes * (double) this._standardSceneSetupData.previewBeatmapLevel.beatsPerMinute / 4.0)), (object) (Mathf.FloorToInt((float) minutes * this._standardSceneSetupData.previewBeatmapLevel.beatsPerMinute) % 4 + 1));
  }

  public virtual void OnDrawGizmos()
  {
    if (!((Object) this._target != (Object) null))
      return;
    Vector3 position = this._target.position;
    Quaternion rotation = this._target.rotation;
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(rotation * this._outerCapsuleStart + position, this._outerCapsuleRadius);
    if (this._characterSpawnController.isCharacterVisible)
      Gizmos.DrawWireSphere(rotation * this._outerCapsuleEnd + position, this._outerCapsuleRadius);
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(rotation * this._innerSphereOffset + position, this._innerSphereRadius);
  }
}
