// Decompiled with JetBrains decompiler
// Type: VRUIControls.MouseButtonEventData
// Assembly: VRUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3BA7334A-77F9-4425-988C-69CB2EE35CCF
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\VRUI.dll

using UnityEngine.EventSystems;

namespace VRUIControls
{
  public class MouseButtonEventData
  {
    public PointerEventData.FramePressState buttonState;
    public PointerEventData buttonData;

    public virtual bool PressedThisFrame() => this.buttonState == PointerEventData.FramePressState.Pressed || this.buttonState == PointerEventData.FramePressState.PressedAndReleased;

    public virtual bool ReleasedThisFrame() => this.buttonState == PointerEventData.FramePressState.Released || this.buttonState == PointerEventData.FramePressState.PressedAndReleased;
  }
}
