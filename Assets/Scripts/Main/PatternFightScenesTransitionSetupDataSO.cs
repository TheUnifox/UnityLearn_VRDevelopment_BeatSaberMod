// Decompiled with JetBrains decompiler
// Type: PatternFightScenesTransitionSetupDataSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class PatternFightScenesTransitionSetupDataSO : ScenesTransitionSetupDataSO
{
  [SerializeField]
  protected EnvironmentInfoSO _environmentInfo;
  [SerializeField]
  protected SceneInfo _patternFightSceneInfo;
  [SerializeField]
  protected SceneInfo _gameCoreSceneInfo;

  public event System.Action<PatternFightScenesTransitionSetupDataSO, PatternFightScenesTransitionSetupDataSO.PatternFightEndStateType> didFinishEvent;

  public virtual void Init(PlayerSpecificSettings playerSpecificSettings)
  {
    ColorScheme colorScheme = new ColorScheme(this._environmentInfo.colorScheme);
    this.Init(new SceneInfo[3]
    {
      this._environmentInfo.sceneInfo,
      this._patternFightSceneInfo,
      this._gameCoreSceneInfo
    }, new SceneSetupData[3]
    {
      (SceneSetupData) new EnvironmentSceneSetupData(this._environmentInfo, (IPreviewBeatmapLevel) null, false),
      (SceneSetupData) new GameCoreSceneSetupData(),
      (SceneSetupData) new PatternFightSceneSetupData(playerSpecificSettings, colorScheme)
    });
  }

  public virtual void Finish(
    PatternFightScenesTransitionSetupDataSO.PatternFightEndStateType endState)
  {
    System.Action<PatternFightScenesTransitionSetupDataSO, PatternFightScenesTransitionSetupDataSO.PatternFightEndStateType> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(this, endState);
  }

  public enum PatternFightEndStateType
  {
    Completed,
    ReturnToMenu,
  }
}
