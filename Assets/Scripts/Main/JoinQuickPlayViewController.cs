// Decompiled with JetBrains decompiler
// Type: JoinQuickPlayViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class JoinQuickPlayViewController : ViewController
{
  [SerializeField]
  protected BeatmapDifficultyDropdown _beatmapDifficultyDropdown;
  [SerializeField]
  protected QuickPlaySongPacksDropdown _songPacksDropdown;
  [SerializeField]
  protected Toggle _levelSelectionToggle;
  [SerializeField]
  protected Button _joinButton;
  [SerializeField]
  protected Button _cancelJoinButton;
  protected MultiplayerModeSettings _multiplayerModeSettings;

  public event System.Action<bool> didFinishEvent;

  public MultiplayerModeSettings multiplayerModeSettings => this._multiplayerModeSettings;

  public virtual void Setup(
    QuickPlaySetupData quickPlaySetupData,
    MultiplayerModeSettings multiplayerModeSettings)
  {
    this._multiplayerModeSettings = multiplayerModeSettings;
    if (quickPlaySetupData.hasOverride)
      this._songPacksDropdown.SetOverrideSongPacks(quickPlaySetupData.quickPlayAvailablePacksOverride);
    this._beatmapDifficultyDropdown.SelectCellWithBeatmapDifficultyMask(multiplayerModeSettings.quickPlayBeatmapDifficulty);
    this._songPacksDropdown.SelectCellWithSerializedName(multiplayerModeSettings.quickPlaySongPackMaskSerializedName);
    this._levelSelectionToggle.isOn = !multiplayerModeSettings.quickPlayEnableLevelSelection;
  }

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);
    if (!firstActivation)
      return;
    this.buttonBinder.AddBinding(this._joinButton, (System.Action) (() => this.ButtonPressed(true)));
    this.buttonBinder.AddBinding(this._cancelJoinButton, (System.Action) (() => this.ButtonPressed(false)));
  }

  public virtual void ButtonPressed(bool success)
  {
    this.multiplayerModeSettings.quickPlayBeatmapDifficulty = this._beatmapDifficultyDropdown.GetSelectedBeatmapDifficultyMask();
    this.multiplayerModeSettings.quickPlaySongPackMaskSerializedName = this._songPacksDropdown.GetSelectedSerializedName();
    this.multiplayerModeSettings.quickPlayEnableLevelSelection = true;
    System.Action<bool> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(success);
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__12_0() => this.ButtonPressed(true);

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__12_1() => this.ButtonPressed(false);
}
