using System;
using System.Collections.Generic;
using System.Diagnostics;
using BGNet.Logging;
using LiteNetLib.Utils;

// Token: 0x02000065 RID: 101
public class NetworkPacketSerializer<TType, TData> : INetworkPacketSerializer<TData>, INetworkPacketSubSerializer<TData> where TType : struct, IConvertible
{
    // Token: 0x06000472 RID: 1138 RVA: 0x0000B4DC File Offset: 0x000096DC
    public void RegisterCallback<TPacket>(TType packetType, Action<TPacket> callback) where TPacket : INetSerializable, new()
    {
        this.RegisterCallback<TPacket>(packetType, delegate (TPacket packet, TData data)
        {
            Action<TPacket> callback2 = callback;
            if (callback2 == null)
            {
                return;
            }
            callback2(packet);
        });
    }

    // Token: 0x06000473 RID: 1139 RVA: 0x0000B50C File Offset: 0x0000970C
    public void RegisterCallback<TPacket>(TType packetType, Action<TPacket> callback, Func<TPacket> constructor) where TPacket : INetSerializable
    {
        this.RegisterCallback<TPacket>(packetType, delegate (TPacket packet, TData data)
        {
            Action<TPacket> callback2 = callback;
            if (callback2 == null)
            {
                return;
            }
            callback2(packet);
        }, (TData data) => constructor());
    }

    // Token: 0x06000474 RID: 1140 RVA: 0x0000B54C File Offset: 0x0000974C
    public void RegisterCallback<TPacket>(TType packetType, Action<TPacket, TData> callback) where TPacket : INetSerializable, new()
    {
        this.RegisterCallback<TPacket>(packetType, callback, (TData data) => Activator.CreateInstance<TPacket>());
    }

    // Token: 0x06000475 RID: 1141 RVA: 0x0000B578 File Offset: 0x00009778
    public void RegisterCallback<TPacket>(TType packetType, Action<TPacket, TData> callback, Func<TPacket> constructor) where TPacket : INetSerializable
    {
        this.RegisterCallback<TPacket>(packetType, callback, (TData data) => constructor());
    }

    // Token: 0x06000476 RID: 1142 RVA: 0x0000B5A8 File Offset: 0x000097A8
    public void RegisterCallback<TPacket>(TType packetType, Action<TPacket, TData> callback, Func<TData, TPacket> constructor) where TPacket : INetSerializable
    {
        byte b = (byte)Convert.ChangeType(packetType, typeof(byte));
        this._typeRegistry[typeof(TPacket)] = b;
        Func<NetDataReader, int, TData, TPacket> deserialize = delegate (NetDataReader reader, int size, TData data)
        {
            TPacket tpacket = constructor(data);
            if (tpacket == null)
            {
                BGNet.Logging.Debug.LogError("Constructor for " + typeof(TPacket) + " returned null!");
                reader.SkipBytes(size);
            }
            else
            {
                tpacket.Deserialize(reader);
            }
            return tpacket;
        };
        this._messsageHandlers[b] = delegate (NetDataReader reader, int size, TData data)
        {
            callback(deserialize(reader, size, data), data);
        };
    }

    // Token: 0x06000477 RID: 1143 RVA: 0x0000B624 File Offset: 0x00009824
    public void UnregisterCallback<TPacket>(TType packetType)
    {
        byte key = (byte)((object)packetType);
        this._typeRegistry.Remove(typeof(TPacket));
        this._messsageHandlers.Remove(key);
    }

    // Token: 0x06000478 RID: 1144 RVA: 0x0000B660 File Offset: 0x00009860
    public void RegisterSubSerializer(TType packetType, INetworkPacketSubSerializer<TData> subSubSerializer)
    {
        byte b = (byte)((object)packetType);
        this._subSerializerRegistry[subSubSerializer] = b;
        this._messsageHandlers[b] = delegate (NetDataReader reader, int size, TData data)
        {
            subSubSerializer.Deserialize(reader, size, data);
        };
    }

    // Token: 0x06000479 RID: 1145 RVA: 0x0000B6B0 File Offset: 0x000098B0
    public void UnregisterSubSerializer(TType packetType, INetworkPacketSubSerializer<TData> subSubSerializer)
    {
        byte key = (byte)((object)packetType);
        this._subSerializerRegistry.Remove(subSubSerializer);
        this._messsageHandlers.Remove(key);
    }

    // Token: 0x0600047A RID: 1146 RVA: 0x0000B6E4 File Offset: 0x000098E4
    public void CopyFrom(NetworkPacketSerializer<TType, TData> other)
    {
        foreach (KeyValuePair<Type, byte> keyValuePair in other._typeRegistry)
        {
            this._typeRegistry[keyValuePair.Key] = keyValuePair.Value;
        }
        foreach (KeyValuePair<byte, Action<NetDataReader, int, TData>> keyValuePair2 in other._messsageHandlers)
        {
            this._messsageHandlers[keyValuePair2.Key] = keyValuePair2.Value;
        }
        foreach (KeyValuePair<INetworkPacketSubSerializer<TData>, byte> keyValuePair3 in other._subSerializerRegistry)
        {
            this._subSerializerRegistry[keyValuePair3.Key] = keyValuePair3.Value;
        }
    }

    // Token: 0x0600047B RID: 1147 RVA: 0x0000B7F4 File Offset: 0x000099F4
    public void SerializePacket(NetDataWriter writer, INetSerializable packet)
    {
        this.SerializePacketInternal(writer, packet, true);
    }

    // Token: 0x0600047C RID: 1148 RVA: 0x0000B800 File Offset: 0x00009A00
    private void SerializePacketInternal(NetDataWriter externalWriter, INetSerializable packet, bool prependLength)
    {
        byte value;
        INetworkPacketSubSerializer<TData> networkPacketSubSerializer;
        if (!this.TryGetPacketType(packet.GetType(), out value, out networkPacketSubSerializer))
        {
            return;
        }
        NetDataWriter netDataWriter = prependLength ? this._internalWriter : externalWriter;
        netDataWriter.Put(value);
        if (networkPacketSubSerializer != null)
        {
            networkPacketSubSerializer.Serialize(netDataWriter, packet);
        }
        else
        {
            packet.Serialize(netDataWriter);
        }
        if (prependLength)
        {
            externalWriter.PutVarUInt((uint)this._internalWriter.Length);
            externalWriter.Put(this._internalWriter.Data, 0, this._internalWriter.Length);
            this._internalWriter.Reset();
        }
    }

    // Token: 0x0600047D RID: 1149 RVA: 0x0000B884 File Offset: 0x00009A84
    public void ProcessAllPackets(NetDataReader reader, TData data)
    {
        while (this.ProcessPacket(reader, data))
        {
        }
    }

    // Token: 0x0600047E RID: 1150 RVA: 0x0000B890 File Offset: 0x00009A90
    public bool ProcessPacket(NetDataReader reader, TData data)
    {
        if (reader.EndOfData)
        {
            return false;
        }
        int varUInt = (int)reader.GetVarUInt();
        this.ProcessPacketInternal(reader, varUInt, data);
        return true;
    }

    // Token: 0x0600047F RID: 1151 RVA: 0x0000B8B8 File Offset: 0x00009AB8
    private void ProcessPacketInternal(NetDataReader reader, int length, TData data)
    {
        byte @byte = reader.GetByte();
        length--;
        Action<NetDataReader, int, TData> action;
        if (this._messsageHandlers.TryGetValue(@byte, out action))
        {
            if (action != null)
            {
                action(reader, length, data);
                return;
            }
        }
        else
        {
            reader.SkipBytes(length);
        }
    }

    // Token: 0x06000480 RID: 1152 RVA: 0x0000B8F8 File Offset: 0x00009AF8
    private bool TryGetPacketType(Type type, out byte packetType, out INetworkPacketSubSerializer<TData> subSerializer)
    {
        packetType = 0;
        subSerializer = null;
        while (type != null)
        {
            if (this._typeRegistry.TryGetValue(type, out packetType))
            {
                return true;
            }
            foreach (KeyValuePair<INetworkPacketSubSerializer<TData>, byte> keyValuePair in this._subSerializerRegistry)
            {
                if (keyValuePair.Key.HandlesType(type))
                {
                    subSerializer = keyValuePair.Key;
                    packetType = keyValuePair.Value;
                    return true;
                }
            }
            type = type.BaseType;
        }
        return false;
    }

    // Token: 0x06000481 RID: 1153 RVA: 0x0000B998 File Offset: 0x00009B98
    public bool HandlesType(Type type)
    {
        byte b;
        INetworkPacketSubSerializer<TData> networkPacketSubSerializer;
        return this.TryGetPacketType(type, out b, out networkPacketSubSerializer);
    }

    // Token: 0x06000482 RID: 1154 RVA: 0x0000B9B0 File Offset: 0x00009BB0
    void INetworkPacketSubSerializer<TData>.Serialize(NetDataWriter writer, INetSerializable packet)

    {
        this.SerializePacketInternal(writer, packet, false);
    }

    // Token: 0x06000483 RID: 1155 RVA: 0x0000B9BB File Offset: 0x00009BBB
    void INetworkPacketSubSerializer<TData>.Deserialize(NetDataReader reader, int length, TData data)

    {
        this.ProcessPacketInternal(reader, length, data);
    }

    // Token: 0x06000484 RID: 1156 RVA: 0x0000B9C6 File Offset: 0x00009BC6
    [Conditional("VERBOSE_LOGGING")]
    private void Log(string message)
    {
        BGNet.Logging.Debug.Log("[NetworkPacketSerializer] " + message);
    }

    // Token: 0x0400019B RID: 411
    private Dictionary<byte, Action<NetDataReader, int, TData>> _messsageHandlers = new Dictionary<byte, Action<NetDataReader, int, TData>>();

    // Token: 0x0400019C RID: 412
    private Dictionary<Type, byte> _typeRegistry = new Dictionary<Type, byte>();

    // Token: 0x0400019D RID: 413
    private Dictionary<INetworkPacketSubSerializer<TData>, byte> _subSerializerRegistry = new Dictionary<INetworkPacketSubSerializer<TData>, byte>();

    // Token: 0x0400019E RID: 414
    private readonly NetDataWriter _internalWriter = new NetDataWriter();
}
