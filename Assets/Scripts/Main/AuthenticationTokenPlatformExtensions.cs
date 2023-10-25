// Decompiled with JetBrains decompiler
// Type: AuthenticationTokenPlatformExtensions
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public static class AuthenticationTokenPlatformExtensions
{
  public static AuthenticationToken.Platform ToAuthenticationTokenPlatform(
    this UserInfo.Platform platform)
  {
    switch (platform)
    {
      case UserInfo.Platform.Steam:
        return AuthenticationToken.Platform.Steam;
      case UserInfo.Platform.Oculus:
        return AuthenticationToken.Platform.OculusRift;
      case UserInfo.Platform.PS4:
        return AuthenticationToken.Platform.PS4;
      default:
        return AuthenticationToken.Platform.Test;
    }
  }

  public static UserInfo.Platform ToUserInfoPlatform(this AuthenticationToken.Platform platform)
  {
    switch (platform)
    {
      case AuthenticationToken.Platform.OculusRift:
      case AuthenticationToken.Platform.OculusQuest:
        return UserInfo.Platform.Oculus;
      case AuthenticationToken.Platform.Steam:
        return UserInfo.Platform.Steam;
      case AuthenticationToken.Platform.PS4:
        return UserInfo.Platform.PS4;
      default:
        return UserInfo.Platform.Test;
    }
  }
}
