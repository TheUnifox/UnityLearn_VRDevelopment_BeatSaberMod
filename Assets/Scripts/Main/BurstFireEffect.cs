// Decompiled with JetBrains decompiler
// Type: BurstFireEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Tweening;
using UnityEngine;
using Zenject;

public class BurstFireEffect : FireEffect
{
  [Space]
  [SerializeField]
  protected float _fadeOutDuration = 1f;
  [SerializeField]
  protected AnimationCurve _flipbookFadeOutCurve = AnimationCurve.Linear(0.0f, 1f, 1f, 0.0f);
  [SerializeField]
  protected AnimationCurve _bloomFadeOutCurve = AnimationCurve.Linear(0.0f, 0.0f, 1f, 0.0f);
  [Inject]
  protected readonly IAudioTimeSource _audioTimeSource;
  [Inject]
  protected readonly SongTimeTweeningManager _songTimeTweeningManager;
  protected FloatTween _fadeOutTween;

  public virtual void Awake()
  {
    FloatTween floatTween = new FloatTween(0.0f, 1f, new System.Action<float>(this.UpdateFadeOutProgress), this._fadeOutDuration, EaseType.Linear);
    floatTween.onCompleted = new System.Action(this.EndEffect);
    floatTween.loop = false;
    this._fadeOutTween = floatTween;
  }

  protected override void Start()
  {
    base.Start();
    this.SetInitialValues();
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
    if (!((UnityEngine.Object) this._songTimeTweeningManager != (UnityEngine.Object) null))
      return;
    this._songTimeTweeningManager.KillAllTweens((object) this);
  }

  protected override void HandleColorChangeBeatmapEvent(LightColorBeatmapEventData e)
  {
    if ((double) Mathf.Abs(e.time - this._audioTimeSource.songTime) > (double) Time.deltaTime)
    {
      if (this._fadeOutTween.isKilled)
        return;
      this.EndEffect();
      this._fadeOutTween.Kill();
    }
    else
    {
      if (this._fadeOutTween.isActive || (double) e.brightness <= 0.0)
        return;
      this.StartEffect(e.time);
    }
  }

  public virtual void StartEffect(float time)
  {
    this.SetRenderersEnabled(true);
    this._flipBookPropertyBlockController.materialPropertyBlock.SetFloat(FireEffectShaderHelper.effectStartSongTimePropertyId, time);
    this._flipBookPropertyBlockController.ApplyChanges();
    this.UpdateFadeOutProgress(0.0f);
    this._songTimeTweeningManager.RestartTween((Tween) this._fadeOutTween, (object) this);
  }

  public virtual void EndEffect()
  {
    this.UpdateFadeOutProgress(1f);
    this.SetRenderersEnabled(false);
    this.NotifyAlphaWasChanged(0.0f);
  }

  public virtual void SetInitialValues()
  {
    this._flipBookPropertyBlockController.materialPropertyBlock.SetFloat(FireEffectShaderHelper.effectStartSongTimePropertyId, -1000f);
    this.UpdateFadeOutProgress(1f);
    this.SetRenderersEnabled(false);
    this.NotifyAlphaWasChanged(0.0f);
  }

  public virtual void UpdateFadeOutProgress(float fadeOutProgress)
  {
    MaterialPropertyBlock materialPropertyBlock1 = this._flipBookPropertyBlockController.materialPropertyBlock;
    float a = this._flipbookFadeOutCurve.Evaluate(fadeOutProgress);
    int colorPropertyId1 = FireEffectShaderHelper.colorPropertyId;
    Color color1 = new Color(1f, 1f, 1f, a);
    materialPropertyBlock1.SetColor(colorPropertyId1, color1);
    this._flipBookPropertyBlockController.ApplyChanges();
    MaterialPropertyBlock materialPropertyBlock2 = this._bloomPropertyBlockController.materialPropertyBlock;
    float currentAlpha = this._bloomFadeOutCurve.Evaluate(fadeOutProgress);
    int colorPropertyId2 = FireEffectShaderHelper.colorPropertyId;
    Color color2 = new Color(1f, 1f, 1f, currentAlpha * this._bloomIntensityMultiplier);
    materialPropertyBlock2.SetColor(colorPropertyId2, color2);
    this._bloomPropertyBlockController.ApplyChanges();
    this.NotifyAlphaWasChanged(currentAlpha);
    this._privatePointLightPropertyBlockController.materialPropertyBlock.SetColor(FireEffectShaderHelper.privatePointLightColorPropertyId, this._pointLightColor * currentAlpha);
    this._privatePointLightPropertyBlockController.ApplyChanges();
  }
}
