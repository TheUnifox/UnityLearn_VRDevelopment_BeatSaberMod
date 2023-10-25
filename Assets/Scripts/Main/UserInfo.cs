// Decompiled with JetBrains decompiler
// Type: UserInfo
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public class UserInfo
{
  public readonly UserInfo.Platform platform;
  public readonly string platformUserId;
  public readonly string userName;

  public UserInfo(UserInfo.Platform platform, string platformUserId, string userName)
  {
    this.platform = platform;
    this.platformUserId = platformUserId;
    this.userName = userName;
  }

  public enum Platform
  {
    Test,
    Steam,
    Oculus,
    PS4,
  }
}
