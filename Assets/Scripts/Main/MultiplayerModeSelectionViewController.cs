// Decompiled with JetBrains decompiler
// Type: MultiplayerModeSelectionViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MultiplayerModeSelectionViewController : ViewController
{
  [SerializeField]
  protected Button _quickPlayButton;
  [SerializeField]
  protected Button _gameBrowserButton;
  [SerializeField]
  protected Button _joinWithCodeButton;
  [SerializeField]
  protected Button _createServerButton;
  [Space]
  [SerializeField]
  protected TextMeshProUGUI _maintenanceMessageText;
  [SerializeField]
  protected TextMeshProUGUI _customServerEndPointText;
  [Inject]
  protected readonly INetworkConfig _networkConfig;

  public event System.Action<MultiplayerModeSelectionViewController, MultiplayerModeSelectionViewController.MenuButton> didFinishEvent;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (!firstActivation)
      return;
    if (this._networkConfig is CustomNetworkConfig)
    {
      this._customServerEndPointText.gameObject.SetActive(true);
      this._customServerEndPointText.text = this._networkConfig.masterServerEndPoint.ToString();
    }
    else
      this._customServerEndPointText.gameObject.SetActive(false);
    this.buttonBinder.AddBinding(this._quickPlayButton, (System.Action) (() => this.HandleMenuButton(MultiplayerModeSelectionViewController.MenuButton.QuickPlay)));
    this.buttonBinder.AddBinding(this._createServerButton, (System.Action) (() => this.HandleMenuButton(MultiplayerModeSelectionViewController.MenuButton.CreateServer)));
    this.buttonBinder.AddBinding(this._joinWithCodeButton, (System.Action) (() => this.HandleMenuButton(MultiplayerModeSelectionViewController.MenuButton.JoinWithCode)));
    this.buttonBinder.AddBinding(this._gameBrowserButton, (System.Action) (() => this.HandleMenuButton(MultiplayerModeSelectionViewController.MenuButton.GameBrowser)));
  }

  public virtual void SetData(MultiplayerStatusData multiplayerStatusData)
  {
    if (multiplayerStatusData != null && multiplayerStatusData.status == MultiplayerStatusData.AvailabilityStatus.MaintenanceUpcoming && multiplayerStatusData.maintenanceStartTime > DateTime.UtcNow.ToUnixTime())
    {
      DateTime localTime1 = multiplayerStatusData.maintenanceStartTime.AsUnixTime().ToLocalTime();
      DateTime localTime2 = multiplayerStatusData.maintenanceEndTime.AsUnixTime().ToLocalTime();
      CultureInfo provider = new CultureInfo("en");
      this._maintenanceMessageText.text = Localization.GetFormat("LABEL_MULTIPLAYER_MAINTENANCE_UPCOMING", (object) localTime1.ToString("d MMMM, H:mm (\"GMT\"zzz)", (IFormatProvider) provider), (object) localTime2.ToString("d MMMM, H:mm (\"GMT\"zzz)", (IFormatProvider) provider));
      this._maintenanceMessageText.gameObject.SetActive(true);
    }
    else
      this._maintenanceMessageText.gameObject.SetActive(false);
  }

  public virtual void HandleMenuButton(
    MultiplayerModeSelectionViewController.MenuButton menuButton)
  {
    System.Action<MultiplayerModeSelectionViewController, MultiplayerModeSelectionViewController.MenuButton> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(this, menuButton);
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__11_0() => this.HandleMenuButton(MultiplayerModeSelectionViewController.MenuButton.QuickPlay);

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__11_1() => this.HandleMenuButton(MultiplayerModeSelectionViewController.MenuButton.CreateServer);

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__11_2() => this.HandleMenuButton(MultiplayerModeSelectionViewController.MenuButton.JoinWithCode);

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__11_3() => this.HandleMenuButton(MultiplayerModeSelectionViewController.MenuButton.GameBrowser);

  public enum MenuButton
  {
    QuickPlay,
    CreateServer,
    JoinWithCode,
    GameBrowser,
  }
}
