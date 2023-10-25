// Decompiled with JetBrains decompiler
// Type: SignalListener
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;
using UnityEngine.Events;

public class SignalListener : MonoBehaviour
{
  [SerializeField]
  protected Signal _signal;
  [SerializeField]
  protected UnityEvent _unityEvent;

  public virtual void OnEnable() => this._signal.Subscribe(new System.Action(this.HandleEvent));

  public virtual void OnDisable() => this._signal.Unsubscribe(new System.Action(this.HandleEvent));

  public virtual void HandleEvent() => this._unityEvent.Invoke();
}
