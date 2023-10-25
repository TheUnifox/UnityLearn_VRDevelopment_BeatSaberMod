// Decompiled with JetBrains decompiler
// Type: BezierCurve
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public readonly struct BezierCurve
{
  public readonly Vector3 p0;
  public readonly Vector3 p1;
  public readonly Vector3 p2;
  public readonly Vector3 p3;

  public BezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
  {
    this.p0 = p0;
    this.p1 = p1;
    this.p2 = p2;
    this.p3 = p3;
  }
}
