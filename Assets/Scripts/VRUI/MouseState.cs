// Decompiled with JetBrains decompiler
// Type: VRUIControls.MouseState
// Assembly: VRUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3BA7334A-77F9-4425-988C-69CB2EE35CCF
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\VRUI.dll

using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace VRUIControls
{
  public class MouseState
  {
    protected List<ButtonState> _trackedButtons = new List<ButtonState>();

    public virtual bool AnyPressesThisFrame()
    {
      for (int index = 0; index < this._trackedButtons.Count; ++index)
      {
        if (this._trackedButtons[index].eventData.PressedThisFrame())
          return true;
      }
      return false;
    }

    public virtual bool AnyReleasesThisFrame()
    {
      for (int index = 0; index < this._trackedButtons.Count; ++index)
      {
        if (this._trackedButtons[index].eventData.ReleasedThisFrame())
          return true;
      }
      return false;
    }

    public virtual ButtonState GetButtonState(PointerEventData.InputButton button)
    {
      ButtonState buttonState = (ButtonState) null;
      for (int index = 0; index < this._trackedButtons.Count; ++index)
      {
        if (this._trackedButtons[index].button == button)
        {
          buttonState = this._trackedButtons[index];
          break;
        }
      }
      if (buttonState == null)
      {
        buttonState = new ButtonState()
        {
          button = button,
          eventData = new MouseButtonEventData(),
          pressedValue = 0.0f
        };
        this._trackedButtons.Add(buttonState);
      }
      return buttonState;
    }

    public virtual void SetButtonState(
      PointerEventData.InputButton button,
      PointerEventData.FramePressState stateForMouseButton,
      PointerEventData data)
    {
      ButtonState buttonState = this.GetButtonState(button);
      buttonState.eventData.buttonState = stateForMouseButton;
      buttonState.eventData.buttonData = data;
    }
  }
}
