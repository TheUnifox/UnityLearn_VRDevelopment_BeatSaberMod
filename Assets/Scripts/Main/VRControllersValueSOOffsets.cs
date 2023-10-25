// Decompiled with JetBrains decompiler
// Type: VRControllersValueSOOffsets
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class VRControllersValueSOOffsets : VRControllerTransformOffset
{
  [SerializeField]
  protected Vector3SO _positionOffset;
  [SerializeField]
  protected Vector3SO _rotationOffset;
  [SerializeField]
  protected bool _mirror;

  public override Vector3 positionOffset
  {
    get
    {
      if (!this._mirror)
        return this._positionOffset.value;
      Vector3 positionOffset = this._positionOffset.value;
      positionOffset.x = -positionOffset.x;
      return positionOffset;
    }
  }

  public override Vector3 rotationOffset
  {
    get
    {
      if (!this._mirror)
        return this._rotationOffset.value;
      Vector3 rotationOffset = this._rotationOffset.value;
      rotationOffset.y = -rotationOffset.y;
      rotationOffset.z = -rotationOffset.z;
      return rotationOffset;
    }
  }
}
