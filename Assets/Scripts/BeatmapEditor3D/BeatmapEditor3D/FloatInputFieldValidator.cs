// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.FloatInputFieldValidator
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using UnityEngine;

namespace BeatmapEditor3D
{
  public class FloatInputFieldValidator : BaseInputFieldValidator<float>
  {
    [SerializeField]
    private FloatInputFieldValidator.ValidatorType _validatorType;
    [Space]
    [SerializeField]
    private float _min;
    [SerializeField]
    private float _max;

    protected override void ValidateInput(string input)
    {
      float result;
      if (!float.TryParse(input, out result))
      {
        this.ResetInputValue();
      }
      else
      {
        switch (this._validatorType)
        {
          case FloatInputFieldValidator.ValidatorType.Clamp:
            result = Mathf.Clamp(result, this._min, this._max);
            break;
          case FloatInputFieldValidator.ValidatorType.Min:
            result = Mathf.Min(result, this._min);
            break;
          case FloatInputFieldValidator.ValidatorType.Max:
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
}
