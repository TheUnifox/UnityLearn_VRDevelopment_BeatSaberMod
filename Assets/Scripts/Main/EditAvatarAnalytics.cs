// Decompiled with JetBrains decompiler
// Type: EditAvatarAnalytics
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EditAvatarAnalytics : MonoBehaviour
{
  [SerializeField]
  protected EditAvatarFlowCoordinator _editAvatarFlowCoordinator;
  [Inject]
  protected readonly EditAvatarViewController _editAvatarViewController;
  [Inject]
  protected readonly IAnalyticsModel _analyticsModel;
  [Inject]
  protected readonly AvatarDataModel _avatarDataModel;
  protected EditAvatarFlowCoordinator.EditAvatarType _lastEditAvatarType;

  public virtual void Awake()
  {
    this._editAvatarFlowCoordinator.wasSetupEvent += new System.Action<EditAvatarFlowCoordinator.EditAvatarType>(this.HandleEditAvatarFlowCoordinatorWasSetup);
    this._editAvatarViewController.randomizeAllButtonWasPressedEvent += new System.Action(this.HandleEditAvatarViewControllerRandomizeAllButtonWasPressed);
    this._editAvatarViewController.didFinishEvent += new System.Action<EditAvatarViewController.FinishAction>(this.HandleEditAvatarViewControllerDidFinish);
  }

  public virtual void OnDestroy()
  {
    if ((UnityEngine.Object) this._editAvatarFlowCoordinator != (UnityEngine.Object) null)
      this._editAvatarFlowCoordinator.wasSetupEvent -= new System.Action<EditAvatarFlowCoordinator.EditAvatarType>(this.HandleEditAvatarFlowCoordinatorWasSetup);
    if (!((UnityEngine.Object) this._editAvatarViewController != (UnityEngine.Object) null))
      return;
    this._editAvatarViewController.randomizeAllButtonWasPressedEvent -= new System.Action(this.HandleEditAvatarViewControllerRandomizeAllButtonWasPressed);
    this._editAvatarViewController.didFinishEvent -= new System.Action<EditAvatarViewController.FinishAction>(this.HandleEditAvatarViewControllerDidFinish);
  }

  public virtual void HandleEditAvatarFlowCoordinatorWasSetup(
    EditAvatarFlowCoordinator.EditAvatarType editAvatarType)
  {
    this._lastEditAvatarType = editAvatarType;
    this._analyticsModel.LogEditAvatarEvent("avatar_start_session", this.CreateEditAvatarEventData());
  }

  public virtual void HandleEditAvatarViewControllerDidFinish(
    EditAvatarViewController.FinishAction finishAction)
  {
    Dictionary<string, string> editAvatarEventData = this.CreateEditAvatarEventData();
    this._analyticsModel.LogEditAvatarEvent(finishAction == EditAvatarViewController.FinishAction.Apply ? "avatar_apply" : "avatar_cancel", editAvatarEventData);
  }

  public virtual void HandleEditAvatarViewControllerRandomizeAllButtonWasPressed() => this._analyticsModel.LogEditAvatarEvent("avatar_randomize", this.CreateEditAvatarEventData());

  public virtual Dictionary<string, string> CreateEditAvatarEventData() => new Dictionary<string, string>()
  {
    {
      "page",
      string.Format("{0}", (object) this._lastEditAvatarType)
    },
    {
      "skin_color",
      this._avatarDataModel.avatarData.skinColorId ?? ""
    },
    {
      "clothes",
      this._avatarDataModel.avatarData.clothesId ?? ""
    },
    {
      "eyes",
      this._avatarDataModel.avatarData.eyesId ?? ""
    },
    {
      "hands",
      this._avatarDataModel.avatarData.handsId ?? ""
    },
    {
      "head_top",
      this._avatarDataModel.avatarData.headTopId ?? ""
    }
  };
}
