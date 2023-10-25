// Decompiled with JetBrains decompiler
// Type: LeaderboardsDTO.GameplayModifiersDto
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;

namespace LeaderboardsDTO
{
  [Serializable]
  public enum GameplayModifiersDto : uint
  {
    None = 0,
    NoFail = 1,
    InstaFail = 2,
    FailOnSaberClash = 4,
    FastNotes = 8,
    DisappearingArrows = 16, // 0x00000010
    NoBombs = 32, // 0x00000020
    SongSpeedFaster = 64, // 0x00000040
    SongSpeedSlower = 128, // 0x00000080
    EnabledObstacleTypeFullHeightOnly = 256, // 0x00000100
    EnabledObstacleTypeNoObstacles = 512, // 0x00000200
    EnergyTypeBattery = 1024, // 0x00000400
    StrictAngles = 2048, // 0x00000800
    NoArrows = 4096, // 0x00001000
    GhostNotes = 8192, // 0x00002000
  }
}
