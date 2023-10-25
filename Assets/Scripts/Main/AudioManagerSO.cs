// Decompiled with JetBrains decompiler
// Type: AudioManagerSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using UnityEngine.Audio;

public class AudioManagerSO : PersistentScriptableObject
{
  [SerializeField]
  protected AudioMixer _audioMixer;
  [SerializeField]
  protected float _spatializerPluginLatency = 0.035f;
  [SerializeField]
  protected float _spatializerSfxVolumeOffset = -2.5f;
  public const float kDefaultMusicVolume = -4f;
  protected const string kMsHrtfSpatializerPluginName = "MS HRTF Spatializer";
  protected const string kSfxVolume = "SFXVolume";
  protected const string kMainVolume = "MainVolume";
  protected const string kMusicVolume = "MusicVolume";
  protected const string kMusicPitch = "MusicPitch";
  protected const string kMusicSpeed = "MusicSpeed";
  protected const string kMusicPitchShifterWet = "MusicPitchShifterWet";
  protected float _musicVolumeOffset = -2f;
  protected float _sfxVolumeOffset;
  protected float _sfxVolume;
  protected bool _sfxEnabled = true;

  public float sfxLatency => !(AudioSettings.GetSpatializerPluginName() == "MS HRTF Spatializer") ? 0.0f : this._spatializerPluginLatency;

  public virtual void Init()
  {
    this._audioMixer.updateMode = AudioMixerUpdateMode.UnscaledTime;
    this._sfxVolumeOffset = !(AudioSettings.GetSpatializerPluginName() == "MS HRTF Spatializer") ? 0.0f : this._spatializerSfxVolumeOffset;
    this._audioMixer.GetFloat("SFXVolume", out this._sfxVolume);
  }

  public float mainVolume
  {
    set => this._audioMixer.SetFloat("MainVolume", value);
  }

  public float musicVolume
  {
    set => this._audioMixer.SetFloat("MusicVolume", value + this._musicVolumeOffset);
  }

  public float sfxVolume
  {
    get => !this._sfxEnabled ? 0.0f : this._sfxVolume;
    set
    {
      this._sfxVolume = value;
      if (!this._sfxEnabled)
        return;
      this._audioMixer.SetFloat("SFXVolume", this._sfxVolume + this._sfxVolumeOffset);
    }
  }

  public bool sfxEnabled
  {
    get => this._sfxEnabled;
    set
    {
      this._sfxEnabled = value;
      this.sfxVolume = value ? this._sfxVolume : -100f;
    }
  }

  public float musicPitch
  {
    set
    {
      this._audioMixer.SetFloat("MusicPitch", value);
      if (Mathf.Approximately(value, 1f))
        this._audioMixer.SetFloat("MusicPitchShifterWet", -100f);
      else
        this._audioMixer.SetFloat("MusicPitchShifterWet", 0.0f);
    }
  }

  public float musicSpeed
  {
    set => this._audioMixer.SetFloat("MusicSpeed", value);
  }
}
