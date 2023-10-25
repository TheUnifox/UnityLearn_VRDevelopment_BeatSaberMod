// Decompiled with JetBrains decompiler
// Type: MultiplayerResultsPyramidView
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MultiplayerResultsPyramidView : MonoBehaviour
{
  [SerializeField]
  protected MultiplayerOffsetPositionByLocalPlayerPosition _multiplayerOffsetByLocalPlayerPosition;
  [SerializeField]
  protected Transform[] _spawnPoints;
  [SerializeField]
  protected Transform _spawnPointsParent;
  [SerializeField]
  protected float _evenCountOffset = 1f;
  [Inject]
  protected readonly MultiplayerResultsPyramidViewAvatar.Factory _avatarsFactory;
  protected readonly Dictionary<string, MultiplayerResultsPyramidViewAvatar> _avatarsDictionary = new Dictionary<string, MultiplayerResultsPyramidViewAvatar>();
  protected GameObject[] _resultAvatarDirectors;
  protected GameObject[] _badgeTimelines;
  protected MultiplayerResultsPyramidViewAvatar _anyResultsAvatar;

  public GameObject[] resultAvatarDirectors => this._resultAvatarDirectors;

  public GameObject[] badgeTimelines => this._badgeTimelines;

  public virtual void PrespawnAvatars(IReadOnlyList<IConnectedPlayer> activePlayers)
  {
    foreach (IConnectedPlayer activePlayer in (IEnumerable<IConnectedPlayer>) activePlayers)
    {
      MultiplayerResultsPyramidViewAvatar pyramidViewAvatar = this._avatarsFactory.Create(activePlayer);
      this._avatarsDictionary[activePlayer.userId] = pyramidViewAvatar;
      pyramidViewAvatar.gameObject.SetActive(false);
      if (this._anyResultsAvatar == null)
        this._anyResultsAvatar = pyramidViewAvatar;
    }
  }

  public virtual void SetupResults(
    IReadOnlyList<MultiplayerPlayerResultsData> resultsData,
    Transform badgeStartTransform,
    Transform badgeMidTransform)
  {
    this._multiplayerOffsetByLocalPlayerPosition.SetEnabled(true);
    this._anyResultsAvatar.SetupBadgeTimeline(badgeStartTransform, badgeMidTransform);
    if (resultsData.Count % 2 == 0)
      this._spawnPointsParent.localPosition += new Vector3(this._evenCountOffset, 0.0f, 0.0f);
    int index1 = 0;
    this._badgeTimelines = new GameObject[3];
    this._resultAvatarDirectors = new GameObject[resultsData.Count];
    for (int index2 = resultsData.Count - 1; index2 >= 0; --index2)
    {
      MultiplayerPlayerResultsData resultData = resultsData[index2];
      MultiplayerResultsPyramidViewAvatar pyramidViewAvatar;
      if (!this._avatarsDictionary.TryGetValue(resultData.connectedPlayer.userId, out pyramidViewAvatar))
      {
        Debug.LogError((object) ("Unable to find results avatar for player id " + resultData.connectedPlayer.userId + ", this should not happen"));
      }
      else
      {
        pyramidViewAvatar.transform.SetParent(this._spawnPoints[index2], false);
        pyramidViewAvatar.Setup(resultData, index2 + 1, resultsData.Count);
        this._resultAvatarDirectors[index2] = pyramidViewAvatar.gameObject;
        if (resultData.badge != null)
        {
          this._badgeTimelines[index1] = pyramidViewAvatar.badgeDirector.gameObject;
          ++index1;
        }
      }
    }
    if (index1 == this._badgeTimelines.Length)
      return;
    Array.Resize<GameObject>(ref this._badgeTimelines, index1 + 1);
  }
}
