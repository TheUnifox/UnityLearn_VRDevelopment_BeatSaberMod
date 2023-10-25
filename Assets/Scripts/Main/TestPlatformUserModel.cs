// Decompiled with JetBrains decompiler
// Type: TestPlatformUserModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using System.Threading.Tasks;

public class TestPlatformUserModel : IPlatformUserModel
{
  public virtual async Task<UserInfo> GetUserInfo() => await Task.FromResult<UserInfo>(new UserInfo(UserInfo.Platform.Test, "testId", "TestUser"));

  public virtual async Task<IReadOnlyList<string>> GetUserFriendsUserIds(bool cached) => await Task.FromResult<IReadOnlyList<string>>((IReadOnlyList<string>) new string[1]
  {
    "friendId"
  });

  public virtual async Task<PlatformUserAuthTokenData> GetUserAuthToken() => await Task.FromResult<PlatformUserAuthTokenData>(new PlatformUserAuthTokenData("testToken", PlatformUserAuthTokenData.PlatformEnviroment.Development));

  public virtual async Task<IReadOnlyList<string>> GetUserNamesForUserIds(
    IReadOnlyList<string> userIds)
  {
    return await Task.FromResult<IReadOnlyList<string>>(userIds);
  }
}
