using System;
using System.Collections.Generic;
using LiteNetLib.Utils;
using UnityEngine.Scripting;

// Token: 0x0200002C RID: 44
public class PlayersMissingEntitlementsNetSerializable : PoolableSerializable
{
    // Token: 0x17000031 RID: 49
    // (get) Token: 0x060000E0 RID: 224 RVA: 0x00005186 File Offset: 0x00003386
    public List<string> playersWithoutEntitlements
    {
        get
        {
            return this._playersWithoutEntitlements;
        }
    }

    // Token: 0x060000E1 RID: 225 RVA: 0x0000518E File Offset: 0x0000338E
    public static PlayersMissingEntitlementsNetSerializable Obtain()
    {
        return PoolableSerializable.Obtain<PlayersMissingEntitlementsNetSerializable>();
    }

    // Token: 0x060000E2 RID: 226 RVA: 0x00005195 File Offset: 0x00003395
    [Preserve]
    public PlayersMissingEntitlementsNetSerializable()
    {
    }

    // Token: 0x060000E3 RID: 227 RVA: 0x000051A8 File Offset: 0x000033A8
    public virtual PlayersMissingEntitlementsNetSerializable Init(IEnumerable<string> playersWithoutEntitlements)
    {
        this._playersWithoutEntitlements.Clear();
        this._playersWithoutEntitlements.AddRange(playersWithoutEntitlements);
        return this;
    }

    // Token: 0x060000E4 RID: 228 RVA: 0x000051C4 File Offset: 0x000033C4
    public override void Serialize(NetDataWriter writer)
    {
        writer.Put(this._playersWithoutEntitlements.Count);
        foreach (string value in this._playersWithoutEntitlements)
        {
            writer.Put(value);
        }
    }

    // Token: 0x060000E5 RID: 229 RVA: 0x00005228 File Offset: 0x00003428
    public override void Deserialize(NetDataReader reader)
    {
        int @int = reader.GetInt();
        this._playersWithoutEntitlements.Clear();
        for (int i = 0; i < @int; i++)
        {
            this._playersWithoutEntitlements.Add(reader.GetString());
        }
    }

    // Token: 0x040000DB RID: 219
    protected readonly List<string> _playersWithoutEntitlements = new List<string>();
}
