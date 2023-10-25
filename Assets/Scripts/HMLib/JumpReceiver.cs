// Decompiled with JetBrains decompiler
// Type: JumpReceiver
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Playables;

public class JumpReceiver : MonoBehaviour, INotificationReceiver
{
  [CompilerGenerated]
  protected bool m_jumpToDestinationValid;

  public bool jumpToDestinationValid
  {
    get => this.m_jumpToDestinationValid;
    set => this.m_jumpToDestinationValid = value;
  }

  public virtual void OnNotify(Playable origin, INotification notification, object context)
  {
    if (!(notification is JumpMarker jumpMarker) || !this.jumpToDestinationValid)
      return;
    JumpDestinationMarker jumpDestination = jumpMarker.jumpDestination;
    if ((Object) jumpDestination == (Object) null)
      return;
    origin.GetGraph<Playable>().GetRootPlayable(0).SetTime<Playable>(jumpDestination.time);
  }
}
