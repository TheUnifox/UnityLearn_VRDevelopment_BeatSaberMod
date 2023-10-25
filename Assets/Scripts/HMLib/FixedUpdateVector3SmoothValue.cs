// Decompiled with JetBrains decompiler
// Type: FixedUpdateVector3SmoothValue
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;

public class FixedUpdateVector3SmoothValue : FixedUpdateSmoothValue<Vector3>
{
  public FixedUpdateVector3SmoothValue(float smooth)
    : base(smooth)
  {
  }

  protected override Vector3 Interpolate(Vector3 value0, Vector3 value1, float t) => Vector3.LerpUnclamped(value0, value1, t);
}
