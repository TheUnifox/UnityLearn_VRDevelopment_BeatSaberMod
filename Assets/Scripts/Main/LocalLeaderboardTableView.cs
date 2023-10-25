// Decompiled with JetBrains decompiler
// Type: LocalLeaderboardTableView
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;

public class LocalLeaderboardTableView : LeaderboardTableView
{
  public virtual void SetScores(
    List<LocalLeaderboardsModel.ScoreData> scores,
    int specialScorePos,
    int maxNumberOfCells)
  {
    List<LeaderboardTableView.ScoreData> scores1 = new List<LeaderboardTableView.ScoreData>();
    if (scores != null)
    {
      for (int index = 0; index < scores.Count && index < maxNumberOfCells; ++index)
      {
        LocalLeaderboardsModel.ScoreData score = scores[index];
        scores1.Add(new LeaderboardTableView.ScoreData(score._score, score._playerName, index + 1, score._fullCombo));
      }
    }
    for (int count = scores != null ? scores.Count : 0; count < maxNumberOfCells; ++count)
      scores1.Add(new LeaderboardTableView.ScoreData(-1, "", count + 1, false));
    this.SetScores(scores1, specialScorePos);
  }
}
