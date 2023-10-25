// Decompiled with JetBrains decompiler
// Type: InstancedMaterialLightWithId
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using UnityEngine;

public class InstancedMaterialLightWithId : LightWithIdMonoBehaviour
{
  [SerializeField]
  protected MaterialPropertyBlockColorSetter _materialPropertyBlockColorSetter;
  [SerializeField]
  protected bool _setColorOnly;
  [SerializeField]
  [DrawIf("_setColorOnly", false, DrawIfAttribute.DisablingType.DontDraw)]
  protected float _intensity = 1f;
  [SerializeField]
  [DrawIf("_setColorOnly", false, DrawIfAttribute.DisablingType.DontDraw)]
  protected float _minAlpha;
  [SerializeField]
  protected bool _hdr;
  protected Color _color;
  protected bool _startColorWasSet;

  public override void ColorWasSet(Color newColor)
  {
    if (!this._startColorWasSet)
    {
      this._color = newColor;
      this._startColorWasSet = true;
    }
    this._color = this._setColorOnly ? newColor.ColorWithAlpha(this._color.a) : newColor.ColorWithAlpha(Mathf.Max(this._minAlpha, newColor.a) * this._intensity);
    if (this._hdr)
      this._color *= this._intensity;
    this._materialPropertyBlockColorSetter.SetColor(this._color);
  }
}
