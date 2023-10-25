// Decompiled with JetBrains decompiler
// Type: PosesPlayback
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

[DefaultExecutionOrder(-1500)]
public class PosesPlayback : MonoBehaviour
{
  [Inject]
  protected readonly AudioTimeSyncController _audioTimeSyncController;
  protected IBeatSaberLogger _logger;
  protected Transform[] _transforms;
  protected PosesRecordingData _data;
  protected int _keyframeIndex;

  public virtual void Update() => this.PlaybackTick(this._audioTimeSyncController.songTime);

  public virtual void Init(
    PoseObject[] poseObjects,
    PosesRecordingData data,
    IBeatSaberLogger logger)
  {
    this._logger = logger;
    this._transforms = new Transform[data.objectIds.Length];
    for (int index = 0; index < this._transforms.Length; ++index)
    {
      this._transforms[index] = (Transform) null;
      foreach (PoseObject poseObject in poseObjects)
      {
        if (poseObject.id == data.objectIds[index])
        {
          this._transforms[index] = poseObject.objectTransform;
          break;
        }
      }
    }
    this._data = data;
    for (int index = 0; index < this._transforms.Length; ++index)
    {
      if ((Object) this._transforms[index] != (Object) null)
      {
        Pose pose = this._data.keyframes[0].poses[index];
        this._transforms[index].SetPositionAndRotation(pose.position, pose.rotation);
      }
    }
  }

  public virtual void StartPlayback()
  {
    if (this._data != null && this._transforms != null)
      this.enabled = true;
    else
      this._logger.LogError("Playback is not started. PosePlayback is not initialized.");
  }

  public virtual void PlaybackTick(float time)
  {
    while (this._keyframeIndex < this._data.keyframes.Count - 2 && (double) this._data.keyframes[this._keyframeIndex + 1].time < (double) time)
      ++this._keyframeIndex;
    float t = (time - this._data.keyframes[this._keyframeIndex].time) / Mathf.Max(1E-06f, this._data.keyframes[this._keyframeIndex + 1].time - this._data.keyframes[this._keyframeIndex].time);
    for (int index = 0; index < this._transforms.Length; ++index)
    {
      if ((Object) this._transforms[index] != (Object) null)
      {
        Vector3 position = Vector3.Lerp(this._data.keyframes[this._keyframeIndex].poses[index].position, this._data.keyframes[this._keyframeIndex + 1].poses[index].position, t);
        Quaternion rotation = Quaternion.Slerp(this._data.keyframes[this._keyframeIndex].poses[index].rotation, this._data.keyframes[this._keyframeIndex + 1].poses[index].rotation, t);
        this._transforms[index].SetPositionAndRotation(position, rotation);
      }
    }
  }

  public virtual void StopPlayback() => this.enabled = false;
}
