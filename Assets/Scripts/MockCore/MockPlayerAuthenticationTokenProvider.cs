using System;
using System.Threading.Tasks;

// Token: 0x02000009 RID: 9
public class MockPlayerAuthenticationTokenProvider : IAuthenticationTokenProvider
{
    // Token: 0x17000013 RID: 19
    // (get) Token: 0x06000031 RID: 49 RVA: 0x00002310 File Offset: 0x00000510
    public string hashedUserId { get; }

    // Token: 0x17000014 RID: 20
    // (get) Token: 0x06000032 RID: 50 RVA: 0x00002318 File Offset: 0x00000518
    public string userName { get; }

    // Token: 0x06000033 RID: 51 RVA: 0x00002320 File Offset: 0x00000520
    public MockPlayerAuthenticationTokenProvider(string userId, string userName, string password)
    {
        this._userId = userId;
        this.userName = userName;
        this.hashedUserId = NetworkUtility.GetHashedUserId(userId, AuthenticationToken.Platform.Test);
        this._password = password;
    }

    // Token: 0x06000034 RID: 52 RVA: 0x0000234A File Offset: 0x0000054A
    public Task<AuthenticationToken> GetAuthenticationToken()
    {
        return Task.FromResult<AuthenticationToken>(new AuthenticationToken(AuthenticationToken.Platform.Test, this._userId, this.userName, this._password));
    }

    // Token: 0x04000018 RID: 24
    private readonly string _userId;

    // Token: 0x04000019 RID: 25
    private readonly string _password;
}
