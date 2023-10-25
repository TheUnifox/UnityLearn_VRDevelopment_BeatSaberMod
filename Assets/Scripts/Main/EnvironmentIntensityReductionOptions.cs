// Decompiled with JetBrains decompiler
// Type: EnvironmentIntensityReductionOptions
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;

[Serializable]
public class EnvironmentIntensityReductionOptions
{
  [SerializeField]
  protected EnvironmentIntensityReductionOptions.CompressExpandReductionType _compressExpand;
  [SerializeField]
  protected EnvironmentIntensityReductionOptions.RotateRingsReductionType _rotateRings;

  public EnvironmentIntensityReductionOptions.CompressExpandReductionType compressExpand => this._compressExpand;

  public EnvironmentIntensityReductionOptions.RotateRingsReductionType rotateRings => this._rotateRings;

  public enum CompressExpandReductionType
  {
    Keep,
    RemoveWithStrobeFilter,
  }

  public enum RotateRingsReductionType
  {
    Keep,
    RemoveWithStrobeFilter,
  }
}
