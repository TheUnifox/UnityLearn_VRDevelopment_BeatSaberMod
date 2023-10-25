// Decompiled with JetBrains decompiler
// Type: TutorialScenesTransitionSetupDataSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;
using UnityEngine;

public class TutorialScenesTransitionSetupDataSO : ScenesTransitionSetupDataSO
{
  [SerializeField]
  protected EnvironmentInfoSO _environmentInfo;
  [SerializeField]
  protected SceneInfo _tutorialSceneInfo;
  [SerializeField]
  protected SceneInfo _gameCoreSceneInfo;
  [CompilerGenerated]
  protected PlayerSpecificSettings m_CplayerSpecificSettings;

  public event System.Action<TutorialScenesTransitionSetupDataSO, TutorialScenesTransitionSetupDataSO.TutorialEndStateType> didFinishEvent;

  public PlayerSpecificSettings playerSpecificSettings
  {
    get => this.m_CplayerSpecificSettings;
    private set => this.m_CplayerSpecificSettings = value;
  }

  public virtual void Init(PlayerSpecificSettings playerSpecificSettings)
  {
    ColorScheme colorScheme = new ColorScheme(this._environmentInfo.colorScheme);
    SceneInfo[] scenes = new SceneInfo[3]
    {
      this._environmentInfo.sceneInfo,
      this._tutorialSceneInfo,
      this._gameCoreSceneInfo
    };
    SceneSetupData[] sceneSetupData = new SceneSetupData[3]
    {
      (SceneSetupData) new EnvironmentSceneSetupData(this._environmentInfo, (IPreviewBeatmapLevel) null, false),
      (SceneSetupData) new GameCoreSceneSetupData(),
      (SceneSetupData) new TutorialSceneSetupData(colorScheme, playerSpecificSettings)
    };
    this.playerSpecificSettings = playerSpecificSettings;
    this.Init(scenes, sceneSetupData);
  }

  public virtual void Finish(
    TutorialScenesTransitionSetupDataSO.TutorialEndStateType endState)
  {
    System.Action<TutorialScenesTransitionSetupDataSO, TutorialScenesTransitionSetupDataSO.TutorialEndStateType> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(this, endState);
  }

  public enum TutorialEndStateType
  {
    Completed,
    ReturnToMenu,
    Restart,
  }
}
