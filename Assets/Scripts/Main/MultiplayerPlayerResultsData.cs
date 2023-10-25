// Decompiled with JetBrains decompiler
// Type: MultiplayerPlayerResultsData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;

public class MultiplayerPlayerResultsData : IComparable
{
  public readonly IConnectedPlayer connectedPlayer;
  public readonly MultiplayerLevelCompletionResults multiplayerLevelCompletionResults;
  public MultiplayerBadgeAwardData badge;

  public MultiplayerPlayerResultsData(
    IConnectedPlayer connectedPlayer,
    MultiplayerLevelCompletionResults multiplayerLevelCompletionResults)
  {
    this.connectedPlayer = connectedPlayer;
    this.multiplayerLevelCompletionResults = multiplayerLevelCompletionResults;
  }

  public virtual int CompareTo(object obj)
  {
    if (obj == null)
      return 1;
    int num = obj is MultiplayerPlayerResultsData playerResultsData ? this.multiplayerLevelCompletionResults.CompareTo((object) playerResultsData.multiplayerLevelCompletionResults) : throw new ArgumentException("Comparing not comparable data.");
    return num == 0 ? this.connectedPlayer.sortIndex.CompareTo(playerResultsData.connectedPlayer.sortIndex) : num;
  }
}
