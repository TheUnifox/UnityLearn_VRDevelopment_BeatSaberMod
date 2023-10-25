// Decompiled with JetBrains decompiler
// Type: EnvironmentSizeData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;

[Serializable]
public class EnvironmentSizeData
{
  [SerializeField]
  protected EnvironmentSizeData.FloorType _floorType;
  [SerializeField]
  protected EnvironmentSizeData.CeilingType _ceilingType;
  [SerializeField]
  protected EnvironmentSizeData.TrackLaneType _trackLaneType;

  public EnvironmentSizeData.FloorType floorType => this._floorType;

  public EnvironmentSizeData.CeilingType ceilingType => this._ceilingType;

  public EnvironmentSizeData.TrackLaneType trackLaneType => this._trackLaneType;

  public enum FloorType
  {
    NoFloor,
    CloseTo0,
  }

  public enum CeilingType
  {
    NoCeiling,
    LowCeiling,
  }

  public enum TrackLaneType
  {
    None,
    Normal,
  }
}
