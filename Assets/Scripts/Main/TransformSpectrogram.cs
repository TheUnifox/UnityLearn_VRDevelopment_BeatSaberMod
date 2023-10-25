// Decompiled with JetBrains decompiler
// Type: TransformSpectrogram
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class TransformSpectrogram : MonoBehaviour
{
  [SerializeField]
  protected Transform[] _transforms;
  [Space]
  [SerializeField]
  protected LightAxis _axis = LightAxis.Y;
  [SerializeField]
  protected float _minPosition;
  [SerializeField]
  protected float _maxPosition;
  [SerializeField]
  [Tooltip("Scale the samples based on element count")]
  protected bool _scaleSamples = true;
  [SerializeField]
  [Tooltip("Allows repeating the spectrogram or only using a part of it")]
  protected float _scale = 1f;
  [Inject]
  protected readonly BasicSpectrogramData _spectrogramData;
  protected Vector3 _direction;
  protected Vector3[] _defaultPositions;

  public virtual void Awake()
  {
    Vector3 a = Vector3.zero;
    switch (this._axis)
    {
      case LightAxis.X:
        this._direction = new Vector3(1f, 0.0f, 0.0f);
        a = new Vector3(0.0f, 1f, 1f);
        break;
      case LightAxis.Y:
        this._direction = new Vector3(0.0f, 1f, 0.0f);
        a = new Vector3(1f, 0.0f, 1f);
        break;
      case LightAxis.Z:
        this._direction = new Vector3(0.0f, 0.0f, 1f);
        a = new Vector3(1f, 1f, 0.0f);
        break;
    }
    this._defaultPositions = new Vector3[this._transforms.Length];
    for (int index = 0; index < this._transforms.Length; ++index)
      this._defaultPositions[index] = Vector3.Scale(a, this._transforms[index].localPosition);
  }

  public virtual void Update()
  {
    for (int index = 0; index < this._transforms.Length; ++index)
    {
      float processedSample = this._spectrogramData.ProcessedSamples[(this._scaleSamples ? Mathf.RoundToInt((float) ((double) index / ((double) this._transforms.Length - 1.0) * 63.0) * this._scale) : index) % 64];
      this._transforms[index].localPosition = this._defaultPositions[index] + this._direction * Mathf.Lerp(this._minPosition, this._maxPosition, processedSample);
    }
  }
}
