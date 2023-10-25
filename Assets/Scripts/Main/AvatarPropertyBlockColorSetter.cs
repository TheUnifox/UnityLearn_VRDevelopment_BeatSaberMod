// Decompiled with JetBrains decompiler
// Type: AvatarPropertyBlockColorSetter
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

[ExecuteInEditMode]
public class AvatarPropertyBlockColorSetter : MonoBehaviour
{
  [SerializeField]
  protected Color _defaultColor = Color.white;
  [SerializeField]
  protected float _darkerColorMultiplier = 0.4f;
  [SerializeField]
  protected float _whiteBoost = 0.05f;
  [SerializeField]
  protected bool _editInPlayMode;
  [SerializeField]
  protected Renderer _renderer;
  [DoesNotRequireDomainReloadInit]
  protected static MaterialPropertyBlock _materialPropertyBlock;
  protected Color _rimLightColor;
  protected Color _mainColor;
  protected Color _boostColor;
  protected bool _highlighted;

  public virtual void Awake() => this._boostColor = Color.white * this._whiteBoost;

  public virtual void OnValidate()
  {
    if (Application.isPlaying && !this._editInPlayMode || (Object) this._renderer == (Object) null)
      return;
    this._boostColor = Color.white * this._whiteBoost;
    this.SetColor(this._defaultColor);
  }

  public virtual void SetColor(Color color) => this.SetColors(color * this._darkerColorMultiplier, this._boostColor + color);

  public virtual void SetColors(Color mainColor, Color rimLightColor)
  {
    this._mainColor = mainColor;
    this._rimLightColor = rimLightColor;
    this.UpdateRenderer();
  }

  public virtual void SetHighlight(bool highlighted, int uvSegment)
  {
    this._highlighted = highlighted;
    this.UpdateRenderer();
  }

  public virtual void UpdateRenderer()
  {
    if (AvatarPropertyBlockColorSetter._materialPropertyBlock == null)
      AvatarPropertyBlockColorSetter._materialPropertyBlock = new MaterialPropertyBlock();
    AvatarPropertyBlockColorSetter._materialPropertyBlock.Clear();
    AvatarPropertyBlockColorSetter._materialPropertyBlock.SetColor(AvatarColorPropertyIds.colorPropertyId, this._mainColor);
    AvatarPropertyBlockColorSetter._materialPropertyBlock.SetColor(AvatarColorPropertyIds.rimLightColorPropertyId, this._rimLightColor);
    AvatarPropertyBlockColorSetter._materialPropertyBlock.SetInt(AvatarColorPropertyIds.segmentToHighlightPropertyId, this._highlighted ? 0 : -1);
    this._renderer.SetPropertyBlock(AvatarPropertyBlockColorSetter._materialPropertyBlock);
  }
}
