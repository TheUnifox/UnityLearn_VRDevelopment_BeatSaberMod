// Decompiled with JetBrains decompiler
// Type: HorizontalCameraFov
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System;
using UnityEngine;

[RequireComponent(typeof (Camera))]
public class HorizontalCameraFov : MonoBehaviour
{
  public float _horizontalFOV;

  public virtual void Awake() => this.GetComponent<Camera>().fieldOfView = (float) (57.295780181884766 * (2.0 * (double) Mathf.Atan(Mathf.Tan((float) ((double) this._horizontalFOV * (Math.PI / 180.0) * 0.5)) / this.GetComponent<Camera>().aspect)));
}
