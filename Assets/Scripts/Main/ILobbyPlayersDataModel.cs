// Decompiled with JetBrains decompiler
// Type: ILobbyPlayersDataModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections;
using System.Collections.Generic;

public interface ILobbyPlayersDataModel : 
  IReadOnlyDictionary<string, ILobbyPlayerData>,
  IEnumerable<KeyValuePair<string, ILobbyPlayerData>>,
  IEnumerable,
  IReadOnlyCollection<KeyValuePair<string, ILobbyPlayerData>>
{
  event System.Action<string> didChangeEvent;

  string localUserId { get; }

  string partyOwnerId { get; }

  void SetLocalPlayerBeatmapLevel(PreviewDifficultyBeatmap beatmapLevel);

  void ClearLocalPlayerBeatmapLevel();

  void SetLocalPlayerGameplayModifiers(GameplayModifiers modifiers);

  void ClearLocalPlayerGameplayModifiers();

  void SetLocalPlayerIsActive(bool isActive);

  void SetLocalPlayerIsReady(bool isReady);

  void SetLocalPlayerIsInLobby(bool isInLobby);

  void RequestKickPlayer(string kickedUserId);

  void ClearData();

  void ClearRecommendations();

  void Activate();

  void Deactivate();
}
