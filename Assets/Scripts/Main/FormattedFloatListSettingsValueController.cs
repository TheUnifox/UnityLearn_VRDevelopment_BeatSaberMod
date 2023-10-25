// Decompiled with JetBrains decompiler
// Type: FormattedFloatListSettingsValueController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class FormattedFloatListSettingsValueController : ListSettingsController
{
  [SerializeField]
  protected FloatSO _settingsValue;
  [SerializeField]
  protected float[] _values;
  [SerializeField]
  protected string _formattingString = "{0:0.0}";
  [SerializeField]
  protected FormattedFloatListSettingsValueController.ValueType valueType;
  protected float _min = float.MaxValue;
  protected float _max = float.MinValue;

  protected override bool GetInitValues(out int idx, out int numberOfElements)
  {
    idx = 0;
    numberOfElements = this._values.Length;
    for (int index = 0; index < this._values.Length; ++index)
    {
      if ((double) (float) (ObservableVariableSO<float>) this._settingsValue == (double) this._values[index])
        idx = index;
      if ((double) this._values[index] < (double) this._min)
        this._min = this._values[index];
      if ((double) this._values[index] > (double) this._max)
        this._max = this._values[index];
    }
    return true;
  }

  protected override void ApplyValue(int idx) => this._settingsValue.value = this._values[idx];

  protected override string TextForValue(int idx)
  {
    float num = this._values[idx];
    if ((double) this._min != (double) this._max)
    {
      if (this.valueType == FormattedFloatListSettingsValueController.ValueType.Normalized)
        num = (float) (((double) num - (double) this._min) / ((double) this._max - (double) this._min));
      else if (this.valueType == FormattedFloatListSettingsValueController.ValueType.InvertedNormalized)
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
