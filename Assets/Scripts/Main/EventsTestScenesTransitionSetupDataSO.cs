// Decompiled with JetBrains decompiler
// Type: EventsTestScenesTransitionSetupDataSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class EventsTestScenesTransitionSetupDataSO : ScenesTransitionSetupDataSO
{
  [SerializeField]
  protected EnvironmentInfoSO _environmentInfo;
  [SerializeField]
  protected SceneInfo _eventsTestSceneInfo;
  [SerializeField]
  protected SceneInfo _gameCoreSceneInfo;

  public virtual void Init() => this.Init(new SceneInfo[3]
  {
    this._environmentInfo.sceneInfo,
    this._eventsTestSceneInfo,
    this._gameCoreSceneInfo
  }, new SceneSetupData[2]
  {
    (SceneSetupData) new EnvironmentSceneSetupData(this._environmentInfo, (IPreviewBeatmapLevel) null, false),
    (SceneSetupData) new GameCoreSceneSetupData()
  });
}
