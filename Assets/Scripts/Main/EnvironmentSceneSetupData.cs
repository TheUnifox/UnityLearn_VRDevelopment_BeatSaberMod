// Decompiled with JetBrains decompiler
// Type: EnvironmentSceneSetupData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public class EnvironmentSceneSetupData : SceneSetupData
{
  public readonly bool hideBranding;
  public readonly EnvironmentInfoSO environmentInfo;
  public readonly IPreviewBeatmapLevel previewBeatmapLevel;

  public EnvironmentSceneSetupData(
    EnvironmentInfoSO environmentInfo,
    IPreviewBeatmapLevel previewBeatmapLevel,
    bool hideBranding)
  {
    this.hideBranding = hideBranding;
    this.environmentInfo = environmentInfo;
    this.previewBeatmapLevel = previewBeatmapLevel;
  }
}
