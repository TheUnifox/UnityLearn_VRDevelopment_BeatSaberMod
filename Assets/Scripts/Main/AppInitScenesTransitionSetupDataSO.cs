// Decompiled with JetBrains decompiler
// Type: AppInitScenesTransitionSetupDataSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;

public class AppInitScenesTransitionSetupDataSO : SingleFixedSceneScenesTransitionSetupDataSO
{
  public virtual void Init() => this.Init((SceneSetupData) new AppInitScenesTransitionSetupDataSO.AppInitSceneSetupData(AppInitScenesTransitionSetupDataSO.AppInitOverrideStartType.AppRestart, (MockPlayersModel) null));

  public virtual void InitAsAppStart() => this.Init((SceneSetupData) new AppInitScenesTransitionSetupDataSO.AppInitSceneSetupData(AppInitScenesTransitionSetupDataSO.AppInitOverrideStartType.AppStart, (MockPlayersModel) null));

  public virtual void __Init(
    AppInitScenesTransitionSetupDataSO.AppInitOverrideStartType appInitOverrideStartType,
    MockPlayersModel mockPlayersModel)
  {
    this.Init((SceneSetupData) new AppInitScenesTransitionSetupDataSO.AppInitSceneSetupData(appInitOverrideStartType, mockPlayersModel));
  }

  public enum AppInitOverrideStartType
  {
    DoNotOverride,
    AppStart,
    AppRestart,
    MultiSceneEditor,
  }

  public class AppInitSceneSetupData : SceneSetupData
  {
    [CompilerGenerated]
    protected AppInitScenesTransitionSetupDataSO.AppInitOverrideStartType m_CappInitOverrideStartType;
    [CompilerGenerated]
    protected MockPlayersModel m_CoverrideMockPlayersModel;

    public AppInitScenesTransitionSetupDataSO.AppInitOverrideStartType appInitOverrideStartType
    {
      get => this.m_CappInitOverrideStartType;
      private set => this.m_CappInitOverrideStartType = value;
    }

    public MockPlayersModel overrideMockPlayersModel
    {
      get => this.m_CoverrideMockPlayersModel;
      private set => this.m_CoverrideMockPlayersModel = value;
    }

    public AppInitSceneSetupData(
      AppInitScenesTransitionSetupDataSO.AppInitOverrideStartType appInitOverrideStartType,
      MockPlayersModel overrideMockPlayersModel)
    {
      this.appInitOverrideStartType = appInitOverrideStartType;
      this.overrideMockPlayersModel = overrideMockPlayersModel;
    }
  }
}
