// Decompiled with JetBrains decompiler
// Type: HMUI.RangeValuesTextSlider
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace HMUI
{
  public class RangeValuesTextSlider : TextSlider
  {
    [SerializeField]
    protected float _minValue;
    [SerializeField]
    protected float _maxValue = 1f;
    [SerializeField]
    [NullAllowed]
    protected Button _decButton;
    [SerializeField]
    [NullAllowed]
    protected Button _incButton;
    protected ButtonBinder _buttonBinder;

    public float minValue
    {
      get => this._minValue;
      set
      {
        if (!SetPropertyUtility.SetStruct<float>(ref this._minValue, value))
          return;
        this.UpdateVisuals();
      }
    }

    public float maxValue
    {
      get => this._maxValue;
      set
      {
        if (!SetPropertyUtility.SetStruct<float>(ref this._maxValue, value))
          return;
        this.UpdateVisuals();
      }
    }

    public float value
    {
      set => this.normalizedValue = this.NormalizeValue(value);
      get => this.ConvertFromNormalizedValue(this.normalizedValue);
    }

    public event Action<RangeValuesTextSlider, float> valueDidChangeEvent;

    protected override void Awake()
    {
      base.Awake();
      this.normalizedValueDidChangeEvent += new Action<TextSlider, float>(this.HandleNormalizedValueDidChange);
      if (!((UnityEngine.Object) this._decButton != (UnityEngine.Object) null) || !((UnityEngine.Object) this._incButton != (UnityEngine.Object) null))
        return;
      this._buttonBinder = new ButtonBinder();
      this._buttonBinder.AddBinding(this._decButton, (Action) (() => this.SetNormalizedValue(this.normalizedValue - (this.numberOfSteps > 0 ? 1f / (float) this.numberOfSteps : 0.1f))));
      this._buttonBinder.AddBinding(this._incButton, (Action) (() => this.SetNormalizedValue(this.normalizedValue + (this.numberOfSteps > 0 ? 1f / (float) this.numberOfSteps : 0.1f))));
    }

    protected override void OnDestroy()
    {
      this.normalizedValueDidChangeEvent -= new Action<TextSlider, float>(this.HandleNormalizedValueDidChange);
      this._buttonBinder?.ClearBindings();
      base.OnDestroy();
    }

    public virtual void HandleNormalizedValueDidChange(TextSlider slider, float normalizedValue)
    {
      Action<RangeValuesTextSlider, float> valueDidChangeEvent = this.valueDidChangeEvent;
      if (valueDidChangeEvent == null)
        return;
      valueDidChangeEvent(this, this.ConvertFromNormalizedValue(normalizedValue));
    }

    public virtual float ConvertFromNormalizedValue(float normalizedValue) => normalizedValue * (this._maxValue - this._minValue) + this._minValue;

    public virtual float NormalizeValue(float rangeValue) => (float) (((double) rangeValue - (double) this._minValue) / ((double) this._maxValue - (double) this._minValue));

    protected override string TextForNormalizedValue(float normalizedValue) => this.TextForValue(this.ConvertFromNormalizedValue(normalizedValue));

    protected virtual string TextForValue(float value) => value.ToString((IFormatProvider) CultureInfo.InvariantCulture);

    [CompilerGenerated]
    public virtual void Awake_b__17_0() => this.SetNormalizedValue(this.normalizedValue - (this.numberOfSteps > 0 ? 1f / (float) this.numberOfSteps : 0.1f));

    [CompilerGenerated]
    public virtual void Awake_b__17_1() => this.SetNormalizedValue(this.normalizedValue + (this.numberOfSteps > 0 ? 1f / (float) this.numberOfSteps : 0.1f));
  }
}
