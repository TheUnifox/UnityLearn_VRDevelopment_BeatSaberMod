// Decompiled with JetBrains decompiler
// Type: FormattedFloatListSettingsController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class FormattedFloatListSettingsController : ListSettingsController
{
  [SerializeField]
  protected float[] _values;
  [SerializeField]
  protected string _formattingString = "{0:0.0}";
  [SerializeField]
  protected FormattedFloatListSettingsController.ValueType valueType;
  protected float _value;
  protected float _min = float.MaxValue;
  protected float _max = float.MinValue;
  protected bool _hasValue;

  public event System.Action<FormattedFloatListSettingsController, float> valueDidChangeEvent;

  public float value => this._value;

  public float[] values
  {
    get => this._values;
    set
    {
      this._values = value;
      this.Refresh(false);
    }
  }

  public virtual void SetValue(float value, bool callCallback = false)
  {
    this._hasValue = true;
    this._value = value;
    this.Refresh(callCallback);
  }

  protected override bool GetInitValues(out int idx, out int numberOfElements)
  {
    idx = 0;
    if (!this._hasValue)
    {
      numberOfElements = 0;
      return false;
    }
    numberOfElements = this._values.Length;
    for (int index = 0; index < this._values.Length; ++index)
    {
      if (Mathf.Approximately(this._value, this._values[index]))
        idx = index;
      if ((double) this._values[index] < (double) this._min)
        this._min = this._values[index];
      if ((double) this._values[index] > (double) this._max)
        this._max = this._values[index];
    }
    return true;
  }

  protected override void ApplyValue(int idx)
  {
    this._value = this._values[idx];
    System.Action<FormattedFloatListSettingsController, float> valueDidChangeEvent = this.valueDidChangeEvent;
    if (valueDidChangeEvent == null)
      return;
    valueDidChangeEvent(this, this._value);
  }

  protected override string TextForValue(int idx)
  {
    float num = this._values[idx];
    if ((double) this._min != (double) this._max)
    {
      if (this.valueType == FormattedFloatListSettingsController.ValueType.Normalized)
        num = (float) (((double) num - (double) this._min) / ((double) this._max - (double) this._min));
      else if (this.valueType == FormattedFloatListSettingsController.ValueType.InvertedNormalized)
        num = (float) (1.0 - ((double) num - (double) this._min) / ((double) this._max - (double) this._min));
    }
    return string.Format(this._formattingString, (object) num);
  }

  public enum ValueType
  {
    Normal,
    Normalized,
    InvertedNormalized,
  }
}
