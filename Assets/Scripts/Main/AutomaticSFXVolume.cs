// Decompiled with JetBrains decompiler
// Type: AutomaticSFXVolume
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

[RequireComponent(typeof (AudioSource))]
public class AutomaticSFXVolume : MonoBehaviour
{
  protected const float kBaseVolume = 0.4f;
  [SerializeField]
  protected AutomaticSFXVolumeParamsSO _params;
  [Space]
  [SerializeField]
  protected AudioManagerSO _audioManager;
  [Inject]
  protected AutomaticSFXVolume.InitData _initData;
  protected float _sampleRate = 44100f;
  protected float _volume = 1f;
  protected float _envelope;
  protected float _attackSamples;
  protected float _releaseSamples;
  protected float _attackCoef;
  protected float _releaseCoef;
  protected float _maxVolume;

  public virtual void Start() => this.RecalculateParams();

  public virtual void OnDisable()
  {
    if (!((Object) this._audioManager != (Object) null))
      return;
    this._audioManager.sfxVolume = 0.0f;
  }

  public virtual void OnValidate() => this.RecalculateParams();

  public virtual void RecalculateParams()
  {
    this._sampleRate = (float) AudioSettings.outputSampleRate;
    float num1 = Mathf.Max(this._params.attackTime, 0.01f);
    float num2 = Mathf.Max(this._params.releaseTime, 0.01f);
    this._attackSamples = num1 * this._sampleRate;
    this._attackCoef = 1f / this._attackSamples;
    this._releaseSamples = num2 * this._sampleRate;
    this._releaseCoef = 1f / this._releaseSamples;
    this._maxVolume = this._initData != null ? Mathf.Min(this._params.maxVolume, this._initData.maxVolume) : this._params.maxVolume;
  }

  public virtual void OnAudioFilterRead(float[] data, int channels)
  {
    if (!this._initData.adaptiveSfx)
      return;
    int num1 = data.Length / channels;
    for (int index1 = 0; index1 < num1; ++index1)
    {
      float num2 = 0.0f;
      for (int index2 = 0; index2 < channels; ++index2)
        num2 += Mathf.Abs(data[index1 * channels + index2]) * this._params.musicVolumeMultiplier;
      if (((double) num2 <= (double) this._params.threshold ? 0.0 : (double) (num2 - this._params.threshold)) > (double) this._envelope)
        this._envelope += this._attackCoef;
      else if ((double) this._envelope > 0.0)
        this._envelope -= this._releaseCoef;
    }
    this._volume = Mathf.Lerp(this._volume, this._params.minVolume + this._envelope * this._params.impact, (float) num1 / this._sampleRate * this._params.volumeSmooth);
    if ((double) this._volume <= (double) this._maxVolume)
      return;
    this._volume = this._maxVolume;
  }

  public virtual void Update()
  {
    if (this._initData.adaptiveSfx)
      this._audioManager.sfxVolume = this._volume + this._initData.volumeOffset;
    else
      this._audioManager.sfxVolume = 0.4f + this._initData.volumeOffset;
  }

  public class InitData
  {
    public readonly float volumeOffset;
    public readonly float maxVolume;
    public readonly bool adaptiveSfx;

    public InitData(float volumeOffset, bool adaptiveSfx, float maxVolume = float.PositiveInfinity)
    {
      this.volumeOffset = volumeOffset;
      this.adaptiveSfx = adaptiveSfx;
      this.maxVolume = maxVolume;
    }
  }
}
