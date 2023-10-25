// Decompiled with JetBrains decompiler
// Type: ShaderWarmupSceneSetupData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;

public class ShaderWarmupSceneSetupData : SceneSetupData
{
  [CompilerGenerated]
  protected ScenesTransitionSetupDataSO m_CnextScenesTransitionSetupData;

  public ScenesTransitionSetupDataSO nextScenesTransitionSetupData
  {
    get => this.m_CnextScenesTransitionSetupData;
    private set => this.m_CnextScenesTransitionSetupData = value;
  }

  public ShaderWarmupSceneSetupData(
    ScenesTransitionSetupDataSO nextScenesTransitionSetupData)
  {
    this.nextScenesTransitionSetupData = nextScenesTransitionSetupData;
  }
}
