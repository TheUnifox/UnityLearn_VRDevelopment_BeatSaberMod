// Decompiled with JetBrains decompiler
// Type: MaterialPropertyBlockPositionUpdater
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using UnityEngine;

[ExecuteAlways]
public class MaterialPropertyBlockPositionUpdater : MaterialPropertyBlockAnimator
{
  [Space]
  [SerializeField]
  protected Transform _targetTransform;

  protected override void SetProperty()
  {
    if ((Object) this._targetTransform == (Object) null)
      return;
    this._materialPropertyBlockController.materialPropertyBlock.SetVector(this.propertyId, (Vector4) this._targetTransform.position);
  }
}
