// Decompiled with JetBrains decompiler
// Type: PosesRecorder
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

[DefaultExecutionOrder(30400)]
public class PosesRecorder : MonoBehaviour
{
  [Inject]
  protected readonly AudioTimeSyncController _audioTimeSyncController;
  protected Transform[] _transforms;
  protected PosesRecordingData _data;

  public PosesRecordingData data => this._data;

  public virtual void LateUpdate() => this.RecordTick(this._audioTimeSyncController.songTime);

  public virtual void Init(
    PoseObject[] poseObjects,
    PosesRecordingData.ExternalCameraCalibration externalCameraCalibration)
  {
    this._transforms = ((IEnumerable<PoseObject>) poseObjects).Select<PoseObject, Transform>((Func<PoseObject, Transform>) (ro => ro.objectTransform)).ToArray<Transform>();
    this._data = new PosesRecordingData(((IEnumerable<PoseObject>) poseObjects).Select<PoseObject, string>((Func<PoseObject, string>) (ro => ro.id)).ToArray<string>(), externalCameraCalibration);
  }

  public virtual void StartRecording() => this.enabled = true;

  public virtual void RecordTick(float time)
  {
    Pose[] poses = new Pose[this._transforms.Length];
    for (int index = 0; index < this._transforms.Length; ++index)
      poses[index] = new Pose(this._transforms[index].position, this._transforms[index].rotation);
    this._data.AddKeyframe(new PosesRecordingData.TransformsKeyframe(poses, time));
  }

  public virtual void StopRecording() => this.enabled = false;
}
