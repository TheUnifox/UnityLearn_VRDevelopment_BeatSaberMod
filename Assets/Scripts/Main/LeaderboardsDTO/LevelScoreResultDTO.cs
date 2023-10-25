// Decompiled with JetBrains decompiler
// Type: LeaderboardsDTO.LevelScoreResultDTO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;

namespace LeaderboardsDTO
{
  [Serializable]
  public class LevelScoreResultDTO
  {
    public string guid;
    public string guids;
    public string guidInstance;
    public int multipliedScore;
    public int modifiedScore;
    public bool fullCombo;
    public int goodCutsCount;
    public int badCutsCount;
    public int missedCount;
    public int maxCombo;
    public GameplayModifiersDto[] gameplayModifiers;
    public string leaderboardId;
    public string deviceModel;
    public string extraDataBase64;
  }
}
