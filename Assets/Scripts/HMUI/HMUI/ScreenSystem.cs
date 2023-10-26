// Decompiled with JetBrains decompiler
// Type: HMUI.ScreenSystem
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace HMUI
{
  public class ScreenSystem : MonoBehaviour
  {
    [SerializeField]
    protected Screen _mainScreen;
    [SerializeField]
    protected Screen _leftScreen;
    [SerializeField]
    protected Screen _rightScreen;
    [SerializeField]
    protected Screen _bottomScreen;
    [SerializeField]
    protected Screen _topScreen;
    [SerializeField]
    protected Button _backButton;
    [SerializeField]
    protected TitleViewController _titleViewController;
    protected bool _backButtonIsVisible;
    protected ButtonBinder _buttonBinder;

    public TitleViewController titleViewController => this._titleViewController;

    public Screen mainScreen => this._mainScreen;

    public Screen leftScreen => this._leftScreen;

    public Screen rightScreen => this._rightScreen;

    public Screen bottomScreen => this._bottomScreen;

    public Screen topScreen => this._topScreen;

    public event Action backButtonWasPressedEvent;

    public virtual void Awake()
    {
      this._buttonBinder = new ButtonBinder();
      this._buttonBinder.AddBinding(this._backButton, (Action) (() =>
      {
        Action buttonWasPressedEvent = this.backButtonWasPressedEvent;
        if (buttonWasPressedEvent == null)
          return;
        buttonWasPressedEvent();
      }));
    }

    public virtual void OnDestroy() => this._buttonBinder.ClearBindings();

    public virtual void SetBackButton(bool visible, bool animated) => this._backButton.gameObject.SetActive(visible);

    [CompilerGenerated]
    public virtual void Awake_b__24_0()
    {
      Action buttonWasPressedEvent = this.backButtonWasPressedEvent;
      if (buttonWasPressedEvent == null)
        return;
      buttonWasPressedEvent();
    }
  }
}
