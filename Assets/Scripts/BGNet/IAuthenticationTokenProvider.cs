using System;
using System.Threading.Tasks;

// Token: 0x0200002F RID: 47
public interface IAuthenticationTokenProvider
{
    // Token: 0x0600018F RID: 399
    Task<AuthenticationToken> GetAuthenticationToken();

    // Token: 0x17000044 RID: 68
    // (get) Token: 0x06000190 RID: 400
    string hashedUserId { get; }

    // Token: 0x17000045 RID: 69
    // (get) Token: 0x06000191 RID: 401
    string userName { get; }
}
