// Decompiled with JetBrains decompiler
// Type: LeaderboardUploadData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;

[Serializable]
public class LeaderboardUploadData
{
  public string playerName;
  public string playerId;
  public string score;
  public string leaderboardId;
  public string songName;
  public string songSubName;
  public string authorName;
  public string bpm;
  public string difficulty;
  public string infoHash;
  public List<string> modifiers;

  public LeaderboardUploadData(
    string playerName,
    string playerId,
    string score,
    string leaderboardId,
    string songName,
    string songSubName,
    string authorName,
    string bpm,
    string difficulty,
    string infoHash,
    List<string> modifiers)
  {
    this.playerName = playerName;
    this.playerId = playerId;
    this.score = score;
    this.leaderboardId = leaderboardId;
    this.songName = songName;
    this.songSubName = songSubName;
    this.authorName = authorName;
    this.bpm = bpm;
    this.difficulty = difficulty;
    this.infoHash = infoHash;
    this.modifiers = modifiers;
  }
}
