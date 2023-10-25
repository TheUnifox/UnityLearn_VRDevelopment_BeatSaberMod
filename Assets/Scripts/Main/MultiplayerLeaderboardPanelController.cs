// Decompiled with JetBrains decompiler
// Type: MultiplayerLeaderboardPanelController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MultiplayerLeaderboardPanelController : MonoBehaviour
{
  [SerializeField]
  protected MultiplayerLeaderboardPanelItem[] _items;
  [Inject]
  protected readonly MultiplayerScoreProvider _scoreProvider;

  public virtual void Start()
  {
    foreach (MultiplayerLeaderboardPanelItem leaderboardPanelItem in this._items)
      leaderboardPanelItem.hide = true;
  }

  public virtual void Update()
  {
    IReadOnlyList<MultiplayerScoreProvider.RankedPlayer> rankedPlayers = this._scoreProvider.rankedPlayers;
    int numberOfPlayers = Mathf.Min(rankedPlayers.Count, this._items.Length);
    int index1;
    for (index1 = 0; index1 < this._items.Length - numberOfPlayers; ++index1)
      this._items[index1].hide = true;
    for (; index1 < this._items.Length; ++index1)
    {
      int index2 = index1 - this._items.Length + numberOfPlayers;
      MultiplayerScoreProvider.RankedPlayer rankedPlayer = rankedPlayers[index2];
      MultiplayerLeaderboardPanelItem leaderboardPanelItem = this._items[index1];
      leaderboardPanelItem.SetData(index2 + 1, rankedPlayer.userName, rankedPlayer.score, rankedPlayer.isFailed, numberOfPlayers);
      leaderboardPanelItem.hide = false;
    }
  }
}
