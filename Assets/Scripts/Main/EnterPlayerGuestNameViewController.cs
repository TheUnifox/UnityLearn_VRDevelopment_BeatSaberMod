// Decompiled with JetBrains decompiler
// Type: EnterPlayerGuestNameViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnterPlayerGuestNameViewController : ViewController
{
  protected const int kMaxPlayerNameCompoundLength = 40;
  protected const int kMaxShowPlayer = 5;
  [SerializeField]
  [SignalSender]
  protected StringSignal _playerNameWasEnteredSignal;
  [Space]
  [SerializeField]
  protected HMUI.UIKeyboard _uiKeyboard;
  [SerializeField]
  protected InputFieldView _nameInputFieldView;
  [SerializeField]
  protected GuestNameButtonsListItemsList _guestNameButtonsListItemsList;
  [Inject]
  protected readonly PlayerDataModel _playerDataModel;
  protected EnterPlayerGuestNameViewController.FinishDelegate _didFinishCallback;

  public virtual void Init(
    EnterPlayerGuestNameViewController.FinishDelegate didFinishCallback)
  {
    this._didFinishCallback = didFinishCallback;
  }

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (!addedToHierarchy)
      return;
    this._uiKeyboard.okButtonWasPressedEvent += new System.Action(this.OkButtonPressed);
    this._nameInputFieldView.ClearInput();
    this._nameInputFieldView.ActivateKeyboard(this._uiKeyboard);
    List<string> guestPlayerNames = this._playerDataModel.playerData.guestPlayerNames;
    int num = 0;
    int index;
    for (index = 0; index < Mathf.Min(guestPlayerNames.Count, 5); ++index)
    {
      num += guestPlayerNames[index].Length + 1;
      if (num > 40)
        break;
    }
    this._guestNameButtonsListItemsList.SetData(Mathf.Min(guestPlayerNames.Count, index + 1), (UIItemsList<GuestNameButtonsListItem>.DataCallback) ((idx, item) =>
    {
      string guestPlayerName = guestPlayerNames[idx];
      item.nameText = guestPlayerName.Truncate(40, true);
      item.buttonPressed = (System.Action) (() => this._nameInputFieldView.SetText(guestPlayerName));
    }));
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    if (!removedFromHierarchy)
      return;
    this._uiKeyboard.okButtonWasPressedEvent -= new System.Action(this.OkButtonPressed);
    this._nameInputFieldView.DeactivateKeyboard(this._uiKeyboard);
    this._didFinishCallback = (EnterPlayerGuestNameViewController.FinishDelegate) null;
  }

  public virtual void OkButtonPressed()
  {
    string str = this._nameInputFieldView.text.Trim();
    if (string.IsNullOrEmpty(str))
      str = Localization.Get("NO_NAME_PLAYER");
    EnterPlayerGuestNameViewController.FinishDelegate didFinishCallback = this._didFinishCallback;
    if (didFinishCallback != null)
      didFinishCallback(this, str);
    this._didFinishCallback = (EnterPlayerGuestNameViewController.FinishDelegate) null;
    this._playerDataModel.playerData.AddGuestPlayerName(str);
    this._playerNameWasEnteredSignal.Raise(str);
  }

  public delegate void FinishDelegate(
    EnterPlayerGuestNameViewController viewController,
    string playerName);
}
