// Decompiled with JetBrains decompiler
// Type: AvatarVisualController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AvatarVisualController : MonoBehaviour
{
  [SerializeField]
  protected MeshFilter _headTopMeshFilter;
  [SerializeField]
  protected MeshFilter _glassesMeshFilter;
  [SerializeField]
  protected MeshFilter _facialHairMeshFilter;
  [SerializeField]
  protected MeshFilter _leftHandsHairMeshFilter;
  [SerializeField]
  protected MeshFilter _rightHandsHairMeshFilter;
  [SerializeField]
  protected SpriteRenderer _eyesSprite;
  [SerializeField]
  protected SpriteRenderer _mouthSprite;
  [SerializeField]
  protected MeshFilter _bodyMeshFilter;
  [Space]
  [SerializeField]
  protected MulticolorAvatarPartPropertyBlockSetter _headTopPropertyBlockColorSetter;
  [SerializeField]
  protected AvatarPropertyBlockColorSetter _glassesPropertyBlockColorSetter;
  [SerializeField]
  protected AvatarPropertyBlockColorSetter _facialHairPropertyBlockColorSetter;
  [SerializeField]
  protected AvatarPropertyBlockColorSetter _skinPropertyBlockColorSetter;
  [SerializeField]
  protected MulticolorAvatarPartPropertyBlockSetter _clothesPropertyBlockSetter;
  [SerializeField]
  protected MulticolorAvatarPartPropertyBlockSetter _leftHandPropertyBlockSetter;
  [SerializeField]
  protected MulticolorAvatarPartPropertyBlockSetter _rightHandPropertyBlockSetter;
  [Inject]
  protected readonly AvatarPartsModel _avatarPartsModel;
  protected Dictionary<EditAvatarViewController.AvatarEditPart, AvatarVisualController.HighlighterDelegate> _avatarPartHighlightSetters;
  protected Color _lightColor = Color.white;
  protected AvatarData _avatarData;
  protected AvatarVisualController.HighlighterDelegate _currentHighlighter;

  public Color lightColor => this._lightColor;

  public virtual void Awake() => this._avatarPartHighlightSetters = new Dictionary<EditAvatarViewController.AvatarEditPart, AvatarVisualController.HighlighterDelegate>()
  {
    [EditAvatarViewController.AvatarEditPart.HeadTopPrimaryColor] = new AvatarVisualController.HighlighterDelegate(this._headTopPropertyBlockColorSetter.SetHighlight),
    [EditAvatarViewController.AvatarEditPart.HeadTopSecondaryColor] = new AvatarVisualController.HighlighterDelegate(this._headTopPropertyBlockColorSetter.SetHighlight),
    [EditAvatarViewController.AvatarEditPart.GlassesColor] = new AvatarVisualController.HighlighterDelegate(this._glassesPropertyBlockColorSetter.SetHighlight),
    [EditAvatarViewController.AvatarEditPart.FacialHairColor] = new AvatarVisualController.HighlighterDelegate(this._facialHairPropertyBlockColorSetter.SetHighlight),
    [EditAvatarViewController.AvatarEditPart.ClothesModelPrimaryColor] = new AvatarVisualController.HighlighterDelegate(this._clothesPropertyBlockSetter.SetHighlight),
    [EditAvatarViewController.AvatarEditPart.ClothesModelSecondaryColor] = new AvatarVisualController.HighlighterDelegate(this._clothesPropertyBlockSetter.SetHighlight),
    [EditAvatarViewController.AvatarEditPart.ClothesModelDetailColor] = new AvatarVisualController.HighlighterDelegate(this._clothesPropertyBlockSetter.SetHighlight),
    [EditAvatarViewController.AvatarEditPart.HandsColor] = new AvatarVisualController.HighlighterDelegate(this.SetHandsHighlight)
  };

  public virtual void UpdateAvatarVisual(AvatarData avatarData)
  {
    this._headTopMeshFilter.mesh = (this._avatarPartsModel.headTopCollection.GetById(avatarData.headTopId) ?? this._avatarPartsModel.headTopCollection.GetDefault()).mesh;
    this._glassesMeshFilter.mesh = (this._avatarPartsModel.glassesCollection.GetById(avatarData.glassesId) ?? this._avatarPartsModel.glassesCollection.GetDefault()).mesh;
    this._facialHairMeshFilter.mesh = (this._avatarPartsModel.facialHairCollection.GetById(avatarData.facialHairId) ?? this._avatarPartsModel.facialHairCollection.GetDefault()).mesh;
    AvatarMeshPartSO avatarMeshPartSo = this._avatarPartsModel.handsCollection.GetById(avatarData.handsId) ?? this._avatarPartsModel.handsCollection.GetDefault();
    this._leftHandsHairMeshFilter.mesh = avatarMeshPartSo.mesh;
    this._rightHandsHairMeshFilter.mesh = avatarMeshPartSo.mesh;
    this._bodyMeshFilter.mesh = (this._avatarPartsModel.clothesCollection.GetById(avatarData.clothesId) ?? this._avatarPartsModel.clothesCollection.GetDefault()).mesh;
    this._eyesSprite.sprite = (this._avatarPartsModel.eyesCollection.GetById(avatarData.eyesId) ?? this._avatarPartsModel.eyesCollection.GetDefault()).sprite;
    this._mouthSprite.sprite = (this._avatarPartsModel.mouthCollection.GetById(avatarData.mouthId) ?? this._avatarPartsModel.mouthCollection.GetDefault()).sprite;
    this._avatarData = avatarData;
    this.UpdateAvatarColors();
  }

  public virtual void SetLightColor(Color color)
  {
    this._lightColor = color;
    this.UpdateAvatarColors();
  }

  public virtual void UpdateAvatarColors()
  {
    if (this._avatarData == null)
      return;
    Color color1 = this._avatarPartsModel.GetSkinColorById(this._avatarData.skinColorId).Color * this._lightColor;
    this._headTopPropertyBlockColorSetter.SetColors(this._avatarData.headTopPrimaryColor * this._lightColor, this._avatarData.headTopSecondaryColor * this._lightColor);
    this._glassesPropertyBlockColorSetter.SetColor(this._avatarData.glassesColor * this._lightColor);
    this._facialHairPropertyBlockColorSetter.SetColor(this._avatarData.facialHairColor * this._lightColor);
    this._clothesPropertyBlockSetter.SetColors(this._avatarData.clothesPrimaryColor * this._lightColor, this._avatarData.clothesSecondaryColor * this._lightColor, this._avatarData.clothesDetailColor * this._lightColor);
    this._skinPropertyBlockColorSetter.SetColor(color1);
    Color color2 = this._avatarData.handsColor * this._lightColor;
    this._leftHandPropertyBlockSetter.SetColors(color1, color2);
    this._rightHandPropertyBlockSetter.SetColors(color1, color2);
  }

  public virtual void HighlightEditedPart(
    EditAvatarViewController.AvatarEditPart editPart,
    int uvSegment)
  {
    AvatarVisualController.HighlighterDelegate highlighterDelegate;
    if (!this._avatarPartHighlightSetters.TryGetValue(editPart, out highlighterDelegate))
      return;
    highlighterDelegate(true, uvSegment);
    this._currentHighlighter = highlighterDelegate;
  }

  public virtual void DisableEditedPartHighlight()
  {
    AvatarVisualController.HighlighterDelegate currentHighlighter = this._currentHighlighter;
    if (currentHighlighter != null)
      currentHighlighter(false, -1);
    this._currentHighlighter = (AvatarVisualController.HighlighterDelegate) null;
  }

  public virtual void SetHandsHighlight(bool highlighted, int uvSegment)
  {
    this._leftHandPropertyBlockSetter.SetHighlight(true, uvSegment);
    this._rightHandPropertyBlockSetter.SetHighlight(true, uvSegment);
  }

  public delegate void HighlighterDelegate(bool highlighted, int uvSegmentNumber);
}
