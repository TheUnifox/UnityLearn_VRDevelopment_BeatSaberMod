using System;
using System.Collections.Generic;
using System.Linq;

// Token: 0x02000009 RID: 9
public static class BloomFilterUtil
{
    // Token: 0x06000057 RID: 87 RVA: 0x00002E44 File Offset: 0x00001044
    private static uint MurmurHash2(string key)
    {
        uint num = (uint)key.Length;
        uint num2 = 33U ^ num;
        int num3 = 0;
        while (num >= 4U)
        {
            uint num4 = (uint)((uint)key[num3 + 3] << 24 | (uint)key[num3 + 2] << 16 | (uint)key[num3 + 1] << 8 | key[num3]);
            num4 *= 1540483477U;
            num4 ^= num4 >> 24;
            num4 *= 1540483477U;
            num2 *= 1540483477U;
            num2 ^= num4;
            num3 += 4;
            num -= 4U;
        }
        switch (num)
        {
            case 1U:
                num2 ^= (uint)key[num3];
                num2 *= 1540483477U;
                break;
            case 2U:
                num2 ^= (uint)((uint)key[num3 + 1] << 8);
                num2 ^= (uint)key[num3];
                num2 *= 1540483477U;
                break;
            case 3U:
                num2 ^= (uint)((uint)key[num3 + 2] << 16);
                num2 ^= (uint)((uint)key[num3 + 1] << 8);
                num2 ^= (uint)key[num3];
                num2 *= 1540483477U;
                break;
        }
        num2 ^= num2 >> 13;
        num2 *= 1540483477U;
        return num2 ^ num2 >> 15;
    }

    // Token: 0x06000058 RID: 88 RVA: 0x00002F50 File Offset: 0x00001150
    public static T ToBloomFilter<T>(this string value, int hashCount = 3, int hashBits = 8) where T : IBitMask<T>, new()
    {
        return Activator.CreateInstance<T>().AddBloomFilterEntry(value, hashCount, hashBits);
    }

    // Token: 0x06000059 RID: 89 RVA: 0x00002F60 File Offset: 0x00001160
    public static T ToBloomFilter<T>(this IEnumerable<string> strings, int hashCount = 3, int hashBits = 8) where T : IBitMask<T>, new()
    {
        return strings.Aggregate(Activator.CreateInstance<T>(), (T bloomFilter, string str) => bloomFilter.AddBloomFilterEntry(str, hashCount, hashBits));
    }

    // Token: 0x0600005A RID: 90 RVA: 0x00002F98 File Offset: 0x00001198
    public static T AddBloomFilterEntry<T>(this T bitMask, string value, int hashCount = 3, int hashBits = 8) where T : IBitMask<T>
    {
        uint num = BloomFilterUtil.MurmurHash2(value);
        for (int i = 0; i < hashCount; i++)
        {
            bitMask = bitMask.SetBits((int)((ulong)num % (ulong)((long)bitMask.bitCount)), 1UL);
            num >>= hashBits;
        }
        return bitMask;
    }

    // Token: 0x0600005B RID: 91 RVA: 0x00002FE4 File Offset: 0x000011E4
    public static bool ContainsBloomFilterEntry<T>(this T bitMask, string value, int hashCount = 3, int hashBits = 8) where T : IBitMask<T>
    {
        uint num = BloomFilterUtil.MurmurHash2(value);
        for (int i = 0; i < hashCount; i++)
        {
            if (bitMask.GetBits((int)((ulong)num % (ulong)((long)bitMask.bitCount)), 1) == 0UL)
            {
                return false;
            }
            num >>= hashBits;
        }
        return true;
    }
}
