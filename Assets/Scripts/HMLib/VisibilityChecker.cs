// Decompiled with JetBrains decompiler
// Type: VisibilityChecker
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;

[RequireComponent(typeof (MeshRenderer))]
public class VisibilityChecker : MonoBehaviour
{
  public event System.Action OnBecameVisibleEvent;

  public event System.Action OnBecameInvisibleEvent;

  public virtual void OnBecameVisible()
  {
    if (this.OnBecameVisibleEvent == null)
      return;
    this.OnBecameVisibleEvent();
  }

  public virtual void OnBecameInvisible()
  {
    if (this.OnBecameInvisibleEvent == null)
      return;
    this.OnBecameInvisibleEvent();
  }
}
