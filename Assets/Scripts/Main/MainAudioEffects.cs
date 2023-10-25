// Decompiled with JetBrains decompiler
// Type: MainAudioEffects
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class MainAudioEffects : MonoBehaviour
{
  [SerializeField]
  protected AudioLowPassFilter _audioLowPassFilter;
  [SerializeField]
  protected float _smooth = 8f;
  protected const int kDefaultCutoffFrequency = 22000;
  protected const int kLowPassCutoffFrequency = 150;
  protected float _targetFrequency;

  public virtual void Start()
  {
    this._audioLowPassFilter.enabled = false;
    this.enabled = false;
  }

  public virtual void LateUpdate()
  {
    double cutoffFrequency = (double) this._audioLowPassFilter.cutoffFrequency;
    float num = Mathf.Lerp((float) cutoffFrequency, this._targetFrequency, Time.deltaTime * this._smooth);
    if ((double) Mathf.Abs((float) cutoffFrequency - this._targetFrequency) < 1.0)
    {
      this.enabled = false;
      num = this._targetFrequency;
      if ((double) num == 22000.0)
        this._audioLowPassFilter.enabled = false;
    }
    this._audioLowPassFilter.cutoffFrequency = num;
  }

  public virtual void ResumeNormalSound()
  {
    this.enabled = true;
    this._targetFrequency = 22000f;
  }

  public virtual void TriggerLowPass()
  {
    this.enabled = true;
    this._audioLowPassFilter.enabled = true;
    this._targetFrequency = 150f;
  }
}
