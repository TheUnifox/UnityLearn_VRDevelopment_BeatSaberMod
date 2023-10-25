// Decompiled with JetBrains decompiler
// Type: BeatSaberAPI.DataTransferObjects.UserAuthenticationData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;

namespace BeatSaberAPI.DataTransferObjects
{
  [Serializable]
  public class UserAuthenticationData
  {
    public UserPlatform userPlatform;
    public PlatformEnviroment platformEnviroment;
    public string platformUserId;
    public string platformAuthToken;
    public string bsAuthToken;
    public string gameBuildVersion;
    public string publicUserDisplayName;
  }
}
