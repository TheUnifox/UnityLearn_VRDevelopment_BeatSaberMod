// Decompiled with JetBrains decompiler
// Type: MissionCompletionResults
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public class MissionCompletionResults
{
  public readonly LevelCompletionResults levelCompletionResults;
  public readonly MissionObjectiveResult[] missionObjectiveResults;

  public bool IsMissionComplete
  {
    get
    {
      if (this.levelCompletionResults.levelEndStateType != LevelCompletionResults.LevelEndStateType.Cleared)
        return false;
      foreach (MissionObjectiveResult missionObjectiveResult in this.missionObjectiveResults)
      {
        if (!missionObjectiveResult.cleared)
          return false;
      }
      return true;
    }
  }

  public MissionCompletionResults(
    LevelCompletionResults levelCompletionResults,
    MissionObjectiveResult[] missionObjectiveResults)
  {
    this.levelCompletionResults = levelCompletionResults;
    this.missionObjectiveResults = missionObjectiveResults;
  }
}
