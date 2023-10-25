// Decompiled with JetBrains decompiler
// Type: IntInputFieldValidator
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D;
using UnityEngine;

public class IntInputFieldValidator : BaseInputFieldValidator<int>
{
  [SerializeField]
  private IntInputFieldValidator.ValidatorType _validatorType;
  [Space]
  [SerializeField]
  private int _min;
  [SerializeField]
  private int _max;

  protected override void ValidateInput(string input)
  {
    int result;
    if (!int.TryParse(input, out result))
    {
      this.ResetInputValue();
    }
    else
    {
      switch (this._validatorType)
      {
        case IntInputFieldValidator.ValidatorType.Clamp:
          result = Mathf.Clamp(result, this._min, this._max);
          break;
        case IntInputFieldValidator.ValidatorType.Min:
          result = Mathf.Min(result, this._min);
          break;
        case IntInputFieldValidator.ValidatorType.Max:
          result = Mathf.Max(result, this._max);
          break;
      }
      this.TriggerOnValidated(result);
    }
  }

  private enum ValidatorType
  {
    Clamp,
    Min,
    Max,
    None,
  }
}
