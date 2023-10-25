// Decompiled with JetBrains decompiler
// Type: MultiplayerResultsData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;

public class MultiplayerResultsData
{
  protected readonly string _gameId;
  protected readonly MultiplayerPlayerResultsData _localPlayerResultData;
  protected readonly IReadOnlyList<MultiplayerPlayerResultsData> _otherPlayersData;
  protected readonly IReadOnlyList<MultiplayerPlayerResultsData> _allPlayersSortedData;

  public string gameId => this._gameId;

  public MultiplayerPlayerResultsData localPlayerResultData => this._localPlayerResultData;

  public IReadOnlyList<MultiplayerPlayerResultsData> otherPlayersData => this._otherPlayersData;

  public IReadOnlyList<MultiplayerPlayerResultsData> allPlayersSortedData => this._allPlayersSortedData;

  public MultiplayerResultsData(
    string gameId,
    MultiplayerLevelCompletionResults localPlayerResultData,
    Dictionary<string, MultiplayerLevelCompletionResults> otherPlayersResultData,
    MultiplayerBadgesProvider badgesProvider,
    IMultiplayerSessionManager multiplayerSessionManager)
  {
    this._gameId = gameId;
    // ISSUE: explicit non-virtual call
    List<MultiplayerPlayerResultsData> playerResults = new List<MultiplayerPlayerResultsData>((otherPlayersResultData != null ? __nonvirtual (otherPlayersResultData.Count) : 0) + 1);
    // ISSUE: explicit non-virtual call
    List<MultiplayerPlayerResultsData> playerResultsDataList = new List<MultiplayerPlayerResultsData>(otherPlayersResultData != null ? __nonvirtual (otherPlayersResultData.Count) : 0);
    this._localPlayerResultData = new MultiplayerPlayerResultsData(multiplayerSessionManager.localPlayer, localPlayerResultData);
    if (localPlayerResultData.hasAnyResults)
      playerResults.Add(this._localPlayerResultData);
    if (otherPlayersResultData != null)
    {
      foreach (KeyValuePair<string, MultiplayerLevelCompletionResults> keyValuePair in otherPlayersResultData)
      {
        IConnectedPlayer playerByUserId = multiplayerSessionManager.GetPlayerByUserId(keyValuePair.Key);
        if (playerByUserId != null && keyValuePair.Value.hasAnyResults)
        {
          MultiplayerPlayerResultsData playerResultsData = new MultiplayerPlayerResultsData(playerByUserId, keyValuePair.Value);
          playerResultsDataList.Add(playerResultsData);
          playerResults.Add(playerResultsData);
        }
      }
    }
    playerResultsDataList.Sort();
    playerResults.Sort();
    badgesProvider.SelectBadgesAndPutThemIntoResults((IReadOnlyList<MultiplayerPlayerResultsData>) playerResults);
    this._otherPlayersData = (IReadOnlyList<MultiplayerPlayerResultsData>) playerResultsDataList;
    this._allPlayersSortedData = (IReadOnlyList<MultiplayerPlayerResultsData>) playerResults;
  }
}
