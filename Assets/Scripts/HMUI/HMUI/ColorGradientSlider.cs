// Decompiled with JetBrains decompiler
// Type: HMUI.ColorGradientSlider
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HMUI
{
  public class ColorGradientSlider : TextSlider, IPointerUpHandler, IEventSystemHandler
  {
    [SerializeField]
    protected string _textPrefix;
    [Space]
    [SerializeField]
    protected Color _color0;
    [SerializeField]
    protected Color _color1;
    [SerializeField]
    protected ImageView[] _gradientImages;
    [SerializeField]
    protected Color _darkColor;
    [SerializeField]
    protected Color _lightColor;
    [DoesNotRequireDomainReloadInit]
    protected static readonly StringBuilder _stringBuilder = new StringBuilder(16);

    public event Action<ColorGradientSlider, Color, ColorChangeUIEventType> colorDidChangeEvent;

    protected override void Awake()
    {
      base.Awake();
      this.numberOfSteps = 256;
      this.normalizedValueDidChangeEvent += new Action<TextSlider, float>(this.HandleNormalizedValueDidChange);
    }

    protected override void OnDestroy()
    {
      this.normalizedValueDidChangeEvent -= new Action<TextSlider, float>(this.HandleNormalizedValueDidChange);
      base.OnDestroy();
    }

    public virtual void SetColors(Color color0, Color color1)
    {
      this._color0 = color0;
      this._color1 = color1;
      this.UpdateVisuals();
    }

    protected override void UpdateVisuals()
    {
      base.UpdateVisuals();
      if ((double) Color.Lerp(this._color0, this._color1, this.normalizedValue).grayscale > 0.699999988079071)
      {
        this.handleColor = this._darkColor;
        this.valueTextColor = this._darkColor;
      }
      else
      {
        this.handleColor = this._lightColor;
        this.valueTextColor = this._lightColor;
      }
      foreach (ImageView gradientImage in this._gradientImages)
      {
        gradientImage.color0 = this._color0;
        gradientImage.color1 = this._color1;
      }
    }

    protected override string TextForNormalizedValue(float normalizedValue)
    {
      ColorGradientSlider._stringBuilder.Clear();
      ColorGradientSlider._stringBuilder.Append(this._textPrefix);
      ColorGradientSlider._stringBuilder.Append(Mathf.RoundToInt(normalizedValue * (float) byte.MaxValue));
      return ColorGradientSlider._stringBuilder.ToString();
    }

    public virtual void HandleNormalizedValueDidChange(TextSlider slider, float normalizedValue)
    {
      Action<ColorGradientSlider, Color, ColorChangeUIEventType> colorDidChangeEvent = this.colorDidChangeEvent;
      if (colorDidChangeEvent == null)
        return;
      colorDidChangeEvent(this, Color.Lerp(this._color0, this._color1, normalizedValue), ColorChangeUIEventType.Drag);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
      base.OnPointerUp(eventData);
      Action<ColorGradientSlider, Color, ColorChangeUIEventType> colorDidChangeEvent = this.colorDidChangeEvent;
      if (colorDidChangeEvent == null)
        return;
      colorDidChangeEvent(this, Color.Lerp(this._color0, this._color1, this.normalizedValue), ColorChangeUIEventType.PointerUp);
    }
  }
}
