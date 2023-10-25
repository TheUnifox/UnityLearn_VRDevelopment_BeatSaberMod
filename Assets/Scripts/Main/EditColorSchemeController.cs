// Decompiled with JetBrains decompiler
// Type: EditColorSchemeController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class EditColorSchemeController : MonoBehaviour
{
  [SerializeField]
  protected ColorSchemeColorsToggleGroup _colorSchemeColorsToggleGroup;
  [SerializeField]
  protected RGBPanelController _rgbPanelController;
  [SerializeField]
  protected HSVPanelController _hsvPanelController;
  [SerializeField]
  protected PreviousColorPanelController _previousColorPanelController;
  [SerializeField]
  protected Button _closeButton;
  protected ButtonBinder _buttonBinder;

  public event System.Action didFinishEvent;

  public event System.Action<ColorScheme> didChangeColorSchemeEvent;

  public virtual void SetColorScheme(ColorScheme colorScheme) => this._colorSchemeColorsToggleGroup.SetColorScheme(colorScheme);

  public virtual void Start()
  {
    this._colorSchemeColorsToggleGroup.selectedColorDidChangeEvent += new System.Action<Color>(this.HandleColorSchemeColorsToggleGroupSelectedColorDidChange);
    this._rgbPanelController.colorDidChangeEvent += new System.Action<Color, ColorChangeUIEventType>(this.HandleRGBPanelControllerColorDidChange);
    this._hsvPanelController.colorDidChangeEvent += new System.Action<Color, ColorChangeUIEventType>(this.HandleHSVPanelControllerColorDidChange);
    this._previousColorPanelController.colorWasSelectedEvent += new System.Action<Color>(this.HandlePreviousColorPanelControllerColorWasSelected);
    this._rgbPanelController.color = this._colorSchemeColorsToggleGroup.color;
    this._hsvPanelController.color = this._colorSchemeColorsToggleGroup.color;
    this._previousColorPanelController.AddColor(this._colorSchemeColorsToggleGroup.color);
    this._buttonBinder = new ButtonBinder();
    this._buttonBinder.AddBinding(this._closeButton, (System.Action) (() =>
    {
      System.Action didFinishEvent = this.didFinishEvent;
      if (didFinishEvent == null)
        return;
      didFinishEvent();
    }));
  }

  public virtual void OnDestroy()
  {
    if ((UnityEngine.Object) this._colorSchemeColorsToggleGroup != (UnityEngine.Object) null)
      this._colorSchemeColorsToggleGroup.selectedColorDidChangeEvent -= new System.Action<Color>(this.HandleColorSchemeColorsToggleGroupSelectedColorDidChange);
    if ((UnityEngine.Object) this._rgbPanelController != (UnityEngine.Object) null)
      this._rgbPanelController.colorDidChangeEvent -= new System.Action<Color, ColorChangeUIEventType>(this.HandleRGBPanelControllerColorDidChange);
    if ((UnityEngine.Object) this._previousColorPanelController != (UnityEngine.Object) null)
      this._previousColorPanelController.colorWasSelectedEvent -= new System.Action<Color>(this.HandlePreviousColorPanelControllerColorWasSelected);
    this._buttonBinder.ClearBindings();
  }

  public virtual void HandleColorSchemeColorsToggleGroupSelectedColorDidChange(Color color)
  {
    this._rgbPanelController.color = color;
    this._hsvPanelController.color = color;
    this._previousColorPanelController.AddColor(color);
  }

  public virtual void HandleRGBPanelControllerColorDidChange(
    Color color,
    ColorChangeUIEventType colorChangeUIEventType)
  {
    this._colorSchemeColorsToggleGroup.color = color;
    this._hsvPanelController.color = color;
    if (colorChangeUIEventType != ColorChangeUIEventType.PointerUp)
      return;
    this._previousColorPanelController.AddColor(color);
    ColorScheme fromEditedColors = this._colorSchemeColorsToggleGroup.CreateColorSchemeFromEditedColors();
    System.Action<ColorScheme> colorSchemeEvent = this.didChangeColorSchemeEvent;
    if (colorSchemeEvent == null)
      return;
    colorSchemeEvent(fromEditedColors);
  }

  public virtual void HandleHSVPanelControllerColorDidChange(
    Color color,
    ColorChangeUIEventType colorChangeUIEventType)
  {
    this._rgbPanelController.color = color;
    this._colorSchemeColorsToggleGroup.color = color;
    if (colorChangeUIEventType != ColorChangeUIEventType.PointerUp)
      return;
    this._previousColorPanelController.AddColor(color);
    ColorScheme fromEditedColors = this._colorSchemeColorsToggleGroup.CreateColorSchemeFromEditedColors();
    System.Action<ColorScheme> colorSchemeEvent = this.didChangeColorSchemeEvent;
    if (colorSchemeEvent == null)
      return;
    colorSchemeEvent(fromEditedColors);
  }

  public virtual void HandlePreviousColorPanelControllerColorWasSelected(Color color)
  {
    this._colorSchemeColorsToggleGroup.color = color;
    this._rgbPanelController.color = color;
    this._hsvPanelController.color = color;
    ColorScheme fromEditedColors = this._colorSchemeColorsToggleGroup.CreateColorSchemeFromEditedColors();
    System.Action<ColorScheme> colorSchemeEvent = this.didChangeColorSchemeEvent;
    if (colorSchemeEvent == null)
      return;
    colorSchemeEvent(fromEditedColors);
  }

  [CompilerGenerated]
  public virtual void m_CStartm_Eb__13_0()
  {
    System.Action didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent();
  }
}
