// Decompiled with JetBrains decompiler
// Type: ServerPasswordEntryViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using UnityEngine;

public class ServerPasswordEntryViewController : ViewController
{
  [Header("Password Field")]
  [SerializeField]
  protected InputFieldView _passwordInput;
  [SerializeField]
  protected HMUI.UIKeyboard _uiKeyboard;
  protected INetworkPlayer _selectedNetworkPlayer;

  public event System.Action<INetworkPlayer, string> didFinishEvent;

  public virtual void Setup(INetworkPlayer selectedPlayer = null) => this._selectedNetworkPlayer = selectedPlayer;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    this._passwordInput.ClearInput();
    if (!addedToHierarchy)
      return;
    this._uiKeyboard.okButtonWasPressedEvent += new System.Action(this.HandleUIKeyboardOkButtonWasPressed);
    this._passwordInput.ActivateKeyboard(this._uiKeyboard);
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    if (!removedFromHierarchy)
      return;
    this._uiKeyboard.okButtonWasPressedEvent -= new System.Action(this.HandleUIKeyboardOkButtonWasPressed);
    this._passwordInput.DeactivateKeyboard(this._uiKeyboard);
  }

  public virtual void HandleJoinClicked()
  {
    System.Action<INetworkPlayer, string> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(this._selectedNetworkPlayer, this._passwordInput.text);
  }

  public virtual void HandleUIKeyboardOkButtonWasPressed()
  {
    System.Action<INetworkPlayer, string> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(this._selectedNetworkPlayer, this._passwordInput.text);
  }
}
