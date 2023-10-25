using System;
using LiteNetLib.Utils;

// Token: 0x0200003E RID: 62
public static class VarIntExtensions
{
    // Token: 0x06000170 RID: 368 RVA: 0x00007056 File Offset: 0x00005256
    public static void PutVarInt(this NetDataWriter writer, int val)
    {
        writer.PutVarLong((long)val);
    }

    // Token: 0x06000171 RID: 369 RVA: 0x00007060 File Offset: 0x00005260
    public static int GetVarInt(this NetDataReader reader)
    {
        return (int)reader.GetVarLong();
    }

    // Token: 0x06000172 RID: 370 RVA: 0x00007069 File Offset: 0x00005269
    public static void PutVarUInt(this NetDataWriter writer, uint val)
    {
        writer.PutVarULong((ulong)val);
    }

    // Token: 0x06000173 RID: 371 RVA: 0x00007073 File Offset: 0x00005273
    public static uint GetVarUInt(this NetDataReader reader)
    {
        return (uint)reader.GetVarULong();
    }

    // Token: 0x06000174 RID: 372 RVA: 0x0000707C File Offset: 0x0000527C
    public static void PutVarLong(this NetDataWriter writer, long val)
    {
        writer.PutVarULong((ulong)((val < 0L) ? (ulong)((-(val + 1L) << 1) + 1L) : ((ulong)val << 1)));
    }

    // Token: 0x06000175 RID: 373 RVA: 0x00007098 File Offset: 0x00005298
    public static long GetVarLong(this NetDataReader reader)
    {
        ulong varULong = reader.GetVarULong();
        if ((varULong & 1UL) != 1UL)
        {
            return (long)(varULong >> 1);
        }
        return (-(long)((varULong >> 1) - 1UL));
    }

    // Token: 0x06000176 RID: 374 RVA: 0x000070C0 File Offset: 0x000052C0
    public static void PutVarULong(this NetDataWriter writer, ulong val)
    {
        do
        {
            byte b = (byte)(val & 127UL);
            val >>= 7;
            if (val != 0UL)
            {
                b |= 128;
            }
            writer.Put(b);
        }
        while (val != 0UL);
    }

    // Token: 0x06000177 RID: 375 RVA: 0x000070F0 File Offset: 0x000052F0
    public static ulong GetVarULong(this NetDataReader reader)
    {
        ulong num = 0UL;
        int num2 = 0;
        ulong num3;
        while (((num3 = (ulong)reader.GetByte()) & 128UL) != 0UL)
        {
            num |= (num3 & 127UL) << num2;
            num2 += 7;
        }
        return num | num3 << num2;
    }

    // Token: 0x06000178 RID: 376 RVA: 0x00007134 File Offset: 0x00005334
    public static bool TryGetVarUInt(this NetDataReader reader, out uint value)
    {
        ulong num;
        if (reader.TryGetVarULong(out num) && num >> 32 == 0UL)
        {
            value = (uint)num;
            return true;
        }
        value = 0U;
        return false;
    }

    // Token: 0x06000179 RID: 377 RVA: 0x0000715C File Offset: 0x0000535C
    public static bool TryGetVarULong(this NetDataReader reader, out ulong value)
    {
        value = 0UL;
        int num = 0;
        byte b;
        while (num <= 63 && reader.TryGetByte(out b))
        {
            value |= (ulong)((ulong)((long)(b & 127)) << num);
            num += 7;
            if ((b & 128) == 0)
            {
                return true;
            }
        }
        value = 0UL;
        return false;
    }

    // Token: 0x0600017A RID: 378 RVA: 0x000071A1 File Offset: 0x000053A1
    public static int GetSize(int val)
    {
        return VarIntExtensions.GetSize((long)val);
    }

    // Token: 0x0600017B RID: 379 RVA: 0x000071AA File Offset: 0x000053AA
    public static int GetSize(uint val)
    {
        return VarIntExtensions.GetSize((ulong)val);
    }

    // Token: 0x0600017C RID: 380 RVA: 0x000071B3 File Offset: 0x000053B3
    public static int GetSize(long val)
    {
        return VarIntExtensions.GetSize((ulong)((val < 0L) ? (ulong)((-(val + 1L) << 1) + 1L) : ((ulong)val << 1)));
    }

    // Token: 0x0600017D RID: 381 RVA: 0x000071D0 File Offset: 0x000053D0
    public static int GetSize(ulong val)
    {
        int num = 0;
        do
        {
            val >>= 7;
            num++;
        }
        while (val != 0UL);
        return num;
    }
}
