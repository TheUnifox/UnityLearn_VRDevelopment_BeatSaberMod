// Decompiled with JetBrains decompiler
// Type: ServerCodeEntryViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using UnityEngine;
using UnityEngine.UI;

public class ServerCodeEntryViewController : ViewController
{
  [SerializeField]
  protected HMUI.UIKeyboard _uiKeyboard;
  [SerializeField]
  protected InputFieldView _codeInputField;
  [SerializeField]
  protected Button _joinButton;
  [SerializeField]
  protected Button _cancelButton;
  protected readonly InputFieldViewChangeBinder _inputFieldViewChangeBinder = new InputFieldViewChangeBinder();

  public event System.Action<bool, string> didFinishEvent;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (addedToHierarchy)
    {
      this._codeInputField.ClearInput();
      this._codeInputField.ActivateKeyboard(this._uiKeyboard);
      this._joinButton.interactable = false;
    }
    if (!firstActivation)
      return;
    this.buttonBinder.AddBinding(this._joinButton, new System.Action(this.HandleJoinButtonPressed));
    this.buttonBinder.AddBinding(this._cancelButton, new System.Action(this.HandleCancelButtonPressed));
    this._inputFieldViewChangeBinder.AddBinding(this._codeInputField, new System.Action<InputFieldView>(this.HandleInputFieldChanged));
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    if (!removedFromHierarchy)
      return;
    this._codeInputField.DeactivateKeyboard(this._uiKeyboard);
  }

  public virtual void HandleInputFieldChanged(InputFieldView obj) => this._joinButton.interactable = this._codeInputField.text.Length > 0;

  protected override void OnDestroy()
  {
    base.OnDestroy();
    this._inputFieldViewChangeBinder.ClearBindings();
  }

  public virtual void HandleJoinButtonPressed()
  {
    System.Action<bool, string> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(true, this._codeInputField.text);
  }

  public virtual void HandleCancelButtonPressed()
  {
    System.Action<bool, string> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(false, (string) null);
  }
}
