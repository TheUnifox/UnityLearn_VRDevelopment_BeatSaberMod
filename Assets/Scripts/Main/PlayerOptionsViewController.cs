// Decompiled with JetBrains decompiler
// Type: PlayerOptionsViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerOptionsViewController : ViewController
{
  [SerializeField]
  protected PlayerSettingsPanelController _playerSettingsPanelController;
  [SerializeField]
  protected Button _okButton;
  [Inject]
  protected readonly PlayerDataModel _playerDataModel;

  public event System.Action<ViewController> didFinishEvent;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    this._playerSettingsPanelController.SetLayout(PlayerSettingsPanelController.PlayerSettingsPanelLayout.All);
    this._playerSettingsPanelController.SetData(this._playerDataModel.playerData.playerSpecificSettings);
    this._playerSettingsPanelController.Refresh();
    if (!firstActivation)
      return;
    this.buttonBinder.AddBinding(this._okButton, (System.Action) (() =>
    {
      System.Action<ViewController> didFinishEvent = this.didFinishEvent;
      if (didFinishEvent == null)
        return;
      didFinishEvent((ViewController) this);
    }));
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    if (!((UnityEngine.Object) this._playerDataModel != (UnityEngine.Object) null))
      return;
    this._playerDataModel.playerData.SetPlayerSpecificSettings(this._playerSettingsPanelController.playerSpecificSettings);
    this._playerDataModel.Save();
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__6_0()
  {
    System.Action<ViewController> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent((ViewController) this);
  }
}
