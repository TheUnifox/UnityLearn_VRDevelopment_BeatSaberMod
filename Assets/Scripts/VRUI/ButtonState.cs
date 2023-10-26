// Decompiled with JetBrains decompiler
// Type: VRUIControls.ButtonState
// Assembly: VRUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3BA7334A-77F9-4425-988C-69CB2EE35CCF
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\VRUI.dll

using UnityEngine.EventSystems;

namespace VRUIControls
{
  public class ButtonState
  {
    protected PointerEventData.InputButton _button;
    protected MouseButtonEventData _eventData;
    protected float _pressedValue;

    public MouseButtonEventData eventData
    {
      get => this._eventData;
      set => this._eventData = value;
    }

    public PointerEventData.InputButton button
    {
      get => this._button;
      set => this._button = value;
    }

    public float pressedValue
    {
      get => this._pressedValue;
      set => this._pressedValue = value;
    }
  }
}
