using System;
using System.Collections.Generic;
using BGNet.Logging;
using LiteNetLib.Utils;
using UnityEngine;

// Token: 0x02000073 RID: 115
public abstract class RemoteProcedureCall : IRemoteProcedureCall, INetSerializable, IPoolablePacket
{
    // Token: 0x170000C9 RID: 201
    // (get) Token: 0x060004EB RID: 1259 RVA: 0x0000D25C File Offset: 0x0000B45C
    // (set) Token: 0x060004EC RID: 1260 RVA: 0x0000D264 File Offset: 0x0000B464
    public float syncTime { get; protected set; }

    // Token: 0x060004ED RID: 1261 RVA: 0x00002273 File Offset: 0x00000473
    protected virtual void SerializeData(NetDataWriter writer, uint protocolVersion)
    {
    }

    // Token: 0x060004EE RID: 1262 RVA: 0x00002273 File Offset: 0x00000473
    protected virtual void DeserializeData(NetDataReader reader, uint protocolVersion)
    {
    }

    // Token: 0x060004EF RID: 1263 RVA: 0x0000D26D File Offset: 0x0000B46D
    void INetSerializable.Serialize(NetDataWriter writer)
    {
        writer.Put(this.syncTime);
        this.SerializeData(writer, 8U);
    }

    // Token: 0x060004F0 RID: 1264 RVA: 0x0000D283 File Offset: 0x0000B483
    void INetSerializable.Deserialize(NetDataReader reader)
    {
        this.syncTime = reader.GetFloat();
        this.DeserializeData(reader, 8U);
    }

    // Token: 0x060004F1 RID: 1265 RVA: 0x0000D299 File Offset: 0x0000B499
    public virtual void Release()
    {
        RpcPool.Release(this);
    }

    // Token: 0x060004F2 RID: 1266 RVA: 0x0000D2A1 File Offset: 0x0000B4A1
    public IRemoteProcedureCall Init(float syncTime)
    {
        this.syncTime = syncTime;
        return this;
    }

    // Token: 0x0200014B RID: 331
    protected class TypeWrapper<T>
    {
        // Token: 0x17000165 RID: 357
        // (get) Token: 0x06000853 RID: 2131 RVA: 0x00015A9C File Offset: 0x00013C9C
        public bool hasValue
        {
            get
            {
                return !EqualityComparer<T>.Default.Equals(this._v, default(T));
            }
        }

        // Token: 0x17000166 RID: 358
        // (get) Token: 0x06000854 RID: 2132 RVA: 0x00015AC5 File Offset: 0x00013CC5
        public T value
        {
            get
            {
                return this._v;
            }
        }

        // Token: 0x06000855 RID: 2133 RVA: 0x00015ACD File Offset: 0x00013CCD
        public void Set(T v)
        {
            this._v = v;
        }

        // Token: 0x06000856 RID: 2134 RVA: 0x00015AD6 File Offset: 0x00013CD6
        public void Clear()
        {
            this._v = default(T);
        }

        // Token: 0x06000857 RID: 2135 RVA: 0x00015AE4 File Offset: 0x00013CE4
        public void Release()
        {
            if (this.hasValue && typeof(IPoolableSerializable).IsAssignableFrom(typeof(T)))
            {
                ((IPoolableSerializable)((object)this._v)).Release();
                this._v = default(T);
            }
        }

        // Token: 0x06000858 RID: 2136 RVA: 0x00015B38 File Offset: 0x00013D38
        public void Serialize(NetDataWriter writer)
        {
            RemoteProcedureCall.TypeWrapper<int> typeWrapper;
            if ((typeWrapper = (this as RemoteProcedureCall.TypeWrapper<int>)) != null)
            {
                writer.PutVarInt(typeWrapper._v);
                return;
            }
            RemoteProcedureCall.TypeWrapper<long> typeWrapper2;
            if ((typeWrapper2 = (this as RemoteProcedureCall.TypeWrapper<long>)) != null)
            {
                writer.PutVarLong(typeWrapper2._v);
                return;
            }
            RemoteProcedureCall.TypeWrapper<float> typeWrapper3;
            if ((typeWrapper3 = (this as RemoteProcedureCall.TypeWrapper<float>)) != null)
            {
                writer.Put(typeWrapper3._v);
                return;
            }
            RemoteProcedureCall.TypeWrapper<byte> typeWrapper4;
            if ((typeWrapper4 = (this as RemoteProcedureCall.TypeWrapper<byte>)) != null)
            {
                writer.Put(typeWrapper4._v);
                return;
            }
            RemoteProcedureCall.TypeWrapper<bool> typeWrapper5;
            if ((typeWrapper5 = (this as RemoteProcedureCall.TypeWrapper<bool>)) != null)
            {
                writer.Put(typeWrapper5._v);
                return;
            }
            RemoteProcedureCall.TypeWrapper<string> typeWrapper6;
            if ((typeWrapper6 = (this as RemoteProcedureCall.TypeWrapper<string>)) != null)
            {
                writer.Put(typeWrapper6._v);
                return;
            }
            RemoteProcedureCall.TypeWrapper<int[]> typeWrapper7;
            if ((typeWrapper7 = (this as RemoteProcedureCall.TypeWrapper<int[]>)) != null)
            {
                writer.PutArray(typeWrapper7._v);
                return;
            }
            RemoteProcedureCall.TypeWrapper<long[]> typeWrapper8;
            if ((typeWrapper8 = (this as RemoteProcedureCall.TypeWrapper<long[]>)) != null)
            {
                writer.PutArray(typeWrapper8._v);
                return;
            }
            RemoteProcedureCall.TypeWrapper<float[]> typeWrapper9;
            if ((typeWrapper9 = (this as RemoteProcedureCall.TypeWrapper<float[]>)) != null)
            {
                writer.PutArray(typeWrapper9._v);
                return;
            }
            RemoteProcedureCall.TypeWrapper<byte[]> typeWrapper10;
            if ((typeWrapper10 = (this as RemoteProcedureCall.TypeWrapper<byte[]>)) != null)
            {
                byte[] v = typeWrapper10._v;
                writer.PutVarUInt((uint)v.Length);
                writer.Put(v);
                return;
            }
            RemoteProcedureCall.TypeWrapper<bool[]> typeWrapper11;
            if ((typeWrapper11 = (this as RemoteProcedureCall.TypeWrapper<bool[]>)) != null)
            {
                writer.PutArray(typeWrapper11._v);
                return;
            }
            RemoteProcedureCall.TypeWrapper<string[]> typeWrapper12;
            if ((typeWrapper12 = (this as RemoteProcedureCall.TypeWrapper<string[]>)) != null)
            {
                writer.PutArray(typeWrapper12._v);
                return;
            }
            RemoteProcedureCall.TypeWrapper<Vector3Serializable> typeWrapper13;
            if ((typeWrapper13 = (this as RemoteProcedureCall.TypeWrapper<Vector3Serializable>)) != null)
            {
                typeWrapper13._v.Serialize(writer);
                return;
            }
            RemoteProcedureCall.TypeWrapper<Vector4Serializable> typeWrapper14;
            if ((typeWrapper14 = (this as RemoteProcedureCall.TypeWrapper<Vector4Serializable>)) != null)
            {
                typeWrapper14._v.Serialize(writer);
                return;
            }
            RemoteProcedureCall.TypeWrapper<QuaternionSerializable> typeWrapper15;
            if ((typeWrapper15 = (this as RemoteProcedureCall.TypeWrapper<QuaternionSerializable>)) != null)
            {
                typeWrapper15._v.Serialize(writer);
                return;
            }
            if (typeof(T).IsEnum)
            {
                writer.PutVarLong(Convert.ToInt64(this._v));
                return;
            }
            if (typeof(INetSerializable).IsAssignableFrom(typeof(T)))
            {
                INetSerializable netSerializable = (INetSerializable)((object)this._v);
                if (netSerializable == null)
                {
                    throw new NullReferenceException("NetSerializable is Null, but we tried to serialize it anyway");
                }
                netSerializable.Serialize(writer);
                return;
            }
            else
            {
                if (!typeof(INetImmutableSerializable<T>).IsAssignableFrom(typeof(T)))
                {
                    BGNet.Logging.Debug.LogError("Trying to serialize object of type " + typeof(T) + " but does not implement INetSerializable");
                    return;
                }
                INetImmutableSerializable<T> netImmutableSerializable = (INetImmutableSerializable<T>)((object)this._v);
                if (netImmutableSerializable == null)
                {
                    throw new NullReferenceException("INetImmutableSerializable is Null, but we tried to serialize it anyway");
                }
                netImmutableSerializable.Serialize(writer);
                return;
            }
        }

        // Token: 0x06000859 RID: 2137 RVA: 0x00015DA4 File Offset: 0x00013FA4
        public void Deserialize(NetDataReader reader)
        {
            RemoteProcedureCall.TypeWrapper<int> typeWrapper;
            if ((typeWrapper = (this as RemoteProcedureCall.TypeWrapper<int>)) != null)
            {
                typeWrapper._v = reader.GetVarInt();
                return;
            }
            RemoteProcedureCall.TypeWrapper<long> typeWrapper2;
            if ((typeWrapper2 = (this as RemoteProcedureCall.TypeWrapper<long>)) != null)
            {
                typeWrapper2._v = reader.GetVarLong();
                return;
            }
            RemoteProcedureCall.TypeWrapper<float> typeWrapper3;
            if ((typeWrapper3 = (this as RemoteProcedureCall.TypeWrapper<float>)) != null)
            {
                typeWrapper3._v = reader.GetFloat();
                return;
            }
            RemoteProcedureCall.TypeWrapper<byte> typeWrapper4;
            if ((typeWrapper4 = (this as RemoteProcedureCall.TypeWrapper<byte>)) != null)
            {
                typeWrapper4._v = reader.GetByte();
                return;
            }
            RemoteProcedureCall.TypeWrapper<bool> typeWrapper5;
            if ((typeWrapper5 = (this as RemoteProcedureCall.TypeWrapper<bool>)) != null)
            {
                typeWrapper5._v = reader.GetBool();
                return;
            }
            RemoteProcedureCall.TypeWrapper<string> typeWrapper6;
            if ((typeWrapper6 = (this as RemoteProcedureCall.TypeWrapper<string>)) != null)
            {
                typeWrapper6._v = reader.GetString();
                return;
            }
            RemoteProcedureCall.TypeWrapper<int[]> typeWrapper7;
            if ((typeWrapper7 = (this as RemoteProcedureCall.TypeWrapper<int[]>)) != null)
            {
                typeWrapper7._v = reader.GetIntArray();
                return;
            }
            RemoteProcedureCall.TypeWrapper<long[]> typeWrapper8;
            if ((typeWrapper8 = (this as RemoteProcedureCall.TypeWrapper<long[]>)) != null)
            {
                typeWrapper8._v = reader.GetLongArray();
                return;
            }
            RemoteProcedureCall.TypeWrapper<float[]> typeWrapper9;
            if ((typeWrapper9 = (this as RemoteProcedureCall.TypeWrapper<float[]>)) != null)
            {
                typeWrapper9._v = reader.GetFloatArray();
                return;
            }
            RemoteProcedureCall.TypeWrapper<byte[]> typeWrapper10;
            if ((typeWrapper10 = (this as RemoteProcedureCall.TypeWrapper<byte[]>)) != null)
            {
                uint varUInt = reader.GetVarUInt();
                byte[] array = new byte[varUInt];
                reader.GetBytes(array, 0, (int)varUInt);
                typeWrapper10._v = array;
                return;
            }
            RemoteProcedureCall.TypeWrapper<bool[]> typeWrapper11;
            if ((typeWrapper11 = (this as RemoteProcedureCall.TypeWrapper<bool[]>)) != null)
            {
                typeWrapper11._v = reader.GetBoolArray();
                return;
            }
            RemoteProcedureCall.TypeWrapper<string[]> typeWrapper12;
            if ((typeWrapper12 = (this as RemoteProcedureCall.TypeWrapper<string[]>)) != null)
            {
                typeWrapper12._v = reader.GetStringArray();
                return;
            }
            RemoteProcedureCall.TypeWrapper<Vector3> typeWrapper13;
            if ((typeWrapper13 = (this as RemoteProcedureCall.TypeWrapper<Vector3>)) != null)
            {
                typeWrapper13._v = new Vector3Serializable(reader);
                return;
            }
            RemoteProcedureCall.TypeWrapper<Vector4> typeWrapper14;
            if ((typeWrapper14 = (this as RemoteProcedureCall.TypeWrapper<Vector4>)) != null)
            {
                typeWrapper14._v = new Vector4Serializable(reader);
                return;
            }
            RemoteProcedureCall.TypeWrapper<Quaternion> typeWrapper15;
            if ((typeWrapper15 = (this as RemoteProcedureCall.TypeWrapper<Quaternion>)) != null)
            {
                typeWrapper15._v = new QuaternionSerializable(reader);
                return;
            }
            if (typeof(T).IsEnum)
            {
                this._v = (T)((object)Enum.ToObject(typeof(T), reader.GetVarLong()));
                return;
            }
            if (typeof(IPoolableSerializable).IsAssignableFrom(typeof(T)))
            {
                INetSerializable netSerializable = (INetSerializable)((object)PoolableSerializable.Obtain<T>());
                netSerializable.Deserialize(reader);
                this._v = (T)((object)netSerializable);
                return;
            }
            if (typeof(INetSerializable).IsAssignableFrom(typeof(T)))
            {
                INetSerializable netSerializable2 = (INetSerializable)Activator.CreateInstance(typeof(T));
                netSerializable2.Deserialize(reader);
                this._v = (T)((object)netSerializable2);
                return;
            }
            if (typeof(INetImmutableSerializable<T>).IsAssignableFrom(typeof(T)))
            {
                INetImmutableSerializable<T> netImmutableSerializable = (this._v == null) ? ((INetImmutableSerializable<T>)Activator.CreateInstance(typeof(T))) : ((INetImmutableSerializable<T>)((object)this._v));
                this._v = netImmutableSerializable.CreateFromSerializedData(reader);
            }
        }

        // Token: 0x04000446 RID: 1094
        private T _v;
    }
}

public abstract class RemoteProcedureCall<T0> : RemoteProcedureCall
{
    // Token: 0x170000CA RID: 202
    // (get) Token: 0x060004F4 RID: 1268 RVA: 0x0000D2AB File Offset: 0x0000B4AB
    public T0 value0
    {
        get
        {
            return this._value0.value;
        }
    }

    // Token: 0x060004F5 RID: 1269 RVA: 0x0000D2B8 File Offset: 0x0000B4B8
    protected override void SerializeData(NetDataWriter writer, uint protocolVersion)
    {
        byte value = (byte)(this._value0.hasValue ? 1 : 0);
        if (protocolVersion > 6U)
        {
            writer.Put(value);
            if (this._value0.hasValue)
            {
                this._value0.Serialize(writer);
                return;
            }
        }
        else
        {
            this._value0.Serialize(writer);
        }
    }

    // Token: 0x060004F6 RID: 1270 RVA: 0x0000D309 File Offset: 0x0000B509
    protected override void DeserializeData(NetDataReader reader, uint protocolVersion)
    {
        if (protocolVersion <= 6U)
        {
            this._value0.Deserialize(reader);
            return;
        }
        if ((reader.GetByte() & 1) != 0)
        {
            this._value0.Deserialize(reader);
            return;
        }
        this._value0.Clear();
    }

    // Token: 0x060004F7 RID: 1271 RVA: 0x0000D33E File Offset: 0x0000B53E
    public IRemoteProcedureCall Init(float syncTime, T0 value0)
    {
        base.syncTime = syncTime;
        this._value0.Set(value0);
        return this;
    }

    // Token: 0x060004F8 RID: 1272 RVA: 0x0000D354 File Offset: 0x0000B554
    public override void Release()
    {
        this._value0.Release();
        base.Release();
    }

    // Token: 0x040001E3 RID: 483
    private readonly RemoteProcedureCall.TypeWrapper<T0> _value0 = new RemoteProcedureCall.TypeWrapper<T0>();
}

public class RemoteProcedureCall<T0, T1> : RemoteProcedureCall
{
    // Token: 0x170000CB RID: 203
    // (get) Token: 0x060004FA RID: 1274 RVA: 0x0000D37A File Offset: 0x0000B57A
    public T0 value0
    {
        get
        {
            return this._value0.value;
        }
    }

    // Token: 0x170000CC RID: 204
    // (get) Token: 0x060004FB RID: 1275 RVA: 0x0000D387 File Offset: 0x0000B587
    public T1 value1
    {
        get
        {
            return this._value1.value;
        }
    }

    // Token: 0x060004FC RID: 1276 RVA: 0x0000D394 File Offset: 0x0000B594
    protected override void SerializeData(NetDataWriter writer, uint protocolVersion)
    {
        if (protocolVersion > 6U)
        {
            byte value = (byte)((this._value0.hasValue ? 1 : 0) | (this._value1.hasValue ? 2 : 0));
            writer.Put(value);
            if (this._value0.hasValue)
            {
                this._value0.Serialize(writer);
            }
            if (this._value1.hasValue)
            {
                this._value1.Serialize(writer);
                return;
            }
        }
        else
        {
            this._value0.Serialize(writer);
            this._value1.Serialize(writer);
        }
    }

    // Token: 0x060004FD RID: 1277 RVA: 0x0000D41C File Offset: 0x0000B61C
    protected override void DeserializeData(NetDataReader reader, uint protocolVersion)
    {
        if (protocolVersion <= 6U)
        {
            this._value0.Deserialize(reader);
            this._value1.Deserialize(reader);
            return;
        }
        byte @byte = reader.GetByte();
        if ((@byte & 1) != 0)
        {
            this._value0.Deserialize(reader);
        }
        else
        {
            this._value0.Clear();
        }
        if ((@byte & 2) != 0)
        {
            this._value1.Deserialize(reader);
            return;
        }
        this._value1.Clear();
    }

    // Token: 0x060004FE RID: 1278 RVA: 0x0000D486 File Offset: 0x0000B686
    public IRemoteProcedureCall Init(float syncTime, T0 value0, T1 value1)
    {
        base.syncTime = syncTime;
        this._value0.Set(value0);
        this._value1.Set(value1);
        return this;
    }

    // Token: 0x060004FF RID: 1279 RVA: 0x0000D4A8 File Offset: 0x0000B6A8
    public override void Release()
    {
        this._value0.Release();
        this._value1.Release();
        base.Release();
    }

    // Token: 0x040001E4 RID: 484
    private readonly RemoteProcedureCall.TypeWrapper<T0> _value0 = new RemoteProcedureCall.TypeWrapper<T0>();

    // Token: 0x040001E5 RID: 485
    private readonly RemoteProcedureCall.TypeWrapper<T1> _value1 = new RemoteProcedureCall.TypeWrapper<T1>();
}

public class RemoteProcedureCall<T0, T1, T2> : RemoteProcedureCall
{
    // Token: 0x170000CD RID: 205
    // (get) Token: 0x06000501 RID: 1281 RVA: 0x0000D4E4 File Offset: 0x0000B6E4
    public T0 value0
    {
        get
        {
            return this._value0.value;
        }
    }

    // Token: 0x170000CE RID: 206
    // (get) Token: 0x06000502 RID: 1282 RVA: 0x0000D4F1 File Offset: 0x0000B6F1
    public T1 value1
    {
        get
        {
            return this._value1.value;
        }
    }

    // Token: 0x170000CF RID: 207
    // (get) Token: 0x06000503 RID: 1283 RVA: 0x0000D4FE File Offset: 0x0000B6FE
    public T2 value2
    {
        get
        {
            return this._value2.value;
        }
    }

    // Token: 0x06000504 RID: 1284 RVA: 0x0000D50C File Offset: 0x0000B70C
    protected override void SerializeData(NetDataWriter writer, uint protocolVersion)
    {
        if (protocolVersion > 6U)
        {
            byte value = (byte)((this._value0.hasValue ? 1 : 0) | (this._value1.hasValue ? 2 : 0) | (this._value2.hasValue ? 4 : 0));
            writer.Put(value);
            if (this._value0.hasValue)
            {
                this._value0.Serialize(writer);
            }
            if (this._value1.hasValue)
            {
                this._value1.Serialize(writer);
            }
            if (this._value2.hasValue)
            {
                this._value2.Serialize(writer);
                return;
            }
        }
        else
        {
            this._value0.Serialize(writer);
            this._value1.Serialize(writer);
            this._value2.Serialize(writer);
        }
    }

    // Token: 0x06000505 RID: 1285 RVA: 0x0000D5D0 File Offset: 0x0000B7D0
    protected override void DeserializeData(NetDataReader reader, uint protocolVersion)
    {
        if (protocolVersion <= 6U)
        {
            this._value0.Deserialize(reader);
            this._value1.Deserialize(reader);
            this._value2.Deserialize(reader);
            return;
        }
        byte @byte = reader.GetByte();
        if ((@byte & 1) != 0)
        {
            this._value0.Deserialize(reader);
        }
        else
        {
            this._value0.Clear();
        }
        if ((@byte & 2) != 0)
        {
            this._value1.Deserialize(reader);
        }
        else
        {
            this._value1.Clear();
        }
        if ((@byte & 4) != 0)
        {
            this._value2.Deserialize(reader);
            return;
        }
        this._value2.Clear();
    }

    // Token: 0x06000506 RID: 1286 RVA: 0x0000D664 File Offset: 0x0000B864
    public IRemoteProcedureCall Init(float syncTime, T0 value0, T1 value1, T2 value2)
    {
        base.syncTime = syncTime;
        this._value0.Set(value0);
        this._value1.Set(value1);
        this._value2.Set(value2);
        return this;
    }

    // Token: 0x06000507 RID: 1287 RVA: 0x0000D693 File Offset: 0x0000B893
    public override void Release()
    {
        this._value0.Release();
        this._value1.Release();
        this._value2.Release();
        base.Release();
    }

    // Token: 0x040001E6 RID: 486
    private readonly RemoteProcedureCall.TypeWrapper<T0> _value0 = new RemoteProcedureCall.TypeWrapper<T0>();

    // Token: 0x040001E7 RID: 487
    private readonly RemoteProcedureCall.TypeWrapper<T1> _value1 = new RemoteProcedureCall.TypeWrapper<T1>();

    // Token: 0x040001E8 RID: 488
    private readonly RemoteProcedureCall.TypeWrapper<T2> _value2 = new RemoteProcedureCall.TypeWrapper<T2>();
}

public class RemoteProcedureCall<T0, T1, T2, T3> : RemoteProcedureCall
{
    // Token: 0x170000D0 RID: 208
    // (get) Token: 0x06000509 RID: 1289 RVA: 0x0000D6E5 File Offset: 0x0000B8E5
    public T0 value0
    {
        get
        {
            return this._value0.value;
        }
    }

    // Token: 0x170000D1 RID: 209
    // (get) Token: 0x0600050A RID: 1290 RVA: 0x0000D6F2 File Offset: 0x0000B8F2
    public T1 value1
    {
        get
        {
            return this._value1.value;
        }
    }

    // Token: 0x170000D2 RID: 210
    // (get) Token: 0x0600050B RID: 1291 RVA: 0x0000D6FF File Offset: 0x0000B8FF
    public T2 value2
    {
        get
        {
            return this._value2.value;
        }
    }

    // Token: 0x170000D3 RID: 211
    // (get) Token: 0x0600050C RID: 1292 RVA: 0x0000D70C File Offset: 0x0000B90C
    public T3 value3
    {
        get
        {
            return this._value3.value;
        }
    }

    // Token: 0x0600050D RID: 1293 RVA: 0x0000D71C File Offset: 0x0000B91C
    protected override void SerializeData(NetDataWriter writer, uint protocolVersion)
    {
        if (protocolVersion > 6U)
        {
            byte value = (byte)((this._value0.hasValue ? 1 : 0) | (this._value1.hasValue ? 2 : 0) | (this._value2.hasValue ? 4 : 0) | (this._value3.hasValue ? 8 : 0));
            writer.Put(value);
            if (this._value0.hasValue)
            {
                this._value0.Serialize(writer);
            }
            if (this._value1.hasValue)
            {
                this._value1.Serialize(writer);
            }
            if (this._value2.hasValue)
            {
                this._value2.Serialize(writer);
            }
            if (this._value3.hasValue)
            {
                this._value3.Serialize(writer);
                return;
            }
        }
        else
        {
            this._value0.Serialize(writer);
            this._value1.Serialize(writer);
            this._value2.Serialize(writer);
            this._value3.Serialize(writer);
        }
    }

    // Token: 0x0600050E RID: 1294 RVA: 0x0000D818 File Offset: 0x0000BA18
    protected override void DeserializeData(NetDataReader reader, uint protocolVersion)
    {
        if (protocolVersion <= 6U)
        {
            this._value0.Deserialize(reader);
            this._value1.Deserialize(reader);
            this._value2.Deserialize(reader);
            this._value3.Deserialize(reader);
            return;
        }
        byte @byte = reader.GetByte();
        if ((@byte & 1) != 0)
        {
            this._value0.Deserialize(reader);
        }
        else
        {
            this._value0.Clear();
        }
        if ((@byte & 2) != 0)
        {
            this._value1.Deserialize(reader);
        }
        else
        {
            this._value1.Clear();
        }
        if ((@byte & 4) != 0)
        {
            this._value2.Deserialize(reader);
        }
        else
        {
            this._value2.Clear();
        }
        if ((@byte & 8) != 0)
        {
            this._value3.Deserialize(reader);
            return;
        }
        this._value3.Clear();
    }

    // Token: 0x0600050F RID: 1295 RVA: 0x0000D8D6 File Offset: 0x0000BAD6
    public IRemoteProcedureCall Init(float syncTime, T0 value0, T1 value1, T2 value2, T3 value3)
    {
        base.syncTime = syncTime;
        this._value0.Set(value0);
        this._value1.Set(value1);
        this._value2.Set(value2);
        this._value3.Set(value3);
        return this;
    }

    // Token: 0x06000510 RID: 1296 RVA: 0x0000D912 File Offset: 0x0000BB12
    public override void Release()
    {
        this._value0.Release();
        this._value1.Release();
        this._value2.Release();
        this._value3.Release();
        base.Release();
    }

    // Token: 0x040001E9 RID: 489
    private readonly RemoteProcedureCall.TypeWrapper<T0> _value0 = new RemoteProcedureCall.TypeWrapper<T0>();

    // Token: 0x040001EA RID: 490
    private readonly RemoteProcedureCall.TypeWrapper<T1> _value1 = new RemoteProcedureCall.TypeWrapper<T1>();

    // Token: 0x040001EB RID: 491
    private readonly RemoteProcedureCall.TypeWrapper<T2> _value2 = new RemoteProcedureCall.TypeWrapper<T2>();

    // Token: 0x040001EC RID: 492
    private readonly RemoteProcedureCall.TypeWrapper<T3> _value3 = new RemoteProcedureCall.TypeWrapper<T3>();
}
