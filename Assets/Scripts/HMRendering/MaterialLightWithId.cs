// Decompiled with JetBrains decompiler
// Type: MaterialLightWithId
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using UnityEngine;

public class MaterialLightWithId : LightWithIdMonoBehaviour
{
  [SerializeField]
  protected MeshRenderer _meshRenderer;
  [SerializeField]
  protected bool _setAlphaOnly;
  [SerializeField]
  [DrawIf("_setAlphaOnly", false, DrawIfAttribute.DisablingType.DontDraw)]
  protected bool _alphaIntoColor;
  [SerializeField]
  [DrawIf("_setAlphaOnly", false, DrawIfAttribute.DisablingType.DontDraw)]
  protected bool _setColorOnly;
  [SerializeField]
  protected string _colorProperty = "_Color";
  [SerializeField]
  [DrawIf("_setColorOnly", false, DrawIfAttribute.DisablingType.DontDraw)]
  protected float _alphaIntensity = 1f;
  [SerializeField]
  [DrawIf("_setAlphaOnly", false, DrawIfAttribute.DisablingType.DontDraw)]
  protected bool _multiplyColorWithAlpha;
  [SerializeField]
  [DrawIf("_setAlphaOnly", false, DrawIfAttribute.DisablingType.DontDraw)]
  protected bool _multiplyColor;
  [SerializeField]
  [DrawIf("_multiplyColor", true, DrawIfAttribute.DisablingType.DontDraw)]
  protected float _colorMultiplier = 1f;
  [DoesNotRequireDomainReloadInit]
  protected static MaterialPropertyBlock _materialPropertyBlock;
  protected Color _color;
  protected float _alpha;
  protected int _propertyId;

  public Color color => this._color;

  public virtual void Awake()
  {
    this._propertyId = Shader.PropertyToID(this._colorProperty);
    if (this._setColorOnly)
      this._alpha = this._meshRenderer.sharedMaterial.GetColor(this._propertyId).a;
    if (!this._setAlphaOnly)
      return;
    this._color = this._meshRenderer.sharedMaterial.GetColor(this._propertyId);
  }

  public override void ColorWasSet(Color color)
  {
    if (MaterialLightWithId._materialPropertyBlock == null)
      MaterialLightWithId._materialPropertyBlock = new MaterialPropertyBlock();
    color.a *= this._alphaIntensity;
    if (this._setAlphaOnly)
      this._color.a = color.a;
    else
      this._color = this._alphaIntoColor ? new Color(color.a, color.a, color.a) : color;
    if (this._setColorOnly)
      this._color.a = this._alpha;
    float num = 1f;
    if (this._multiplyColorWithAlpha)
      num *= color.a;
    if (this._multiplyColor)
      num *= this._colorMultiplier;
    if (this._multiplyColorWithAlpha || this._multiplyColor)
    {
      this._color.r *= num;
      this._color.g *= num;
      this._color.b *= num;
    }
    MaterialLightWithId._materialPropertyBlock.Clear();
    MaterialLightWithId._materialPropertyBlock.SetColor(this._propertyId, this._color);
    this._meshRenderer.SetPropertyBlock(MaterialLightWithId._materialPropertyBlock);
  }
}
