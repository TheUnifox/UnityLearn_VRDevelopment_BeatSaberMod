// Decompiled with JetBrains decompiler
// Type: CuttableBySaber
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public abstract class CuttableBySaber : MonoBehaviour
{
  public event CuttableBySaber.WasCutBySaberDelegate wasCutBySaberEvent;

  protected void CallWasCutBySaberEvent(
    Saber saber,
    Vector3 cutPoint,
    Quaternion orientation,
    Vector3 cutDirVec)
  {
    CuttableBySaber.WasCutBySaberDelegate wasCutBySaberEvent = this.wasCutBySaberEvent;
    if (wasCutBySaberEvent == null)
      return;
    wasCutBySaberEvent(saber, cutPoint, orientation, cutDirVec);
  }

  public abstract bool canBeCut { get; set; }

  public abstract float radius { get; }

  public abstract void Cut(
    Saber saber,
    Vector3 cutPoint,
    Quaternion orientation,
    Vector3 cutDirVec);

  public delegate void WasCutBySaberDelegate(
    Saber saber,
    Vector3 cutPoint,
    Quaternion orientation,
    Vector3 cutDirVec);
}
