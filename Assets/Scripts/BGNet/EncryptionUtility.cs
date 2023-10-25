using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BGNet.Core;
using BGNet.Logging;
using LiteNetLib.Utils;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;
using UnityEngine;

// Token: 0x02000019 RID: 25
public static class EncryptionUtility
{
    // Token: 0x060000CC RID: 204 RVA: 0x00004949 File Offset: 0x00002B49
    public static EncryptionUtility.IEncryptionState CreateEncryptionState(byte[] preMasterSecret, byte[] serverSeed, byte[] clientSeed, bool isClient)
    {
        return new EncryptionUtility.EncryptionState(preMasterSecret, serverSeed, clientSeed, isClient);
    }

    // Token: 0x060000CD RID: 205 RVA: 0x00004954 File Offset: 0x00002B54
    public static Task<EncryptionUtility.IEncryptionState> CreateEncryptionStateAsync(ITaskUtility taskUtility, byte[] preMasterSecret, byte[] serverSeed, byte[] clientSeed, bool isClient)
    {
        return taskUtility.Run<EncryptionUtility.IEncryptionState>(() => new EncryptionUtility.EncryptionState(preMasterSecret, serverSeed, clientSeed, isClient), default(CancellationToken));
    }

    // Token: 0x060000CE RID: 206 RVA: 0x0000499F File Offset: 0x00002B9F
    public static bool IsValidLength(int length)
    {
        return length >= 36 && (length - 4 - 16) % 16 == 0;
    }

    // Token: 0x060000CF RID: 207 RVA: 0x000049B8 File Offset: 0x00002BB8
    private static void EncryptData(EncryptionUtility.EncryptionState state, byte[] data, ref int offset, ref int length, int extraPrefixedData)
    {
        int num = offset + extraPrefixedData + 4 + 16;
        Array.Copy(data, offset, data, num, length);
        offset += extraPrefixedData;
        uint nextSentSequenceNum = state.GetNextSentSequenceNum();
        SecureRandomProvider.GetBytes(data, offset + 4 - 1, 17);
        int num2 = (int)(data[offset + 4 - 1] % 32);
        FastBitConverter.GetBytes(data, offset, nextSentSequenceNum);
        FastBitConverter.GetBytes(data, num + length, nextSentSequenceNum);
        EncryptionUtility.FastCopyMac(state.ComputeSendMac(data, num, length + 4), 0, data, num + length);
        length += 10;
        int num3 = (length + num2 + 1) % 16;
        num2 -= num3;
        if (num2 < 0)
        {
            num2 += 16;
        }
        if (num + length + num2 >= data.Length)
        {
            num2 -= 16;
        }
        int i = num + length;
        int num4 = num + length + num2;
        while (i <= num4)
        {
            data[i] = (byte)num2;
            i++;
        }
        length += num2 + 1;
        byte[] array = EncryptionUtility._tempIV;
        if (array == null)
        {
            array = (EncryptionUtility._tempIV = new byte[16]);
        }
        EncryptionUtility.FastCopyBlock(data, offset + 4, array, 0);
        using (ICryptoTransform cryptoTransform = EncryptionUtility._aes.CreateEncryptor(state.sendKey, array))
        {
            int num5;
            for (int j = length; j >= cryptoTransform.InputBlockSize; j -= num5)
            {
                int inputCount = cryptoTransform.CanTransformMultipleBlocks ? (j / cryptoTransform.InputBlockSize * cryptoTransform.InputBlockSize) : cryptoTransform.InputBlockSize;
                num5 = cryptoTransform.TransformBlock(data, num, inputCount, data, num);
                num += num5;
            }
            length = num - offset;
        }
    }

    // Token: 0x060000D0 RID: 208 RVA: 0x00004B38 File Offset: 0x00002D38
    private static bool TryDecryptData(EncryptionUtility.EncryptionState state, byte[] data, ref int offset, ref int length)
    {
        if (!EncryptionUtility.IsValidLength(length))
        {
            return false;
        }
        uint num = BitConverter.ToUInt32(data, offset);
        offset += 4;
        length -= 4;
        if (!state.IsValidSequenceNum(num))
        {
            return false;
        }
        byte[] array = EncryptionUtility._tempIV;
        if (array == null)
        {
            array = (EncryptionUtility._tempIV = new byte[16]);
        }
        EncryptionUtility.FastCopyBlock(data, offset, array, 0);
        offset += array.Length;
        length -= array.Length;
        using (ICryptoTransform cryptoTransform = EncryptionUtility._aes.CreateDecryptor(state.receiveKey, array))
        {
            int num2 = offset;
            int num3;
            for (int i = length; i >= cryptoTransform.InputBlockSize; i -= num3)
            {
                int inputCount = cryptoTransform.CanTransformMultipleBlocks ? (i / cryptoTransform.InputBlockSize * cryptoTransform.InputBlockSize) : cryptoTransform.InputBlockSize;
                num3 = cryptoTransform.TransformBlock(data, num2, inputCount, data, num2);
                num2 += num3;
            }
            length = num2 - offset;
        }
        int num4 = (int)data[offset + length - 1];
        bool flag = true;
        if (num4 + 10 + 1 > length)
        {
            num4 = 0;
            flag = false;
        }
        length -= num4 + 10 + 1;
        ulong num5 = BitConverter.ToUInt64(data, offset + length);
        ushort num6 = BitConverter.ToUInt16(data, offset + length + 8);
        FastBitConverter.GetBytes(data, offset + length, num);
        byte[] value = state.ComputeReceiveMac(data, offset, length + 4);
        return !(num5 != BitConverter.ToUInt64(value, 0) | num6 != BitConverter.ToUInt16(value, 8) | !flag) && state.PutSequenceNum(num);
    }

    // Token: 0x060000D1 RID: 209 RVA: 0x00004CC0 File Offset: 0x00002EC0
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void FastCopyBlock(byte[] inArr, int inOff, byte[] outArr, int outOff)
    {
        outArr[outOff] = inArr[inOff];
        outArr[outOff + 1] = inArr[inOff + 1];
        outArr[outOff + 2] = inArr[inOff + 2];
        outArr[outOff + 3] = inArr[inOff + 3];
        outArr[outOff + 4] = inArr[inOff + 4];
        outArr[outOff + 5] = inArr[inOff + 5];
        outArr[outOff + 6] = inArr[inOff + 6];
        outArr[outOff + 7] = inArr[inOff + 7];
        outArr[outOff + 8] = inArr[inOff + 8];
        outArr[outOff + 9] = inArr[inOff + 9];
        outArr[outOff + 10] = inArr[inOff + 10];
        outArr[outOff + 11] = inArr[inOff + 11];
        outArr[outOff + 12] = inArr[inOff + 12];
        outArr[outOff + 13] = inArr[inOff + 13];
        outArr[outOff + 14] = inArr[inOff + 14];
        outArr[outOff + 15] = inArr[inOff + 15];
    }

    // Token: 0x060000D2 RID: 210 RVA: 0x00004D78 File Offset: 0x00002F78
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void FastCopyMac(byte[] inArr, int inOff, byte[] outArr, int outOff)
    {
        outArr[outOff] = inArr[inOff];
        outArr[outOff + 1] = inArr[inOff + 1];
        outArr[outOff + 2] = inArr[inOff + 2];
        outArr[outOff + 3] = inArr[inOff + 3];
        outArr[outOff + 4] = inArr[inOff + 4];
        outArr[outOff + 5] = inArr[inOff + 5];
        outArr[outOff + 6] = inArr[inOff + 6];
        outArr[outOff + 7] = inArr[inOff + 7];
        outArr[outOff + 8] = inArr[inOff + 8];
        outArr[outOff + 9] = inArr[inOff + 9];
    }

    // Token: 0x060000D3 RID: 211 RVA: 0x00004DE7 File Offset: 0x00002FE7
    [Conditional("VERBOSE_LOGGING")]
    public static void Log(string message)
    {
        BGNet.Logging.Debug.Log("[Encryption] " + message);
    }

    // Token: 0x060000D4 RID: 212 RVA: 0x00004DE7 File Offset: 0x00002FE7
    [Conditional("VERBOSE_VERBOSE_LOGGING")]
    public static void LogV(string message)
    {
        BGNet.Logging.Debug.Log("[Encryption] " + message);
    }

    // Token: 0x04000084 RID: 132
    private const int kMacHashSize = 10;

    // Token: 0x04000085 RID: 133
    private const int kSequenceNumberSize = 4;

    // Token: 0x04000086 RID: 134
    private const int kMaxPadding = 32;

    // Token: 0x04000087 RID: 135
    private const int kMacKeySize = 64;

    // Token: 0x04000088 RID: 136
    private const int kKeySize = 32;

    // Token: 0x04000089 RID: 137
    private const int kBlockSize = 16;

    // Token: 0x0400008A RID: 138
    private const int kIvSize = 16;

    // Token: 0x0400008B RID: 139
    public const int kMasterKeySize = 48;

    // Token: 0x0400008C RID: 140
    public const int kRandomBlockSize = 32;

    // Token: 0x0400008D RID: 141
    public const int kExtraSize = 62;

    // Token: 0x0400008E RID: 142
    private static byte[] _masterSecretSeed = Encoding.UTF8.GetBytes("master secret");

    // Token: 0x0400008F RID: 143
    private static byte[] _keyExpansionSeed = Encoding.UTF8.GetBytes("key expansion");

    // Token: 0x04000090 RID: 144
    [ThreadStatic]
    private static byte[] _tempIV;

    // Token: 0x04000091 RID: 145
    [ThreadStatic]
    private static byte[] _tempHash;

    // Token: 0x04000092 RID: 146
    private static AesCryptoServiceProvider _aes = new AesCryptoServiceProvider
    {
        Mode = CipherMode.CBC,
        Padding = PaddingMode.None
    };

    // Token: 0x020000DD RID: 221
    public interface IEncryptionState : IDisposable
    {
        // Token: 0x1700014A RID: 330
        // (get) Token: 0x0600077D RID: 1917
        bool isValid { get; }

        // Token: 0x0600077E RID: 1918
        void EncryptData(byte[] data, ref int offset, ref int length, int extraPrefixBytes = 0);

        // Token: 0x0600077F RID: 1919
        bool TryDecryptData(byte[] data, ref int offset, ref int length);
    }

    // Token: 0x020000DE RID: 222
    private class EncryptionState : EncryptionUtility.IEncryptionState, IDisposable
    {
        // Token: 0x1700014B RID: 331
        // (get) Token: 0x06000780 RID: 1920 RVA: 0x00013DD4 File Offset: 0x00011FD4
        public bool isValid
        {
            get
            {
                return this._isValid;
            }
        }

        // Token: 0x06000781 RID: 1921 RVA: 0x00013DDE File Offset: 0x00011FDE
        public void EncryptData(byte[] data, ref int offset, ref int length, int extraPrefixBytes = 0)
        {
            EncryptionUtility.EncryptData(this, data, ref offset, ref length, extraPrefixBytes);
        }

        // Token: 0x06000782 RID: 1922 RVA: 0x00013DEB File Offset: 0x00011FEB
        public bool TryDecryptData(byte[] data, ref int offset, ref int length)
        {
            return EncryptionUtility.TryDecryptData(this, data, ref offset, ref length);
        }

        // Token: 0x06000783 RID: 1923 RVA: 0x00013DF8 File Offset: 0x00011FF8
        public byte[] ComputeSendMac(byte[] data, int offset, int count)
        {
            HMac hmac;
            if (!this._sendMacQueue.TryDequeue(out hmac))
            {
                hmac = new HMac(new Sha256Digest());
                hmac.Init(new KeyParameter(this._sendMacKey));
            }
            if (EncryptionUtility._tempHash == null)
            {
                EncryptionUtility._tempHash = new byte[64];
            }
            hmac.BlockUpdate(data, offset, count);
            hmac.DoFinal(EncryptionUtility._tempHash, 0);
            this._sendMacQueue.Enqueue(hmac);
            return EncryptionUtility._tempHash;
        }

        // Token: 0x06000784 RID: 1924 RVA: 0x00013E6C File Offset: 0x0001206C
        public byte[] ComputeReceiveMac(byte[] data, int offset, int count)
        {
            HMac hmac;
            if (!this._receiveMacQueue.TryDequeue(out hmac))
            {
                hmac = new HMac(new Sha256Digest());
                hmac.Init(new KeyParameter(this._receiveMacKey));
            }
            if (EncryptionUtility._tempHash == null)
            {
                EncryptionUtility._tempHash = new byte[64];
            }
            hmac.BlockUpdate(data, offset, count);
            hmac.DoFinal(EncryptionUtility._tempHash, 0);
            this._receiveMacQueue.Enqueue(hmac);
            return EncryptionUtility._tempHash;
        }

        // Token: 0x06000785 RID: 1925 RVA: 0x00013EE0 File Offset: 0x000120E0
        public bool IsValidSequenceNum(uint sequenceNum)
        {
            bool[] receivedSequenceNumBuffer = this._receivedSequenceNumBuffer;
            bool result;
            lock (receivedSequenceNumBuffer)
            {
                if (!this._hasReceivedSequenceNum)
                {
                    result = true;
                }
                else if (sequenceNum > this._lastReceivedSequenceNum)
                {
                    result = true;
                }
                else if (sequenceNum + 64U <= this._lastReceivedSequenceNum)
                {
                    result = false;
                }
                else
                {
                    result = !this._receivedSequenceNumBuffer[(int)(sequenceNum % 64U)];
                }
            }
            return result;
        }

        // Token: 0x06000786 RID: 1926 RVA: 0x00013F54 File Offset: 0x00012154
        public bool PutSequenceNum(uint sequenceNum)
        {
            bool[] receivedSequenceNumBuffer = this._receivedSequenceNumBuffer;
            bool result;
            lock (receivedSequenceNumBuffer)
            {
                if (!this._hasReceivedSequenceNum)
                {
                    this._hasReceivedSequenceNum = true;
                    this._lastReceivedSequenceNum = sequenceNum;
                }
                else if (sequenceNum > this._lastReceivedSequenceNum)
                {
                    int num = (int)(sequenceNum - this._lastReceivedSequenceNum);
                    if (num >= 64)
                    {
                        Array.Clear(this._receivedSequenceNumBuffer, 0, 64);
                    }
                    else
                    {
                        for (int i = 1; i < num; i++)
                        {
                            this._receivedSequenceNumBuffer[(int)(checked((IntPtr)(unchecked((ulong)this._lastReceivedSequenceNum + (ulong)((long)i)) % 64UL)))] = false;
                        }
                    }
                    this._lastReceivedSequenceNum = sequenceNum;
                }
                else
                {
                    if (sequenceNum + 64U <= this._lastReceivedSequenceNum)
                    {
                        return false;
                    }
                    if (this._receivedSequenceNumBuffer[(int)(sequenceNum % 64U)])
                    {
                        return false;
                    }
                }
                this._receivedSequenceNumBuffer[(int)(sequenceNum % 64U)] = true;
                result = true;
            }
            return result;
        }

        // Token: 0x06000787 RID: 1927 RVA: 0x00014030 File Offset: 0x00012230
        public uint GetNextSentSequenceNum()
        {
            return (uint)Interlocked.Increment(ref this._lastSentSequenceNum);
        }

        // Token: 0x06000788 RID: 1928 RVA: 0x00014040 File Offset: 0x00012240
        public EncryptionState(byte[] preMasterSecret, byte[] serverSeed, byte[] clientSeed, bool isClient)
        {
            byte[] sourceArray = EncryptionUtility.EncryptionState.PRF(EncryptionUtility.EncryptionState.PRF(preMasterSecret, EncryptionUtility.EncryptionState.MakeSeed(EncryptionUtility._masterSecretSeed, serverSeed, clientSeed), 48), EncryptionUtility.EncryptionState.MakeSeed(EncryptionUtility._keyExpansionSeed, serverSeed, clientSeed), 192);
            this.sendKey = new byte[32];
            this.receiveKey = new byte[32];
            this._sendMacKey = new byte[64];
            this._receiveMacKey = new byte[64];
            Array.Copy(sourceArray, 0, isClient ? this.receiveKey : this.sendKey, 0, 32);
            Array.Copy(sourceArray, 32, isClient ? this.sendKey : this.receiveKey, 0, 32);
            Array.Copy(sourceArray, 64, isClient ? this._receiveMacKey : this._sendMacKey, 0, 64);
            Array.Copy(sourceArray, 128, isClient ? this._sendMacKey : this._receiveMacKey, 0, 64);
        }

        // Token: 0x06000789 RID: 1929 RVA: 0x0001415C File Offset: 0x0001235C
        private static byte[] MakeSeed(byte[] baseSeed, byte[] serverSeed, byte[] clientSeed)
        {
            byte[] array = new byte[baseSeed.Length + serverSeed.Length + clientSeed.Length];
            Array.Copy(baseSeed, 0, array, 0, baseSeed.Length);
            Array.Copy(serverSeed, 0, array, baseSeed.Length, serverSeed.Length);
            Array.Copy(clientSeed, 0, array, baseSeed.Length + serverSeed.Length, clientSeed.Length);
            return array;
        }

        // Token: 0x0600078A RID: 1930 RVA: 0x000141A8 File Offset: 0x000123A8
        private static byte[] PRF(byte[] key, byte[] seed, int length)
        {
            int i = 0;
            byte[] array = new byte[length + seed.Length];
            while (i < length)
            {
                Array.Copy(seed, 0, array, i, seed.Length);
                EncryptionUtility.EncryptionState.PRF_Hash(key, array, ref i);
            }
            byte[] array2 = new byte[length];
            Array.Copy(array, 0, array2, 0, length);
            return array2;
        }

        // Token: 0x0600078B RID: 1931 RVA: 0x000141F0 File Offset: 0x000123F0
        private static void PRF_Hash(byte[] key, byte[] seed, ref int length)
        {
            using (HMACSHA256 hmacsha = new HMACSHA256(key))
            {
                byte[] array = hmacsha.ComputeHash(seed, 0, length);
                int num = Mathf.Min(length + array.Length, seed.Length);
                Array.Copy(array, 0, seed, length, num - length);
                length = num;
            }
        }

        // Token: 0x0600078C RID: 1932 RVA: 0x0001424C File Offset: 0x0001244C
        public void Dispose()
        {
            this._isValid = false;
        }

        // Token: 0x04000341 RID: 833
        private volatile bool _isValid = true;

        // Token: 0x04000342 RID: 834
        private const int kReceivedSequencNumBufferLength = 64;

        // Token: 0x04000343 RID: 835
        private int _lastSentSequenceNum = -1;

        // Token: 0x04000344 RID: 836
        private bool _hasReceivedSequenceNum;

        // Token: 0x04000345 RID: 837
        private uint _lastReceivedSequenceNum;

        // Token: 0x04000346 RID: 838
        private readonly bool[] _receivedSequenceNumBuffer = new bool[64];

        // Token: 0x04000347 RID: 839
        public readonly byte[] sendKey;

        // Token: 0x04000348 RID: 840
        public readonly byte[] receiveKey;

        // Token: 0x04000349 RID: 841
        private readonly byte[] _sendMacKey;

        // Token: 0x0400034A RID: 842
        private readonly byte[] _receiveMacKey;

        // Token: 0x0400034B RID: 843
        private readonly ConcurrentQueue<HMac> _sendMacQueue = new ConcurrentQueue<HMac>();

        // Token: 0x0400034C RID: 844
        private readonly ConcurrentQueue<HMac> _receiveMacQueue = new ConcurrentQueue<HMac>();
    }
}
