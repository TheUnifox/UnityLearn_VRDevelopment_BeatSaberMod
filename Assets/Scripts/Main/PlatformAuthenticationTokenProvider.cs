// Decompiled with JetBrains decompiler
// Type: PlatformAuthenticationTokenProvider
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Security.Authentication;
using System.Threading.Tasks;

public class PlatformAuthenticationTokenProvider : IAuthenticationTokenProvider
{
  protected readonly IPlatformUserModel _platformUserModel;
  protected readonly string _userId;
  protected readonly string _userName;
  protected readonly string _hashedUserId;
  protected readonly AuthenticationToken.Platform _platform;

  public string hashedUserId => this._hashedUserId;

  public string userName => this._userName;

  public PlatformAuthenticationTokenProvider(
    IPlatformUserModel platformUserModel,
    UserInfo userInfo)
  {
    this._platformUserModel = platformUserModel;
    this._userId = userInfo.platformUserId;
    this._userName = userInfo.userName;
    this._platform = userInfo.platform.ToAuthenticationTokenPlatform();
    this._hashedUserId = NetworkUtility.GetHashedUserId(this._userId, this._platform);
  }

  public virtual async Task<AuthenticationToken> GetAuthenticationToken()
  {
    PlatformUserAuthTokenData userAuthToken = await this._platformUserModel.GetUserAuthToken();
    string sessionToken = userAuthToken != null && !string.IsNullOrEmpty(userAuthToken.token) ? userAuthToken.token : throw new AuthenticationException(string.Format("Failed to retrieve an AuthenticationToken for Platform {0}", (object) this._platform));
    AuthenticationToken.Platform platform = this._platform;
    if (platform == AuthenticationToken.Platform.PS4)
    {
      if (userAuthToken.validPlatformEnvironment == PlatformUserAuthTokenData.PlatformEnviroment.Development)
        platform = AuthenticationToken.Platform.PS4Dev;
      else if (userAuthToken.validPlatformEnvironment == PlatformUserAuthTokenData.PlatformEnviroment.Certification)
        platform = AuthenticationToken.Platform.PS4Cert;
    }
    return new AuthenticationToken(platform, this._userId, this._userName, sessionToken);
  }
}
