// Decompiled with JetBrains decompiler
// Type: LightColorGroupEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;
using Tweening;
using UnityEngine;
using Zenject;

public class LightColorGroupEffect
{
  protected readonly ColorManager _colorManager;
  protected readonly int _lightId;
  protected readonly LightWithIdManager _lightManager;
  protected readonly SongTimeTweeningManager _tweeningManager;
  protected readonly BeatmapCallbacksController _beatmapCallbacksController;
  protected readonly IBpmController _bpmController;
  protected readonly FloatTween _floatTween;
  protected readonly BeatmapDataCallbackWrapper _lightColorBeatmapEventCallbackWrapper;
  protected float _fromStrobeFrequency;
  protected float _toStrobeFrequency;
  protected Color _fromColor;
  protected Color _toColor;
  protected Color _alternativeFromColor;
  protected Color _alternativeToColor;
  protected bool _usingBoostColors;
  [DoesNotRequireDomainReloadInit]
  protected static readonly Color offColor = Color.clear;

  [Inject]
  public LightColorGroupEffect(
    LightColorGroupEffect.InitData initData,
    LightWithIdManager lightManager,
    SongTimeTweeningManager tweeningManager,
    ColorManager colorManager,
    BeatmapCallbacksController beatmapCallbacksController,
    IBpmController bpmController)
  {
    this._lightId = initData.lightId;
    this._lightManager = lightManager;
    this._tweeningManager = tweeningManager;
    this._colorManager = colorManager;
    this._beatmapCallbacksController = beatmapCallbacksController;
    this._bpmController = bpmController;
    this._usingBoostColors = false;
    Color color1 = this._colorManager.ColorForType(EnvironmentColorType.Color0, false).ColorWithAlpha(0.0f);
    Color color2 = this._colorManager.ColorForType(EnvironmentColorType.Color0, true).ColorWithAlpha(0.0f);
    this._floatTween = new FloatTween(0.0f, 1f, new System.Action<float>(this.SetColor), 0.0f, EaseType.Linear);
    this.SetData(color1, color1, color2, color2, 0.0f, 0.0f);
    this._floatTween.ForceOnUpdate();
    this._lightColorBeatmapEventCallbackWrapper = this._beatmapCallbacksController.AddBeatmapCallback<LightColorBeatmapEventData>(new BeatmapDataCallback<LightColorBeatmapEventData>(this.HandleColorChangeBeatmapEvent), LightColorBeatmapEventData.SubtypeIdentifier(initData.groupId, initData.elementId));
  }

  public virtual void Cleanup()
  {
    if ((UnityEngine.Object) this._tweeningManager != (UnityEngine.Object) null)
      this._tweeningManager.KillAllTweens((object) this);
    if (this._beatmapCallbacksController == null)
      return;
    this._beatmapCallbacksController.RemoveBeatmapCallback(this._lightColorBeatmapEventCallbackWrapper);
  }

  public virtual void HandleColorChangeBeatmapEvent(LightColorBeatmapEventData currentEventData)
  {
    EnvironmentColorType colorType1 = currentEventData.colorType;
    float brightness1 = currentEventData.brightness;
    LightColorBeatmapEventData sameTypeEventData = (LightColorBeatmapEventData) currentEventData.nextSameTypeEventData;
    if (sameTypeEventData == null || sameTypeEventData.transitionType != BeatmapEventTransitionType.Interpolate)
    {
      Color color1 = this.GetColor(colorType1, this._usingBoostColors, brightness1);
      Color color2 = this.GetColor(colorType1, !this._usingBoostColors, brightness1);
      this.SetData(color1, color1, color2, color2, (float) currentEventData.strobeBeatFrequency, (float) currentEventData.strobeBeatFrequency);
      if (currentEventData.strobeBeatFrequency > 0)
      {
        this._floatTween.SetStartTimeAndEndTime(currentEventData.time, sameTypeEventData != null ? sameTypeEventData.time : currentEventData.time + 1000f);
        this._tweeningManager.ResumeTween((Tween) this._floatTween, (object) this);
      }
      else
      {
        this._floatTween.Kill();
        this.SetColor(0.0f);
      }
    }
    else
    {
      EnvironmentColorType colorType2 = sameTypeEventData.colorType;
      float brightness2 = sameTypeEventData.brightness;
      Color color3 = this.GetColor(colorType1, this._usingBoostColors, brightness1);
      Color color4 = this.GetColor(colorType1, !this._usingBoostColors, brightness1);
      Color color5 = this.GetColor(colorType2, this._usingBoostColors, brightness2);
      Color color6 = this.GetColor(colorType2, !this._usingBoostColors, brightness2);
      this.SetData(color3, color5, color4, color6, (float) currentEventData.strobeBeatFrequency, (float) sameTypeEventData.strobeBeatFrequency);
      this._floatTween.SetStartTimeAndEndTime(currentEventData.time, sameTypeEventData.time);
      this._tweeningManager.ResumeTween((Tween) this._floatTween, (object) this);
    }
  }

  public virtual void UseBoostColors(bool useBoostColors)
  {
    if (useBoostColors == this._usingBoostColors)
      return;
    this.SetData(this._alternativeFromColor, this._alternativeToColor, this._fromColor, this._toColor, this._fromStrobeFrequency, this._toStrobeFrequency);
    this._usingBoostColors = useBoostColors;
    this._floatTween.ForceOnUpdate();
  }

  protected virtual Color GetColor(
    EnvironmentColorType colorType,
    bool colorBoost,
    float brightness)
  {
    Color color = this._colorManager.ColorForType(colorType, colorBoost);
    return color.ColorWithAlpha(color.a * brightness);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual void SetData(
    Color fromColor,
    Color toColor,
    Color alternativeFromColor,
    Color alternativeToColor,
    float fromStrobeBeatFrequency,
    float toStrobeBeatFrequency)
  {
    float num = this._bpmController.currentBpm.OneBeatDuration();
    this._fromColor = fromColor;
    this._toColor = toColor;
    this._alternativeFromColor = alternativeFromColor;
    this._alternativeToColor = alternativeToColor;
    this._fromStrobeFrequency = fromStrobeBeatFrequency / num;
    this._toStrobeFrequency = toStrobeBeatFrequency / num;
  }

  public virtual void SetColor(float t)
  {
    Color color = Color.LerpUnclamped(this._fromColor, this._toColor, t);
    if ((double) this._fromStrobeFrequency > 0.0 || (double) this._toStrobeFrequency > 0.0)
    {
      float num1 = t * this._floatTween.duration;
      float num2 = (float) ((double) num1 * (double) num1 / (2.0 * (double) this._floatTween.duration));
      if ((-(double) this._fromStrobeFrequency * (double) num2 + (double) this._fromStrobeFrequency * (double) num1 + (double) this._toStrobeFrequency * (double) num2) % 1.0 > 0.5)
        color = LightColorGroupEffect.offColor;
    }
    this._lightManager.SetColorForId(this._lightId, color);
  }

  public class InitData
  {
    public readonly int groupId;
    public readonly int elementId;
    public readonly int lightId;

    public InitData(int groupId, int elementId, int lightId)
    {
      this.groupId = groupId;
      this.elementId = elementId;
      this.lightId = lightId;
    }
  }
}
