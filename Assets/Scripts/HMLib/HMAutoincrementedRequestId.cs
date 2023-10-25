// Decompiled with JetBrains decompiler
// Type: HMAutoincrementedRequestId
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System;

public class HMAutoincrementedRequestId : IEquatable<HMAutoincrementedRequestId>
{
  protected static ulong _nextRequestId;
  protected readonly ulong _requestId;

  public ulong RequestId => this._requestId;

  public HMAutoincrementedRequestId()
  {
    this._requestId = HMAutoincrementedRequestId._nextRequestId;
    ++HMAutoincrementedRequestId._nextRequestId;
  }

  public virtual bool Equals(HMAutoincrementedRequestId obj) => obj != null && (long) obj.RequestId == (long) this._requestId;

  public override bool Equals(object obj) => (obj != null || !(obj is HMAutoincrementedRequestId)) && (long) ((HMAutoincrementedRequestId) obj).RequestId == (long) this._requestId;

  public override int GetHashCode() => (int) (this._requestId % (ulong) int.MaxValue);
}
