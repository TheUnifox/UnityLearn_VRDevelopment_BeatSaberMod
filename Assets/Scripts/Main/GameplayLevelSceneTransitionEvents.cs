// Decompiled with JetBrains decompiler
// Type: GameplayLevelSceneTransitionEvents
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public class GameplayLevelSceneTransitionEvents
{
  protected readonly StandardLevelScenesTransitionSetupDataSO _standardLevelScenesTransitionSetupData;
  protected readonly MissionLevelScenesTransitionSetupDataSO _missionLevelScenesTransitionSetupData;
  protected readonly MultiplayerLevelScenesTransitionSetupDataSO _multiplayerLevelScenesTransitionSetupData;

  public event System.Action anyGameplayLevelDidFinishEvent;

  public GameplayLevelSceneTransitionEvents(
    StandardLevelScenesTransitionSetupDataSO standardLevelScenesTransitionSetupData,
    MissionLevelScenesTransitionSetupDataSO missionLevelScenesTransitionSetupData,
    MultiplayerLevelScenesTransitionSetupDataSO multiplayerLevelScenesTransitionSetupData)
  {
    this._standardLevelScenesTransitionSetupData = standardLevelScenesTransitionSetupData;
    this._missionLevelScenesTransitionSetupData = missionLevelScenesTransitionSetupData;
    this._multiplayerLevelScenesTransitionSetupData = multiplayerLevelScenesTransitionSetupData;
    this._standardLevelScenesTransitionSetupData.didFinishEvent += new System.Action<StandardLevelScenesTransitionSetupDataSO, LevelCompletionResults>(this.HandleStandardLevelDidFinish);
    this._multiplayerLevelScenesTransitionSetupData.didFinishEvent += new System.Action<MultiplayerLevelScenesTransitionSetupDataSO, MultiplayerResultsData>(this.HandleMultiplayerLevelDidFinish);
    this._missionLevelScenesTransitionSetupData.didFinishEvent += new System.Action<MissionLevelScenesTransitionSetupDataSO, MissionCompletionResults>(this.HandleMissionLevelDidFinish);
  }

  ~GameplayLevelSceneTransitionEvents()
  {
    this._standardLevelScenesTransitionSetupData.didFinishEvent -= new System.Action<StandardLevelScenesTransitionSetupDataSO, LevelCompletionResults>(this.HandleStandardLevelDidFinish);
    this._multiplayerLevelScenesTransitionSetupData.didFinishEvent -= new System.Action<MultiplayerLevelScenesTransitionSetupDataSO, MultiplayerResultsData>(this.HandleMultiplayerLevelDidFinish);
    this._missionLevelScenesTransitionSetupData.didFinishEvent -= new System.Action<MissionLevelScenesTransitionSetupDataSO, MissionCompletionResults>(this.HandleMissionLevelDidFinish);
  }

  public virtual void HandleStandardLevelDidFinish(
    StandardLevelScenesTransitionSetupDataSO standardLevelScenesTransitionSetupData,
    LevelCompletionResults levelCompletionResults)
  {
    this.InvokeAnyGameplayLevelDidFinish();
  }

  public virtual void HandleMultiplayerLevelDidFinish(
    MultiplayerLevelScenesTransitionSetupDataSO multiplayerLevelScenesTransitionSetupData,
    MultiplayerResultsData multiplayerResultsData)
  {
    this.InvokeAnyGameplayLevelDidFinish();
  }

  public virtual void HandleMissionLevelDidFinish(
    MissionLevelScenesTransitionSetupDataSO missionLevelScenesTransitionSetupData,
    MissionCompletionResults missionCompletionResults)
  {
    this.InvokeAnyGameplayLevelDidFinish();
  }

  public virtual void InvokeAnyGameplayLevelDidFinish()
  {
    System.Action levelDidFinishEvent = this.anyGameplayLevelDidFinishEvent;
    if (levelDidFinishEvent == null)
      return;
    levelDidFinishEvent();
  }
}
