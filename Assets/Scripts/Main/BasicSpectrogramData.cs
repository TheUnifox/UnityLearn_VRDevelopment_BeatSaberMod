// Decompiled with JetBrains decompiler
// Type: BasicSpectrogramData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;

public class BasicSpectrogramData : MonoBehaviour
{
  [SerializeField]
  protected AudioSource _audioSource;
  [SerializeField]
  protected float _instantChangeThreshold = 0.1f;
  public const int kNumberOfSamples = 64;
  protected bool _hasData;
  protected bool _hasProcessedData;
  protected float[] _samples = new float[64];
  protected List<float> _processedSamples = new List<float>(64);

  public float[] Samples
  {
    get
    {
      if (!this._hasData && (bool) (Object) this._audioSource)
      {
        this._audioSource.GetSpectrumData(this._samples, 0, FFTWindow.BlackmanHarris);
        this._hasData = true;
      }
      return this._samples;
    }
  }

  public List<float> ProcessedSamples
  {
    get
    {
      if (!this._hasProcessedData)
      {
        this.ProcessSamples(this.Samples, this._processedSamples);
        this._hasProcessedData = true;
      }
      return this._processedSamples;
    }
  }

  public virtual void Awake()
  {
    this._hasData = false;
    this._hasProcessedData = false;
    for (int index = 0; index < 64; ++index)
      this._processedSamples.Add(0.0f);
  }

  public virtual void LateUpdate()
  {
    this._hasData = false;
    this._hasProcessedData = false;
  }

  public virtual void ProcessSamples(float[] sourceSamples, List<float> processedSamples)
  {
    float deltaTime = Time.deltaTime;
    for (int index = 0; index < sourceSamples.Length; ++index)
    {
      float b = Mathf.Log(sourceSamples[index] + 1f) * (float) (index + 1);
      processedSamples[index] = (double) processedSamples[index] >= (double) b ? Mathf.Lerp(processedSamples[index], b, deltaTime * 4f) : ((double) b - (double) processedSamples[index] <= (double) this._instantChangeThreshold ? Mathf.Lerp(processedSamples[index], b, deltaTime * 8f) : b);
    }
  }
}
