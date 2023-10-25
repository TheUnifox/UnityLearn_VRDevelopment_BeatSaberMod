// Decompiled with JetBrains decompiler
// Type: CenterStageScreenController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

public class CenterStageScreenController : MonoBehaviour
{
  [SerializeField]
  protected MenuLightsPresetSO _defaultMenuLightsPreset;
  [SerializeField]
  protected MenuLightsPresetSO _lobbyLightsPreset;
  [SerializeField]
  protected MenuLightsPresetSO _countdownMenuLightsPreset;
  [Space]
  [SerializeField]
  protected BeatmapSelectionView _beatmapSelectionView;
  [SerializeField]
  protected ModifiersSelectionView _modifiersSelectionView;
  [Space]
  [SerializeField]
  protected CountdownController _countdownController;
  [SerializeField]
  protected MultiplayerLobbyCenterScreenLayoutAnimator _multiplayerLobbyCenterScreenLayoutAnimator;
  [Inject]
  protected readonly ILobbyGameStateController _lobbyGameStateController;
  [Inject]
  protected readonly MenuLightsManager _menuLightsManager;
  [CompilerGenerated]
  protected bool m_CcountdownShown;
  protected float _countdownEndTime;

  public bool countdownShown
  {
    get => this.m_CcountdownShown;
    private set => this.m_CcountdownShown = value;
  }

  public virtual void Setup(bool showModifiers) => this._modifiersSelectionView.gameObject.SetActive(showModifiers);

  public virtual void Show()
  {
    this.gameObject.SetActive(true);
    this.ShowLobbyColorPreset(false);
    this._lobbyGameStateController.selectedLevelGameplaySetupDataChangedEvent += new System.Action<ILevelGameplaySetupData>(this.HandleLobbyGameStateControllerSelectedLevelGameplaySetupDataChanged);
    this.SetNextGameplaySetupData(this._lobbyGameStateController.selectedLevelGameplaySetupData);
  }

  public virtual void Hide()
  {
    this.HideCountdown(true);
    this._menuLightsManager.SetColorPreset(this._defaultMenuLightsPreset, false);
    this._lobbyGameStateController.selectedLevelGameplaySetupDataChangedEvent -= new System.Action<ILevelGameplaySetupData>(this.HandleLobbyGameStateControllerSelectedLevelGameplaySetupDataChanged);
    this.gameObject.SetActive(false);
  }

  public virtual void ShowCountdown(float countdownEndTime)
  {
    this.countdownShown = true;
    this._countdownEndTime = countdownEndTime;
    this._countdownController.StartCountdown(this._countdownEndTime);
    this._multiplayerLobbyCenterScreenLayoutAnimator.StartCountdown();
  }

  public virtual void HideCountdown(bool instant = false)
  {
    this.countdownShown = false;
    this._countdownController.StopCountdown();
    this._multiplayerLobbyCenterScreenLayoutAnimator.StopCountdown(instant);
  }

  public virtual void SetCountdownEndTime(float countdownEndTime)
  {
    if (!this.gameObject.activeInHierarchy)
      return;
    this._countdownEndTime = countdownEndTime;
    this._countdownController.UpdateCountdown(this._countdownEndTime);
  }

  public virtual void ShowCountdownColorPreset(bool animated = true) => this._menuLightsManager.SetColorPreset(this._countdownMenuLightsPreset, animated);

  public virtual void ShowLobbyColorPreset(bool animated = true) => this._menuLightsManager.SetColorPreset(this._lobbyLightsPreset, animated);

  public virtual void HandleLobbyGameStateControllerSelectedLevelGameplaySetupDataChanged(
    ILevelGameplaySetupData levelGameplaySetupData)
  {
    this.SetNextGameplaySetupData(levelGameplaySetupData);
  }

  public virtual void SetNextGameplaySetupData(ILevelGameplaySetupData levelGameplaySetupData)
  {
    this._beatmapSelectionView.SetBeatmap(levelGameplaySetupData.beatmapLevel);
    this._modifiersSelectionView.SetGameplayModifiers(levelGameplaySetupData.gameplayModifiers);
  }
}
