// Decompiled with JetBrains decompiler
// Type: HMUI.EventSystemListener
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HMUI
{
  [RequireComponent(typeof (Graphic))]
  public class EventSystemListener : 
    MonoBehaviour,
    IPointerEnterHandler,
    IEventSystemHandler,
    IPointerExitHandler
  {
    public event Action<PointerEventData> pointerDidEnterEvent;

    public event Action<PointerEventData> pointerDidExitEvent;

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
      Action<PointerEventData> pointerDidEnterEvent = this.pointerDidEnterEvent;
      if (pointerDidEnterEvent == null)
        return;
      pointerDidEnterEvent(eventData);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
      Action<PointerEventData> pointerDidExitEvent = this.pointerDidExitEvent;
      if (pointerDidExitEvent == null)
        return;
      pointerDidExitEvent(eventData);
    }
  }
}
