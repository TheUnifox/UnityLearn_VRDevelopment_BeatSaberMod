// Decompiled with JetBrains decompiler
// Type: RGBPanelController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using UnityEngine;

public class RGBPanelController : MonoBehaviour
{
  [SerializeField]
  protected ColorGradientSlider _redSlider;
  [SerializeField]
  protected ColorGradientSlider _greenSlider;
  [SerializeField]
  protected ColorGradientSlider _blueSlider;
  protected Color _color;

  public event System.Action<Color, ColorChangeUIEventType> colorDidChangeEvent;

  public Color color
  {
    get => this._color;
    set
    {
      this._color = value;
      this.RefreshSlidersColors();
      this.RefreshSlidersValues();
    }
  }

  public virtual void Awake()
  {
    this._redSlider.colorDidChangeEvent += new System.Action<ColorGradientSlider, Color, ColorChangeUIEventType>(this.HandleSliderColorDidChange);
    this._greenSlider.colorDidChangeEvent += new System.Action<ColorGradientSlider, Color, ColorChangeUIEventType>(this.HandleSliderColorDidChange);
    this._blueSlider.colorDidChangeEvent += new System.Action<ColorGradientSlider, Color, ColorChangeUIEventType>(this.HandleSliderColorDidChange);
  }

  public virtual void OnDestroy()
  {
    if ((UnityEngine.Object) this._redSlider != (UnityEngine.Object) null)
      this._redSlider.colorDidChangeEvent -= new System.Action<ColorGradientSlider, Color, ColorChangeUIEventType>(this.HandleSliderColorDidChange);
    if ((UnityEngine.Object) this._greenSlider != (UnityEngine.Object) null)
      this._greenSlider.colorDidChangeEvent -= new System.Action<ColorGradientSlider, Color, ColorChangeUIEventType>(this.HandleSliderColorDidChange);
    if (!((UnityEngine.Object) this._blueSlider != (UnityEngine.Object) null))
      return;
    this._blueSlider.colorDidChangeEvent -= new System.Action<ColorGradientSlider, Color, ColorChangeUIEventType>(this.HandleSliderColorDidChange);
  }

  public virtual void HandleSliderColorDidChange(
    ColorGradientSlider slider,
    Color color,
    ColorChangeUIEventType colorChangeUIEventType)
  {
    this._color = color;
    System.Action<Color, ColorChangeUIEventType> colorDidChangeEvent = this.colorDidChangeEvent;
    if (colorDidChangeEvent != null)
      colorDidChangeEvent(this._color, colorChangeUIEventType);
    this.RefreshSlidersColors();
  }

  public virtual void RefreshSlidersValues()
  {
    this._redSlider.normalizedValue = this._color.r;
    this._greenSlider.normalizedValue = this._color.g;
    this._blueSlider.normalizedValue = this._color.b;
  }

  public virtual void RefreshSlidersColors()
  {
    this._redSlider.SetColors(this._color.ColorWithR(0.0f), this._color.ColorWithR(1f));
    this._greenSlider.SetColors(this._color.ColorWithG(0.0f), this._color.ColorWithG(1f));
    this._blueSlider.SetColors(this._color.ColorWithB(0.0f), this._color.ColorWithB(1f));
  }
}
