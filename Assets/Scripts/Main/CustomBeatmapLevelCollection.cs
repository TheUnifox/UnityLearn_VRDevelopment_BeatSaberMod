// Decompiled with JetBrains decompiler
// Type: CustomBeatmapLevelCollection
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;

public class CustomBeatmapLevelCollection : IBeatmapLevelCollection
{
  protected readonly IReadOnlyList<CustomPreviewBeatmapLevel> _customPreviewBeatmapLevels;

  public IReadOnlyList<IPreviewBeatmapLevel> beatmapLevels => (IReadOnlyList<IPreviewBeatmapLevel>) this._customPreviewBeatmapLevels;

  public CustomBeatmapLevelCollection(
    CustomPreviewBeatmapLevel[] customPreviewBeatmapLevels)
  {
    this._customPreviewBeatmapLevels = (IReadOnlyList<CustomPreviewBeatmapLevel>) customPreviewBeatmapLevels;
  }
}
