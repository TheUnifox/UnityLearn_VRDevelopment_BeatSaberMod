// Decompiled with JetBrains decompiler
// Type: MaterialPropertyBlockFloatCurve
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using UnityEngine;

[ExecuteAlways]
public class MaterialPropertyBlockFloatCurve : MaterialPropertyBlockAnimator
{
  [Space]
  [SerializeField]
  protected AnimationCurve _curve;
  [SerializeField]
  protected float _valueMultiplier = 1f;
  [SerializeField]
  protected float _speedMultiplier = 1f;

  protected override void SetProperty() => this._materialPropertyBlockController.materialPropertyBlock.SetFloat(this.propertyId, this._curve.Evaluate(Time.time * this._speedMultiplier % (float) this._curve.length) * this._valueMultiplier);
}
