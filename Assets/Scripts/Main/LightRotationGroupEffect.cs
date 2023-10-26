// Decompiled with JetBrains decompiler
// Type: LightRotationGroupEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Tweening;
using UnityEngine;
using Zenject;

public class LightRotationGroupEffect
{
  protected readonly Transform _transform;
  protected readonly LightAxis _axis;
  protected readonly bool _mirrored;
  protected readonly SongTimeTweeningManager _tweeningManager;
  protected readonly BeatmapCallbacksController _beatmapCallbacksController;
  protected readonly FloatTween _rotationTween;
  protected readonly BeatmapDataCallbackWrapper _lightRotationBeatmapEventCallbackWrapper;

  [Inject]
  public LightRotationGroupEffect(
    LightRotationGroupEffect.InitData initData,
    SongTimeTweeningManager tweeningManager,
    BeatmapCallbacksController beatmapCallbacksController)
  {
    this._transform = initData.transform;
    this._axis = initData.axis;
    this._mirrored = initData.mirrored;
    this._tweeningManager = tweeningManager;
    this._beatmapCallbacksController = beatmapCallbacksController;
    this._rotationTween = new FloatTween(0.0f, 0.0f, new System.Action<float>(this.SetRotation), 0.0f, EaseType.Linear);
    this._rotationTween.ForceOnUpdate();
    this._lightRotationBeatmapEventCallbackWrapper = this._beatmapCallbacksController.AddBeatmapCallback<LightRotationBeatmapEventData>(new BeatmapDataCallback<LightRotationBeatmapEventData>(this.HandleRotationChangeBeatmapEvent), LightRotationBeatmapEventData.SubtypeIdentifier(initData.groupId, initData.elementId, initData.axis));
  }

  public virtual void Cleanup()
  {
    if ((UnityEngine.Object) this._tweeningManager != (UnityEngine.Object) null)
      this._tweeningManager.KillAllTweens((object) this);
    if (this._beatmapCallbacksController == null)
      return;
    this._beatmapCallbacksController.RemoveBeatmapCallback(this._lightRotationBeatmapEventCallbackWrapper);
  }

  public virtual void HandleRotationChangeBeatmapEvent(
    LightRotationBeatmapEventData currentEventData)
  {
    float num = Mathf.Repeat(currentEventData.rotation, 360f);
    this._rotationTween.Kill();
    this.SetRotation(num);
    LightRotationBeatmapEventData sameTypeEventData = (LightRotationBeatmapEventData) currentEventData.nextSameTypeEventData;
    if (sameTypeEventData == null || sameTypeEventData.easeType.ToEaseType() == EaseType.None)
      return;
    float targetAngle = Mathf.Repeat(sameTypeEventData.rotation, 360f);
    int loopCount = sameTypeEventData.loopCount;
    this._rotationTween.fromValue = num;
    this._rotationTween.toValue = LightRotationGroupEffect.ComputeTargetAngle(num, targetAngle, loopCount, sameTypeEventData.rotationDirection);
    this._rotationTween.SetStartTimeAndEndTime(currentEventData.time, sameTypeEventData.time);
    this._rotationTween.easeType = sameTypeEventData.easeType.ToEaseType();
    this._tweeningManager.ResumeTween((Tween) this._rotationTween, (object) this);
  }

  public virtual void SetRotation(float rotation)
  {
    if (this._mirrored)
      rotation *= -1f;
    Vector3 euler = Vector3.zero;
    switch (this._axis)
    {
      case LightAxis.X:
        euler = new Vector3(rotation, 0.0f, 0.0f);
        break;
      case LightAxis.Y:
        euler = new Vector3(0.0f, rotation, 0.0f);
        break;
      case LightAxis.Z:
        euler = new Vector3(0.0f, 0.0f, rotation);
        break;
    }
    this._transform.localRotation = Quaternion.Euler(euler);
  }

  public static float ComputeTargetAngle(
    float startAngle,
    float targetAngle,
    int loopCount,
    LightRotationDirection rotationOrientation)
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    float f = Mathf.DeltaAngle(startAngle, targetAngle);
    switch (rotationOrientation)
    {
      case LightRotationDirection.Automatic:
        num1 = startAngle + f;
        num2 = (float) ((double) Mathf.Sign(f) * (double) loopCount * 360.0);
        break;
      case LightRotationDirection.Clockwise:
        num1 = (double) f < 0.0 ? (float) ((double) startAngle + (double) f + 360.0) : startAngle + f;
        num2 = (float) loopCount * 360f;
        break;
      case LightRotationDirection.Counterclockwise:
        num1 = (double) f > 0.0 ? (float) ((double) startAngle + (double) f - 360.0) : startAngle + f;
        num2 = (float) -loopCount * 360f;
        break;
    }
    return num1 + num2;
  }

  public class InitData
  {
    public readonly int groupId;
    public readonly int elementId;
    public readonly LightAxis axis;
    public readonly bool mirrored;
    public readonly Transform transform;

    public InitData(
      int groupId,
      int elementId,
      LightAxis axis,
      bool mirrored,
      Transform transform)
    {
      this.groupId = groupId;
      this.elementId = elementId;
      this.axis = axis;
      this.mirrored = mirrored;
      this.transform = transform;
    }
  }
}
