// Decompiled with JetBrains decompiler
// Type: BeatmapLevelPackCollection
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public class BeatmapLevelPackCollection : IBeatmapLevelPackCollection
{
  protected IBeatmapLevelPack[] _beatmapLevelPacks;

  public IBeatmapLevelPack[] beatmapLevelPacks => this._beatmapLevelPacks;

  public BeatmapLevelPackCollection(IBeatmapLevelPack[] beatmapLevelPacks) => this._beatmapLevelPacks = beatmapLevelPacks;
}
