// Decompiled with JetBrains decompiler
// Type: SmoothCameraSmoothnessSettingsController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class SmoothCameraSmoothnessSettingsController : ListSettingsController
{
  [SerializeField]
  protected FloatSO _smoothCameraPositionSmooth;
  [SerializeField]
  protected FloatSO _smoothCameraRotationSmooth;
  protected float[] _smoothnesses;

  protected override bool GetInitValues(out int idx, out int numberOfElements)
  {
    this._smoothnesses = new float[11]
    {
      22f,
      20f,
      18f,
      16f,
      14f,
      12f,
      10f,
      8f,
      6f,
      4f,
      2f
    };
    FloatSO cameraPositionSmooth = this._smoothCameraPositionSmooth;
    idx = 2;
    numberOfElements = this._smoothnesses.Length;
    for (int index = 0; index < this._smoothnesses.Length; ++index)
    {
      if ((double) (float) (ObservableVariableSO<float>) cameraPositionSmooth == (double) this._smoothnesses[index])
      {
        idx = index;
        return true;
      }
    }
    return true;
  }

  protected override void ApplyValue(int idx)
  {
    this._smoothCameraPositionSmooth.value = this._smoothnesses[idx];
    this._smoothCameraRotationSmooth.value = this._smoothnesses[idx];
  }

  protected override string TextForValue(int idx) => string.Format("{0:0.0}", (object) (float) (1.0 - ((double) this._smoothnesses[idx] - (double) this._smoothnesses[this._smoothnesses.Length - 1]) / ((double) this._smoothnesses[0] - (double) this._smoothnesses[this._smoothnesses.Length - 1])));
}
