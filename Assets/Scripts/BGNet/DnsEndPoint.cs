using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using BGNet.Core;
using LiteNetLib;

// Token: 0x02000018 RID: 24
public class DnsEndPoint : IEquatable<global::DnsEndPoint>
{
    // Token: 0x1700002A RID: 42
    // (get) Token: 0x060000C2 RID: 194 RVA: 0x000047D5 File Offset: 0x000029D5
    public IPEndPoint endPoint
    {
        get
        {
            if (this._getEndPointTask == null || !this._getEndPointTask.IsCompleted)
            {
                return null;
            }
            return this._getEndPointTask.Result;
        }
    }

    // Token: 0x060000C3 RID: 195 RVA: 0x000047F9 File Offset: 0x000029F9
    public DnsEndPoint(string hostName, int port)
    {
        this.hostName = hostName;
        this.port = port;
    }

    // Token: 0x060000C4 RID: 196 RVA: 0x0000480F File Offset: 0x00002A0F
    public DnsEndPoint(IPEndPoint endPoint)
    {
        this.hostName = endPoint.Address.ToString();
        this.port = endPoint.Port;
        this._getEndPointTask = Task.FromResult<IPEndPoint>(endPoint);
    }

    // Token: 0x060000C5 RID: 197 RVA: 0x00004840 File Offset: 0x00002A40
    public Task<IPEndPoint> GetEndPointAsync(ITaskUtility taskUtility)
    {
        this._getEndPointTask = taskUtility.Run<IPEndPoint>(new Func<IPEndPoint>(this.GetEndPointInternal), default(CancellationToken));
        return this._getEndPointTask;
    }

    // Token: 0x060000C6 RID: 198 RVA: 0x00004874 File Offset: 0x00002A74
    public IPEndPoint GetEndPoint()
    {
        if (this.endPoint == null)
        {
            this._getEndPointTask = Task.FromResult<IPEndPoint>(this.GetEndPointInternal());
        }
        return this.endPoint;
    }

    // Token: 0x060000C7 RID: 199 RVA: 0x00004895 File Offset: 0x00002A95
    private IPEndPoint GetEndPointInternal()
    {
        return NetUtils.MakeEndPoint((this.hostName == "localhost") ? NetUtils.GetLocalIp(LocalAddrType.IPv4) : this.hostName, this.port);
    }

    // Token: 0x060000C8 RID: 200 RVA: 0x000048C2 File Offset: 0x00002AC2
    public override string ToString()
    {
        return string.Format("{0}:{1}", this.hostName, this.port);
    }

    // Token: 0x060000C9 RID: 201 RVA: 0x000048E0 File Offset: 0x00002AE0
    public override bool Equals(object obj)
    {
        global::DnsEndPoint other;
        return (other = (obj as global::DnsEndPoint)) != null && this.Equals(other);
    }

    // Token: 0x060000CA RID: 202 RVA: 0x00004900 File Offset: 0x00002B00
    public bool Equals(global::DnsEndPoint other)
    {
        return other != null && this.hostName == other.hostName && this.port == other.port;
    }

    // Token: 0x060000CB RID: 203 RVA: 0x0000492A File Offset: 0x00002B2A
    public override int GetHashCode()
    {
        return ((this.hostName != null) ? this.hostName.GetHashCode() : 0) ^ this.port;
    }

    // Token: 0x04000081 RID: 129
    public readonly string hostName;

    // Token: 0x04000082 RID: 130
    public readonly int port;

    // Token: 0x04000083 RID: 131
    private Task<IPEndPoint> _getEndPointTask;
}
