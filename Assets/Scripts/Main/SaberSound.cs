// Decompiled with JetBrains decompiler
// Type: SaberSound
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class SaberSound : MonoBehaviour
{
  [SerializeField]
  protected Transform _saberTop;
  [SerializeField]
  protected AudioSource _audioSource;
  [SerializeField]
  protected AnimationCurve _pitchBySpeedCurve;
  [SerializeField]
  protected AnimationCurve _gainBySpeedCurve;
  [SerializeField]
  protected float _speedMultiplier = 0.05f;
  [SerializeField]
  protected float _upSmooth = 4f;
  [SerializeField]
  protected float _downSmooth = 4f;
  [Tooltip("No sound is produced if saber point moves more than this distance in one frame. This basically fixes the start sound problem.")]
  [SerializeField]
  protected float _noSoundTopThresholdSqr = 1f;
  protected Vector3 _prevPos;
  protected float _speed;

  public virtual void Start() => this._prevPos = this._saberTop.position;

  public virtual void Update()
  {
    Vector3 position = this._saberTop.position;
    if ((double) (this._prevPos - position).sqrMagnitude > (double) this._noSoundTopThresholdSqr)
      this._prevPos = position;
    float b = (double) Time.deltaTime != 0.0 ? this._speedMultiplier * Vector3.Distance(position, this._prevPos) / Time.deltaTime : 0.0f;
    this._speed = (double) b >= (double) this._speed ? Mathf.Clamp01(Mathf.Lerp(this._speed, b, Time.deltaTime * this._upSmooth)) : Mathf.Clamp01(Mathf.Lerp(this._speed, b, Time.deltaTime * this._downSmooth));
    this._audioSource.pitch = this._pitchBySpeedCurve.Evaluate(this._speed);
    this._audioSource.volume = this._gainBySpeedCurve.Evaluate(this._speed);
    this._prevPos = position;
  }
}
