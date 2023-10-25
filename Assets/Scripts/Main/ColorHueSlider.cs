// Decompiled with JetBrains decompiler
// Type: ColorHueSlider
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using UnityEngine;
using UnityEngine.EventSystems;

public class ColorHueSlider : CircleSlider, IPointerUpHandler, IEventSystemHandler
{
  [SerializeField]
  protected Color _darkColor;
  [SerializeField]
  protected Color _lightColor;

  public event System.Action<ColorHueSlider, float, ColorChangeUIEventType> colorHueDidChangeEvent;

  protected override void Awake()
  {
    base.Awake();
    this.normalizedValueDidChangeEvent += new System.Action<CircleSlider, float>(this.HandleNormalizedValueDidChange);
  }

  protected override void OnDestroy()
  {
    this.normalizedValueDidChangeEvent -= new System.Action<CircleSlider, float>(this.HandleNormalizedValueDidChange);
    base.OnDestroy();
  }

  protected override void UpdateVisuals()
  {
    base.UpdateVisuals();
    if ((double) Color.HSVToRGB(this.normalizedValue, 1f, 1f).grayscale > 0.699999988079071)
      this.handleColor = this._darkColor;
    else
      this.handleColor = this._lightColor;
  }

  public virtual void HandleNormalizedValueDidChange(CircleSlider slider, float normalizedValue)
  {
    System.Action<ColorHueSlider, float, ColorChangeUIEventType> hueDidChangeEvent = this.colorHueDidChangeEvent;
    if (hueDidChangeEvent == null)
      return;
    hueDidChangeEvent(this, normalizedValue, ColorChangeUIEventType.Drag);
  }

  public override void OnPointerUp(PointerEventData eventData)
  {
    base.OnPointerUp(eventData);
    System.Action<ColorHueSlider, float, ColorChangeUIEventType> hueDidChangeEvent = this.colorHueDidChangeEvent;
    if (hueDidChangeEvent == null)
      return;
    hueDidChangeEvent(this, this.normalizedValue, ColorChangeUIEventType.PointerUp);
  }
}
