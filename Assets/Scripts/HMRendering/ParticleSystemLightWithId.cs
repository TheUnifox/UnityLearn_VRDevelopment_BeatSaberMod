// Decompiled with JetBrains decompiler
// Type: ParticleSystemLightWithId
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using UnityEngine;

[ExecuteAlways]
public class ParticleSystemLightWithId : LightWithIdMonoBehaviour
{
  [SerializeField]
  protected ParticleSystem _particleSystem;
  [SerializeField]
  protected bool setOnlyOnce;
  [SerializeField]
  protected bool _setColorOnly;
  [SerializeField]
  [DrawIf("_setColorOnly", false, DrawIfAttribute.DisablingType.DontDraw)]
  protected float _intensity = 1f;
  [SerializeField]
  [DrawIf("_setColorOnly", false, DrawIfAttribute.DisablingType.DontDraw)]
  protected float _minAlpha;
  protected ParticleSystem.MainModule _mainModule;
  protected ParticleSystem.Particle[] _particles;

  public Color color => this._mainModule.startColor.color;

  public virtual void Awake()
  {
    this._mainModule = this._particleSystem.main;
    this._particles = new ParticleSystem.Particle[this._mainModule.maxParticles];
  }

  public override void ColorWasSet(Color color)
  {
    color.a = this._setColorOnly ? this._mainModule.startColor.color.a : Mathf.Max(this._minAlpha, color.a * this._intensity);
    this._mainModule.startColor = new ParticleSystem.MinMaxGradient(color);
    this._particleSystem.GetParticles(this._particles, this._particles.Length);
    for (int index = 0; index < this._particleSystem.particleCount; ++index)
      this._particles[index].startColor = (Color32) color;
    this._particleSystem.SetParticles(this._particles, this._particleSystem.particleCount);
    if (!this.setOnlyOnce)
      return;
    this.enabled = false;
  }
}
