// Decompiled with JetBrains decompiler
// Type: HMUI.ButtonSpriteSwapToggle
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using UnityEngine;
using UnityEngine.UI;

namespace HMUI
{
  public class ButtonSpriteSwapToggle : ButtonSpriteSwap
  {
    [SerializeField]
    protected bool _resetToggleOnEnable = true;
    [SerializeField]
    protected bool _ignoreHighlight = true;
    protected bool _isToggled;

    public bool isToggled
    {
      get => this._isToggled;
      set
      {
        if (this._isToggled == value)
          return;
        this._isToggled = value;
        this.RefreshVisualState();
      }
    }

    protected override void OnEnable()
    {
      if (this._resetToggleOnEnable)
        this._isToggled = false;
      base.OnEnable();
    }

    protected override void HandleButtonSelectionStateDidChange(
      NoTransitionsButton.SelectionState state)
    {
      if (!this._didStart || !this.isActiveAndEnabled)
        return;
      Sprite sprite = (Sprite) null;
      switch (state)
      {
        case NoTransitionsButton.SelectionState.Normal:
          sprite = this._isToggled ? this._pressedStateSprite : this._normalStateSprite;
          break;
        case NoTransitionsButton.SelectionState.Highlighted:
          if (!this._ignoreHighlight)
          {
            sprite = this._highlightStateSprite;
            break;
          }
          break;
        case NoTransitionsButton.SelectionState.Pressed:
          this._isToggled = !this._isToggled;
          sprite = this._isToggled ? this._pressedStateSprite : this._normalStateSprite;
          break;
        case NoTransitionsButton.SelectionState.Disabled:
          sprite = this._disabledStateSprite;
          break;
      }
      if (!((Object) sprite != (Object) null))
        return;
      foreach (Image image in this._images)
        image.sprite = sprite;
    }
  }
}
