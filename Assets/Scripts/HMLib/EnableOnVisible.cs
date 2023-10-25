// Decompiled with JetBrains decompiler
// Type: EnableOnVisible
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;

public class EnableOnVisible : MonoBehaviour
{
  public Behaviour[] _components;

  public event System.Action<bool> VisibilityChangedEvent;

  public virtual void Awake()
  {
    for (int index = 0; index < this._components.Length; ++index)
      this._components[index].enabled = false;
  }

  public virtual void OnBecameVisible()
  {
    for (int index = 0; index < this._components.Length; ++index)
      this._components[index].enabled = true;
    if (this.VisibilityChangedEvent == null)
      return;
    this.VisibilityChangedEvent(true);
  }

  public virtual void OnBecameInvisible()
  {
    for (int index = 0; index < this._components.Length; ++index)
      this._components[index].enabled = false;
    if (this.VisibilityChangedEvent == null)
      return;
    this.VisibilityChangedEvent(false);
  }
}
