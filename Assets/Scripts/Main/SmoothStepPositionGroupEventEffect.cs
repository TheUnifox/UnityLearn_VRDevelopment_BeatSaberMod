// Decompiled with JetBrains decompiler
// Type: SmoothStepPositionGroupEventEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Tweening;
using UnityEngine;
using Zenject;

public class SmoothStepPositionGroupEventEffect : MonoBehaviour
{
  [SerializeField]
  protected BasicBeatmapEventType _event = BasicBeatmapEventType.VoidEvent;
  [SerializeField]
  protected Transform[] _elements;
  [SerializeField]
  protected bool _clampValue;
  [SerializeField]
  [DrawIf("_clampValue", true, DrawIfAttribute.DisablingType.DontDraw)]
  protected int _eventValueMin;
  [SerializeField]
  [DrawIf("_clampValue", true, DrawIfAttribute.DisablingType.DontDraw)]
  protected int _eventValueMax = 1;
  [Space(10f)]
  [SerializeField]
  protected Vector3 _baseOffset = Vector3.zero;
  [SerializeField]
  protected Vector3 _movementVector = Vector3.up;
  [SerializeField]
  protected float _stepSize = 1f;
  [SerializeField]
  protected EaseType _easeType = EaseType.InOutCubic;
  [Inject]
  protected readonly BeatmapCallbacksController _beatmapCallbacksController;
  [Inject]
  protected readonly SongTimeTweeningManager _tweeningManager;
  protected Vector3Tween _positionTween;
  protected Transform _transform;
  protected Vector3 _startPos;
  protected BeatmapDataCallbackWrapper _beatmapDataCallbackWrapper;

  public virtual void Awake()
  {
    this._transform = this.transform;
    this._startPos = this._transform.localPosition;
    this._positionTween = new Vector3Tween(this._startPos, this._startPos, new System.Action<Vector3>(this.SetPosition), 0.0f, this._easeType);
  }

  public virtual void Start() => this._beatmapDataCallbackWrapper = this._beatmapCallbacksController.AddBeatmapCallback<BasicBeatmapEventData>(new BeatmapDataCallback<BasicBeatmapEventData>(this.HandleBeatmapEvent), BasicBeatmapEventData.SubtypeIdentifier(this._event));

  public virtual void OnDestroy()
  {
    if (this._beatmapCallbacksController != null)
      this._beatmapCallbacksController.RemoveBeatmapCallback(this._beatmapDataCallbackWrapper);
    if (!((UnityEngine.Object) this._tweeningManager != (UnityEngine.Object) null))
      return;
    this._tweeningManager.KillAllTweens((object) this);
  }

  public virtual void HandleBeatmapEvent(BasicBeatmapEventData basicBeatmapEventData)
  {
    this._positionTween.Kill();
    this._positionTween.fromValue = this.GetPositionForValue(basicBeatmapEventData.value);
    this._positionTween.toValue = this._positionTween.fromValue;
    BasicBeatmapEventData sameTypeEventData = basicBeatmapEventData.nextSameTypeEventData;
    if (sameTypeEventData == null)
      return;
    this._positionTween.toValue = this.GetPositionForValue(sameTypeEventData.value);
    this._positionTween.SetStartTimeAndEndTime(basicBeatmapEventData.time, sameTypeEventData.time);
    this._tweeningManager.ResumeTween((Tween) this._positionTween, (object) this);
  }

  public virtual Vector3 GetPositionForValue(int value)
  {
    if (this._clampValue)
      value = Mathf.Clamp(value, this._eventValueMin, this._eventValueMax);
    return this._movementVector * (this._stepSize * (float) value) + this._baseOffset;
  }

  public virtual void SetPosition(Vector3 position)
  {
    for (int index = 0; index < this._elements.Length; ++index)
      this._elements[index].localPosition = (float) index * position;
  }
}
