// Decompiled with JetBrains decompiler
// Type: ParticleSystemEventEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class ParticleSystemEventEffect : MonoBehaviour
{
  [SerializeField]
  protected ColorSO _lightColor0;
  [SerializeField]
  protected ColorSO _lightColor1;
  [SerializeField]
  protected ColorSO _highlightColor0;
  [SerializeField]
  protected ColorSO _highlightColor1;
  [SerializeField]
  protected bool _lightOnStart;
  [SerializeField]
  protected BasicBeatmapEventType _colorEvent;
  [Space]
  [SerializeField]
  protected ParticleSystem _particleSystem;
  [Inject]
  protected readonly BeatmapCallbacksController _beatmapCallbacksController;
  protected bool _lightIsOn;
  protected Color _offColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
  protected float _highlightValue;
  protected Color _afterHighlightColor;
  protected Color _highlightColor;
  protected float kFadeSpeed = 2f;
  protected ParticleSystem.MainModule _mainModule;
  protected ParticleSystem.Particle[] _particles;
  protected Color _particleColor;
  protected BeatmapDataCallbackWrapper _beatmapDataCallbackWrapper;

  public virtual void Start()
  {
    this._beatmapDataCallbackWrapper = this._beatmapCallbacksController.AddBeatmapCallback<BasicBeatmapEventData>(new BeatmapDataCallback<BasicBeatmapEventData>(this.HandleBeatmapEvent), BasicBeatmapEventData.SubtypeIdentifier(this._colorEvent));
    this._mainModule = this._particleSystem.main;
    this._particles = new ParticleSystem.Particle[this._mainModule.maxParticles];
    this._lightIsOn = this._lightOnStart;
    this._offColor = this._lightColor0.color.ColorWithAlpha(0.0f);
    this._particleColor = this._lightIsOn ? (Color) this._lightColor0 : this._offColor;
    this.RefreshParticles();
    this.enabled = false;
  }

  public virtual void OnDestroy()
  {
    if (this._beatmapCallbacksController == null)
      return;
    this._beatmapCallbacksController.RemoveBeatmapCallback(this._beatmapDataCallbackWrapper);
  }

  public virtual void Update()
  {
    if (!this._lightIsOn && (double) this._highlightValue == 0.0)
      return;
    this._particleColor = Color.Lerp(this._afterHighlightColor, this._highlightColor, this._highlightValue);
    this._highlightValue = Mathf.Lerp(this._highlightValue, 0.0f, Time.deltaTime * this.kFadeSpeed);
    if ((double) this._highlightValue < 9.9999997473787516E-05)
    {
      this._highlightValue = 0.0f;
      this._particleColor = this._afterHighlightColor;
      this.enabled = false;
    }
    this.RefreshParticles();
  }

  public virtual void HandleBeatmapEvent(BasicBeatmapEventData basicBeatmapEventData)
  {
    if (basicBeatmapEventData.value == 0)
    {
      this._lightIsOn = false;
      this._highlightValue = 0.0f;
      this.enabled = false;
      this._particleColor = this._offColor;
      this.RefreshParticles();
    }
    else if (basicBeatmapEventData.value == 1 || basicBeatmapEventData.value == 5)
    {
      this._lightIsOn = true;
      this._highlightValue = 0.0f;
      this.enabled = false;
      Color color = basicBeatmapEventData.value == 1 ? this._lightColor0.color : this._lightColor1.color;
      this._particleColor = color;
      this._offColor = color.ColorWithAlpha(0.0f);
      this.RefreshParticles();
    }
    else if (basicBeatmapEventData.value == 2 || basicBeatmapEventData.value == 6)
    {
      this._lightIsOn = true;
      this._highlightValue = 1f;
      this.enabled = true;
      this._highlightColor = (Color) (basicBeatmapEventData.value == 2 ? this._highlightColor0 : this._highlightColor1);
      this._offColor = this._highlightColor.ColorWithAlpha(0.0f);
      this._particleColor = this._highlightColor;
      this._afterHighlightColor = (Color) (basicBeatmapEventData.value == 2 ? this._lightColor0 : this._lightColor1);
    }
    else
    {
      if (basicBeatmapEventData.value != 3 && basicBeatmapEventData.value != 7 && basicBeatmapEventData.value != -1)
        return;
      this._lightIsOn = true;
      this._highlightValue = 1f;
      this.enabled = true;
      this._highlightColor = (Color) (basicBeatmapEventData.value == 3 ? this._highlightColor0 : this._highlightColor1);
      this._offColor = this._highlightColor.ColorWithAlpha(0.0f);
      this._particleColor = this._highlightColor;
      this._afterHighlightColor = this._offColor;
    }
  }

  public virtual void RefreshParticles()
  {
    this._mainModule.startColor = (ParticleSystem.MinMaxGradient) this._particleColor;
    this._particleSystem.GetParticles(this._particles, this._particles.Length);
    for (int index = 0; index < this._particleSystem.particleCount; ++index)
      this._particles[index].startColor = (Color32) this._particleColor;
    this._particleSystem.SetParticles(this._particles, this._particleSystem.particleCount);
  }
}
