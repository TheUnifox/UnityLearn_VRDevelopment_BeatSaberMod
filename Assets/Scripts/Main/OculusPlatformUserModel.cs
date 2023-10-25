// Decompiled with JetBrains decompiler
// Type: OculusPlatformUserModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Oculus.Platform;
using Oculus.Platform.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class OculusPlatformUserModel : IPlatformUserModel
{
  protected string[] _friendsUserIds;
  protected UserInfo _userInfo;

  public virtual async Task<UserInfo> GetUserInfo()
  {
    if (this._userInfo != null)
      return this._userInfo;
    TaskCompletionSource<UserInfo> tcs = new TaskCompletionSource<UserInfo>();
    while (!Core.IsInitialized())
      await Task.Delay(1000);
    Users.GetLoggedInUser().OnComplete((Message<User>.Callback) (message =>
    {
      if (message.IsError)
      {
        tcs.TrySetResult((UserInfo) null);
      }
      else
      {
        this._userInfo = new UserInfo(UserInfo.Platform.Oculus, message.Data.ID.ToString(), message.Data.OculusID);
        tcs.TrySetResult(this._userInfo);
      }
    }));
    return await tcs.Task;
  }

  public virtual async Task<IReadOnlyList<string>> GetUserFriendsUserIds(bool cached)
  {
    if (this._friendsUserIds != null & cached)
      return (IReadOnlyList<string>) this._friendsUserIds;
    TaskCompletionSource<IReadOnlyList<string>> tcs = new TaskCompletionSource<IReadOnlyList<string>>();
    Users.GetLoggedInUserFriends().OnComplete((Message<UserList>.Callback) (message =>
    {
      if (message.IsError)
      {
        tcs.TrySetResult((IReadOnlyList<string>) null);
      }
      else
      {
        List<string> stringList = new List<string>(message.Data.Count);
        foreach (User user in (DeserializableList<User>) message.Data)
          stringList.Add(user.ID.ToString());
        this._friendsUserIds = stringList.ToArray();
        tcs.TrySetResult((IReadOnlyList<string>) this._friendsUserIds);
      }
    }));
    return await tcs.Task;
  }

  public virtual async Task<PlatformUserAuthTokenData> GetUserAuthToken()
  {
    TaskCompletionSource<PlatformUserAuthTokenData> tcs = new TaskCompletionSource<PlatformUserAuthTokenData>();
    Users.GetUserProof().OnComplete((Message<UserProof>.Callback) (message => tcs.TrySetResult(message.IsError ? (PlatformUserAuthTokenData) null : new PlatformUserAuthTokenData(message.Data.Value, PlatformUserAuthTokenData.PlatformEnviroment.Production))));
    return await tcs.Task;
  }

}
