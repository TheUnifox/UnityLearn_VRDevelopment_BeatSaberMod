// Decompiled with JetBrains decompiler
// Type: BaseMultiplayerStartGameCountdownViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class BaseMultiplayerStartGameCountdownViewController : ViewController
{
  [SerializeField]
  protected BeatmapSelectionView _beatmapSelectionView;
  [SerializeField]
  protected ModifiersSelectionView _modifiersSelectionView;
  [SerializeField]
  protected Toggle _spectateToggle;
  protected readonly ToggleBinder _toggleBinder = new ToggleBinder();

  public event System.Action<bool> playerActiveStateChangedEvent;

  public virtual void SetLevelGameplaySetupData(ILevelGameplaySetupData levelGameplaySetupData)
  {
    this._beatmapSelectionView.SetBeatmap(levelGameplaySetupData.beatmapLevel);
    this._modifiersSelectionView.SetGameplayModifiers(levelGameplaySetupData.gameplayModifiers);
  }

  public virtual void SetLobbyPlayerData(ILobbyPlayerData lobbyPlayerData) => this._spectateToggle.isOn = !lobbyPlayerData.isActive;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    this._toggleBinder.AddBinding(this._spectateToggle, (System.Action<bool>) (value =>
    {
      System.Action<bool> stateChangedEvent = this.playerActiveStateChangedEvent;
      if (stateChangedEvent == null)
        return;
      stateChangedEvent(!value);
    }));
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling) => this._toggleBinder.ClearBindings();

  protected override void OnDestroy() => this._toggleBinder.ClearBindings();

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__9_0(bool value)
  {
    System.Action<bool> stateChangedEvent = this.playerActiveStateChangedEvent;
    if (stateChangedEvent == null)
      return;
    stateChangedEvent(!value);
  }
}
