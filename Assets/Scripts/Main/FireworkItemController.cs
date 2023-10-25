// Decompiled with JetBrains decompiler
// Type: FireworkItemController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class FireworkItemController : MonoBehaviour
{
  [Header("Particle Systems")]
  [SerializeField]
  [NullAllowed]
  protected FireworkItemController.FireworkItemParticleSystem[] _particleSystems;
  [Header("Other")]
  [SerializeField]
  protected TubeBloomPrePassLight[] _lights;
  [SerializeField]
  protected AudioSource _audioSource;
  [Space]
  [SerializeField]
  protected float _lightFlashDuration = 1f;
  [SerializeField]
  protected AnimationCurve _lightIntensityCurve;
  [SerializeField]
  protected float _lightIntensityMultiplier = 1f;
  [Header("Color")]
  [SerializeField]
  protected bool _randomizeColor;
  [SerializeField]
  [DrawIf("_randomizeColor", false, DrawIfAttribute.DisablingType.DontDraw)]
  protected Color _lightsColor;
  [SerializeField]
  [DrawIf("_randomizeColor", true, DrawIfAttribute.DisablingType.DontDraw)]
  protected Gradient _lightsColorGradient;
  [Space]
  [SerializeField]
  protected bool _randomizeSpeed;
  [SerializeField]
  [DrawIf("_randomizeSpeed", true, DrawIfAttribute.DisablingType.DontDraw)]
  protected float _minSpeedMultiplier = 0.8f;
  [SerializeField]
  [DrawIf("_randomizeSpeed", true, DrawIfAttribute.DisablingType.DontDraw)]
  protected float _maxSpeedMultiplier = 1.16f;
  [SerializeField]
  protected AudioClip[] _explosionClips;
  protected RandomObjectPicker<AudioClip> _randomAudioPicker;
  protected DirectionalLight _directionalLight;
  protected float _directionalLightIntensity;
  protected bool _initialized;

  public DirectionalLight directionalLight
  {
    set => this._directionalLight = value;
  }

  public float directionalLightIntensity
  {
    set => this._directionalLightIntensity = value;
  }

  public event System.Action<FireworkItemController> didFinishEvent;

  public virtual void Awake() => this._randomAudioPicker = new RandomObjectPicker<AudioClip>(this._explosionClips, 0.2f);

  public virtual void OnDisable()
  {
    this.SetLightsColor(this._lightIntensityCurve.Evaluate(1f) * this._lightIntensityMultiplier);
    System.Action<FireworkItemController> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(this);
  }

  public virtual void Fire() => this.StartCoroutine(this.FireCoroutine());

  public virtual IEnumerator FireCoroutine()
  {
    FireworkItemController fireworkItemController = this;
    float soundTimeToCenter = fireworkItemController.transform.position.magnitude / 343f;
    // ISSUE: explicit non-virtual call
    __nonvirtual (fireworkItemController.InitializeParticleSystem());
    // ISSUE: explicit non-virtual call
    __nonvirtual (fireworkItemController.SetLightsColor(0.0f));
    foreach (FireworkItemController.FireworkItemParticleSystem particleSystem in fireworkItemController._particleSystems)
    {
      if (!particleSystem._isSubemitter)
        particleSystem._particleSystem.Play(false);
    }
    float elapsedTime = 0.0f;
    bool explosionSoundFired = false;
    while ((double) elapsedTime < (double) fireworkItemController._lightFlashDuration)
    {
      if (!explosionSoundFired && (double) elapsedTime > (double) soundTimeToCenter)
      {
        // ISSUE: explicit non-virtual call
        __nonvirtual (fireworkItemController.PlayExplosionSound());
        explosionSoundFired = true;
      }
      float time = elapsedTime / fireworkItemController._lightFlashDuration;
      float intensity = fireworkItemController._lightIntensityCurve.Evaluate(time) * fireworkItemController._lightIntensityMultiplier;
      // ISSUE: explicit non-virtual call
      __nonvirtual (fireworkItemController.SetLightsColor(intensity));
      elapsedTime += Time.deltaTime;
      yield return (object) null;
    }
    // ISSUE: explicit non-virtual call
    __nonvirtual (fireworkItemController.SetLightsColor(fireworkItemController._lightIntensityCurve.Evaluate(1f) * fireworkItemController._lightIntensityMultiplier));
    System.Action<FireworkItemController> didFinishEvent = fireworkItemController.didFinishEvent;
    if (didFinishEvent != null)
      didFinishEvent(fireworkItemController);
  }

  public virtual void SetLightsColor(float intensity)
  {
    Color color = this._lightsColor.ColorWithAlpha(intensity);
    for (int index = 0; index < this._lights.Length; ++index)
      this._lights[index].color = color;
    if (!((UnityEngine.Object) this._directionalLight != (UnityEngine.Object) null))
      return;
    this._directionalLight.color = color;
    this._directionalLight.intensity = color.a * this._directionalLightIntensity;
  }

  public virtual void PlayExplosionSound()
  {
    AudioClip clip = this._randomAudioPicker.PickRandomObject();
    if (!((UnityEngine.Object) clip != (UnityEngine.Object) null))
      return;
    this._audioSource.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
    this._audioSource.PlayOneShot(clip);
  }

  public virtual void InitializeParticleSystem()
  {
    if (this._initialized || !this._randomizeColor && !this._randomizeSpeed)
      return;
    ParticleSystem.MainModule[] mainModuleArray = new ParticleSystem.MainModule[this._particleSystems.Length];
    for (int index = 0; index < this._particleSystems.Length; ++index)
      mainModuleArray[index] = this._particleSystems[index]._particleSystem.main;
    float num1 = UnityEngine.Random.Range(0.0f, 1f);
    float num2 = 1f;
    if (this._randomizeSpeed)
      num2 = Mathf.Lerp(this._minSpeedMultiplier, this._maxSpeedMultiplier, num1);
    if (this._randomizeColor)
      this._lightsColor = this._lightsColorGradient.Evaluate(num1);
    for (int index = 0; index < this._particleSystems.Length; ++index)
    {
      if (this._particleSystems[index]._useMainColor)
        mainModuleArray[index].startColor = (ParticleSystem.MinMaxGradient) this._lightsColor;
      else if (this._particleSystems[index]._useOwnGradient)
        mainModuleArray[index].startColor = (ParticleSystem.MinMaxGradient) this._particleSystems[index]._particleColorGradient.Evaluate(num1);
    }
    for (int index = 0; index < this._particleSystems.Length; ++index)
    {
      if (this._particleSystems[index]._randomizeSpeed && this._randomizeSpeed)
        mainModuleArray[index].startSpeedMultiplier *= num2;
    }
    this._initialized = true;
  }

  [Serializable]
  public class FireworkItemParticleSystem
  {
    public ParticleSystem _particleSystem;
    public bool _isSubemitter;
    public bool _useMainColor = true;
    [DrawIf("_useMainColor", false, DrawIfAttribute.DisablingType.DontDraw)]
    public bool _useOwnGradient;
    [DrawIf("_useOwnGradient", true, DrawIfAttribute.DisablingType.DontDraw)]
    public Gradient _particleColorGradient;
    public bool _randomizeSpeed;
  }

  public class Pool : MonoMemoryPool<FireworkItemController>
  {
  }
}
