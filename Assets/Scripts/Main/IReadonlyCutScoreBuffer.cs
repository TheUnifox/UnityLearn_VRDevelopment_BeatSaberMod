// Decompiled with JetBrains decompiler
// Type: IReadonlyCutScoreBuffer
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public interface IReadonlyCutScoreBuffer
{
  int maxPossibleCutScore { get; }

  int cutScore { get; }

  int beforeCutScore { get; }

  int centerDistanceCutScore { get; }

  int afterCutScore { get; }

  bool isFinished { get; }

  ScoreModel.NoteScoreDefinition noteScoreDefinition { get; }

  NoteCutInfo noteCutInfo { get; }

  float beforeCutSwingRating { get; }

  float afterCutSwingRating { get; }

  void RegisterDidChangeReceiver(ICutScoreBufferDidChangeReceiver receiver);

  void RegisterDidFinishReceiver(ICutScoreBufferDidFinishReceiver receiver);

  void UnregisterDidChangeReceiver(ICutScoreBufferDidChangeReceiver receiver);

  void UnregisterDidFinishReceiver(ICutScoreBufferDidFinishReceiver receiver);
}
