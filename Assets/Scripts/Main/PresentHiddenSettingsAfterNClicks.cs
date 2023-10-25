// Decompiled with JetBrains decompiler
// Type: PresentHiddenSettingsAfterNClicks
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PresentHiddenSettingsAfterNClicks : MonoBehaviour
{
  [SerializeField]
  protected Button _hiddenSettingsButton;
  [SerializeField]
  protected ViewController _hiddenSettingsViewController;
  [SerializeField]
  protected int _numberOfClicksRequired = 5;
  [Inject]
  protected readonly SettingsFlowCoordinator _settingsFlowCoordinator;
  protected int _currentNumberOfClicks;
  protected readonly ButtonBinder _buttonBinder = new ButtonBinder();

  public virtual void OnEnable() => this._buttonBinder.AddBinding(this._hiddenSettingsButton, (System.Action) (() =>
  {
    ++this._currentNumberOfClicks;
    if (this._currentNumberOfClicks < this._numberOfClicksRequired)
      return;
    this._currentNumberOfClicks = 0;
    this._settingsFlowCoordinator.ShowSecretViewController(this._hiddenSettingsViewController);
  }));

  public virtual void OnDisable() => this._buttonBinder.ClearBindings();

  [CompilerGenerated]
  public virtual void m_COnEnablem_Eb__6_0()
  {
    ++this._currentNumberOfClicks;
    if (this._currentNumberOfClicks < this._numberOfClicksRequired)
      return;
    this._currentNumberOfClicks = 0;
    this._settingsFlowCoordinator.ShowSecretViewController(this._hiddenSettingsViewController);
  }
}
