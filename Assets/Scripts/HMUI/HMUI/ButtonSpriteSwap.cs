// Decompiled with JetBrains decompiler
// Type: HMUI.ButtonSpriteSwap
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using UnityEngine;
using UnityEngine.UI;

namespace HMUI
{
  public class ButtonSpriteSwap : MonoBehaviour
  {
    [SerializeField]
    protected Sprite _normalStateSprite;
    [SerializeField]
    protected Sprite _highlightStateSprite;
    [SerializeField]
    protected Sprite _pressedStateSprite;
    [SerializeField]
    protected Sprite _disabledStateSprite;
    [Space]
    [SerializeField]
    protected NoTransitionsButton _button;
    [SerializeField]
    protected Image[] _images;
    protected bool _didStart;

    public virtual void Awake() => this._button.selectionStateDidChangeEvent += new Action<NoTransitionsButton.SelectionState>(this.HandleButtonSelectionStateDidChange);

    public virtual void Start()
    {
      this._didStart = true;
      this.RefreshVisualState();
    }

    protected virtual void OnEnable() => this.RefreshVisualState();

    public virtual void OnDestroy()
    {
      if (!((UnityEngine.Object) this._button != (UnityEngine.Object) null))
        return;
      this._button.selectionStateDidChangeEvent -= new Action<NoTransitionsButton.SelectionState>(this.HandleButtonSelectionStateDidChange);
    }

    protected virtual void HandleButtonSelectionStateDidChange(
      NoTransitionsButton.SelectionState state)
    {
      if (!this._didStart || !this.isActiveAndEnabled)
        return;
      Sprite sprite = (Sprite) null;
      switch (state)
      {
        case NoTransitionsButton.SelectionState.Normal:
          sprite = this._normalStateSprite;
          break;
        case NoTransitionsButton.SelectionState.Highlighted:
          sprite = this._highlightStateSprite;
          break;
        case NoTransitionsButton.SelectionState.Pressed:
          sprite = this._pressedStateSprite;
          break;
        case NoTransitionsButton.SelectionState.Disabled:
          sprite = this._disabledStateSprite;
          break;
      }
      foreach (Image image in this._images)
        image.sprite = sprite;
    }

    public virtual void RefreshVisualState() => this.HandleButtonSelectionStateDidChange(this._button.selectionState);
  }
}
