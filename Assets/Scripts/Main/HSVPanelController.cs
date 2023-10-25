// Decompiled with JetBrains decompiler
// Type: HSVPanelController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class HSVPanelController : MonoBehaviour
{
  [SerializeField]
  protected ColorSaturationValueSlider _colorSaturationValueSlider;
  [SerializeField]
  protected ColorHueSlider _colorHueSlider;
  protected Vector3 _hsvColor;

  public event System.Action<Color, ColorChangeUIEventType> colorDidChangeEvent;

  public Color color
  {
    get => Color.HSVToRGB(this._hsvColor.x, this._hsvColor.y, this._hsvColor.z);
    set
    {
      Color.RGBToHSV(value, out this._hsvColor.x, out this._hsvColor.y, out this._hsvColor.z);
      this.RefreshSlidersColors();
      this.RefreshSlidersValues();
    }
  }

  public virtual void Awake()
  {
    this._colorSaturationValueSlider.colorSaturationOrValueDidChangeEvent += new System.Action<ColorSaturationValueSlider, Vector2, ColorChangeUIEventType>(this.HandleColorSaturationOrValueDidChange);
    this._colorHueSlider.colorHueDidChangeEvent += new System.Action<ColorHueSlider, float, ColorChangeUIEventType>(this.HandleColorHueDidChange);
  }

  public virtual void OnDestroy()
  {
    if ((UnityEngine.Object) this._colorSaturationValueSlider != (UnityEngine.Object) null)
      this._colorSaturationValueSlider.colorSaturationOrValueDidChangeEvent -= new System.Action<ColorSaturationValueSlider, Vector2, ColorChangeUIEventType>(this.HandleColorSaturationOrValueDidChange);
    if (!((UnityEngine.Object) this._colorHueSlider != (UnityEngine.Object) null))
      return;
    this._colorHueSlider.colorHueDidChangeEvent -= new System.Action<ColorHueSlider, float, ColorChangeUIEventType>(this.HandleColorHueDidChange);
  }

  public virtual void HandleColorSaturationOrValueDidChange(
    ColorSaturationValueSlider slider,
    Vector2 colorSaturationAndValue,
    ColorChangeUIEventType colorChangeUIEventType)
  {
    this._hsvColor.y = colorSaturationAndValue.x;
    this._hsvColor.z = colorSaturationAndValue.y;
    System.Action<Color, ColorChangeUIEventType> colorDidChangeEvent = this.colorDidChangeEvent;
    if (colorDidChangeEvent != null)
      colorDidChangeEvent(Color.HSVToRGB(this._hsvColor.x, this._hsvColor.y, this._hsvColor.z), colorChangeUIEventType);
    this.RefreshSlidersColors();
  }

  public virtual void HandleColorHueDidChange(
    ColorHueSlider slider,
    float hue,
    ColorChangeUIEventType colorChangeUIEventType)
  {
    this._hsvColor.x = hue;
    System.Action<Color, ColorChangeUIEventType> colorDidChangeEvent = this.colorDidChangeEvent;
    if (colorDidChangeEvent != null)
      colorDidChangeEvent(Color.HSVToRGB(this._hsvColor.x, this._hsvColor.y, this._hsvColor.z), colorChangeUIEventType);
    this.RefreshSlidersColors();
  }

  public virtual void RefreshSlidersValues()
  {
    this._colorSaturationValueSlider.normalizedValue = new Vector2(this._hsvColor.y, this._hsvColor.z);
    this._colorHueSlider.normalizedValue = this._hsvColor.x;
  }

  public virtual void RefreshSlidersColors() => this._colorSaturationValueSlider.SetHue(this._hsvColor.x);
}
