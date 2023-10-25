// Decompiled with JetBrains decompiler
// Type: MulticolorAvatarPartPropertyBlockSetter
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MulticolorAvatarPartPropertyBlockSetter : MonoBehaviour
{
  [SerializeField]
  protected MulticolorAvatarPartPropertyBlockSetter.ColorData[] _colorDataList;
  [Space]
  [SerializeField]
  protected Renderer _renderer;
  [Space]
  [SerializeField]
  protected bool _editInPlayMode;
  [DoesNotRequireDomainReloadInit]
  protected static MaterialPropertyBlock _materialPropertyBlock;
  protected readonly Vector4[] _colors = new Vector4[3];
  protected readonly Vector4[] _rimLightColors = new Vector4[3];
  protected Color[] _boostColors;
  protected bool _highlighted;
  protected int _uvSegment;

  public virtual void OnValidate()
  {
    if (Application.isPlaying && !this._editInPlayMode || (UnityEngine.Object) this._renderer == (UnityEngine.Object) null || this._colorDataList == null)
      return;
    this.SetColors(((IEnumerable<MulticolorAvatarPartPropertyBlockSetter.ColorData>) this._colorDataList).Select<MulticolorAvatarPartPropertyBlockSetter.ColorData, Color>((Func<MulticolorAvatarPartPropertyBlockSetter.ColorData, Color>) (x => x.defaultColor)).ToArray<Color>());
  }

  public virtual void SetColors(params Color[] colors)
  {
    for (int index = 0; index < colors.Length && index < this._colorDataList.Length && index < this._colors.Length; ++index)
    {
      this._colors[index] = (Vector4) (colors[index] * this._colorDataList[index].darkerColorMultiplier).linear;
      this._rimLightColors[index] = (Vector4) (colors[index] + this._colorDataList[index].whiteBoost * Color.white).linear;
    }
    this.UpdateRenderer();
  }

  public virtual void SetHighlight(bool highlighted, int uvSegment)
  {
    this._highlighted = highlighted;
    this._uvSegment = uvSegment;
    this.UpdateRenderer();
  }

  public virtual void UpdateRenderer()
  {
    if (MulticolorAvatarPartPropertyBlockSetter._materialPropertyBlock == null)
      MulticolorAvatarPartPropertyBlockSetter._materialPropertyBlock = new MaterialPropertyBlock();
    MulticolorAvatarPartPropertyBlockSetter._materialPropertyBlock.Clear();
    MulticolorAvatarPartPropertyBlockSetter._materialPropertyBlock.SetVectorArray(AvatarColorPropertyIds.uvColorsPropertyId, this._colors);
    MulticolorAvatarPartPropertyBlockSetter._materialPropertyBlock.SetVectorArray(AvatarColorPropertyIds.uvRimLightColorsPropertyId, this._rimLightColors);
    MulticolorAvatarPartPropertyBlockSetter._materialPropertyBlock.SetInt(AvatarColorPropertyIds.segmentToHighlightPropertyId, this._highlighted ? this._uvSegment : -1);
    this._renderer.SetPropertyBlock(MulticolorAvatarPartPropertyBlockSetter._materialPropertyBlock);
  }

  [Serializable]
  public class ColorData
  {
    [SerializeField]
    protected Color _defaultColor = Color.white;
    [SerializeField]
    protected float _darkerColorMultiplier = 0.4f;
    [SerializeField]
    protected float _whiteBoost = 0.05f;

    public Color defaultColor => this._defaultColor;

    public float darkerColorMultiplier => this._darkerColorMultiplier;

    public float whiteBoost => this._whiteBoost;
  }
}
