// Decompiled with JetBrains decompiler
// Type: GameplaySetupViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameplaySetupViewController : ViewController
{
  [SerializeField]
  protected TextSegmentedControl _selectionSegmentedControl;
  [SerializeField]
  protected PlayerSettingsPanelController _playerSettingsPanelController;
  [SerializeField]
  protected GameplayModifiersPanelController _gameplayModifiersPanelController;
  [SerializeField]
  protected EnvironmentOverrideSettingsPanelController _environmentOverrideSettingsPanelController;
  [SerializeField]
  protected ColorsOverrideSettingsPanelController _colorsOverrideSettingsPanelController;
  [SerializeField]
  protected MultiplayerSettingsPanelController _multiplayerSettingsPanelController;
  [Inject]
  protected readonly PlayerDataModel _playerDataModel;
  protected List<GameplaySetupViewController.Panel> _panels;
  protected int _activePanelIdx;
  protected bool _showModifiers;
  protected bool _showEnvironmentOverrideSettings;
  protected bool _showColorSchemesSettings;
  protected bool _showMultiplayer;
  protected bool _shouldRefreshContent;
  protected bool _isInitialized;

  public event System.Action didChangeGameplayModifiersEvent;

  public PlayerSpecificSettings playerSettings => this._playerSettingsPanelController.playerSpecificSettings;

  public GameplayModifiers gameplayModifiers => this._gameplayModifiersPanelController.gameplayModifiers;

  public OverrideEnvironmentSettings environmentOverrideSettings => this._playerDataModel.playerData.overrideEnvironmentSettings;

  public ColorSchemesSettings colorSchemesSettings => this._playerDataModel.playerData.colorSchemesSettings;

  public virtual void Setup(
    bool showModifiers,
    bool showEnvironmentOverrideSettings,
    bool showColorSchemesSettings,
    bool showMultiplayer,
    PlayerSettingsPanelController.PlayerSettingsPanelLayout playerSettingsPanelLayout)
  {
    this.Init();
    this._showModifiers = showModifiers;
    this._showEnvironmentOverrideSettings = showEnvironmentOverrideSettings;
    this._showColorSchemesSettings = showColorSchemesSettings;
    this._showMultiplayer = showMultiplayer;
    this._playerSettingsPanelController.SetLayout(playerSettingsPanelLayout);
    if (this.isActiveAndEnabled)
    {
      this.RefreshContent();
      this._shouldRefreshContent = false;
    }
    else
      this._shouldRefreshContent = true;
  }

  public virtual void Init()
  {
    this._playerSettingsPanelController.SetData(this._playerDataModel.playerData.playerSpecificSettings);
    this._gameplayModifiersPanelController.SetData(this._playerDataModel.playerData.gameplayModifiers);
    this._environmentOverrideSettingsPanelController.SetData(this.environmentOverrideSettings);
    this._colorsOverrideSettingsPanelController.SetData(this.colorSchemesSettings);
  }

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (firstActivation)
    {
      this.Init();
      this._selectionSegmentedControl.didSelectCellEvent += new System.Action<SegmentedControl, int>(this.HandleSelectionSegmentedControlDidSelectCell);
      this._gameplayModifiersPanelController.didChangeGameplayModifiersEvent += new System.Action(this.HandleGameplayModifiersPanelControllerDidChangeGameplayModifiers);
      this._playerSettingsPanelController.didChangePlayerSettingsEvent += new System.Action(this.HandlePlayerSettingsPanelControllerDidChangePlayerSettings);
    }
    if (!(this._shouldRefreshContent | firstActivation))
      return;
    this._shouldRefreshContent = false;
    this.RefreshContent();
  }

  public virtual void OnDisable()
  {
    if (!((UnityEngine.Object) this._playerDataModel != (UnityEngine.Object) null))
      return;
    this._playerDataModel.playerData.SetGameplayModifiers(this.gameplayModifiers);
    this._playerDataModel.playerData.SetPlayerSpecificSettings(this.playerSettings);
    this._playerDataModel.Save();
  }

  public virtual void HandleSelectionSegmentedControlDidSelectCell(
    SegmentedControl segmentedControl,
    int cellIdx)
  {
    this.SetActivePanel(cellIdx);
  }

  public virtual void HandlePlayerSettingsPanelControllerDidChangePlayerSettings() => this._playerDataModel.playerData.SetPlayerSpecificSettings(this.playerSettings);

  public virtual void HandleGameplayModifiersPanelControllerDidChangeGameplayModifiers()
  {
    this._playerDataModel.playerData.SetGameplayModifiers(this.gameplayModifiers);
    System.Action gameplayModifiersEvent = this.didChangeGameplayModifiersEvent;
    if (gameplayModifiersEvent == null)
      return;
    gameplayModifiersEvent();
  }

  public virtual void SetActivePanel(int panelIdx)
  {
    for (int index = 0; index < this._panels.Count; ++index)
      this._panels[index].gameObject.SetActive(index == panelIdx);
    this._activePanelIdx = panelIdx;
    this.RefreshActivePanel();
  }

  public virtual void RefreshContent()
  {
    this._panels = new List<GameplaySetupViewController.Panel>(4);
    if (this._showMultiplayer)
      this._panels.Add(new GameplaySetupViewController.Panel(Localization.Get("MULTIPLAYER"), (IRefreshable) this._multiplayerSettingsPanelController, this._multiplayerSettingsPanelController.gameObject));
    else
      this._multiplayerSettingsPanelController.gameObject.SetActive(false);
    if (this._showModifiers)
      this._panels.Add(new GameplaySetupViewController.Panel(Localization.Get("BUTTON_MODIFIERS"), (IRefreshable) this._gameplayModifiersPanelController, this._gameplayModifiersPanelController.gameObject));
    else
      this._gameplayModifiersPanelController.gameObject.SetActive(false);
    this._panels.Add(new GameplaySetupViewController.Panel(Localization.Get("BUTTON_PLAYER_OPTIONS"), (IRefreshable) this._playerSettingsPanelController, this._playerSettingsPanelController.gameObject));
    if (this._showEnvironmentOverrideSettings)
      this._panels.Add(new GameplaySetupViewController.Panel(Localization.Get("BUTTON_ENVIRONMENTS"), (IRefreshable) this._environmentOverrideSettingsPanelController, this._environmentOverrideSettingsPanelController.gameObject));
    else
      this._environmentOverrideSettingsPanelController.gameObject.SetActive(false);
    if (this._showColorSchemesSettings)
      this._panels.Add(new GameplaySetupViewController.Panel(Localization.Get("BUTTON_COLORS"), (IRefreshable) this._colorsOverrideSettingsPanelController, this._colorsOverrideSettingsPanelController.gameObject));
    else
      this._colorsOverrideSettingsPanelController.gameObject.SetActive(false);
    List<string> texts = new List<string>(this._panels.Count);
    foreach (GameplaySetupViewController.Panel panel in this._panels)
      texts.Add(panel.title);
    this._selectionSegmentedControl.SetTexts((IReadOnlyList<string>) texts);
    this.SetActivePanel(this._selectionSegmentedControl.selectedCellNumber);
  }

  public virtual void RefreshActivePanel()
  {
    if (this._panels == null)
      return;
    this._panels[this._activePanelIdx].refreshable.Refresh();
  }

  public class Panel
  {
    public readonly string title;
    public readonly IRefreshable refreshable;
    public readonly GameObject gameObject;

    public Panel(string title, IRefreshable refreshable, GameObject gameObject)
    {
      this.title = title;
      this.refreshable = refreshable;
      this.gameObject = gameObject;
    }
  }
}
