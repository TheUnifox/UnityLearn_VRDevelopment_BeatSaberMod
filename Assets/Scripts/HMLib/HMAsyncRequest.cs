// Decompiled with JetBrains decompiler
// Type: HMAsyncRequest
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

public class HMAsyncRequest : HMAutoincrementedRequestId
{
  protected bool _cancelled;
  protected HMAsyncRequest.CancelHander _cancelHander;

  public HMAsyncRequest.CancelHander CancelHandler
  {
    get => this._cancelHander;
    set => this._cancelHander = value;
  }

  public bool cancelled => this._cancelled;

  public virtual void Cancel()
  {
    this._cancelled = true;
    if (this._cancelHander == null)
      return;
    this._cancelHander(this);
  }

  public delegate void CancelHander(HMAsyncRequest request);
}
