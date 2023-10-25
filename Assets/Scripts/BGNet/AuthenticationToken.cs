using System;
using System.Text;
using LiteNetLib.Utils;

// Token: 0x02000005 RID: 5
public readonly struct AuthenticationToken : INetImmutableSerializable<AuthenticationToken>
{
    // Token: 0x06000013 RID: 19 RVA: 0x00002275 File Offset: 0x00000475
    public AuthenticationToken(AuthenticationToken.Platform platform, string userId, string userName, string sessionToken)
    {
        this.platform = platform;
        this.userId = userId;
        this.userName = userName;
        this.sessionToken = sessionToken;
    }

    // Token: 0x06000014 RID: 20 RVA: 0x00002294 File Offset: 0x00000494
    public void Serialize(NetDataWriter writer)
    {
        writer.Put((byte)this.platform);
        writer.Put(this.userId);
        writer.Put(this.userName);
        byte[] array = (this.platform == AuthenticationToken.Platform.Steam) ? AuthenticationToken.FromHex(this.sessionToken) : AuthenticationToken.FromUtf8(this.sessionToken);
        writer.PutVarUInt((uint)array.Length);
        writer.Put(array);
    }

    // Token: 0x06000015 RID: 21 RVA: 0x000022F7 File Offset: 0x000004F7
    public AuthenticationToken CreateFromSerializedData(NetDataReader reader)
    {
        return AuthenticationToken.Deserialize(reader);
    }

    // Token: 0x06000016 RID: 22 RVA: 0x00002300 File Offset: 0x00000500
    public static AuthenticationToken Deserialize(NetDataReader reader)
    {
        byte @byte = reader.GetByte();
        string @string = reader.GetString();
        string string2 = reader.GetString();
        byte[] array = new byte[reader.GetVarUInt()];
        reader.GetBytes(array, 0, array.Length);
        string text = (@byte == 3) ? AuthenticationToken.ToHex(array) : AuthenticationToken.ToUtf8(array);
        return new AuthenticationToken((AuthenticationToken.Platform)@byte, @string, string2, text);
    }

    // Token: 0x06000017 RID: 23 RVA: 0x00002354 File Offset: 0x00000554
    private static byte[] FromHex(string str)
    {
        byte[] array = new byte[str.Length / 2];
        int i = 0;
        int num = 0;
        int num2 = 1;
        while (i < array.Length)
        {
            array[i] = (byte)((int)AuthenticationToken.GetHexVal(str[num]) << 4 | (int)AuthenticationToken.GetHexVal(str[num2]));
            i++;
            num += 2;
            num2 += 2;
        }
        return array;
    }

    // Token: 0x06000018 RID: 24 RVA: 0x000023AC File Offset: 0x000005AC
    private static byte GetHexVal(char c)
    {
        if (c >= '0' && c <= '9')
        {
            return (byte)(c - '0');
        }
        if (c >= 'a' && c <= 'f')
        {
            return (byte)('\n' + c - 'a');
        }
        if (c >= 'A' && c <= 'F')
        {
            return (byte)('\n' + c - 'A');
        }
        throw new Exception(string.Format("Invalid Hex Char {0}", c));
    }

    // Token: 0x06000019 RID: 25 RVA: 0x00002404 File Offset: 0x00000604
    private static byte[] FromUtf8(string str)
    {
        return Encoding.UTF8.GetBytes(str);
    }

    // Token: 0x0600001A RID: 26 RVA: 0x00002411 File Offset: 0x00000611
    private static string ToHex(byte[] bytes)
    {
        return BitConverter.ToString(bytes).Replace("-", "");
    }

    // Token: 0x0600001B RID: 27 RVA: 0x00002428 File Offset: 0x00000628
    private static string ToUtf8(byte[] bytes)
    {
        return Encoding.UTF8.GetString(bytes);
    }

    // Token: 0x04000007 RID: 7
    public readonly AuthenticationToken.Platform platform;

    // Token: 0x04000008 RID: 8
    public readonly string userId;

    // Token: 0x04000009 RID: 9
    public readonly string userName;

    // Token: 0x0400000A RID: 10
    public readonly string sessionToken;

    // Token: 0x020000C9 RID: 201
    public enum Platform : byte
    {
        // Token: 0x040002EB RID: 747
        Test,
        // Token: 0x040002EC RID: 748
        OculusRift,
        // Token: 0x040002ED RID: 749
        OculusQuest,
        // Token: 0x040002EE RID: 750
        Steam,
        // Token: 0x040002EF RID: 751
        PS4,
        // Token: 0x040002F0 RID: 752
        PS4Dev,
        // Token: 0x040002F1 RID: 753
        PS4Cert,
        // Token: 0x040002F2 RID: 754
        Oculus = 1
    }
}
