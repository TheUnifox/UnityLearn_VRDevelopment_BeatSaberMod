// Decompiled with JetBrains decompiler
// Type: AutomaticSFXVolumeParamsSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class AutomaticSFXVolumeParamsSO : ScriptableObject
{
  [SerializeField]
  protected float _musicVolumeMultiplier = 1f;
  [SerializeField]
  protected float _threshold = 0.3f;
  [SerializeField]
  protected float _impact = 16f;
  [SerializeField]
  protected float _attackTime = 1f;
  [SerializeField]
  protected float _releaseTime = 2f;
  [SerializeField]
  protected float _minVolume = -9f;
  [SerializeField]
  protected float _maxVolume = 20f;
  [SerializeField]
  protected float _volumeSmooth = 4f;

  public float musicVolumeMultiplier => this._musicVolumeMultiplier;

  public float threshold => this._threshold;

  public float impact => this._impact;

  public float attackTime => this._attackTime;

  public float releaseTime => this._releaseTime;

  public float minVolume => this._minVolume;

  public float maxVolume => this._maxVolume;

  public float volumeSmooth => this._volumeSmooth;
}
