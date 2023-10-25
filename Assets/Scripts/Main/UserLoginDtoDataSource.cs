// Decompiled with JetBrains decompiler
// Type: UserLoginDtoDataSource
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using BeatSaberAPI.DataTransferObjects;
using OnlineServices.API;
using System.Threading;
using System.Threading.Tasks;
using Zenject;

public class UserLoginDtoDataSource : IUserLoginDtoDataSource
{
  [Inject]
  protected readonly IPlatformUserModel _platformUserModel;
  protected const string kVersion = "0.0.1";

  public virtual async Task<string> GetPlatformUserIdAsync(CancellationToken cancellationToken) => (await this._platformUserModel.GetUserInfo())?.platformUserId;

  public virtual async Task<UserAuthenticationData> UserAuthenticationDataAsync(
    CancellationToken cancellationToken)
  {
    UserInfo userInfo = await this._platformUserModel.GetUserInfo();
    if (userInfo == null)
      return (UserAuthenticationData) null;
    if (await this._platformUserModel.GetUserFriendsUserIds(false) == null)
      return (UserAuthenticationData) null;
    if (await this._platformUserModel.GetUserAuthToken() == null)
      return (UserAuthenticationData) null;
    return new UserAuthenticationData()
    {
      gameBuildVersion = "0.0.1",
      userPlatform = UserPlatform.Test,
      platformUserId = userInfo.platformUserId,
      publicUserDisplayName = userInfo.userName,
      platformAuthToken = "string",
      platformEnviroment = BeatSaberAPI.DataTransferObjects.PlatformEnviroment.Test
    };
  }

  public virtual async Task<string[]> GetUserFriendsUserIds(CancellationToken cancellationToken) => (string[]) await this._platformUserModel.GetUserFriendsUserIds(false);
}
