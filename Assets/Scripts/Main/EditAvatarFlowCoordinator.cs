// Decompiled with JetBrains decompiler
// Type: EditAvatarFlowCoordinator
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EditAvatarFlowCoordinator : FlowCoordinator
{
  [SerializeField]
  protected AvatarVisualController _avatarVisualController;
  [SerializeField]
  protected AvatarTweenController _avatarTweenController;
  [SerializeField]
  protected GameObject _avatarContainerGameObject;
  [Inject]
  protected readonly EditAvatarViewController _editAvatarViewController;
  [Inject]
  protected readonly EditColorController _editColorViewController;
  [Inject]
  protected readonly AvatarDataModel _avatarDataModel;
  protected Dictionary<EditAvatarViewController.AvatarEditPart, System.Action> _parameterChangedAnimationCallbacks;
  protected EditAvatarFlowCoordinator.EditAvatarType _editAvatarType;

  public event System.Action<EditAvatarFlowCoordinator, EditAvatarFlowCoordinator.EditAvatarType> didFinishEvent;

  public event System.Action<EditAvatarFlowCoordinator.EditAvatarType> wasSetupEvent;

  public virtual void Setup(
    EditAvatarFlowCoordinator.EditAvatarType editAvatarType)
  {
    this._editAvatarType = editAvatarType;
    this._editAvatarViewController.Setup(editAvatarType == EditAvatarFlowCoordinator.EditAvatarType.Create);
    System.Action<EditAvatarFlowCoordinator.EditAvatarType> wasSetupEvent = this.wasSetupEvent;
    if (wasSetupEvent == null)
      return;
    wasSetupEvent(this._editAvatarType);
  }

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    this.SetTitle(this._editAvatarType == EditAvatarFlowCoordinator.EditAvatarType.Create ? Localization.Get("BUTTON_CREATE_AVATAR") : Localization.Get("TITLE_AVATAR"));
    if (firstActivation)
    {
      this._parameterChangedAnimationCallbacks = new Dictionary<EditAvatarViewController.AvatarEditPart, System.Action>();
      this._parameterChangedAnimationCallbacks[EditAvatarViewController.AvatarEditPart.HeadTopModel] = new System.Action(this._avatarTweenController.PopHead);
      this._parameterChangedAnimationCallbacks[EditAvatarViewController.AvatarEditPart.ClothesModel] = new System.Action(this._avatarTweenController.PopClothes);
      this._parameterChangedAnimationCallbacks[EditAvatarViewController.AvatarEditPart.HandsModel] = new System.Action(this._avatarTweenController.PopHands);
      this._parameterChangedAnimationCallbacks[EditAvatarViewController.AvatarEditPart.All] = new System.Action(this._avatarTweenController.PopAll);
    }
    if (addedToHierarchy)
      this.ProvideInitialViewControllers((ViewController) this._editAvatarViewController);
    this._avatarContainerGameObject.SetActive(true);
    this._editAvatarViewController.didRequestColorChangeEvent += new Action<System.Action<Color>, Color, EditAvatarViewController.AvatarEditPart, int>(this.HandleEditAvatarViewControllerDidRequestColorChange);
    this._editAvatarViewController.didChangedAvatarPartEvent += new System.Action<EditAvatarViewController.AvatarEditPart>(this.HandleEditAvatarViewControllerChangedAvatarPart);
    this._editAvatarViewController.didFinishEvent += new System.Action<EditAvatarViewController.FinishAction>(this.HandleEditAvatarViewControllerDidFinished);
    this._editColorViewController.didChangeColorEvent += new System.Action<Color>(this.HandleEditColorViewControllerDidChangedColor);
    this._editColorViewController.didFinishEvent += new System.Action<bool>(this.HandleEditColorViewControllerDidFinish);
    this._editAvatarViewController.InitHistory();
    this._avatarVisualController.UpdateAvatarVisual(this._avatarDataModel.avatarData);
    this._avatarTweenController.PresentAvatar();
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    this._avatarTweenController.HideAvatar();
    this._editAvatarViewController.didRequestColorChangeEvent -= new Action<System.Action<Color>, Color, EditAvatarViewController.AvatarEditPart, int>(this.HandleEditAvatarViewControllerDidRequestColorChange);
    this._editAvatarViewController.didChangedAvatarPartEvent -= new System.Action<EditAvatarViewController.AvatarEditPart>(this.HandleEditAvatarViewControllerChangedAvatarPart);
    this._editAvatarViewController.didFinishEvent -= new System.Action<EditAvatarViewController.FinishAction>(this.HandleEditAvatarViewControllerDidFinished);
    this._editColorViewController.didChangeColorEvent -= new System.Action<Color>(this.HandleEditColorViewControllerDidChangedColor);
    this._editColorViewController.didFinishEvent -= new System.Action<bool>(this.HandleEditColorViewControllerDidFinish);
  }

  public virtual void HandleEditAvatarViewControllerDidRequestColorChange(
    System.Action<Color> colorCallback,
    Color currentColor,
    EditAvatarViewController.AvatarEditPart editPart,
    int uvSegment)
  {
    this._editColorViewController.SetColor(currentColor);
    this._editColorViewController.SetColorCallback(colorCallback);
    this.PresentViewController((ViewController) this._editColorViewController, animationDirection: ViewController.AnimationDirection.Vertical);
    this._avatarVisualController.HighlightEditedPart(editPart, uvSegment);
  }

  public virtual void HandleEditAvatarViewControllerChangedAvatarPart(
    EditAvatarViewController.AvatarEditPart avatarPart)
  {
    this._avatarVisualController.UpdateAvatarVisual(this._avatarDataModel.avatarData);
    System.Action action;
    this._parameterChangedAnimationCallbacks.TryGetValue(avatarPart, out action);
    if (action == null)
      return;
    action();
  }

  public virtual void HandleEditAvatarViewControllerDidFinished(
    EditAvatarViewController.FinishAction _)
  {
    System.Action<EditAvatarFlowCoordinator, EditAvatarFlowCoordinator.EditAvatarType> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(this, this._editAvatarType);
  }

  public virtual void HandleEditColorViewControllerDidChangedColor(Color color) => this._avatarVisualController.UpdateAvatarVisual(this._avatarDataModel.avatarData);

  public virtual void HandleEditColorViewControllerDidFinish(bool apply)
  {
    this.DismissViewController((ViewController) this._editColorViewController, ViewController.AnimationDirection.Vertical);
    this._avatarVisualController.DisableEditedPartHighlight();
    if (apply)
      return;
    this._editAvatarViewController.DiscardLastEdit();
  }

  public enum EditAvatarType
  {
    Create,
    Edit,
  }
}
