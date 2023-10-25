using System;
using System.Security.Cryptography;
using System.Threading;

// Token: 0x0200007C RID: 124
public static class SecureRandomProvider
{
    // Token: 0x0600053A RID: 1338 RVA: 0x0000E4B8 File Offset: 0x0000C6B8
    public static byte[] GetBytes(int length)
    {
        byte[] array = new byte[length];
        SecureRandomProvider._secureRandomState.GetBytes(array, 0, length);
        return array;
    }

    // Token: 0x0600053B RID: 1339 RVA: 0x0000E4DA File Offset: 0x0000C6DA
    public static byte GetByte()
    {
        return SecureRandomProvider._secureRandomState.GetByte();
    }

    // Token: 0x0600053C RID: 1340 RVA: 0x0000E4E6 File Offset: 0x0000C6E6
    public static void GetBytes(byte[] buffer, int offset, int length)
    {
        SecureRandomProvider._secureRandomState.GetBytes(buffer, offset, length);
    }

    // Token: 0x0600053D RID: 1341 RVA: 0x0000E4F5 File Offset: 0x0000C6F5
    public static void GetBytes(byte[] buffer)
    {
        SecureRandomProvider._secureRandomState.GetBytes(buffer, 0, buffer.Length);
    }

    // Token: 0x04000200 RID: 512
    private static SecureRandomProvider.SecureRandomState _secureRandomState = new SecureRandomProvider.SecureRandomState();

    // Token: 0x02000157 RID: 343
    private class SecureRandomState
    {
        // Token: 0x06000871 RID: 2161 RVA: 0x000162B0 File Offset: 0x000144B0
        public void GetBytes(byte[] buffer, int offset, int length)
        {
            int num = Interlocked.Add(ref this._index, length) - length;
            byte[] sourceArray = (num < 0) ? this._randomBuffer1 : this._randomBuffer0;
            num &= int.MaxValue;
            int num2 = 0;
            if (num < 16384)
            {
                int num3 = 16384 - num;
                num2 = ((num3 > length) ? length : num3);
                Array.Copy(sourceArray, num, buffer, offset, num2);
            }
            if (num2 == length)
            {
                return;
            }
            this.FillBuffer();
            this.GetBytes(buffer, offset + num2, length - num2);
        }

        // Token: 0x06000872 RID: 2162 RVA: 0x00016324 File Offset: 0x00014524
        public byte GetByte()
        {
            int num = Interlocked.Add(ref this._index, 1) - 1;
            byte[] array = (num < 0) ? this._randomBuffer1 : this._randomBuffer0;
            num &= int.MaxValue;
            if (num < 16384)
            {
                return array[num];
            }
            this.FillBuffer();
            return this.GetByte();
        }

        // Token: 0x06000873 RID: 2163 RVA: 0x00016374 File Offset: 0x00014574
        private void FillBuffer()
        {
            RNGCryptoServiceProvider random = this._random;
            lock (random)
            {
                int index = this._index;
                if ((index & 2147483647) >= 16384)
                {
                    this._random.GetBytes((index < 0) ? this._randomBuffer0 : this._randomBuffer1);
                    Interlocked.Exchange(ref this._index, (index < 0) ? 0 : int.MinValue);
                }
            }
        }

        // Token: 0x04000452 RID: 1106
        private readonly RNGCryptoServiceProvider _random = new RNGCryptoServiceProvider();

        // Token: 0x04000453 RID: 1107
        private const int kBufferSize = 16384;

        // Token: 0x04000454 RID: 1108
        private readonly byte[] _randomBuffer0 = new byte[16384];

        // Token: 0x04000455 RID: 1109
        private readonly byte[] _randomBuffer1 = new byte[16384];

        // Token: 0x04000456 RID: 1110
        private int _index = 16384;
    }
}
