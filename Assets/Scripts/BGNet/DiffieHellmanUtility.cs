using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BGNet.Core;
using BGNet.Logging;
using Org.BouncyCastle.Crypto.Agreement;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Tls;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;

// Token: 0x02000014 RID: 20
public static class DiffieHellmanUtility
{
    // Token: 0x060000AA RID: 170 RVA: 0x0000457D File Offset: 0x0000277D
    public static Task<IDiffieHellmanKeyPair> GenerateKeysAsync(ITaskUtility taskUtility, CancellationToken cancellationToken = default(CancellationToken), DiffieHellmanUtility.KeyType keyType = DiffieHellmanUtility.KeyType.ElipticalCurve)
    {
        if (keyType == DiffieHellmanUtility.KeyType.ElipticalCurve)
        {
            return taskUtility.Run<IDiffieHellmanKeyPair>(new Func<IDiffieHellmanKeyPair>(DiffieHellmanUtility.GenerateElipticalCurveKeys), cancellationToken);
        }
        return taskUtility.Run<IDiffieHellmanKeyPair>(new Func<IDiffieHellmanKeyPair>(DiffieHellmanUtility.GenerateDiffieHellmanKeys), cancellationToken);
    }

    // Token: 0x060000AB RID: 171 RVA: 0x000045AA File Offset: 0x000027AA
    public static IDiffieHellmanKeyPair GenerateKeys(DiffieHellmanUtility.KeyType keyType = DiffieHellmanUtility.KeyType.ElipticalCurve)
    {
        if (keyType == DiffieHellmanUtility.KeyType.ElipticalCurve)
        {
            return DiffieHellmanUtility.GenerateElipticalCurveKeys();
        }
        return DiffieHellmanUtility.GenerateDiffieHellmanKeys();
    }

    // Token: 0x060000AC RID: 172 RVA: 0x000045BC File Offset: 0x000027BC
    private static DiffieHellmanUtility.DiffieHellmanKeyPair GenerateDiffieHellmanKeys()
    {
        DiffieHellmanUtility.DiffieHellmanKeyPair result;
        using (DiffieHellmanUtility.OperationTimer.Time("Generate Diffie-Hellman Keys"))
        {
            MemoryStream memoryStream = new MemoryStream();
            DHPrivateKeyParameters privateKeyParameters = TlsDHUtilities.GenerateEphemeralClientKeyExchange(DiffieHellmanUtility._secureRandom, DiffieHellmanUtility._dhParameters, memoryStream);
            byte[] publicKey = memoryStream.ToArray();
            result = new DiffieHellmanUtility.DiffieHellmanKeyPair(privateKeyParameters, publicKey);
        }
        return result;
    }

    // Token: 0x060000AD RID: 173 RVA: 0x00004618 File Offset: 0x00002818
    private static DiffieHellmanUtility.ElipticalCurveKeyPair GenerateElipticalCurveKeys()
    {
        DiffieHellmanUtility.ElipticalCurveKeyPair result;
        using (DiffieHellmanUtility.OperationTimer.Time("Generate Elliptical Curve Diffie-Hellman Keys"))
        {
            MemoryStream memoryStream = new MemoryStream();
            ECPrivateKeyParameters privateKeyParameters = TlsEccUtilities.GenerateEphemeralClientKeyExchange(DiffieHellmanUtility._secureRandom, DiffieHellmanUtility._ecPointFormats, DiffieHellmanUtility._ecParameters, memoryStream);
            byte[] publicKey = memoryStream.ToArray();
            result = new DiffieHellmanUtility.ElipticalCurveKeyPair(privateKeyParameters, publicKey);
        }
        return result;
    }

    // Token: 0x060000AE RID: 174 RVA: 0x00004678 File Offset: 0x00002878
    private static byte[] GetPreMasterSecret(DHBasicAgreement dhBasicAgreement, byte[] clientPublicKey)
    {
        byte[] result;
        using (DiffieHellmanUtility.OperationTimer.Time("Get Diffie-Hellman PreMasterSecret"))
        {
            DHPublicKeyParameters pubKey = new DHPublicKeyParameters(TlsDHUtilities.ReadDHParameter(new MemoryStream(clientPublicKey)), DiffieHellmanUtility._dhParameters);
            result = BigIntegers.AsUnsignedByteArray(dhBasicAgreement.CalculateAgreement(pubKey));
        }
        return result;
    }

    // Token: 0x060000AF RID: 175 RVA: 0x000046D0 File Offset: 0x000028D0
    private static byte[] GetPreMasterSecret(ECDHBasicAgreement ecdhBasicAgreement, byte[] clientPublicKey)
    {
        byte[] result;
        using (DiffieHellmanUtility.OperationTimer.Time("Get Elliptical Curve Diffie-Hellman PreMasterSecret"))
        {
            MemoryStream input = new MemoryStream(clientPublicKey);
            ECPublicKeyParameters pubKey = TlsEccUtilities.ValidateECPublicKey(TlsEccUtilities.DeserializeECPublicKey(DiffieHellmanUtility._ecPointFormats, DiffieHellmanUtility._ecParameters, TlsUtilities.ReadOpaque8(input)));
            result = BigIntegers.AsUnsignedByteArray(ecdhBasicAgreement.GetFieldSize(), ecdhBasicAgreement.CalculateAgreement(pubKey));
        }
        return result;
    }

    // Token: 0x04000068 RID: 104
    public const int kMaxDiffieHellmanPublicKeyLength = 2048;

    // Token: 0x04000069 RID: 105
    public const int kMaxElipticalCurvePublicKeyLength = 256;

    // Token: 0x0400006A RID: 106
    private static SecureRandom _secureRandom = new SecureRandom();

    // Token: 0x0400006B RID: 107
    [DoesNotRequireDomainReloadInit]
    private static DHParameters _dhParameters = DHStandardGroups.rfc7919_ffdhe4096;

    // Token: 0x0400006C RID: 108
    [DoesNotRequireDomainReloadInit]
    private static ECDomainParameters _ecParameters = TlsEccUtilities.GetParametersForNamedCurve(24);

    // Token: 0x0400006D RID: 109
    private static byte[] _ecPointFormats = new byte[]
    {
        2
    };

    // Token: 0x020000D9 RID: 217
    public enum KeyType
    {
        // Token: 0x04000339 RID: 825
        DiffieHellman,
        // Token: 0x0400033A RID: 826
        ElipticalCurve
    }

    // Token: 0x020000DA RID: 218
    private class DiffieHellmanKeyPair : IDiffieHellmanKeyPair
    {
        // Token: 0x17000148 RID: 328
        // (get) Token: 0x06000772 RID: 1906 RVA: 0x00013C82 File Offset: 0x00011E82
        public byte[] publicKey
        {
            get
            {
                return this._publicKey;
            }
        }

        // Token: 0x06000773 RID: 1907 RVA: 0x00013C8A File Offset: 0x00011E8A
        public DiffieHellmanKeyPair(DHPrivateKeyParameters privateKeyParameters, byte[] publicKey)
        {
            this._dhBasicAgreement.Init(privateKeyParameters);
            this._publicKey = publicKey;
        }

        // Token: 0x06000774 RID: 1908 RVA: 0x00013CB0 File Offset: 0x00011EB0
        public Task<byte[]> GetPreMasterSecretAsync(ITaskUtility taskUtility, byte[] clientPublicKey)
        {
            return taskUtility.Run<byte[]>(() => DiffieHellmanUtility.GetPreMasterSecret(this._dhBasicAgreement, clientPublicKey), default(CancellationToken));
        }

        // Token: 0x06000775 RID: 1909 RVA: 0x00013CEC File Offset: 0x00011EEC
        public byte[] GetPreMasterSecret(byte[] clientPublicKey)
        {
            return DiffieHellmanUtility.GetPreMasterSecret(this._dhBasicAgreement, clientPublicKey);
        }

        // Token: 0x0400033B RID: 827
        private readonly DHBasicAgreement _dhBasicAgreement = new DHBasicAgreement();

        // Token: 0x0400033C RID: 828
        private readonly byte[] _publicKey;
    }

    // Token: 0x020000DB RID: 219
    private class ElipticalCurveKeyPair : IDiffieHellmanKeyPair
    {
        // Token: 0x17000149 RID: 329
        // (get) Token: 0x06000776 RID: 1910 RVA: 0x00013CFA File Offset: 0x00011EFA
        public byte[] publicKey
        {
            get
            {
                return this._publicKey;
            }
        }

        // Token: 0x06000777 RID: 1911 RVA: 0x00013D02 File Offset: 0x00011F02
        public ElipticalCurveKeyPair(ECPrivateKeyParameters privateKeyParameters, byte[] publicKey)
        {
            this._ecdhBasicAgreement.Init(privateKeyParameters);
            this._publicKey = publicKey;
        }

        // Token: 0x06000778 RID: 1912 RVA: 0x00013D28 File Offset: 0x00011F28
        public Task<byte[]> GetPreMasterSecretAsync(ITaskUtility taskUtility, byte[] clientPublicKey)
        {
            return taskUtility.Run<byte[]>(() => DiffieHellmanUtility.GetPreMasterSecret(this._ecdhBasicAgreement, clientPublicKey), default(CancellationToken));
        }

        // Token: 0x06000779 RID: 1913 RVA: 0x00013D64 File Offset: 0x00011F64
        public byte[] GetPreMasterSecret(byte[] clientPublicKey)
        {
            return DiffieHellmanUtility.GetPreMasterSecret(this._ecdhBasicAgreement, clientPublicKey);
        }

        // Token: 0x0400033D RID: 829
        private readonly ECDHBasicAgreement _ecdhBasicAgreement = new ECDHBasicAgreement();

        // Token: 0x0400033E RID: 830
        private readonly byte[] _publicKey;
    }

    // Token: 0x020000DC RID: 220
    private sealed class OperationTimer : IDisposable
    {
        // Token: 0x0600077A RID: 1914 RVA: 0x00013D72 File Offset: 0x00011F72
        private OperationTimer(string operationName)
        {
            this._operationName = operationName;
            this._stopwatch = new Stopwatch();
            this._stopwatch.Start();
        }

        // Token: 0x0600077B RID: 1915 RVA: 0x00013D97 File Offset: 0x00011F97
        public void Dispose()
        {
            if (this._stopwatch != null)
            {
                this._stopwatch.Stop();
                BGNet.Logging.Debug.Log(string.Format("{0} took {1}ms", this._operationName, this._stopwatch.ElapsedMilliseconds));
            }
        }

        // Token: 0x0600077C RID: 1916 RVA: 0x00013DD1 File Offset: 0x00011FD1
        public static DiffieHellmanUtility.OperationTimer Time(string operation)
        {
            return null;
        }

        // Token: 0x0400033F RID: 831
        private readonly Stopwatch _stopwatch;

        // Token: 0x04000340 RID: 832
        private readonly string _operationName;
    }
}
