// Decompiled with JetBrains decompiler
// Type: SoloModeSelectionViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class SoloModeSelectionViewController : ViewController
{
  [SerializeField]
  protected Button _freePlayModeButton;
  [SerializeField]
  protected Button _oneSaberModeButton;
  [SerializeField]
  protected Button _noArrowsModeButton;
  [SerializeField]
  protected Button _dismissButton;

  public event System.Action<SoloModeSelectionViewController, SoloModeSelectionViewController.MenuType> didFinishEvent;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (!firstActivation)
      return;
    this.buttonBinder.AddBinding(this._freePlayModeButton, (System.Action) (() => this.HandleMenuButton(SoloModeSelectionViewController.MenuType.FreePlayMode)));
    this.buttonBinder.AddBinding(this._oneSaberModeButton, (System.Action) (() => this.HandleMenuButton(SoloModeSelectionViewController.MenuType.OneSaberMode)));
    this.buttonBinder.AddBinding(this._noArrowsModeButton, (System.Action) (() => this.HandleMenuButton(SoloModeSelectionViewController.MenuType.NoArrowsMode)));
    this.buttonBinder.AddBinding(this._dismissButton, (System.Action) (() => this.HandleMenuButton(SoloModeSelectionViewController.MenuType.Back)));
  }

  public virtual void HandleMenuButton(
    SoloModeSelectionViewController.MenuType subMenuType)
  {
    if (this.didFinishEvent == null)
      return;
    this.didFinishEvent(this, subMenuType);
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__8_0() => this.HandleMenuButton(SoloModeSelectionViewController.MenuType.FreePlayMode);

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__8_1() => this.HandleMenuButton(SoloModeSelectionViewController.MenuType.OneSaberMode);

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__8_2() => this.HandleMenuButton(SoloModeSelectionViewController.MenuType.NoArrowsMode);

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__8_3() => this.HandleMenuButton(SoloModeSelectionViewController.MenuType.Back);

  public enum MenuType
  {
    FreePlayMode,
    NoArrowsMode,
    OneSaberMode,
    Back,
  }
}
