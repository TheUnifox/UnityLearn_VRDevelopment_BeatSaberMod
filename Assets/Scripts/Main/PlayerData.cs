// Decompiled with JetBrains decompiler
// Type: PlayerData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class PlayerData
{
  public const int kMaxGuestPlayers = 10;
  public const int kCurrentRegionVersion = 1;
  [CompilerGenerated]
  protected string m_CplayerId;
  [CompilerGenerated]
  protected string m_CplayerName;
  [CompilerGenerated]
  protected bool m_CshouldShowTutorialPrompt;
  [CompilerGenerated]
  protected bool m_CshouldShow360Warning;
  [CompilerGenerated]
  protected bool m_CagreedToEula;
  [CompilerGenerated]
  protected bool m_CdidSelectLanguage;
  [CompilerGenerated]
  protected bool m_CagreedToMultiplayerDisclaimer;
  [CompilerGenerated]
  protected bool m_CavatarCreated;
  [CompilerGenerated]
  protected int m_CdidSelectRegionVersion;
  [CompilerGenerated]
  protected PlayerAgreements m_CplayerAgreements;
  [CompilerGenerated]
  protected BeatmapDifficulty m_ClastSelectedBeatmapDifficulty;
  [CompilerGenerated]
  protected BeatmapCharacteristicSO m_ClastSelectedBeatmapCharacteristic;
  [CompilerGenerated]
  protected GameplayModifiers m_CgameplayModifiers;
  [CompilerGenerated]
  protected PlayerSpecificSettings m_CplayerSpecificSettings;
  [CompilerGenerated]
  protected PracticeSettings m_CpracticeSettings;
  [CompilerGenerated]
  protected PlayerAllOverallStatsData m_CplayerAllOverallStatsData;
  [CompilerGenerated]
  protected List<PlayerLevelStatsData> m_ClevelsStatsData;
  [CompilerGenerated]
  protected List<PlayerMissionStatsData> m_CmissionsStatsData;
  [CompilerGenerated]
  protected List<string> m_CshowedMissionHelpIds;
  [CompilerGenerated]
  protected List<string> m_CguestPlayerNames;
  [CompilerGenerated]
  protected ColorSchemesSettings m_CcolorSchemesSettings;
  [CompilerGenerated]
  protected OverrideEnvironmentSettings m_CoverrideEnvironmentSettings;
  [CompilerGenerated]
  protected HashSet<string> m_CfavoritesLevelIds;
  [CompilerGenerated]
  protected MultiplayerModeSettings m_CmultiplayerModeSettings;
  [CompilerGenerated]
  protected int m_CcurrentDlcPromoDisplayCount;
  [CompilerGenerated]
  protected string m_CcurrentDlcPromoId;

  public string playerId
  {
    get => this.m_CplayerId;
    private set => this.m_CplayerId = value;
  }

  public string playerName
  {
    get => this.m_CplayerName;
    private set => this.m_CplayerName = value;
  }

  public bool shouldShowTutorialPrompt
  {
    get => this.m_CshouldShowTutorialPrompt;
    private set => this.m_CshouldShowTutorialPrompt = value;
  }

  public bool shouldShow360Warning
  {
    get => this.m_CshouldShow360Warning;
    private set => this.m_CshouldShow360Warning = value;
  }

  public bool agreedToEula
  {
    get => this.m_CagreedToEula;
    private set => this.m_CagreedToEula = value;
  }

  public bool didSelectLanguage
  {
    get => this.m_CdidSelectLanguage;
    private set => this.m_CdidSelectLanguage = value;
  }

  public bool agreedToMultiplayerDisclaimer
  {
    get => this.m_CagreedToMultiplayerDisclaimer;
    private set => this.m_CagreedToMultiplayerDisclaimer = value;
  }

  public bool avatarCreated
  {
    get => this.m_CavatarCreated;
    private set => this.m_CavatarCreated = value;
  }

  public int didSelectRegionVersion
  {
    get => this.m_CdidSelectRegionVersion;
    private set => this.m_CdidSelectRegionVersion = value;
  }

  public PlayerAgreements playerAgreements
  {
    get => this.m_CplayerAgreements;
    private set => this.m_CplayerAgreements = value;
  }

  public BeatmapDifficulty lastSelectedBeatmapDifficulty
  {
    get => this.m_ClastSelectedBeatmapDifficulty;
    private set => this.m_ClastSelectedBeatmapDifficulty = value;
  }

  public BeatmapCharacteristicSO lastSelectedBeatmapCharacteristic
  {
    get => this.m_ClastSelectedBeatmapCharacteristic;
    private set => this.m_ClastSelectedBeatmapCharacteristic = value;
  }

  public GameplayModifiers gameplayModifiers
  {
    get => this.m_CgameplayModifiers;
    private set => this.m_CgameplayModifiers = value;
  }

  public PlayerSpecificSettings playerSpecificSettings
  {
    get => this.m_CplayerSpecificSettings;
    private set => this.m_CplayerSpecificSettings = value;
  }

  public PracticeSettings practiceSettings
  {
    get => this.m_CpracticeSettings;
    private set => this.m_CpracticeSettings = value;
  }

  public PlayerAllOverallStatsData playerAllOverallStatsData
  {
    get => this.m_CplayerAllOverallStatsData;
    private set => this.m_CplayerAllOverallStatsData = value;
  }

  public List<PlayerLevelStatsData> levelsStatsData
  {
    get => this.m_ClevelsStatsData;
    private set => this.m_ClevelsStatsData = value;
  }

  public List<PlayerMissionStatsData> missionsStatsData
  {
    get => this.m_CmissionsStatsData;
    private set => this.m_CmissionsStatsData = value;
  }

  public List<string> showedMissionHelpIds
  {
    get => this.m_CshowedMissionHelpIds;
    private set => this.m_CshowedMissionHelpIds = value;
  }

  public List<string> guestPlayerNames
  {
    get => this.m_CguestPlayerNames;
    private set => this.m_CguestPlayerNames = value;
  }

  public ColorSchemesSettings colorSchemesSettings
  {
    get => this.m_CcolorSchemesSettings;
    private set => this.m_CcolorSchemesSettings = value;
  }

  public OverrideEnvironmentSettings overrideEnvironmentSettings
  {
    get => this.m_CoverrideEnvironmentSettings;
    private set => this.m_CoverrideEnvironmentSettings = value;
  }

  public HashSet<string> favoritesLevelIds
  {
    get => this.m_CfavoritesLevelIds;
    private set => this.m_CfavoritesLevelIds = value;
  }

  public MultiplayerModeSettings multiplayerModeSettings
  {
    get => this.m_CmultiplayerModeSettings;
    private set => this.m_CmultiplayerModeSettings = value;
  }

  public int currentDlcPromoDisplayCount
  {
    get => this.m_CcurrentDlcPromoDisplayCount;
    private set => this.m_CcurrentDlcPromoDisplayCount = value;
  }

  public string currentDlcPromoId
  {
    get => this.m_CcurrentDlcPromoId;
    private set => this.m_CcurrentDlcPromoId = value;
  }

  public event System.Action favoriteLevelsSetDidChangeEvent;

  public event System.Action didIncreaseNumberOfGameplaysEvent;

  public PlayerData(
    string playerId,
    string playerName,
    BeatmapCharacteristicSO lastSelectedBeatmapCharacteristic,
    ColorSchemesSettings colorSchemesSettings,
    OverrideEnvironmentSettings overrideEnvironmentSettings)
    : this(playerId, playerName, true, true, false, false, false, false, 0, new PlayerAgreements(), BeatmapDifficulty.Normal, lastSelectedBeatmapCharacteristic, GameplayModifiers.noModifiers, new PlayerSpecificSettings(), new PracticeSettings(), new PlayerAllOverallStatsData(), new List<PlayerLevelStatsData>(), new List<PlayerMissionStatsData>(), new List<string>(), new List<string>(), colorSchemesSettings, overrideEnvironmentSettings, (List<string>) null, new MultiplayerModeSettings(), 0, (string) null)
  {
  }

  public PlayerData(
    string playerId,
    string playerName,
    bool shouldShowTutorialPrompt,
    bool shouldShow360Warning,
    bool agreedToEula,
    bool didSelectLanguage,
    bool agreedToMultiplayerDisclaimer,
    bool avatarCreated,
    int didSelectRegionVersion,
    PlayerAgreements playerAgreements,
    BeatmapDifficulty lastSelectedBeatmapDifficulty,
    BeatmapCharacteristicSO lastSelectedBeatmapCharacteristic,
    GameplayModifiers gameplayModifiers,
    PlayerSpecificSettings playerSpecificSettings,
    PracticeSettings practiceSettings,
    PlayerAllOverallStatsData playerAllOverallStatsData,
    List<PlayerLevelStatsData> levelsStatsData,
    List<PlayerMissionStatsData> missionsStatsData,
    List<string> showedMissionHelpIds,
    List<string> guestPlayerNames,
    ColorSchemesSettings colorSchemesSettings,
    OverrideEnvironmentSettings overrideEnvironmentSettings,
    List<string> favoritesLevelIds,
    MultiplayerModeSettings multiplayerModeSettings,
    int currentDlcPromoDisplayCount,
    string currentDlcPromoId)
  {
    this.playerId = playerId;
    this.playerName = playerName;
    this.shouldShowTutorialPrompt = shouldShowTutorialPrompt;
    this.shouldShow360Warning = shouldShow360Warning;
    this.agreedToEula = agreedToEula;
    this.didSelectLanguage = didSelectLanguage;
    this.agreedToMultiplayerDisclaimer = agreedToMultiplayerDisclaimer;
    this.avatarCreated = avatarCreated;
    this.didSelectRegionVersion = didSelectRegionVersion;
    this.playerAgreements = playerAgreements;
    this.lastSelectedBeatmapDifficulty = lastSelectedBeatmapDifficulty;
    this.lastSelectedBeatmapCharacteristic = lastSelectedBeatmapCharacteristic;
    this.gameplayModifiers = gameplayModifiers;
    this.playerSpecificSettings = playerSpecificSettings;
    this.practiceSettings = practiceSettings;
    this.playerAllOverallStatsData = playerAllOverallStatsData;
    this.levelsStatsData = levelsStatsData;
    this.missionsStatsData = missionsStatsData;
    this.showedMissionHelpIds = showedMissionHelpIds;
    this.guestPlayerNames = guestPlayerNames;
    this.colorSchemesSettings = colorSchemesSettings;
    this.overrideEnvironmentSettings = overrideEnvironmentSettings;
    this.favoritesLevelIds = favoritesLevelIds != null ? new HashSet<string>((IEnumerable<string>) favoritesLevelIds) : new HashSet<string>();
    this.multiplayerModeSettings = multiplayerModeSettings;
    this.currentDlcPromoDisplayCount = currentDlcPromoDisplayCount;
    this.currentDlcPromoId = currentDlcPromoId;
  }

  public virtual void SetNewDlcPromo(string dlcPromoId)
  {
    this.currentDlcPromoId = dlcPromoId;
    this.currentDlcPromoDisplayCount = 0;
  }

  public virtual void IncreaseCurrentDlcPromoDisplayCount() => ++this.currentDlcPromoDisplayCount;

  public virtual PlayerLevelStatsData GetPlayerLevelStatsData(IDifficultyBeatmap difficultyBeatmap) => this.GetPlayerLevelStatsData(difficultyBeatmap.level.levelID, difficultyBeatmap.difficulty, difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic);

  public virtual PlayerLevelStatsData GetPlayerLevelStatsData(
    string levelId,
    BeatmapDifficulty difficulty,
    BeatmapCharacteristicSO beatmapCharacteristic)
  {
    foreach (PlayerLevelStatsData playerLevelStatsData in this.levelsStatsData)
    {
      if (playerLevelStatsData.levelID == levelId && playerLevelStatsData.difficulty == difficulty && (UnityEngine.Object) playerLevelStatsData.beatmapCharacteristic == (UnityEngine.Object) beatmapCharacteristic)
        return playerLevelStatsData;
    }
    PlayerLevelStatsData playerLevelStatsData1 = new PlayerLevelStatsData(levelId, difficulty, beatmapCharacteristic);
    this.levelsStatsData.Add(playerLevelStatsData1);
    return playerLevelStatsData1;
  }

  public virtual PlayerMissionStatsData GetPlayerMissionStatsData(string missionId)
  {
    foreach (PlayerMissionStatsData missionStatsData in this.missionsStatsData)
    {
      if (missionStatsData.missionId == missionId)
        return missionStatsData;
    }
    PlayerMissionStatsData missionStatsData1 = new PlayerMissionStatsData(missionId, false);
    this.missionsStatsData.Add(missionStatsData1);
    return missionStatsData1;
  }

  public virtual bool WasMissionHelpShowed(MissionHelpSO missionHelp) => this.showedMissionHelpIds.Contains(missionHelp.missionHelpId);

  public virtual void MissionHelpWasShowed(MissionHelpSO missionHelp)
  {
    if (this.showedMissionHelpIds.Contains(missionHelp.missionHelpId))
      return;
    this.showedMissionHelpIds.Add(missionHelp.missionHelpId);
  }

  public virtual void IncreaseNumberOfGameplays(PlayerLevelStatsData playerLevelStats)
  {
    playerLevelStats.IncreaseNumberOfGameplays();
    System.Action ofGameplaysEvent = this.didIncreaseNumberOfGameplaysEvent;
    if (ofGameplaysEvent == null)
      return;
    ofGameplaysEvent();
  }

  public virtual bool IsLevelUserFavorite(IPreviewBeatmapLevel level) => this.favoritesLevelIds.Contains(level.levelID);

  public virtual void AddLevelToFavorites(IPreviewBeatmapLevel level)
  {
    if (this.IsLevelUserFavorite(level))
      return;
    this.favoritesLevelIds.Add(level.levelID);
    System.Action setDidChangeEvent = this.favoriteLevelsSetDidChangeEvent;
    if (setDidChangeEvent == null)
      return;
    setDidChangeEvent();
  }

  public virtual void RemoveLevelFromFavorites(IPreviewBeatmapLevel level)
  {
    if (!this.IsLevelUserFavorite(level))
      return;
    this.favoritesLevelIds.Remove(level.levelID);
    System.Action setDidChangeEvent = this.favoriteLevelsSetDidChangeEvent;
    if (setDidChangeEvent == null)
      return;
    setDidChangeEvent();
  }

  public virtual void MarkTutorialAsShown() => this.shouldShowTutorialPrompt = false;

  public virtual void Mark360WarningAsShown() => this.shouldShow360Warning = false;

  public virtual void MarkEulaAsAgreed() => this.playerAgreements.AgreeToEula();

  public virtual void MarkLanguageAsSelected() => this.didSelectLanguage = true;

  public virtual void MarkRegionAsSelected(int version = 1) => this.didSelectRegionVersion = version;

  public virtual void MarkMultiplayerDisclaimerAsAgreed() => this.agreedToMultiplayerDisclaimer = true;

  public virtual void MarkPrivacyPolicyAsAgreed() => this.playerAgreements.AgreeToPrivacyPolicy();

  public virtual void MarkHealthAndSafetyAsAgreed() => this.playerAgreements.AgreeToHealthAndSafety();

  public virtual void MarkAvatarCreated() => this.avatarCreated = true;

  public virtual void AddGuestPlayerName(string guestPlayerName)
  {
    for (int index = 0; index < this.guestPlayerNames.Count; ++index)
    {
      if (this.guestPlayerNames[index] == guestPlayerName)
      {
        if (index <= 0)
          return;
        this.guestPlayerNames.Insert(0, this.guestPlayerNames[index]);
        this.guestPlayerNames.RemoveAt(index + 1);
        return;
      }
    }
    this.guestPlayerNames.Insert(0, guestPlayerName);
    if (this.guestPlayerNames.Count <= 10)
      return;
    this.guestPlayerNames.RemoveAt(this.guestPlayerNames.Count - 1);
  }

  public virtual void DeleteAllGuestPlayers() => this.guestPlayerNames.Clear();

  public virtual void SetLastSelectedBeatmapDifficulty(BeatmapDifficulty beatmapDifficulty) => this.lastSelectedBeatmapDifficulty = beatmapDifficulty;

  public virtual void SetLastSelectedBeatmapCharacteristic(
    BeatmapCharacteristicSO beatmapCharacteristic)
  {
    this.lastSelectedBeatmapCharacteristic = beatmapCharacteristic;
  }

  public virtual void SetGameplayModifiers(GameplayModifiers newGameplayModifiers) => this.gameplayModifiers = newGameplayModifiers;

  public virtual void SetPlayerSpecificSettings(PlayerSpecificSettings newPlayerSpecificSettings) => this.playerSpecificSettings = newPlayerSpecificSettings;

  public virtual void SetMultiplayerModeSettings(MultiplayerModeSettings multiplayerModeSettings) => this.multiplayerModeSettings = multiplayerModeSettings;

  public virtual bool DidSelectRegion() => this.didSelectRegionVersion == 1;
}
