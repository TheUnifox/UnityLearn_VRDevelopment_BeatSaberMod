// Decompiled with JetBrains decompiler
// Type: OnlineServices.API.IUserLoginDtoDataSource
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using BeatSaberAPI.DataTransferObjects;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineServices.API
{
  public interface IUserLoginDtoDataSource
  {
    Task<UserAuthenticationData> UserAuthenticationDataAsync(CancellationToken cancellationToken);

    Task<string[]> GetUserFriendsUserIds(CancellationToken cancellationToken);

    Task<string> GetPlatformUserIdAsync(CancellationToken cancellationToken);
  }
}
