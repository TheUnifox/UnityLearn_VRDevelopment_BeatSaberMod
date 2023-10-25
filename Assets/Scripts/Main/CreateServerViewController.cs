// Decompiled with JetBrains decompiler
// Type: CreateServerViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class CreateServerViewController : ViewController
{
  [SerializeField]
  protected Button _createServerButton;
  [SerializeField]
  protected Button _cancelCreateServerButton;
  [SerializeField]
  protected CreateServerFormController _createServerFormController;
  protected MultiplayerModeSettings _multiplayerModeSettings;

  public event System.Action<bool, CreateServerFormData> didFinishEvent;

  public MultiplayerModeSettings multiplayerModeSettings => this._multiplayerModeSettings;

  public virtual void Setup(MultiplayerModeSettings multiplayerModeSettings)
  {
    this._multiplayerModeSettings = multiplayerModeSettings;
    this._createServerFormController.Setup(multiplayerModeSettings.createServerPlayersCount, false);
    this._createServerButton.interactable = true;
  }

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (!firstActivation)
      return;
    this.buttonBinder.AddBinding(this._createServerButton, (System.Action) (() =>
    {
      System.Action<bool, CreateServerFormData> didFinishEvent = this.didFinishEvent;
      if (didFinishEvent == null)
        return;
      didFinishEvent(true, this.ApplyAndGetData());
    }));
    this.buttonBinder.AddBinding(this._cancelCreateServerButton, (System.Action) (() =>
    {
      System.Action<bool, CreateServerFormData> didFinishEvent = this.didFinishEvent;
      if (didFinishEvent == null)
        return;
      didFinishEvent(false, this.ApplyAndGetData());
    }));
  }

  public virtual CreateServerFormData ApplyAndGetData()
  {
    CreateServerFormData formData = this._createServerFormController.formData;
    this.multiplayerModeSettings.createServerPlayersCount = formData.maxPlayers;
    return formData;
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__10_0()
  {
    System.Action<bool, CreateServerFormData> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(true, this.ApplyAndGetData());
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__10_1()
  {
    System.Action<bool, CreateServerFormData> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(false, this.ApplyAndGetData());
  }
}
