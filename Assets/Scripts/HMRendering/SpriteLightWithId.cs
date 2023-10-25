// Decompiled with JetBrains decompiler
// Type: SpriteLightWithId
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using UnityEngine;

public class SpriteLightWithId : LightWithIdMonoBehaviour
{
  [SerializeField]
  protected SpriteRenderer _spriteRenderer;
  [Space]
  [SerializeField]
  protected bool _hideIfAlphaOutOfRange;
  [SerializeField]
  [DrawIf("_hideIfAlphaOutOfRange", true, DrawIfAttribute.DisablingType.DontDraw)]
  protected float _hideAlphaRangeMin = 1f / 1000f;
  [SerializeField]
  [DrawIf("_hideIfAlphaOutOfRange", true, DrawIfAttribute.DisablingType.DontDraw)]
  protected float _hideAlphaRangeMax = 1f;
  [Space]
  [SerializeField]
  protected float _intensity = 1f;
  [SerializeField]
  protected float _minAlpha;
  [SerializeField]
  protected SpriteLightWithId.MultiplyColorByAlphaType _multiplyColorByAlpha;
  [SerializeField]
  [DrawIf("_setAlphaOnly", false, DrawIfAttribute.DisablingType.DontDraw)]
  protected bool _setColorOnly;
  [SerializeField]
  [DrawIf("_setColorOnly", false, DrawIfAttribute.DisablingType.DontDraw)]
  protected bool _setAlphaOnly;
  [SerializeField]
  protected bool _setOnlyOnce;

  public Color color => this._spriteRenderer.color;

  public override void ColorWasSet(Color color)
  {
    if (this._multiplyColorByAlpha == SpriteLightWithId.MultiplyColorByAlphaType.BeforeApplyingMinAlpha)
    {
      color.r *= color.a;
      color.g *= color.a;
      color.b *= color.a;
    }
    color.a = this._setColorOnly ? this._spriteRenderer.color.a : Mathf.Max(color.a, this._minAlpha);
    if (this._multiplyColorByAlpha == SpriteLightWithId.MultiplyColorByAlphaType.AfterApplyingMinAlpha)
    {
      color.r *= color.a;
      color.g *= color.a;
      color.b *= color.a;
    }
    this._spriteRenderer.color = !this._setAlphaOnly ? color * this._intensity : this._spriteRenderer.color.ColorWithAlpha(color.a * this._intensity);
    if (this._hideIfAlphaOutOfRange)
      this._spriteRenderer.enabled = (double) color.a >= (double) this._hideAlphaRangeMin && (double) color.a <= (double) this._hideAlphaRangeMax;
    if (!this._setOnlyOnce)
      return;
    this.enabled = false;
  }

  public enum MultiplyColorByAlphaType
  {
    None,
    BeforeApplyingMinAlpha,
    AfterApplyingMinAlpha,
  }
}
