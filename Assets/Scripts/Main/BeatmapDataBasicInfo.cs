// Decompiled with JetBrains decompiler
// Type: BeatmapDataBasicInfo
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class BeatmapDataBasicInfo : IBeatmapDataBasicInfo
{
  [CompilerGenerated]
  protected readonly int m_CnumberOfLines;
  [CompilerGenerated]
  protected readonly int m_CcuttableNotesCount;
  [CompilerGenerated]
  protected readonly int m_CobstaclesCount;
  [CompilerGenerated]
  protected readonly int m_CbombsCount;
  [CompilerGenerated]
  protected readonly IEnumerable<string> m_CspecialBasicBeatmapEventKeywords;

  public int numberOfLines => this.m_CnumberOfLines;

  public int cuttableNotesCount => this.m_CcuttableNotesCount;

  public int obstaclesCount => this.m_CobstaclesCount;

  public int bombsCount => this.m_CbombsCount;

  public IEnumerable<string> specialBasicBeatmapEventKeywords => this.m_CspecialBasicBeatmapEventKeywords;

  public BeatmapDataBasicInfo(
    int numberOfLines,
    int cuttableNotesCount,
    int obstaclesCount,
    int bombsCount,
    IEnumerable<string> specialBasicBeatmapEventKeywords)
  {
    this.m_CnumberOfLines = numberOfLines;
    this.m_CcuttableNotesCount = cuttableNotesCount;
    this.m_CobstaclesCount = obstaclesCount;
    this.m_CbombsCount = bombsCount;
    this.m_CspecialBasicBeatmapEventKeywords = specialBasicBeatmapEventKeywords;
  }
}
