using System;

// Token: 0x02000049 RID: 73
public class VersionedPacketPool<T, T2> : IVersionedPacketPool<T>, IPacketPool where T : IPoolablePacket, new() where T2 : T, new()
{
    // Token: 0x060002EB RID: 747 RVA: 0x00006E21 File Offset: 0x00005021
    public VersionedPacketPool(uint overrideVersion)
    {
        this._overrideVersion = overrideVersion;
    }

    // Token: 0x060002EC RID: 748 RVA: 0x00006E46 File Offset: 0x00005046
    public T Obtain(uint version)
    {
        if (version < this._overrideVersion)
        {
            return this._basePool.Obtain();
        }
        return (T)((object)this._overridePool.Obtain());
    }

    // Token: 0x060002ED RID: 749 RVA: 0x00006E74 File Offset: 0x00005074
    public void Release(T packet)
    {
        if (packet is T2)
        {
            T2 t = (T2)((object)packet);
            this._overridePool.Release(t);
            return;
        }
        this._basePool.Release(packet);
    }

    // Token: 0x060002EE RID: 750 RVA: 0x00006EB8 File Offset: 0x000050B8
    public void Release(object o)
    {
        if (o is T)
        {
            T packet = (T)((object)o);
            this.Release(packet);
        }
    }

    // Token: 0x060002EF RID: 751 RVA: 0x00006EDD File Offset: 0x000050DD
    public void Fill()
    {
        this._overridePool.Fill();
        this._basePool.Fill();
    }

    // Token: 0x060002F0 RID: 752 RVA: 0x00006EF5 File Offset: 0x000050F5
    public void Clear()
    {
        this._basePool.Clear();
        this._overridePool.Clear();
    }

    // Token: 0x04000103 RID: 259
    private readonly PacketPool<T> _basePool = new PacketPool<T>();

    // Token: 0x04000104 RID: 260
    private readonly PacketPool<T2> _overridePool = new PacketPool<T2>();

    // Token: 0x04000105 RID: 261
    private readonly uint _overrideVersion;
}
