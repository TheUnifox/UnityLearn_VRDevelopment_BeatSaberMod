// Decompiled with JetBrains decompiler
// Type: ObstacleSaberSoundEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class ObstacleSaberSoundEffect : MonoBehaviour
{
  [SerializeField]
  protected ObstacleSaberSparkleEffectManager _obstacleSaberSparkleEffectManager;
  [SerializeField]
  protected SaberType _saberType;
  [SerializeField]
  protected AudioSource _audioSource;
  [SerializeField]
  protected float _volume;
  protected const float kSmooth = 8f;
  protected float _targetVolume;

  public virtual void Awake()
  {
    this.enabled = false;
    this._audioSource.volume = 0.0f;
    this._obstacleSaberSparkleEffectManager.sparkleEffectDidStartEvent += new System.Action<SaberType>(this.HandleSparkleEffectDidStart);
    this._obstacleSaberSparkleEffectManager.sparkleEffectDidEndEvent += new System.Action<SaberType>(this.HandleSparkleEffecDidEnd);
  }

  public virtual void OnDestroy()
  {
    if (!((UnityEngine.Object) this._obstacleSaberSparkleEffectManager != (UnityEngine.Object) null))
      return;
    this._obstacleSaberSparkleEffectManager.sparkleEffectDidStartEvent -= new System.Action<SaberType>(this.HandleSparkleEffectDidStart);
    this._obstacleSaberSparkleEffectManager.sparkleEffectDidEndEvent -= new System.Action<SaberType>(this.HandleSparkleEffecDidEnd);
  }

  public virtual void LateUpdate()
  {
    this._audioSource.volume = Mathf.Lerp(this._audioSource.volume, this._targetVolume, Time.deltaTime * 8f);
    if ((double) this._audioSource.volume <= 0.0099999997764825821 && this._audioSource.isPlaying)
    {
      this.enabled = false;
      this._audioSource.Stop();
    }
    this.transform.position = this._obstacleSaberSparkleEffectManager.BurnMarkPosForSaberType(this._saberType);
  }

  public virtual void HandleSparkleEffectDidStart(SaberType saberType)
  {
    if (saberType != this._saberType)
      return;
    this.enabled = true;
    if (!this._audioSource.isPlaying)
    {
      this._audioSource.time = UnityEngine.Random.Range(0.0f, Mathf.Max(0.0f, this._audioSource.clip.length - 0.1f));
      this._audioSource.Play();
    }
    this._targetVolume = this._volume;
    this._audioSource.volume = this._volume;
  }

  public virtual void HandleSparkleEffecDidEnd(SaberType saberType)
  {
    if (saberType != this._saberType)
      return;
    this._targetVolume = 0.0f;
  }
}
