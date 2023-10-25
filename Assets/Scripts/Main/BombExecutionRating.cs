// Decompiled with JetBrains decompiler
// Type: BombExecutionRating
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;

public class BombExecutionRating : BeatmapObjectExecutionRating
{
  [CompilerGenerated]
  protected readonly BombExecutionRating.Rating m_Crating;

  public BombExecutionRating.Rating rating => this.m_Crating;

  public BombExecutionRating(float time, BombExecutionRating.Rating rating)
    : base(time)
  {
    this.m_Crating = rating;
  }

  public enum Rating
  {
    OK,
    NotGood,
  }
}
