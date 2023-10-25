// Decompiled with JetBrains decompiler
// Type: IScoreController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public interface IScoreController
{
  event System.Action<int, int> scoreDidChangeEvent;

  event System.Action<int, float> multiplierDidChangeEvent;

  event System.Action<ScoringElement> scoringForNoteStartedEvent;

  event System.Action<ScoringElement> scoringForNoteFinishedEvent;

  int multipliedScore { get; }

  int modifiedScore { get; }

  int immediateMaxPossibleMultipliedScore { get; }

  int immediateMaxPossibleModifiedScore { get; }

  void SetEnabled(bool enabled);
}
