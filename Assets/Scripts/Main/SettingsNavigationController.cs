// Decompiled with JetBrains decompiler
// Type: SettingsNavigationController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class SettingsNavigationController : NavigationController
{
  [SerializeField]
  protected Button _okButton;
  [SerializeField]
  protected Button _applyButton;
  [SerializeField]
  protected Button _cancelButton;

  public event System.Action<SettingsNavigationController.FinishAction> didFinishEvent;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (!firstActivation)
      return;
    this.buttonBinder.AddBinding(this._okButton, (System.Action) (() => this.HandleFinishButton(SettingsNavigationController.FinishAction.Ok)));
    this.buttonBinder.AddBinding(this._cancelButton, (System.Action) (() => this.HandleFinishButton(SettingsNavigationController.FinishAction.Cancel)));
    this.buttonBinder.AddBinding(this._applyButton, (System.Action) (() => this.HandleFinishButton(SettingsNavigationController.FinishAction.Apply)));
  }

  public virtual void HandleFinishButton(
    SettingsNavigationController.FinishAction finishAction)
  {
    System.Action<SettingsNavigationController.FinishAction> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(finishAction);
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__7_0() => this.HandleFinishButton(SettingsNavigationController.FinishAction.Ok);

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__7_1() => this.HandleFinishButton(SettingsNavigationController.FinishAction.Cancel);

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__7_2() => this.HandleFinishButton(SettingsNavigationController.FinishAction.Apply);

  public enum FinishAction
  {
    Ok,
    Cancel,
    Apply,
  }
}
