// Decompiled with JetBrains decompiler
// Type: PosesRecordingData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PosesRecordingData
{
  public readonly string[] objectIds;
  public readonly List<PosesRecordingData.TransformsKeyframe> keyframes;
  public readonly PosesRecordingData.ExternalCameraCalibration externalCameraCalibration;

  public PosesRecordingData(
    string[] objectIds,
    PosesRecordingData.ExternalCameraCalibration externalCameraCalibration)
  {
    this.objectIds = objectIds;
    this.keyframes = new List<PosesRecordingData.TransformsKeyframe>();
    this.externalCameraCalibration = externalCameraCalibration;
  }

  public PosesRecordingData(
    string[] objectIds,
    List<PosesRecordingData.TransformsKeyframe> keyframes,
    PosesRecordingData.ExternalCameraCalibration externalCameraCalibration)
  {
    this.objectIds = objectIds;
    this.keyframes = keyframes;
    this.externalCameraCalibration = externalCameraCalibration;
  }

  public virtual void AddKeyframe(PosesRecordingData.TransformsKeyframe keyframe) => this.keyframes.Add(keyframe);

  public virtual bool Contains(string objectId) => ((IEnumerable<string>) this.objectIds).Contains<string>(objectId);

  public virtual int ObjectIndex(string objectId) => ((IReadOnlyList<string>) this.objectIds).IndexOf<string>(objectId);

  public class TransformsKeyframe
  {
    public readonly Pose[] poses;
    public readonly float time;

    public TransformsKeyframe(Pose[] poses, float time)
    {
      this.poses = poses;
      this.time = time;
    }
  }

  public class ExternalCameraCalibration
  {
    public readonly float fieldOfVision;
    public readonly float nearClip;
    public readonly float farClip;
    public readonly float hmdOffset;
    public readonly float nearOffset;

    public ExternalCameraCalibration(
      float fieldOfVision,
      float nearClip,
      float farClip,
      float hmdOffset,
      float nearOffset)
    {
      this.fieldOfVision = fieldOfVision;
      this.nearClip = nearClip;
      this.farClip = farClip;
      this.hmdOffset = hmdOffset;
      this.nearOffset = nearOffset;
    }

    public ExternalCameraCalibration(Camera camera)
    {
      this.fieldOfVision = camera.fieldOfView;
      this.nearClip = camera.nearClipPlane;
      this.farClip = camera.farClipPlane;
      this.hmdOffset = 0.0f;
      this.nearOffset = 0.0f;
    }
  }
}
