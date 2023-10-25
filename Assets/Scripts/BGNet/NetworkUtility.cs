using System;
using System.Security.Cryptography;
using System.Text;

// Token: 0x02000068 RID: 104
public static class NetworkUtility
{
    // Token: 0x06000489 RID: 1161 RVA: 0x0000BBAE File Offset: 0x00009DAE
    static NetworkUtility()
    {
        NetworkUtility.Init();
    }

    // Token: 0x0600048A RID: 1162 RVA: 0x0000BBB5 File Offset: 0x00009DB5
    private static void Init()
    {
        NetworkUtility._nameEncryptionKey = new byte[]
        {
            224,
            84,
            74,
            22,
            147,
            99,
            78,
            46
        };
        NetworkUtility._nameEncryptionIv = new byte[]
        {
            183,
            121,
            39,
            100,
            169,
            139,
            247,
            96
        };
    }

    // Token: 0x0600048B RID: 1163 RVA: 0x0000BBE4 File Offset: 0x00009DE4
    public static string GetHashedUserId(string userId, AuthenticationToken.Platform platform)
    {
        string input;
        switch (platform)
        {
            case AuthenticationToken.Platform.Test:
                input = "Test#" + userId;
                break;
            case AuthenticationToken.Platform.OculusRift:
            case AuthenticationToken.Platform.OculusQuest:
                input = "Oculus#" + userId;
                break;
            case AuthenticationToken.Platform.Steam:
                input = "Steam#" + userId;
                break;
            case AuthenticationToken.Platform.PS4:
            case AuthenticationToken.Platform.PS4Dev:
            case AuthenticationToken.Platform.PS4Cert:
                input = "PSN#" + userId;
                break;
            default:
                return userId;
        }
        return NetworkUtility.GetHashBase64(input);
    }

    // Token: 0x0600048C RID: 1164 RVA: 0x0000BC58 File Offset: 0x00009E58
    public static string EncryptName(string text)
    {
        string result;
        using (SymmetricAlgorithm symmetricAlgorithm = DES.Create())
        {
            using (ICryptoTransform cryptoTransform = symmetricAlgorithm.CreateEncryptor(NetworkUtility._nameEncryptionKey, NetworkUtility._nameEncryptionIv))
            {
                byte[] bytes = Encoding.Unicode.GetBytes(text);
                result = Convert.ToBase64String(cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length));
            }
        }
        return result;
    }

    // Token: 0x0600048D RID: 1165 RVA: 0x0000BCCC File Offset: 0x00009ECC
    public static string DecryptName(string text)
    {
        string @string;
        using (SymmetricAlgorithm symmetricAlgorithm = DES.Create())
        {
            using (ICryptoTransform cryptoTransform = symmetricAlgorithm.CreateDecryptor(NetworkUtility._nameEncryptionKey, NetworkUtility._nameEncryptionIv))
            {
                byte[] array = Convert.FromBase64String(text);
                byte[] bytes = cryptoTransform.TransformFinalBlock(array, 0, array.Length);
                @string = Encoding.Unicode.GetString(bytes);
            }
        }
        return @string;
    }

    // Token: 0x0600048E RID: 1166 RVA: 0x0000BD44 File Offset: 0x00009F44
    public static string GenerateId()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 22);
    }

    // Token: 0x0600048F RID: 1167 RVA: 0x0000BD6C File Offset: 0x00009F6C
    public static string GetHashBase64(string input)
    {
        string result;
        using (SHA256 sha = SHA256.Create())
        {
            result = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(input))).Substring(0, 22);
        }
        return result;
    }

    // Token: 0x040001B9 RID: 441
    private static byte[] _nameEncryptionKey;

    // Token: 0x040001BA RID: 442
    private static byte[] _nameEncryptionIv;
}
