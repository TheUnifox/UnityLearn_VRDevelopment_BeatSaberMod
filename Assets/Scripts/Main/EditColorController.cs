// Decompiled with JetBrains decompiler
// Type: EditColorController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using UnityEngine;
using UnityEngine.UI;

public class EditColorController : ViewController
{
  [SerializeField]
  protected HSVPanelController _hsvPanelController;
  [SerializeField]
  protected PreviousColorPanelController _previousColorPanelController;
  [Space]
  [SerializeField]
  protected Button _cancelButton;
  [SerializeField]
  protected Button _applyButton;
  protected System.Action<Color> _colorCallback;
  protected Color _initialColor;
  protected bool _colorChanged;

  public event System.Action<Color> didChangeColorEvent;

  public event System.Action<bool> didFinishEvent;

  public virtual void SetColorCallback(System.Action<Color> colorCallback) => this._colorCallback = colorCallback;

  public virtual void SetColor(Color color)
  {
    this._previousColorPanelController.AddColor(color);
    this._hsvPanelController.color = this._initialColor = color;
  }

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    this._hsvPanelController.colorDidChangeEvent += new System.Action<Color, ColorChangeUIEventType>(this.HandleHSVPanelControllerColorDidChange);
    this._previousColorPanelController.colorWasSelectedEvent += new System.Action<Color>(this.HandlePreviousColorPanelControllerColorWasSelected);
    if (firstActivation)
    {
      this.buttonBinder.AddBinding(this._cancelButton, new System.Action(this.HandleCancelButtonWasPressed));
      this.buttonBinder.AddBinding(this._applyButton, new System.Action(this.HandleApplyButtonWasPressed));
    }
    this._colorChanged = false;
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    this._hsvPanelController.colorDidChangeEvent -= new System.Action<Color, ColorChangeUIEventType>(this.HandleHSVPanelControllerColorDidChange);
    this._previousColorPanelController.colorWasSelectedEvent -= new System.Action<Color>(this.HandlePreviousColorPanelControllerColorWasSelected);
  }

  public virtual void HandleHSVPanelControllerColorDidChange(
    Color color,
    ColorChangeUIEventType colorChangeUIEventType)
  {
    if (colorChangeUIEventType == ColorChangeUIEventType.PointerUp)
      this._previousColorPanelController.AddColor(color);
    this.ChangeColor(color);
  }

  public virtual void HandlePreviousColorPanelControllerColorWasSelected(Color color)
  {
    this._previousColorPanelController.DiscardUpcomingColor();
    this._hsvPanelController.color = color;
    this.ChangeColor(color);
  }

  public virtual void HandleCancelButtonWasPressed()
  {
    this._previousColorPanelController.AddColor(this._initialColor);
    this.ChangeColor(this._initialColor);
    System.Action<bool> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(false);
  }

  public virtual void HandleApplyButtonWasPressed()
  {
    System.Action<bool> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(this._colorChanged);
  }

  public virtual void ChangeColor(Color color)
  {
    this._colorChanged = true;
    System.Action<Color> colorCallback = this._colorCallback;
    if (colorCallback != null)
      colorCallback(color);
    System.Action<Color> changeColorEvent = this.didChangeColorEvent;
    if (changeColorEvent == null)
      return;
    changeColorEvent(color);
  }
}
