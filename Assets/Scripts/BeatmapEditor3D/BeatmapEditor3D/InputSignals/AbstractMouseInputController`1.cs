// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.InputSignals.AbstractMouseInputController`1
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using HMUI;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.InputSignals
{
  public abstract class AbstractMouseInputController<TPayload> : MonoBehaviour
  {
    [Inject]
    protected readonly SignalBus signalBus;
    protected readonly MouseBinder mouseBinder = new MouseBinder();
    protected Dictionary<int, Action> gridActions = new Dictionary<int, Action>();
    protected Dictionary<int, Action<TPayload>> objectActions = new Dictionary<int, Action<TPayload>>();

    protected virtual void Update() => this.mouseBinder.ManualUpdate();

    protected void HandleMouseInputSelectionGridPointerDown(MouseInputType mouseInputType)
    {
      (bool flag1, bool flag2, bool flag3) = AbstractMouseInputController<TPayload>.GetModifiers();
      Action action;
      if (!this.gridActions.TryGetValue(AbstractMouseInputController<TPayload>.HashInput(MouseInputEventOrigin.Grid, mouseInputType, MouseEventType.Down, flag3, flag1, flag2), out action) || action == null)
        return;
      action();
    }

    protected void HandleMouseInputSelectionGridPointerUp(MouseInputType mouseInputType)
    {
      (bool flag1, bool flag2, bool flag3) = AbstractMouseInputController<TPayload>.GetModifiers();
      Action action;
      if (!this.gridActions.TryGetValue(AbstractMouseInputController<TPayload>.HashInput(MouseInputEventOrigin.Grid, mouseInputType, MouseEventType.Up, flag3, flag1, flag2), out action) || action == null)
        return;
      action();
    }

    protected void HandleMouseInputSelectionGridPointerHover(MouseInputType mouseInputType)
    {
      Action action;
      if (!this.gridActions.TryGetValue(AbstractMouseInputController<TPayload>.HashInput(MouseInputEventOrigin.Grid, mouseInputType, MouseEventType.None, false, false, false), out action) || action == null)
        return;
      action();
    }

    protected void HandleMouseInputEventSourceObjectPointerDown(
      MouseInputType mouseInputType,
      TPayload payload)
    {
      (bool flag1, bool flag2, bool flag3) = AbstractMouseInputController<TPayload>.GetModifiers();
      Action<TPayload> action;
      if (!this.objectActions.TryGetValue(AbstractMouseInputController<TPayload>.HashInput(MouseInputEventOrigin.Object, mouseInputType, MouseEventType.Down, flag3, flag1, flag2), out action))
        return;
      action(payload);
    }

    protected void HandleMouseInputEventSourceObjectPointerUp(
      MouseInputType mouseInputType,
      TPayload payload)
    {
      (bool flag1, bool flag2, bool flag3) = AbstractMouseInputController<TPayload>.GetModifiers();
      Action<TPayload> action;
      if (!this.objectActions.TryGetValue(AbstractMouseInputController<TPayload>.HashInput(MouseInputEventOrigin.Object, mouseInputType, MouseEventType.Up, flag3, flag1, flag2), out action))
        return;
      action(payload);
    }

    protected virtual void HandleMouseInputEventSourceObjectPointerHover(
      MouseInputType mouseInputType,
      TPayload payload)
    {
      Action<TPayload> action;
      if (!this.objectActions.TryGetValue(AbstractMouseInputController<TPayload>.HashInput(MouseInputEventOrigin.Object, mouseInputType, MouseEventType.None, false, false, false), out action))
        return;
      action(payload);
    }

    protected void HandleMouseInputEventSourceObjectPointerScroll(
      MouseInputType mouseInputType,
      TPayload payload)
    {
      (bool flag1, bool flag2, bool flag3) = AbstractMouseInputController<TPayload>.GetModifiers();
      Action<TPayload> action;
      if (!this.objectActions.TryGetValue(AbstractMouseInputController<TPayload>.HashInput(MouseInputEventOrigin.Object, mouseInputType, MouseEventType.None, flag3, flag1, flag2), out action))
        return;
      action(payload);
    }

    protected static int HashInput(
      MouseInputEventOrigin eventOrigin,
      MouseInputType mouseInputType,
      MouseEventType mouseEventType,
      bool ctrl,
      bool shift,
      bool alt)
    {
      return (int) ((MouseEventType) ((int) ((MouseInputType) ((int) eventOrigin << 3) | mouseInputType) << 2) | mouseEventType) << 3 | (ctrl ? 1 : 0) | (shift ? 2 : 0) | (alt ? 4 : 0);
    }

    private static (bool shiftDown, bool altDown, bool ctrlDown) GetModifiers()
    {
      int num1 = Input.GetKey(KeyCode.LeftShift) ? 1 : (Input.GetKey(KeyCode.RightShift) ? 1 : 0);
      bool flag1 = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
      bool flag2 = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
      int num2 = flag1 ? 1 : 0;
      int num3 = flag2 ? 1 : 0;
      return (num1 != 0, num2 != 0, num3 != 0);
    }
  }
}
