// Decompiled with JetBrains decompiler
// Type: EditAvatarViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using DataModels.PlayerAvatar;
using HMUI;
using Polyglot;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class EditAvatarViewController : ViewController
{
  [SerializeField]
  protected NamedColorListController _skinColorValuePicker;
  [SerializeField]
  protected NamedIntListController _headTopValuePicker;
  [SerializeField]
  protected NamedIntListController _eyesValuePicker;
  [SerializeField]
  protected NamedIntListController _handsValuePicker;
  [SerializeField]
  protected NamedIntListController _clothesValuePicker;
  [Space]
  [SerializeField]
  protected ColorPickerButtonController _headTopPrimaryColorButtonController;
  [SerializeField]
  protected ColorPickerButtonController _headTopSecondaryColorButtonController;
  [SerializeField]
  protected ColorPickerButtonController _handsColorButtonController;
  [SerializeField]
  protected ColorPickerButtonController _clothesColorButtonControllerPrimary;
  [SerializeField]
  protected ColorPickerButtonController _clothesColorButtonControllerSecondary;
  [SerializeField]
  protected ColorPickerButtonController _clothesColorButtonControllerDetail;
  [Space]
  [SerializeField]
  protected Button _randomizeAllButton;
  [SerializeField]
  protected Button _undoButton;
  [SerializeField]
  protected Button _redoButton;
  [SerializeField]
  protected Button _applyButton;
  [SerializeField]
  protected Button _cancelButton;
  [Space]
  [SerializeField]
  protected CurvedTextMeshPro _applyButtonText;
  [SerializeField]
  protected Image _eyesPreviewImage;
  [Inject]
  protected readonly AvatarPartsModel _avatarPartsModel;
  [Inject]
  protected readonly AvatarDataModel _avatarDataModel;
  [Inject]
  protected readonly PlayerDataModel _playerDataModel;
  [LocalizationKey]
  protected const string kEditApplyButtonLocalizationKey = "BUTTON_APPLY";
  [LocalizationKey]
  protected const string kCreateApplyButtonLocalizationKey = "BUTTON_CREATE_AVATAR";
  protected readonly AvatarEditHistory _avatarEditHistory = new AvatarEditHistory();
  protected ValueChangedBinder<int> _intPickerBinder;
  protected EditAvatarViewController.AvatarEditPart _lastEditedPart;

  public event System.Action<EditAvatarViewController.FinishAction> didFinishEvent;

  public event Action<System.Action<Color>, Color, EditAvatarViewController.AvatarEditPart, int> didRequestColorChangeEvent;

  public event System.Action randomizeAllButtonWasPressedEvent;

  public event System.Action<EditAvatarViewController.AvatarEditPart> didChangedAvatarPartEvent;

  public virtual void Setup(bool showAsCreateView) => this._applyButtonText.SetText(Localization.Get(showAsCreateView ? "BUTTON_CREATE_AVATAR" : "BUTTON_APPLY"));

  public virtual void InitHistory()
  {
    this._avatarEditHistory.Clear();
    this._avatarEditHistory.UpdateEditHistory(this._avatarDataModel.avatarData, EditAvatarViewController.AvatarEditPart.All);
    this.UpdateButtons();
  }

  public virtual void DiscardLastEdit() => this._avatarEditHistory.Undo();

  public virtual void Update()
  {
    if (Input.GetKeyDown(KeyCode.R))
      this.HandleRandomizeAllButtonWasPressed();
    if (!Input.GetKeyDown(KeyCode.P))
      return;
    this.HandleUndoButtonWasPressed();
  }

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (firstActivation)
    {
      this.SetupColorButton(this._headTopPrimaryColorButtonController.button, (System.Action<Color>) (color => this._avatarDataModel.avatarData.headTopPrimaryColor = color), (Func<Color>) (() => this._avatarDataModel.avatarData.headTopPrimaryColor), EditAvatarViewController.AvatarEditPart.HeadTopPrimaryColor);
      this.SetupColorButton(this._headTopSecondaryColorButtonController.button, (System.Action<Color>) (color => this._avatarDataModel.avatarData.headTopSecondaryColor = color), (Func<Color>) (() => this._avatarDataModel.avatarData.headTopSecondaryColor), EditAvatarViewController.AvatarEditPart.HeadTopSecondaryColor, 1);
      this.SetupColorButton(this._handsColorButtonController.button, (System.Action<Color>) (color => this._avatarDataModel.avatarData.handsColor = color), (Func<Color>) (() => this._avatarDataModel.avatarData.handsColor), EditAvatarViewController.AvatarEditPart.HandsColor, 1);
      this.SetupColorButton(this._clothesColorButtonControllerPrimary.button, (System.Action<Color>) (color => this._avatarDataModel.avatarData.clothesPrimaryColor = color), (Func<Color>) (() => this._avatarDataModel.avatarData.clothesPrimaryColor), EditAvatarViewController.AvatarEditPart.ClothesModelPrimaryColor);
      this.SetupColorButton(this._clothesColorButtonControllerSecondary.button, (System.Action<Color>) (color => this._avatarDataModel.avatarData.clothesSecondaryColor = color), (Func<Color>) (() => this._avatarDataModel.avatarData.clothesSecondaryColor), EditAvatarViewController.AvatarEditPart.ClothesModelSecondaryColor, 1);
      this.SetupColorButton(this._clothesColorButtonControllerDetail.button, (System.Action<Color>) (color => this._avatarDataModel.avatarData.clothesDetailColor = color), (Func<Color>) (() => this._avatarDataModel.avatarData.clothesDetailColor), EditAvatarViewController.AvatarEditPart.ClothesModelDetailColor, 2);
      this._skinColorValuePicker.Init(this.CreateColorValuePairsForAvatarPartCollection(this._avatarPartsModel.skinColors), this._avatarPartsModel.GetColorIndexById(this._avatarDataModel.avatarData.skinColorId));
      this.buttonBinder.AddBinding(this._randomizeAllButton, new System.Action(this.HandleRandomizeAllButtonWasPressed));
      this.buttonBinder.AddBinding(this._undoButton, new System.Action(this.HandleUndoButtonWasPressed));
      this.buttonBinder.AddBinding(this._redoButton, new System.Action(this.HandleRedoButtonWasPressed));
      this.buttonBinder.AddBinding(this._applyButton, new System.Action(this.HandleApplyButtonWasPressed));
      this.buttonBinder.AddBinding(this._cancelButton, new System.Action(this.HandleCancelButtonWasPressed));
      this._intPickerBinder = new ValueChangedBinder<int>();
      this.SetupValuePicker<AvatarMeshPartSO>(this._avatarPartsModel.headTopCollection, this._headTopValuePicker, (System.Action<string>) (s => this._avatarDataModel.avatarData.headTopId = s), EditAvatarViewController.AvatarEditPart.HeadTopModel);
      this.SetupValuePicker<AvatarMeshPartSO>(this._avatarPartsModel.handsCollection, this._handsValuePicker, (System.Action<string>) (s => this._avatarDataModel.avatarData.handsId = s), EditAvatarViewController.AvatarEditPart.HandsModel);
      this.SetupValuePicker<AvatarMeshPartSO>(this._avatarPartsModel.clothesCollection, this._clothesValuePicker, (System.Action<string>) (s => this._avatarDataModel.avatarData.clothesId = s), EditAvatarViewController.AvatarEditPart.ClothesModel);
      this.SetupValuePicker<AvatarSpritePartSO>(this._avatarPartsModel.eyesCollection, this._eyesValuePicker, new System.Action<string>(this.EyesValuePickerHasChanged), EditAvatarViewController.AvatarEditPart.Unknown);
      this._intPickerBinder.AddBinding((IValueChanger<int>) this._skinColorValuePicker, new System.Action<int>(this.HandleSkinColorDidChanged));
    }
    this.RefreshUi();
  }

  public virtual void RefreshUi()
  {
    this._skinColorValuePicker.SetValue(this._avatarPartsModel.GetColorIndexById(this._avatarDataModel.avatarData.skinColorId));
    this._headTopValuePicker.SetValue(this._avatarPartsModel.headTopCollection.GetIndexById(this._avatarDataModel.avatarData.headTopId));
    this._handsValuePicker.SetValue(this._avatarPartsModel.handsCollection.GetIndexById(this._avatarDataModel.avatarData.handsId));
    this._clothesValuePicker.SetValue(this._avatarPartsModel.clothesCollection.GetIndexById(this._avatarDataModel.avatarData.clothesId));
    this._eyesValuePicker.SetValue(this._avatarPartsModel.eyesCollection.GetIndexById(this._avatarDataModel.avatarData.eyesId));
    this._eyesPreviewImage.sprite = (this._avatarPartsModel.eyesCollection.GetById(this._avatarDataModel.avatarData.eyesId) ?? this._avatarPartsModel.eyesCollection.GetDefault()).sprite;
    this._headTopPrimaryColorButtonController.SetColor(this._avatarDataModel.avatarData.headTopPrimaryColor);
    this._headTopSecondaryColorButtonController.SetColor(this._avatarDataModel.avatarData.headTopSecondaryColor);
    this._handsColorButtonController.SetColor(this._avatarDataModel.avatarData.handsColor);
    this._clothesColorButtonControllerPrimary.SetColor(this._avatarDataModel.avatarData.clothesPrimaryColor);
    this._clothesColorButtonControllerSecondary.SetColor(this._avatarDataModel.avatarData.clothesSecondaryColor);
    this._clothesColorButtonControllerDetail.SetColor(this._avatarDataModel.avatarData.clothesDetailColor);
    this.UpdateButtons();
  }

  public virtual void UpdateButtons()
  {
    this._undoButton.interactable = this._avatarEditHistory.undoAvailable;
    this._redoButton.interactable = this._avatarEditHistory.redoAvailable;
  }

  public virtual void HandleSkinColorDidChanged(int value)
  {
    this._avatarDataModel.avatarData.skinColorId = this._avatarPartsModel.skinColors[value].id;
    System.Action<EditAvatarViewController.AvatarEditPart> changedAvatarPartEvent = this.didChangedAvatarPartEvent;
    if (changedAvatarPartEvent != null)
      changedAvatarPartEvent(EditAvatarViewController.AvatarEditPart.SkinColor);
    this._avatarEditHistory.UpdateEditHistory(this._avatarDataModel.avatarData, EditAvatarViewController.AvatarEditPart.SkinColor);
    this.UpdateButtons();
  }

  public virtual void EyesValuePickerHasChanged(string eyesId)
  {
    this._avatarDataModel.avatarData.eyesId = eyesId;
    this._eyesPreviewImage.sprite = this._avatarPartsModel.eyesCollection.GetById(eyesId).sprite;
  }

  public virtual void HandleUndoButtonWasPressed()
  {
    if (!this._avatarEditHistory.undoAvailable)
      return;
    EditAvatarViewController.AvatarEditPart lastEditedPart = this._avatarEditHistory.lastEditedPart;
    this._avatarEditHistory.Undo();
    this._avatarDataModel.avatarData = this._avatarEditHistory.currentSnapShot.avatarData.Clone();
    System.Action<EditAvatarViewController.AvatarEditPart> changedAvatarPartEvent = this.didChangedAvatarPartEvent;
    if (changedAvatarPartEvent != null)
      changedAvatarPartEvent(lastEditedPart);
    this.RefreshUi();
  }

  public virtual void HandleRedoButtonWasPressed()
  {
    if (!this._avatarEditHistory.redoAvailable)
      return;
    this._avatarEditHistory.Redo();
    this._avatarDataModel.avatarData = this._avatarEditHistory.currentSnapShot.avatarData.Clone();
    System.Action<EditAvatarViewController.AvatarEditPart> changedAvatarPartEvent = this.didChangedAvatarPartEvent;
    if (changedAvatarPartEvent != null)
      changedAvatarPartEvent(this._avatarEditHistory.lastEditedPart);
    this.RefreshUi();
  }

  public virtual void HandleRandomizeAllButtonWasPressed()
  {
    AvatarRandomizer.RandomizeAll(this._avatarDataModel.avatarData, this._avatarPartsModel);
    this.ReportAllChangedAndUpdate();
    System.Action buttonWasPressedEvent = this.randomizeAllButtonWasPressedEvent;
    if (buttonWasPressedEvent == null)
      return;
    buttonWasPressedEvent();
  }

  public virtual void HandleRandomizeModelsButtonWasPressed()
  {
    AvatarRandomizer.RandomizeModels(this._avatarDataModel.avatarData, this._avatarPartsModel);
    this.ReportAllChangedAndUpdate();
  }

  public virtual void HandleRandomizeColorsButtonWasPressed()
  {
    AvatarRandomizer.RandomizeColors(this._avatarDataModel.avatarData);
    this.ReportAllChangedAndUpdate();
  }

  public virtual void ReportAllChangedAndUpdate()
  {
    System.Action<EditAvatarViewController.AvatarEditPart> changedAvatarPartEvent = this.didChangedAvatarPartEvent;
    if (changedAvatarPartEvent != null)
      changedAvatarPartEvent(EditAvatarViewController.AvatarEditPart.All);
    this._avatarEditHistory.disableNextSnapshotOverride = true;
    this._avatarEditHistory.UpdateEditHistory(this._avatarDataModel.avatarData, EditAvatarViewController.AvatarEditPart.All);
    this.RefreshUi();
  }

  public virtual void HandleApplyButtonWasPressed()
  {
    this.SaveAvatar();
    System.Action<EditAvatarViewController.FinishAction> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(EditAvatarViewController.FinishAction.Apply);
  }

  public virtual void SaveAvatar()
  {
    this._avatarDataModel.Save();
    if (this._playerDataModel.playerData.avatarCreated)
      return;
    this._playerDataModel.playerData.MarkAvatarCreated();
  }

  public virtual void HandleCancelButtonWasPressed()
  {
    this._avatarDataModel.Load();
    System.Action<EditAvatarViewController.FinishAction> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(EditAvatarViewController.FinishAction.Cancel);
  }

  private static NamedIntListController.TextValuePair[] CreateTextValuePairsForAvatarPartCollection<T>(
    AvatarPartCollection<T> partCollection)
    where T : UnityEngine.Object, IAvatarPart
  {
    T[] parts = partCollection.parts;
    NamedIntListController.TextValuePair[] avatarPartCollection = new NamedIntListController.TextValuePair[parts.Length];
    for (int index = 0; index < parts.Length; ++index)
    {
      T obj = parts[index];
      avatarPartCollection[index] = new NamedIntListController.TextValuePair()
      {
        localizationKey = obj.localizedName,
        value = index
      };
    }
    return avatarPartCollection;
  }

  public virtual NamedColorListController.ColorValuePair[] CreateColorValuePairsForAvatarPartCollection(
    SkinColorSO[] colors)
  {
    NamedColorListController.ColorValuePair[] avatarPartCollection = new NamedColorListController.ColorValuePair[colors.Length];
    for (int index = 0; index < colors.Length; ++index)
      avatarPartCollection[index] = new NamedColorListController.ColorValuePair()
      {
        color = colors[index].Color,
        value = index
      };
    return avatarPartCollection;
  }

  public virtual void SetupColorButton(
    Button button,
    System.Action<Color> colorSetter,
    Func<Color> currentColor,
    EditAvatarViewController.AvatarEditPart avatarEditPart,
    int uvSegment = 0)
  {
    this.buttonBinder.AddBinding(button, (System.Action) (() =>
    {
      Action<System.Action<Color>, Color, EditAvatarViewController.AvatarEditPart, int> colorChangeEvent = this.didRequestColorChangeEvent;
      if (colorChangeEvent == null)
        return;
      colorChangeEvent((System.Action<Color>) (c =>
      {
        colorSetter(c);
        this._avatarEditHistory.UpdateEditHistory(this._avatarDataModel.avatarData, avatarEditPart);
        this.UpdateButtons();
      }), currentColor(), avatarEditPart, uvSegment);
    }));
  }

  public virtual void SetupValuePicker<T>(
    AvatarPartCollection<T> partCollection,
    NamedIntListController valuePicker,
    System.Action<string> setIdAction,
    EditAvatarViewController.AvatarEditPart avatarEditPart)
    where T : UnityEngine.Object, IAvatarPart
  {
    valuePicker.InitValues(EditAvatarViewController.CreateTextValuePairsForAvatarPartCollection<T>(partCollection));
    this._intPickerBinder.AddBinding((IValueChanger<int>) valuePicker, (System.Action<int>) (idx =>
    {
      setIdAction(partCollection.GetByIndex(idx).id);
      System.Action<EditAvatarViewController.AvatarEditPart> changedAvatarPartEvent = this.didChangedAvatarPartEvent;
      if (changedAvatarPartEvent != null)
        changedAvatarPartEvent(avatarEditPart);
      this._avatarEditHistory.UpdateEditHistory(this._avatarDataModel.avatarData, avatarEditPart);
      this.UpdateButtons();
    }));
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__44_0(Color color) => this._avatarDataModel.avatarData.headTopPrimaryColor = color;

  [CompilerGenerated]
  public virtual Color m_CDidActivatem_Eb__44_1() => this._avatarDataModel.avatarData.headTopPrimaryColor;

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__44_2(Color color) => this._avatarDataModel.avatarData.headTopSecondaryColor = color;

  [CompilerGenerated]
  public virtual Color m_CDidActivatem_Eb__44_3() => this._avatarDataModel.avatarData.headTopSecondaryColor;

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__44_4(Color color) => this._avatarDataModel.avatarData.handsColor = color;

  [CompilerGenerated]
  public virtual Color m_CDidActivatem_Eb__44_5() => this._avatarDataModel.avatarData.handsColor;

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__44_6(Color color) => this._avatarDataModel.avatarData.clothesPrimaryColor = color;

  [CompilerGenerated]
  public virtual Color m_CDidActivatem_Eb__44_7() => this._avatarDataModel.avatarData.clothesPrimaryColor;

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__44_8(Color color) => this._avatarDataModel.avatarData.clothesSecondaryColor = color;

  [CompilerGenerated]
  public virtual Color m_CDidActivatem_Eb__44_9() => this._avatarDataModel.avatarData.clothesSecondaryColor;

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__44_10(Color color) => this._avatarDataModel.avatarData.clothesDetailColor = color;

  [CompilerGenerated]
  public virtual Color m_CDidActivatem_Eb__44_11() => this._avatarDataModel.avatarData.clothesDetailColor;

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__44_12(string s) => this._avatarDataModel.avatarData.headTopId = s;

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__44_13(string s) => this._avatarDataModel.avatarData.handsId = s;

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__44_14(string s) => this._avatarDataModel.avatarData.clothesId = s;

  public enum FinishAction
  {
    Cancel,
    Apply,
  }

  public enum AvatarEditPart
  {
    Unknown,
    All,
    SkinColor,
    HeadTopModel,
    HeadTopPrimaryColor,
    HeadTopSecondaryColor,
    GlassesModel,
    GlassesColor,
    FacialHairModel,
    FacialHairColor,
    HandsModel,
    HandsColor,
    ClothesModel,
    ClothesModelPrimaryColor,
    ClothesModelSecondaryColor,
    ClothesModelDetailColor,
  }
}
