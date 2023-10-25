using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using BGNet.Core;
using BGNet.Logging;
using LiteNetLib.Layers;

// Token: 0x0200006A RID: 106
public class PacketEncryptionLayer : PacketLayerBase
{
    // Token: 0x170000C2 RID: 194
    // (get) Token: 0x060004A2 RID: 1186 RVA: 0x0000C1A5 File Offset: 0x0000A3A5
    // (set) Token: 0x060004A3 RID: 1187 RVA: 0x0000C1AD File Offset: 0x0000A3AD
    public bool filterUnencryptedTraffic { get; set; }

    // Token: 0x170000C3 RID: 195
    // (get) Token: 0x060004A4 RID: 1188 RVA: 0x0000C1B6 File Offset: 0x0000A3B6
    // (set) Token: 0x060004A5 RID: 1189 RVA: 0x0000C1BE File Offset: 0x0000A3BE
    public bool enableStatistics { get; set; }

    // Token: 0x060004A6 RID: 1190 RVA: 0x0000C1C7 File Offset: 0x0000A3C7
    public PacketEncryptionLayer(BGNet.Core.ITimeProvider timeProvider, ITaskUtility taskUtility) : base(63)
    {
        this._taskUtility = taskUtility;
        this._encryptionStates = new ExpiringDictionary<IPEndPoint, EncryptionUtility.IEncryptionState>(timeProvider, 300000L);
        this._pendingEncryptionStates = new ExpiringDictionary<IPAddress, PacketEncryptionLayer.PendingEncryptionStateList>(timeProvider, 10000L);
    }

    // Token: 0x060004A7 RID: 1191 RVA: 0x0000C208 File Offset: 0x0000A408
    public override void ProcessInboundPacket(IPEndPoint remoteEndPoint, ref byte[] data, ref int offset, ref int length)
    {
        Stopwatch stopwatch = null;
        if (this.enableStatistics)
        {
            stopwatch = PacketEncryptionLayer._stopwatch;
            if (stopwatch == null)
            {
                stopwatch = (PacketEncryptionLayer._stopwatch = new Stopwatch());
                stopwatch.Start();
            }
        }
        long num = (stopwatch != null) ? stopwatch.ElapsedTicks : 0L;
        bool flag2;
        bool flag = this.ProcessInboundPacketInternal(remoteEndPoint, ref data, ref offset, ref length, out flag2);
        long num2 = (stopwatch != null) ? stopwatch.ElapsedTicks : 0L;
        if (!flag)
        {
            length = 0;
        }
        if (!this.enableStatistics)
        {
            return;
        }
        if (flag && flag2)
        {
            this.statistics.IncrementPacketsReceivedEncrypted();
            this.statistics.AddDecryptionProcessingTime(num2 - num);
            return;
        }
        if (flag)
        {
            this.statistics.IncrementPacketsReceivedPlaintext();
            return;
        }
        this.statistics.IncrementPacketsReceivedRejected();
    }

    // Token: 0x060004A8 RID: 1192 RVA: 0x0000C2B0 File Offset: 0x0000A4B0
    public override void ProcessOutBoundPacket(IPEndPoint remoteEndPoint, ref byte[] data, ref int offset, ref int length)
    {
        Stopwatch stopwatch = null;
        if (this.enableStatistics)
        {
            stopwatch = PacketEncryptionLayer._stopwatch;
            if (stopwatch == null)
            {
                stopwatch = (PacketEncryptionLayer._stopwatch = new Stopwatch());
                stopwatch.Start();
            }
        }
        long num = (stopwatch != null) ? stopwatch.ElapsedTicks : 0L;
        bool flag2;
        bool flag = this.ProcessOutBoundPacketInternal(remoteEndPoint, ref data, ref offset, ref length, out flag2);
        if (!flag)
        {
            length = 0;
        }
        long num2 = (stopwatch != null) ? stopwatch.ElapsedTicks : 0L;
        if (!this.enableStatistics)
        {
            return;
        }
        if (flag && flag2)
        {
            this.statistics.IncrementPacketsSentEncrypted();
            this.statistics.AddEncryptionProcessingTime(num2 - num);
            return;
        }
        if (flag)
        {
            this.statistics.IncrementPacketsSentPlaintext();
            return;
        }
        this.statistics.IncrementPacketsSentRejected();
    }

    // Token: 0x060004A9 RID: 1193 RVA: 0x0000C357 File Offset: 0x0000A557
    public void SetUnencryptedTrafficFilter(byte[] unencryptedTrafficFilter)
    {
        this._unencryptedTrafficFilter = unencryptedTrafficFilter;
    }

    // Token: 0x060004AA RID: 1194 RVA: 0x0000C360 File Offset: 0x0000A560
    public EncryptionUtility.IEncryptionState AddEncryptedEndpoint(IPEndPoint endPoint, byte[] preMasterSecret, byte[] serverRandom, byte[] clientRandom, bool isClient)
    {
        EncryptionUtility.IEncryptionState encryptionState = EncryptionUtility.CreateEncryptionState(preMasterSecret, serverRandom, clientRandom, isClient);
        ExpiringDictionary<IPEndPoint, EncryptionUtility.IEncryptionState> encryptionStates = this._encryptionStates;
        lock (encryptionStates)
        {
            this._encryptionStates[endPoint] = encryptionState;
        }
        return encryptionState;
    }

    // Token: 0x060004AB RID: 1195 RVA: 0x0000C3B4 File Offset: 0x0000A5B4
    public async Task<EncryptionUtility.IEncryptionState> AddEncryptedEndpointAsync(IPEndPoint endPoint, byte[] preMasterSecret, byte[] serverRandom, byte[] clientRandom, bool isClient)
    {
        EncryptionUtility.IEncryptionState encryptionState = await EncryptionUtility.CreateEncryptionStateAsync(this._taskUtility, preMasterSecret, serverRandom, clientRandom, isClient);
        ExpiringDictionary<IPEndPoint, EncryptionUtility.IEncryptionState> encryptionStates = this._encryptionStates;
        lock (encryptionStates)
        {
            this._encryptionStates[endPoint] = encryptionState;
        }
        return encryptionState;
    }

    // Token: 0x060004AC RID: 1196 RVA: 0x0000C424 File Offset: 0x0000A624
    public bool RemoveEncryptedEndpoint(IPEndPoint endPoint, EncryptionUtility.IEncryptionState encryptedState = null)
    {
        ExpiringDictionary<IPEndPoint, EncryptionUtility.IEncryptionState> encryptionStates = this._encryptionStates;
        lock (encryptionStates)
        {
            EncryptionUtility.IEncryptionState encryptionState;
            if (encryptedState != null && (!this._encryptionStates.TryGetValue(endPoint, out encryptionState) || encryptionState != encryptedState))
            {
                return false;
            }
            if (this._encryptionStates.Remove(endPoint))
            {
                return true;
            }
        }
        ExpiringDictionary<IPAddress, PacketEncryptionLayer.PendingEncryptionStateList> pendingEncryptionStates = this._pendingEncryptionStates;
        bool result;
        lock (pendingEncryptionStates)
        {
            PacketEncryptionLayer.PendingEncryptionStateList pendingEncryptionStateList;
            if (!this._pendingEncryptionStates.TryGetValue(endPoint.Address, out pendingEncryptionStateList))
            {
                result = false;
            }
            else
            {
                bool flag2 = pendingEncryptionStateList.Remove(endPoint.Port);
                if (pendingEncryptionStateList.isEmpty)
                {
                    this._pendingEncryptionStates.Remove(endPoint.Address);
                }
                result = flag2;
            }
        }
        return result;
    }

    // Token: 0x060004AD RID: 1197 RVA: 0x0000C500 File Offset: 0x0000A700
    public async Task AddPendingEncryptedEndpointAsync(IPEndPoint endPoint, byte[] preMasterSecret, byte[] serverRandom, byte[] clientRandom, bool isClient)
    {
        EncryptionUtility.IEncryptionState encryptionState = await EncryptionUtility.CreateEncryptionStateAsync(this._taskUtility, preMasterSecret, serverRandom, clientRandom, isClient);
        ExpiringDictionary<IPAddress, PacketEncryptionLayer.PendingEncryptionStateList> pendingEncryptionStates = this._pendingEncryptionStates;
        lock (pendingEncryptionStates)
        {
            PacketEncryptionLayer.PendingEncryptionStateList pendingEncryptionStateList;
            if (!this._pendingEncryptionStates.TryGetValue(endPoint.Address, out pendingEncryptionStateList))
            {
                pendingEncryptionStateList = new PacketEncryptionLayer.PendingEncryptionStateList();
                this._pendingEncryptionStates[endPoint.Address] = pendingEncryptionStateList;
            }
            else
            {
                this._pendingEncryptionStates.ResetExpiration(endPoint.Address);
            }
            pendingEncryptionStateList.Add(endPoint.Port, encryptionState);
        }
    }

    // Token: 0x060004AE RID: 1198 RVA: 0x0000C570 File Offset: 0x0000A770
    public void PollUpdate()
    {
        ExpiringDictionary<IPEndPoint, EncryptionUtility.IEncryptionState> encryptionStates = this._encryptionStates;
        lock (encryptionStates)
        {
            this._encryptionStates.PollUpdate();
        }
        ExpiringDictionary<IPAddress, PacketEncryptionLayer.PendingEncryptionStateList> pendingEncryptionStates = this._pendingEncryptionStates;
        lock (pendingEncryptionStates)
        {
            this._pendingEncryptionStates.PollUpdate();
        }
    }

    // Token: 0x060004AF RID: 1199 RVA: 0x0000C5EC File Offset: 0x0000A7EC
    public void RemoveAllEndpoints()
    {
        ExpiringDictionary<IPEndPoint, EncryptionUtility.IEncryptionState> encryptionStates = this._encryptionStates;
        lock (encryptionStates)
        {
            this._encryptionStates.Dispose();
        }
        ExpiringDictionary<IPAddress, PacketEncryptionLayer.PendingEncryptionStateList> pendingEncryptionStates = this._pendingEncryptionStates;
        lock (pendingEncryptionStates)
        {
            this._pendingEncryptionStates.Dispose();
        }
    }

    // Token: 0x060004B0 RID: 1200 RVA: 0x0000C668 File Offset: 0x0000A868
    private bool TryGetEncryptionState(IPEndPoint endPoint, out EncryptionUtility.IEncryptionState state)
    {
        state = null;
        if (endPoint == null)
        {
            return false;
        }
        ExpiringDictionary<IPEndPoint, EncryptionUtility.IEncryptionState> encryptionStates = this._encryptionStates;
        bool result;
        lock (encryptionStates)
        {
            result = this._encryptionStates.TryGetValueAndResetExpiration(endPoint, out state);
        }
        return result;
    }

    // Token: 0x060004B1 RID: 1201 RVA: 0x0000C6BC File Offset: 0x0000A8BC
    private bool TryGetPendingEncryptionState(IPEndPoint endPoint, out EncryptionUtility.IEncryptionState state)
    {
        state = null;
        if (endPoint == null)
        {
            return false;
        }
        ExpiringDictionary<IPAddress, PacketEncryptionLayer.PendingEncryptionStateList> pendingEncryptionStates = this._pendingEncryptionStates;
        bool result;
        lock (pendingEncryptionStates)
        {
            PacketEncryptionLayer.PendingEncryptionStateList pendingEncryptionStateList;
            if (!this._pendingEncryptionStates.TryGetValue(endPoint.Address, out pendingEncryptionStateList))
            {
                result = false;
            }
            else
            {
                result = pendingEncryptionStateList.TryGetEncryptionState(endPoint.Port, out state);
            }
        }
        return result;
    }

    // Token: 0x060004B2 RID: 1202 RVA: 0x0000C728 File Offset: 0x0000A928
    private bool TryGetPotentialPendingEncryptionStates(IPEndPoint endPoint, out EncryptionUtility.IEncryptionState[] encryptionStates)
    {
        encryptionStates = null;
        ExpiringDictionary<IPAddress, PacketEncryptionLayer.PendingEncryptionStateList> pendingEncryptionStates = this._pendingEncryptionStates;
        bool result;
        lock (pendingEncryptionStates)
        {
            PacketEncryptionLayer.PendingEncryptionStateList pendingEncryptionStateList;
            if (!this._pendingEncryptionStates.TryGetValue(endPoint.Address, out pendingEncryptionStateList))
            {
                result = false;
            }
            else
            {
                encryptionStates = pendingEncryptionStateList.GetSortedEncryptionStates(endPoint.Port);
                result = true;
            }
        }
        return result;
    }

    // Token: 0x060004B3 RID: 1203 RVA: 0x0000C790 File Offset: 0x0000A990
    private void PromotePendingEncryptionState(IPEndPoint endPoint, EncryptionUtility.IEncryptionState state)
    {
        ExpiringDictionary<IPEndPoint, EncryptionUtility.IEncryptionState> encryptionStates = this._encryptionStates;
        lock (encryptionStates)
        {
            if (this._encryptionStates.ContainsKey(endPoint))
            {
                return;
            }
            this._encryptionStates[endPoint] = state;
        }
        ExpiringDictionary<IPAddress, PacketEncryptionLayer.PendingEncryptionStateList> pendingEncryptionStates = this._pendingEncryptionStates;
        lock (pendingEncryptionStates)
        {
            PacketEncryptionLayer.PendingEncryptionStateList pendingEncryptionStateList;
            if (this._pendingEncryptionStates.TryGetValue(endPoint.Address, out pendingEncryptionStateList))
            {
                pendingEncryptionStateList.Remove(endPoint.Port, state);
                if (pendingEncryptionStateList.isEmpty)
                {
                    this._pendingEncryptionStates.Remove(endPoint.Address);
                }
            }
        }
    }

    // Token: 0x060004B4 RID: 1204 RVA: 0x0000C850 File Offset: 0x0000AA50
    private bool MatchesFilter(byte[] data, int offset, int length)
    {
        if (this._unencryptedTrafficFilter == null)
        {
            return false;
        }
        if (length < this._unencryptedTrafficFilter.Length)
        {
            return false;
        }
        for (int i = 0; i < this._unencryptedTrafficFilter.Length; i++)
        {
            if (data[offset + i] != this._unencryptedTrafficFilter[i])
            {
                return false;
            }
        }
        return true;
    }

    // Token: 0x060004B5 RID: 1205 RVA: 0x0000C89C File Offset: 0x0000AA9C
    private bool ProcessInboundPacketInternal(IPEndPoint remoteEndPoint, ref byte[] data, ref int offset, ref int length, out bool encrypted)
    {
        encrypted = false;
        if (length == 0)
        {
            return false;
        }
        if (remoteEndPoint == null)
        {
            return true;
        }
        byte b = data[offset];
        offset++;
        length--;
        if (b == 0)
        {
            return !this.filterUnencryptedTraffic || this.MatchesFilter(data, offset, length);
        }
        if (b != 1)
        {
            return false;
        }
        encrypted = true;
        if (!EncryptionUtility.IsValidLength(length))
        {
            return false;
        }
        EncryptionUtility.IEncryptionState encryptionState;
        if (this.TryGetEncryptionState(remoteEndPoint, out encryptionState))
        {
            return encryptionState.TryDecryptData(data, ref offset, ref length);
        }
        EncryptionUtility.IEncryptionState[] array;
        if (!this.TryGetPotentialPendingEncryptionStates(remoteEndPoint, out array))
        {
            return false;
        }
        byte[] array2 = new byte[length];
        foreach (EncryptionUtility.IEncryptionState encryptionState in array)
        {
            Array.Copy(data, offset, array2, 0, length);
            int sourceIndex = 0;
            int num = length;
            if (encryptionState.TryDecryptData(array2, ref sourceIndex, ref num))
            {
                Array.Copy(array2, sourceIndex, data, offset, num);
                length = num;
                this.PromotePendingEncryptionState(remoteEndPoint, encryptionState);
                return true;
            }
        }
        return false;
    }

    // Token: 0x060004B6 RID: 1206 RVA: 0x0000C98C File Offset: 0x0000AB8C
    public bool ProcessOutBoundPacketInternal(IPEndPoint remoteEndPoint, ref byte[] data, ref int offset, ref int length, out bool encrypted)
    {
        encrypted = false;
        bool flag = this.MatchesFilter(data, offset, length);
        EncryptionUtility.IEncryptionState encryptionState;
        if (!flag && (this.TryGetEncryptionState(remoteEndPoint, out encryptionState) || this.TryGetPendingEncryptionState(remoteEndPoint, out encryptionState)))
        {
            encrypted = true;
            encryptionState.EncryptData(data, ref offset, ref length, 1);
            offset--;
            length++;
            data[offset] = 1;
            return true;
        }
        if (this.filterUnencryptedTraffic && !flag)
        {
            return false;
        }
        if (offset == 0)
        {
            Array.Copy(data, offset, data, offset + 1, length);
        }
        else
        {
            offset--;
        }
        length++;
        data[offset] = 0;
        return true;
    }

    // Token: 0x060004B7 RID: 1207 RVA: 0x00004DE7 File Offset: 0x00002FE7
    [Conditional("VERBOSE_LOGGING")]
    public static void Log(string message)
    {
        BGNet.Logging.Debug.Log("[Encryption] " + message);
    }

    // Token: 0x060004B8 RID: 1208 RVA: 0x00004DE7 File Offset: 0x00002FE7
    [Conditional("VERBOSE_VERBOSE_LOGGING")]
    public static void LogV(string message)
    {
        BGNet.Logging.Debug.Log("[Encryption] " + message);
    }

    // Token: 0x040001C1 RID: 449
    private const byte kEncryptedPacketType = 1;

    // Token: 0x040001C2 RID: 450
    private const byte kPlaintextPacketType = 0;

    // Token: 0x040001C3 RID: 451
    private const long kEncryptionStateTimeoutMs = 300000L;

    // Token: 0x040001C4 RID: 452
    private const long kPendingEncryptionStateTimeoutMs = 10000L;

    // Token: 0x040001C5 RID: 453
    [ThreadStatic]
    private static Stopwatch _stopwatch;

    // Token: 0x040001C6 RID: 454
    public readonly PacketEncryptionLayer.EncryptionStatistics statistics = new PacketEncryptionLayer.EncryptionStatistics();

    // Token: 0x040001C7 RID: 455
    private readonly ITaskUtility _taskUtility;

    // Token: 0x040001C8 RID: 456
    private readonly ExpiringDictionary<IPEndPoint, EncryptionUtility.IEncryptionState> _encryptionStates;

    // Token: 0x040001C9 RID: 457
    private readonly ExpiringDictionary<IPAddress, PacketEncryptionLayer.PendingEncryptionStateList> _pendingEncryptionStates;

    // Token: 0x040001CA RID: 458
    private byte[] _unencryptedTrafficFilter;

    // Token: 0x0200013E RID: 318
    public class EncryptionStatistics
    {
        // Token: 0x17000159 RID: 345
        // (get) Token: 0x06000816 RID: 2070 RVA: 0x00015001 File Offset: 0x00013201
        public long packetsReceivedPlaintext
        {
            get
            {
                return Interlocked.Read(ref this._packetsReceivedPlaintext);
            }
        }

        // Token: 0x1700015A RID: 346
        // (get) Token: 0x06000817 RID: 2071 RVA: 0x0001500E File Offset: 0x0001320E
        public long packetsReceivedEncrypted
        {
            get
            {
                return Interlocked.Read(ref this._packetsReceivedEncrypted);
            }
        }

        // Token: 0x1700015B RID: 347
        // (get) Token: 0x06000818 RID: 2072 RVA: 0x0001501B File Offset: 0x0001321B
        public long packetsReceivedRejected
        {
            get
            {
                return Interlocked.Read(ref this._packetsReceivedRejected);
            }
        }

        // Token: 0x1700015C RID: 348
        // (get) Token: 0x06000819 RID: 2073 RVA: 0x00015028 File Offset: 0x00013228
        public long packetsSentPlaintext
        {
            get
            {
                return Interlocked.Read(ref this._packetsSentPlaintext);
            }
        }

        // Token: 0x1700015D RID: 349
        // (get) Token: 0x0600081A RID: 2074 RVA: 0x00015035 File Offset: 0x00013235
        public long packetsSentEncrypted
        {
            get
            {
                return Interlocked.Read(ref this._packetsSentEncrypted);
            }
        }

        // Token: 0x1700015E RID: 350
        // (get) Token: 0x0600081B RID: 2075 RVA: 0x00015042 File Offset: 0x00013242
        public long packetsSentRejected
        {
            get
            {
                return Interlocked.Read(ref this._packetsSentRejected);
            }
        }

        // Token: 0x1700015F RID: 351
        // (get) Token: 0x0600081C RID: 2076 RVA: 0x0001504F File Offset: 0x0001324F
        public long encryptionProcessingTime
        {
            get
            {
                return Interlocked.Read(ref this._encryptionProcessingTime) * 1000L / Stopwatch.Frequency;
            }
        }

        // Token: 0x17000160 RID: 352
        // (get) Token: 0x0600081D RID: 2077 RVA: 0x00015069 File Offset: 0x00013269
        public long decryptionProcessingTime
        {
            get
            {
                return Interlocked.Read(ref this._decryptionProcessingTime) * 1000L / Stopwatch.Frequency;
            }
        }

        // Token: 0x0600081E RID: 2078 RVA: 0x00015083 File Offset: 0x00013283
        public void IncrementPacketsReceivedPlaintext()
        {
            Interlocked.Increment(ref this._packetsReceivedPlaintext);
        }

        // Token: 0x0600081F RID: 2079 RVA: 0x00015091 File Offset: 0x00013291
        public void IncrementPacketsReceivedEncrypted()
        {
            Interlocked.Increment(ref this._packetsReceivedEncrypted);
        }

        // Token: 0x06000820 RID: 2080 RVA: 0x0001509F File Offset: 0x0001329F
        public void IncrementPacketsReceivedRejected()
        {
            Interlocked.Increment(ref this._packetsReceivedRejected);
        }

        // Token: 0x06000821 RID: 2081 RVA: 0x000150AD File Offset: 0x000132AD
        public void IncrementPacketsSentPlaintext()
        {
            Interlocked.Increment(ref this._packetsSentPlaintext);
        }

        // Token: 0x06000822 RID: 2082 RVA: 0x000150BB File Offset: 0x000132BB
        public void IncrementPacketsSentEncrypted()
        {
            Interlocked.Increment(ref this._packetsSentEncrypted);
        }

        // Token: 0x06000823 RID: 2083 RVA: 0x000150C9 File Offset: 0x000132C9
        public void IncrementPacketsSentRejected()
        {
            Interlocked.Increment(ref this._packetsSentRejected);
        }

        // Token: 0x06000824 RID: 2084 RVA: 0x000150D7 File Offset: 0x000132D7
        public void AddEncryptionProcessingTime(long time)
        {
            Interlocked.Add(ref this._encryptionProcessingTime, time);
        }

        // Token: 0x06000825 RID: 2085 RVA: 0x000150E6 File Offset: 0x000132E6
        public void AddDecryptionProcessingTime(long time)
        {
            Interlocked.Add(ref this._decryptionProcessingTime, time);
        }

        // Token: 0x04000415 RID: 1045
        private long _packetsReceivedPlaintext;

        // Token: 0x04000416 RID: 1046
        private long _packetsReceivedEncrypted;

        // Token: 0x04000417 RID: 1047
        private long _packetsReceivedRejected;

        // Token: 0x04000418 RID: 1048
        private long _packetsSentPlaintext;

        // Token: 0x04000419 RID: 1049
        private long _packetsSentEncrypted;

        // Token: 0x0400041A RID: 1050
        private long _packetsSentRejected;

        // Token: 0x0400041B RID: 1051
        private long _encryptionProcessingTime;

        // Token: 0x0400041C RID: 1052
        private long _decryptionProcessingTime;
    }

    // Token: 0x0200013F RID: 319
    private class PendingEncryptionStateList : IDisposable
    {
        // Token: 0x17000161 RID: 353
        // (get) Token: 0x06000827 RID: 2087 RVA: 0x000150F5 File Offset: 0x000132F5
        public bool isEmpty
        {
            get
            {
                return this._pendingStatesByPort.Count == 0;
            }
        }

        // Token: 0x06000828 RID: 2088 RVA: 0x00015108 File Offset: 0x00013308
        public void Dispose()
        {
            foreach (EncryptionUtility.IEncryptionState encryptionState in this._pendingStatesByPort.Values)
            {
                encryptionState.Dispose();
            }
            this._pendingStatesByPort.Clear();
        }

        // Token: 0x06000829 RID: 2089 RVA: 0x00015168 File Offset: 0x00013368
        public EncryptionUtility.IEncryptionState[] GetSortedEncryptionStates(int port)
        {
            return (from kvp in this._pendingStatesByPort
                    orderby Math.Abs(kvp.Key - port)
                    select kvp.Value).ToArray<EncryptionUtility.IEncryptionState>();
        }

        // Token: 0x0600082A RID: 2090 RVA: 0x000151C4 File Offset: 0x000133C4
        public bool TryGetEncryptionState(int port, out EncryptionUtility.IEncryptionState encryptionState)
        {
            if (this._pendingStatesByPort.TryGetValue(port, out encryptionState))
            {
                return true;
            }
            int num = int.MaxValue;
            foreach (KeyValuePair<int, EncryptionUtility.IEncryptionState> keyValuePair in this._pendingStatesByPort)
            {
                int num2 = Math.Abs(keyValuePair.Key - port);
                if (num2 < num)
                {
                    num = num2;
                    encryptionState = keyValuePair.Value;
                }
            }
            return encryptionState != null;
        }

        // Token: 0x0600082B RID: 2091 RVA: 0x0001524C File Offset: 0x0001344C
        public void Add(int port, EncryptionUtility.IEncryptionState encryptionState)
        {
            this._pendingStatesByPort[port] = encryptionState;
        }

        // Token: 0x0600082C RID: 2092 RVA: 0x0001525B File Offset: 0x0001345B
        public bool Remove(int port)
        {
            return this._pendingStatesByPort.Remove(port);
        }

        // Token: 0x0600082D RID: 2093 RVA: 0x0001526C File Offset: 0x0001346C
        public bool Remove(int port, EncryptionUtility.IEncryptionState encryptionState)
        {
            EncryptionUtility.IEncryptionState encryptionState2;
            if (this._pendingStatesByPort.TryGetValue(port, out encryptionState2) && encryptionState == encryptionState2)
            {
                return this._pendingStatesByPort.Remove(port);
            }
            KeyValuePair<int, EncryptionUtility.IEncryptionState> keyValuePair = this._pendingStatesByPort.FirstOrDefault((KeyValuePair<int, EncryptionUtility.IEncryptionState> kvp) => kvp.Value == encryptionState);
            return keyValuePair.Key != 0 && this._pendingStatesByPort.Remove(keyValuePair.Key);
        }

        // Token: 0x0400041D RID: 1053
        private readonly Dictionary<int, EncryptionUtility.IEncryptionState> _pendingStatesByPort = new Dictionary<int, EncryptionUtility.IEncryptionState>();
    }
}
