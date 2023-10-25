// Decompiled with JetBrains decompiler
// Type: FixedUpdateSmoothValue`1
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

public abstract class FixedUpdateSmoothValue<T>
{
  private T _currentSmoothedValue;
  private T _prevSmoothedValue;
  private readonly float _smooth;

  protected FixedUpdateSmoothValue(float smooth) => this._smooth = smooth;

  public void SetStartValue(T value)
  {
    this._currentSmoothedValue = value;
    this._prevSmoothedValue = value;
  }

  public void FixedUpdate(T value)
  {
    this._prevSmoothedValue = this._currentSmoothedValue;
    this._currentSmoothedValue = this.Interpolate(this._currentSmoothedValue, value, 1f / this._smooth);
  }

  public T GetValue(float interpolationFactor) => this.Interpolate(this._prevSmoothedValue, this._currentSmoothedValue, interpolationFactor);

  protected abstract T Interpolate(T value0, T value1, float t);
}
