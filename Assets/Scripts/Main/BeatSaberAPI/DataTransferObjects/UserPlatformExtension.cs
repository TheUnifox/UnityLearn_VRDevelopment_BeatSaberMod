// Decompiled with JetBrains decompiler
// Type: BeatSaberAPI.DataTransferObjects.UserPlatformExtension
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

namespace BeatSaberAPI.DataTransferObjects
{
  public static class UserPlatformExtension
  {
    public const string kTest = "Test";
    public const string kSteam = "Steam";
    public const string kPlayStation = "PlayStation";
    public const string kOculusPC = "OculusPC";
    public const string kOculusQuest = "OculusQuest";

    public static UserPlatform GetUserPlatformFromSerializedName(string name)
    {
      switch (name)
      {
        case "Test":
          return UserPlatform.Test;
        case "Steam":
          return UserPlatform.Steam;
        case "PlayStation":
          return UserPlatform.PlayStation;
        case "OculusPC":
          return UserPlatform.OculusPC;
        case "OculusQuest":
          return UserPlatform.OculusQuest;
        default:
          return UserPlatform.None;
      }
    }

    public static string GetUserPlatformSerializedName(this UserPlatform userPlatform)
    {
      switch (userPlatform)
      {
        case UserPlatform.Test:
          return "Test";
        case UserPlatform.Steam:
          return "Steam";
        case UserPlatform.PlayStation:
          return "PlayStation";
        case UserPlatform.OculusPC:
          return "OculusPC";
        case UserPlatform.OculusQuest:
          return "OculusQuest";
        default:
          return string.Empty;
      }
    }
  }
}
