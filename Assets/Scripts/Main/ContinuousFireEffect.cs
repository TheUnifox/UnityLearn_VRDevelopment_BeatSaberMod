// Decompiled with JetBrains decompiler
// Type: ContinuousFireEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class ContinuousFireEffect : FireEffect
{
  [Space]
  [SerializeField]
  protected float _fadeInDuration = 1f;
  [SerializeField]
  protected float _fadeOutDuration = 1f;
  [Space]
  [SerializeField]
  protected float _sustainDuration = 1f;
  [SerializeField]
  protected AnimationCurve _flipbookSustainCurve = AnimationCurve.Linear(0.0f, 1f, 1f, 1f);
  [SerializeField]
  protected AnimationCurve _bloomSustainCurve = AnimationCurve.Linear(0.0f, 1f, 1f, 1f);
  [Inject]
  protected readonly IAudioTimeSource _audioTimeSource;
  protected float _fadeInEndTime;
  protected float _fadeOutStartTime;
  protected float _effectStartTime;
  protected float _effectEndTime;
  protected float _lastSustainProgress;
  protected float _lastFadeOutProgress;

  protected override void Start()
  {
    base.Start();
    this.SetInitialValues();
    this.enabled = false;
  }

  public virtual void Update() => this.UpdateEffect();

  protected override void HandleColorChangeBeatmapEvent(LightColorBeatmapEventData e)
  {
    BeatmapEventData sameTypeEventData = e.nextSameTypeEventData;
    float endTime = sameTypeEventData != null ? sameTypeEventData.time : this._audioTimeSource.songEndTime;
    if ((double) e.brightness <= 0.0)
    {
      this.EndEffect();
      this.enabled = false;
    }
    else
    {
      if (this.enabled)
        return;
      this.StartEffect(e.time, endTime);
      this.enabled = true;
    }
  }

  public virtual void SetInitialValues()
  {
    this._flipBookPropertyBlockController.materialPropertyBlock.SetFloat(FireEffectShaderHelper.effectStartSongTimePropertyId, -1000f);
    this.UpdateRenderers(0.0f, 0.0f);
    this.SetRenderersEnabled(false);
  }

  public virtual void StartEffect(float startTime, float endTime)
  {
    this._effectStartTime = startTime;
    this._effectEndTime = endTime;
    this._fadeInEndTime = Mathf.Min(this._effectStartTime + this._fadeInDuration, this._effectEndTime);
    this._fadeOutStartTime = Mathf.Max(this._effectEndTime - this._fadeOutDuration, this._effectStartTime);
    this.SetRenderersEnabled(true);
    this._flipBookPropertyBlockController.materialPropertyBlock.SetFloat(FireEffectShaderHelper.effectStartSongTimePropertyId, startTime);
    this._flipBookPropertyBlockController.ApplyChanges();
    this.UpdateEffect();
  }

  public virtual void EndEffect()
  {
    this.UpdateRenderers(0.0f, 0.0f);
    this.SetRenderersEnabled(false);
  }

  public virtual void UpdateEffect()
  {
    float songTime = this._audioTimeSource.songTime;
    double t1 = (double) Mathf.InverseLerp(this._effectStartTime, this._fadeInEndTime, Mathf.Clamp(songTime, this._effectStartTime, this._fadeInEndTime));
    float t2 = 1f - Mathf.InverseLerp(this._fadeOutStartTime, this._effectEndTime, Mathf.Clamp(songTime, this._fadeOutStartTime, this._effectEndTime));
    float time = (songTime - this._effectStartTime) / this._sustainDuration;
    float num1 = this._flipbookSustainCurve.Evaluate(time);
    float num2 = this._bloomSustainCurve.Evaluate(time);
    float num3 = Easing.OutQuad((float) t1) * Easing.InOutQuad(t2);
    this.UpdateRenderers(num3 * num1, num3 * num2);
  }

  public virtual void UpdateRenderers(float flipBookAlpha, float bloomAlpha)
  {
    this._flipBookPropertyBlockController.materialPropertyBlock.SetColor(FireEffectShaderHelper.colorPropertyId, new Color(1f, 1f, 1f, flipBookAlpha));
    this._flipBookPropertyBlockController.ApplyChanges();
    this._bloomPropertyBlockController.materialPropertyBlock.SetColor(FireEffectShaderHelper.colorPropertyId, new Color(1f, 1f, 1f, bloomAlpha * this._bloomIntensityMultiplier));
    this._bloomPropertyBlockController.ApplyChanges();
    this.NotifyAlphaWasChanged(bloomAlpha);
    this._privatePointLightPropertyBlockController.materialPropertyBlock.SetColor(FireEffectShaderHelper.privatePointLightColorPropertyId, this._pointLightColor * bloomAlpha);
    this._privatePointLightPropertyBlockController.ApplyChanges();
  }
}
