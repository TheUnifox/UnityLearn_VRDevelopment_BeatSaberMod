using System;
using LiteNetLib.Utils;
using UnityEngine;

// Token: 0x02000037 RID: 55
public struct PoseSerializable : INetSerializable, IEquatable<PoseSerializable>
{
    // Token: 0x06000128 RID: 296 RVA: 0x00006336 File Offset: 0x00004536
    public PoseSerializable(Vector3Serializable position, QuaternionSerializable rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }

    // Token: 0x17000033 RID: 51
    // (get) Token: 0x06000129 RID: 297 RVA: 0x00006348 File Offset: 0x00004548
    public static PoseSerializable identity
    {
        get
        {
            return new PoseSerializable(default(Vector3Serializable), QuaternionSerializable.identity);
        }
    }

    // Token: 0x0600012A RID: 298 RVA: 0x00006368 File Offset: 0x00004568
    public void Serialize(NetDataWriter writer)
    {
        this.position.Serialize(writer);
        this.rotation.Serialize(writer);
    }

    // Token: 0x0600012B RID: 299 RVA: 0x00006382 File Offset: 0x00004582
    public void Deserialize(NetDataReader reader)
    {
        this.position.Deserialize(reader);
        this.rotation.Deserialize(reader);
    }

    // Token: 0x0600012C RID: 300 RVA: 0x0000639C File Offset: 0x0000459C
    public bool Equals(PoseSerializable other)
    {
        return this.position.Equals(other.position) && this.rotation.Equals(other.rotation);
    }

    // Token: 0x0600012D RID: 301 RVA: 0x000063C4 File Offset: 0x000045C4
    public override bool Equals(object obj)
    {
        if (obj is PoseSerializable)
        {
            PoseSerializable other = (PoseSerializable)obj;
            return this.Equals(other);
        }
        return false;
    }

    // Token: 0x0600012E RID: 302 RVA: 0x000063EB File Offset: 0x000045EB
    public override int GetHashCode()
    {
        return this.position.GetHashCode() * 397 ^ this.rotation.GetHashCode();
    }

    // Token: 0x0600012F RID: 303 RVA: 0x00006416 File Offset: 0x00004616
    public override string ToString()
    {
        return string.Format("[pos={0} rot={1}]", this.position, this.rotation);
    }

    // Token: 0x06000130 RID: 304 RVA: 0x00006438 File Offset: 0x00004638
    public int GetSize()
    {
        return this.position.GetSize() + this.rotation.GetSize();
    }

    // Token: 0x06000131 RID: 305 RVA: 0x00006451 File Offset: 0x00004651
    public static implicit operator Pose(PoseSerializable p)
    {
        return new Pose(p.position, p.rotation);
    }

    // Token: 0x06000132 RID: 306 RVA: 0x0000646E File Offset: 0x0000466E
    public static implicit operator PoseSerializable(Pose p)
    {
        return new PoseSerializable(p.position, p.rotation);
    }

    // Token: 0x06000133 RID: 307 RVA: 0x0000648C File Offset: 0x0000468C
    public static PoseSerializable operator +(PoseSerializable a, PoseSerializable b)
    {
        return new PoseSerializable
        {
            position = a.position + b.position,
            rotation = a.rotation + b.rotation
        };
    }

    // Token: 0x06000134 RID: 308 RVA: 0x000064D4 File Offset: 0x000046D4
    public static PoseSerializable operator -(PoseSerializable a, PoseSerializable b)
    {
        return new PoseSerializable
        {
            position = a.position - b.position,
            rotation = a.rotation - b.rotation
        };
    }

    // Token: 0x04000100 RID: 256
    public Vector3Serializable position;

    // Token: 0x04000101 RID: 257
    public QuaternionSerializable rotation;
}
