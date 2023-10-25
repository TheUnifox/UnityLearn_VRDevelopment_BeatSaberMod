using System;

// Token: 0x02000016 RID: 22
public static class IBitMaskUtil
{
    // Token: 0x06000080 RID: 128 RVA: 0x000037E0 File Offset: 0x000019E0
    public static int NumberOfSetBits<T>(this T bitMask) where T : IBitMask<T>
    {
        uint num = 0U;
        for (int i = 0; i < bitMask.bitCount / 64; i++)
        {
            num += BitMaskUtil.NumberOfSetBits(bitMask.GetBits(i * 64, 64));
        }
        return (int)num;
    }

    // Token: 0x06000081 RID: 129 RVA: 0x00003828 File Offset: 0x00001A28
    public static string ToShortString<T>(this T bitMask) where T : IBitMask<T>
    {
        char[] array = new char[(bitMask.bitCount + 5) / 6];
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = IBitMaskUtil.GetBase64Char(bitMask.GetBits(bitMask.bitCount - i * 6 - 6, 6));
        }
        return new string(array);
    }

    // Token: 0x06000082 RID: 130 RVA: 0x0000388C File Offset: 0x00001A8C
    public static byte[] ToBytes<T>(this T bitMask) where T : IBitMask<T>
    {
        byte[] array = new byte[bitMask.bitCount / 8];
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = (byte)bitMask.GetBits(bitMask.bitCount - i * 8 - 8, 8);
        }
        return array;
    }

    // Token: 0x06000083 RID: 131 RVA: 0x000038E2 File Offset: 0x00001AE2
    public static bool TryParse<T>(string stringSerializedMask, out T bitMask) where T : IBitMask<T>, new()
    {
        return IBitMaskUtil.TryParse<T>(stringSerializedMask, 0, stringSerializedMask.Length, out bitMask);
    }

    // Token: 0x06000084 RID: 132 RVA: 0x000038F4 File Offset: 0x00001AF4
    public static bool TryParse<T>(string stringSerializedMask, int offset, int length, out T bitMask) where T : IBitMask<T>, new()
    {
        bitMask = Activator.CreateInstance<T>();
        if (length == bitMask.bitCount / 4)
        {
            for (int i = 0; i < length; i++)
            {
                uint hexDigit;
                if ((hexDigit = IBitMaskUtil.GetHexDigit(stringSerializedMask[offset + i])) == 4294967295U)
                {
                    bitMask = default(T);
                    return false;
                }
                bitMask = bitMask.SetBits(bitMask.bitCount - i * 4 - 4, (ulong)hexDigit);
            }
            return true;
        }
        if (length == (bitMask.bitCount + 5) / 6)
        {
            for (int j = 0; j < length; j++)
            {
                uint base64Digit;
                if ((base64Digit = IBitMaskUtil.GetBase64Digit(stringSerializedMask[offset + j])) == 4294967295U)
                {
                    bitMask = default(T);
                    return false;
                }
                bitMask = bitMask.SetBits(bitMask.bitCount - j * 6 - 6, (ulong)base64Digit);
            }
            return true;
        }
        return false;
    }

    // Token: 0x06000085 RID: 133 RVA: 0x000039D4 File Offset: 0x00001BD4
    public static T FromBytes<T>(byte[] bytes, int offset = 0) where T : IBitMask<T>, new()
    {
        T result = Activator.CreateInstance<T>();
        for (int i = 0; i < result.bitCount / 8; i++)
        {
            result = result.SetBits(result.bitCount - i * 8 - 8, (ulong)bytes[i + offset]);
        }
        return result;
    }

    // Token: 0x06000086 RID: 134 RVA: 0x00003A29 File Offset: 0x00001C29
    private static uint GetHexDigit(char c)
    {
        if (c >= '0' && c <= '9')
        {
            return (uint)(c - '0');
        }
        if (c >= 'a' && c <= 'f')
        {
            return (uint)('\n' + (c - 'a'));
        }
        if (c >= 'A' && c <= 'F')
        {
            return (uint)('\n' + (c - 'A'));
        }
        return uint.MaxValue;
    }

    // Token: 0x06000087 RID: 135 RVA: 0x00003A60 File Offset: 0x00001C60
    private static uint GetBase64Digit(char c)
    {
        if (c >= 'A' && c <= 'Z')
        {
            return (uint)(c - 'A');
        }
        if (c >= 'a' && c <= 'z')
        {
            return (uint)('\u001a' + (c - 'a'));
        }
        if (c >= '0' && c <= '9')
        {
            return (uint)('4' + (c - '0'));
        }
        if (c == '+')
        {
            return 62U;
        }
        if (c == '/')
        {
            return 63U;
        }
        return uint.MaxValue;
    }

    // Token: 0x06000088 RID: 136 RVA: 0x00003AB1 File Offset: 0x00001CB1
    private static char GetBase64Char(ulong digit)
    {
        if (digit < 26UL)
        {
            return (char)(65UL + digit);
        }
        if (digit < 52UL)
        {
            return (char)(97UL + digit - 26UL);
        }
        if (digit < 62UL)
        {
            return (char)(48UL + digit - 52UL);
        }
        if (digit != 62UL)
        {
            return '/';
        }
        return '+';
    }
}
