using System;
using System.Threading.Tasks;
using BGNet.Core;

// Token: 0x02000013 RID: 19
public interface IDiffieHellmanKeyPair
{
    // Token: 0x1700001C RID: 28
    // (get) Token: 0x060000A7 RID: 167
    byte[] publicKey { get; }

    // Token: 0x060000A8 RID: 168
    Task<byte[]> GetPreMasterSecretAsync(ITaskUtility taskUtility, byte[] clientPublicKey);

    // Token: 0x060000A9 RID: 169
    byte[] GetPreMasterSecret(byte[] clientPublicKey);
}
