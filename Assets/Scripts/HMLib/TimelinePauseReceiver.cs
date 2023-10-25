// Decompiled with JetBrains decompiler
// Type: TimelinePauseReceiver
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;
using UnityEngine.Playables;

public class TimelinePauseReceiver : MonoBehaviour, INotificationReceiver
{
  public event System.Action timelinePauseEvent;

  public virtual void OnNotify(Playable origin, INotification notification, object context)
  {
    if (!(notification is TimelinePauseMarker))
      return;
    System.Action timelinePauseEvent = this.timelinePauseEvent;
    if (timelinePauseEvent == null)
      return;
    timelinePauseEvent();
  }
}
