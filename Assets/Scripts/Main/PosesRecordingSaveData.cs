// Decompiled with JetBrains decompiler
// Type: PosesRecordingSaveData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;

[Serializable]
public class PosesRecordingSaveData
{
  public readonly string[] objectIds;
  public readonly PosesRecordingSaveData.TransformsSaveKeyframe[] keyframes;
  public readonly PosesRecordingSaveData.ExternalCameraCalibrationSaveData externalCameraCalibration;

  public PosesRecordingSaveData(
    string[] objectIds,
    PosesRecordingSaveData.TransformsSaveKeyframe[] keyframes,
    PosesRecordingSaveData.ExternalCameraCalibrationSaveData externalCameraCalibration)
  {
    this.objectIds = objectIds;
    this.keyframes = keyframes;
    this.externalCameraCalibration = externalCameraCalibration;
  }

  [Serializable]
  public class PoseSaveData
  {
    public readonly float posX;
    public readonly float posY;
    public readonly float posZ;
    public readonly float rotX;
    public readonly float rotY;
    public readonly float rotZ;
    public readonly float rotW;

    public PoseSaveData(
      float posX,
      float posY,
      float posZ,
      float rotX,
      float rotY,
      float rotZ,
      float rotW)
    {
      this.posX = posX;
      this.posY = posY;
      this.posZ = posZ;
      this.rotX = rotX;
      this.rotY = rotY;
      this.rotZ = rotZ;
      this.rotW = rotW;
    }
  }

  [Serializable]
  public class TransformsSaveKeyframe
  {
    public readonly PosesRecordingSaveData.PoseSaveData[] poses;
    public readonly float time;

    public TransformsSaveKeyframe(PosesRecordingSaveData.PoseSaveData[] poses, float time)
    {
      this.poses = poses;
      this.time = time;
    }
  }

  [Serializable]
  public class ExternalCameraCalibrationSaveData
  {
    public readonly float fieldOfVision;
    public readonly float nearClip;
    public readonly float farClip;
    public readonly float hmdOffset;
    public readonly float nearOffset;

    public ExternalCameraCalibrationSaveData(
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
  }
}
