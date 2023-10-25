// Decompiled with JetBrains decompiler
// Type: PosesRecordingInfoSaveData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;

[Serializable]
public class PosesRecordingInfoSaveData
{
  [SerializeField]
  protected string _version;
  [SerializeField]
  protected string[] _objectIds;
  [SerializeField]
  protected PosesRecordingInfoSaveData.ExternalCameraCalibrationSaveData _externalCameraCalibration;
  [SerializeField]
  protected string _dataFileName;
  protected const string kCurrentVersion = "1.0.0";

  public string version => this._version;

  public string[] objectIds => this._objectIds;

  public PosesRecordingInfoSaveData.ExternalCameraCalibrationSaveData externalCameraCalibration => this._externalCameraCalibration;

  public string dataFileName => this._dataFileName;

  public PosesRecordingInfoSaveData(
    string[] objectIds,
    PosesRecordingInfoSaveData.ExternalCameraCalibrationSaveData externalCameraCalibration,
    string dataFileName)
  {
    this._version = "1.0.0";
    this._objectIds = objectIds;
    this._externalCameraCalibration = externalCameraCalibration;
    this._dataFileName = dataFileName;
  }

  [Serializable]
  public class ExternalCameraCalibrationSaveData
  {
    [SerializeField]
    protected float _fieldOfVision;
    [SerializeField]
    protected float _nearClip;
    [SerializeField]
    protected float _farClip;
    [SerializeField]
    protected float _hmdOffset;
    [SerializeField]
    protected float _nearOffset;

    public float fieldOfVision => this._fieldOfVision;

    public float nearClip => this._nearClip;

    public float farClip => this._farClip;

    public float hmdOffset => this._hmdOffset;

    public float nearOffset => this._nearOffset;

    public ExternalCameraCalibrationSaveData(
      float fieldOfVision,
      float nearClip,
      float farClip,
      float hmdOffset,
      float nearOffset)
    {
      this._fieldOfVision = fieldOfVision;
      this._nearClip = nearClip;
      this._farClip = farClip;
      this._hmdOffset = hmdOffset;
      this._nearOffset = nearOffset;
    }
  }
}
