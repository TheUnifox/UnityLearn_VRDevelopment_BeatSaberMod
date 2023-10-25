// Decompiled with JetBrains decompiler
// Type: TriggerChecker
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;

public class TriggerChecker : MonoBehaviour
{
  public System.Action TriggerCheckerOnEnterEvent;
  public System.Action TriggerCheckerOnExitEvent;
  public System.Action TriggerCheckerOnStayEvent;

  public virtual void OnTriggerEnter(Collider other)
  {
    if (this.TriggerCheckerOnEnterEvent == null)
      return;
    this.TriggerCheckerOnEnterEvent();
  }

  public virtual void OnTriggerExit(Collider other)
  {
    if (this.TriggerCheckerOnExitEvent == null)
      return;
    this.TriggerCheckerOnExitEvent();
  }

  public virtual void OnTriggerStay(Collider other)
  {
    if (this.TriggerCheckerOnStayEvent == null)
      return;
    this.TriggerCheckerOnStayEvent();
  }
}
