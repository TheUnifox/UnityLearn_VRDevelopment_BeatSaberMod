// Decompiled with JetBrains decompiler
// Type: ColorSaturationValueSlider
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorSaturationValueSlider : Slider2D, IPointerUpHandler, IEventSystemHandler
{
  [SerializeField]
  protected float _hue;
  [SerializeField]
  protected Graphic[] _graphics;
  [SerializeField]
  protected Color _darkColor;
  [SerializeField]
  protected Color _lightColor;

  public event System.Action<ColorSaturationValueSlider, Vector2, ColorChangeUIEventType> colorSaturationOrValueDidChangeEvent;

  protected override void Awake()
  {
    base.Awake();
    this.normalizedValueDidChangeEvent += new System.Action<Slider2D, Vector2>(this.HandleNormalizedValueDidChange);
  }

  protected override void OnDestroy()
  {
    this.normalizedValueDidChangeEvent -= new System.Action<Slider2D, Vector2>(this.HandleNormalizedValueDidChange);
    base.OnDestroy();
  }

  public virtual void SetHue(float hue)
  {
    this._hue = hue;
    this.UpdateVisuals();
  }

  protected override void UpdateVisuals()
  {
    base.UpdateVisuals();
    if ((double) Color.HSVToRGB(this._hue, this.normalizedValue.x, this.normalizedValue.y).grayscale > 0.699999988079071)
      this.handleColor = this._darkColor;
    else
      this.handleColor = this._lightColor;
    foreach (Graphic graphic in this._graphics)
      graphic.color = Color.HSVToRGB(this._hue, 1f, 1f);
  }

  public virtual void HandleNormalizedValueDidChange(Slider2D slider, Vector2 normalizedValue)
  {
    System.Action<ColorSaturationValueSlider, Vector2, ColorChangeUIEventType> valueDidChangeEvent = this.colorSaturationOrValueDidChangeEvent;
    if (valueDidChangeEvent == null)
      return;
    valueDidChangeEvent(this, normalizedValue, ColorChangeUIEventType.Drag);
  }

  public override void OnPointerUp(PointerEventData eventData)
  {
    base.OnPointerUp(eventData);
    System.Action<ColorSaturationValueSlider, Vector2, ColorChangeUIEventType> valueDidChangeEvent = this.colorSaturationOrValueDidChangeEvent;
    if (valueDidChangeEvent == null)
      return;
    valueDidChangeEvent(this, this.normalizedValue, ColorChangeUIEventType.PointerUp);
  }
}
