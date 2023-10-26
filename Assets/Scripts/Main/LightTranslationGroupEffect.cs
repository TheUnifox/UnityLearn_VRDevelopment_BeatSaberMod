// Decompiled with JetBrains decompiler
// Type: LightTranslationGroupEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using Tweening;
using UnityEngine;

public class LightTranslationGroupEffect
{
  protected readonly SongTimeTweeningManager _tweeningManager;
  protected readonly BeatmapCallbacksController _beatmapCallbacksController;
  protected readonly List<(Transform transform, Vector3 mask)> _transformMask;
  protected readonly FloatTween _xTranslationTween;
  protected readonly FloatTween _yTranslationTween;
  protected readonly FloatTween _zTranslationTween;
  protected readonly BeatmapDataCallbackWrapper _lightTranslationXBeatmapEventCallbackWrapper;
  protected readonly BeatmapDataCallbackWrapper _lightTranslationYBeatmapEventCallbackWrapper;
  protected readonly BeatmapDataCallbackWrapper _lightTranslationZBeatmapEventCallbackWrapper;

  public LightTranslationGroupEffect(
    LightTranslationGroupEffect.InitData initData,
    SongTimeTweeningManager tweeningManager,
    BeatmapCallbacksController beatmapCallbacksController)
  {
    this._tweeningManager = tweeningManager;
    this._beatmapCallbacksController = beatmapCallbacksController;
    this._transformMask = new List<(Transform, Vector3)>(3);
    if ((UnityEngine.Object) initData.xTransform == (UnityEngine.Object) initData.yTransform && (UnityEngine.Object) initData.xTransform == (UnityEngine.Object) initData.zTransform && (UnityEngine.Object) initData.xTransform != (UnityEngine.Object) null)
      this._transformMask.Add((initData.xTransform, Vector3.one));
    else if ((UnityEngine.Object) initData.xTransform == (UnityEngine.Object) initData.yTransform && (UnityEngine.Object) initData.xTransform != (UnityEngine.Object) null)
    {
      this._transformMask.Add((initData.xTransform, new Vector3(1f, 1f, 0.0f)));
      if ((UnityEngine.Object) initData.zTransform != (UnityEngine.Object) null)
        this._transformMask.Add((initData.zTransform, new Vector3(0.0f, 0.0f, 1f)));
    }
    else if ((UnityEngine.Object) initData.yTransform == (UnityEngine.Object) initData.zTransform && (UnityEngine.Object) initData.yTransform != (UnityEngine.Object) null)
    {
      this._transformMask.Add((initData.yTransform, new Vector3(0.0f, 1f, 1f)));
      if ((UnityEngine.Object) initData.xTransform != (UnityEngine.Object) null)
        this._transformMask.Add((initData.xTransform, new Vector3(1f, 0.0f, 0.0f)));
    }
    else if ((UnityEngine.Object) initData.xTransform == (UnityEngine.Object) initData.zTransform && (UnityEngine.Object) initData.xTransform != (UnityEngine.Object) null)
    {
      this._transformMask.Add((initData.xTransform, new Vector3(1f, 0.0f, 1f)));
      if ((UnityEngine.Object) initData.yTransform != (UnityEngine.Object) null)
        this._transformMask.Add((initData.yTransform, new Vector3(0.0f, 1f, 0.0f)));
    }
    else
    {
      if ((UnityEngine.Object) initData.xTransform != (UnityEngine.Object) null)
        this._transformMask.Add((initData.xTransform, new Vector3(1f, 0.0f, 0.0f)));
      if ((UnityEngine.Object) initData.yTransform != (UnityEngine.Object) null)
        this._transformMask.Add((initData.yTransform, new Vector3(0.0f, 1f, 0.0f)));
      if ((UnityEngine.Object) initData.zTransform != (UnityEngine.Object) null)
        this._transformMask.Add((initData.zTransform, new Vector3(0.0f, 0.0f, 1f)));
    }
    this._xTranslationTween = new FloatTween(0.0f, 0.0f, new System.Action<float>(this.SetTranslation), 0.0f, EaseType.None);
    this._yTranslationTween = new FloatTween(0.0f, 0.0f, new System.Action<float>(this.SetTranslation), 0.0f, EaseType.None);
    this._zTranslationTween = new FloatTween(0.0f, 0.0f, new System.Action<float>(this.SetTranslation), 0.0f, EaseType.None);
    this._xTranslationTween.ForceOnUpdate();
    this._yTranslationTween.ForceOnUpdate();
    this._zTranslationTween.ForceOnUpdate();
    this._lightTranslationXBeatmapEventCallbackWrapper = this._beatmapCallbacksController.AddBeatmapCallback<LightTranslationBeatmapEventData>(this.GetTranslationEventHandler(this._xTranslationTween, initData.xTranslationLimits, initData.xDistributionLimits, initData.xMirrored), LightTranslationBeatmapEventData.SubtypeIdentifier(initData.groupId, initData.elementId, LightAxis.X));
    this._lightTranslationYBeatmapEventCallbackWrapper = this._beatmapCallbacksController.AddBeatmapCallback<LightTranslationBeatmapEventData>(this.GetTranslationEventHandler(this._yTranslationTween, initData.yTranslationLimits, initData.yDistributionLimits, initData.yMirrored), LightTranslationBeatmapEventData.SubtypeIdentifier(initData.groupId, initData.elementId, LightAxis.Y));
    this._lightTranslationZBeatmapEventCallbackWrapper = this._beatmapCallbacksController.AddBeatmapCallback<LightTranslationBeatmapEventData>(this.GetTranslationEventHandler(this._zTranslationTween, initData.zTranslationLimits, initData.zDistributionLimits, initData.zMirrored), LightTranslationBeatmapEventData.SubtypeIdentifier(initData.groupId, initData.elementId, LightAxis.Z));
  }

  public virtual void Cleanup()
  {
    this._tweeningManager.KillAllTweens((object) this);
    if (this._beatmapCallbacksController == null)
      return;
    this._beatmapCallbacksController.RemoveBeatmapCallback(this._lightTranslationXBeatmapEventCallbackWrapper);
    this._beatmapCallbacksController.RemoveBeatmapCallback(this._lightTranslationYBeatmapEventCallbackWrapper);
    this._beatmapCallbacksController.RemoveBeatmapCallback(this._lightTranslationZBeatmapEventCallbackWrapper);
  }

  public virtual BeatmapDataCallback<LightTranslationBeatmapEventData> GetTranslationEventHandler(
    FloatTween translationTween,
    Vector2 translationLimits,
    Vector2 distributionLimits,
    bool mirrored)
  {
    return (BeatmapDataCallback<LightTranslationBeatmapEventData>) (currentEventData =>
    {
      translationTween.Kill();
      float translation1 = LightTranslationGroupEffect.ComputeTranslation(currentEventData.translation, translationLimits, currentEventData.distribution, distributionLimits, mirrored);
      LightTranslationBeatmapEventData sameTypeEventData = (LightTranslationBeatmapEventData) currentEventData.nextSameTypeEventData;
      if (sameTypeEventData == null || sameTypeEventData.easeType.ToEaseType() == EaseType.None)
      {
        LightTranslationGroupEffect.SetTweenData(translationTween, translation1, translation1, currentEventData.time, currentEventData.time, EaseType.Linear);
        translationTween.Update(0.0f);
      }
      else
      {
        float translation2 = LightTranslationGroupEffect.ComputeTranslation(sameTypeEventData.translation, translationLimits, sameTypeEventData.distribution, distributionLimits, mirrored);
        LightTranslationGroupEffect.SetTweenData(translationTween, translation1, translation2, currentEventData.time, sameTypeEventData.time, sameTypeEventData.easeType.ToEaseType());
        this._tweeningManager.ResumeTween((Tween) translationTween, (object) this);
      }
    });
  }

  public virtual void SetTranslation(float _)
  {
    Vector3 b = new Vector3(this._xTranslationTween.GetValue(this._xTranslationTween.progress), this._yTranslationTween.GetValue(this._yTranslationTween.progress), this._zTranslationTween.GetValue(this._zTranslationTween.progress));
    foreach ((Transform transform, Vector3 mask) tuple in this._transformMask)
      tuple.transform.localPosition = Vector3.Scale(tuple.mask, b);
  }

  private static void SetTweenData(
    FloatTween tween,
    float from,
    float to,
    float startTime,
    float endTime,
    EaseType easeType)
  {
    tween.fromValue = from;
    tween.toValue = to;
    tween.SetStartTimeAndEndTime(startTime, endTime);
    tween.easeType = easeType;
  }

  private static float ComputeTranslation(
    float translation,
    Vector2 translationLimits,
    float distribution,
    Vector2 distributionLimits,
    bool mirrored)
  {
    float t1 = (float) (((mirrored ? -(double) translation : (double) translation) + 1.0) * 0.5);
    float t2 = (float) (((mirrored ? -(double) distribution : (double) distribution) + 1.0) * 0.5);
    return Mathf.LerpUnclamped(translationLimits.x, translationLimits.y, t1) + Mathf.LerpUnclamped(distributionLimits.x, distributionLimits.y, t2);
  }

  public class InitData
  {
    public readonly int groupId;
    public readonly int elementId;
    public readonly bool xMirrored;
    public readonly bool yMirrored;
    public readonly bool zMirrored;
    public readonly Transform xTransform;
    public readonly Transform yTransform;
    public readonly Transform zTransform;
    public readonly Vector2 xTranslationLimits;
    public readonly Vector2 xDistributionLimits;
    public readonly Vector2 yTranslationLimits;
    public readonly Vector2 yDistributionLimits;
    public readonly Vector2 zTranslationLimits;
    public readonly Vector2 zDistributionLimits;

    public InitData(
      int groupId,
      int elementId,
      bool xMirrored,
      bool yMirrored,
      bool zMirrored,
      Transform xTransform,
      Transform yTransform,
      Transform zTransform,
      Vector2 xTranslationLimits,
      Vector2 xDistributionLimits,
      Vector2 yTranslationLimits,
      Vector2 yDistributionLimits,
      Vector2 zTranslationLimits,
      Vector2 zDistributionLimits)
    {
      this.groupId = groupId;
      this.elementId = elementId;
      this.xMirrored = xMirrored;
      this.yMirrored = yMirrored;
      this.zMirrored = zMirrored;
      this.xTransform = xTransform;
      this.yTransform = yTransform;
      this.zTransform = zTransform;
      this.xTranslationLimits = xTranslationLimits;
      this.xDistributionLimits = xDistributionLimits;
      this.yTranslationLimits = yTranslationLimits;
      this.yDistributionLimits = yDistributionLimits;
      this.zTranslationLimits = zTranslationLimits;
      this.zDistributionLimits = zDistributionLimits;
    }
  }
}
