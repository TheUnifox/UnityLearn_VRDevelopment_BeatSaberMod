// Decompiled with JetBrains decompiler
// Type: MaterialLightWithIds
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using UnityEngine;

public class MaterialLightWithIds : RuntimeLightWithIds
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
  [DoesNotRequireDomainReloadInit]
  protected static MaterialPropertyBlock _materialPropertyBlock;
  protected Color _color;
  protected float _alpha;
  protected int _propertyId;

  protected override void Awake()
  {
    base.Awake();
    this._propertyId = Shader.PropertyToID(this._colorProperty);
    if (this._setColorOnly)
      this._alpha = this._meshRenderer.sharedMaterial.GetColor(this._propertyId).a;
    if (!this._setAlphaOnly)
      return;
    this._color = this._meshRenderer.sharedMaterial.GetColor(this._propertyId);
  }

  protected override void ColorWasSet(Color color)
  {
    if (MaterialLightWithIds._materialPropertyBlock == null)
      MaterialLightWithIds._materialPropertyBlock = new MaterialPropertyBlock();
    if (this._setAlphaOnly)
      this._color.a = color.a;
    else
      this._color = this._alphaIntoColor ? new Color(color.a, color.a, color.a) : color;
    if (this._setColorOnly)
      this._color.a = this._alpha;
    MaterialLightWithIds._materialPropertyBlock.Clear();
    MaterialLightWithIds._materialPropertyBlock.SetColor(this._propertyId, this._color);
    this._meshRenderer.SetPropertyBlock(MaterialLightWithIds._materialPropertyBlock);
  }
}
