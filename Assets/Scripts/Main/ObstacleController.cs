// Decompiled with JetBrains decompiler
// Type: ObstacleController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections;
using UnityEngine;
using Zenject;

public class ObstacleController : ObstacleControllerBase, IBeatmapObjectController
{
  [SerializeField]
  protected StretchableObstacle _stretchableObstacle;
  [Space]
  [SerializeField]
  protected float _endDistanceOffset = 500f;
  [SerializeField]
  protected GameObject[] _visualWrappers;
  [Inject]
  protected readonly PlayerTransforms _playerTransforms;
  [Inject]
  protected readonly IAudioTimeSource _audioTimeSyncController;
  [Inject]
  protected readonly ColorManager _colorManager;
  public const float kAvoidMarkTimeOffset = 0.15f;
  protected float _width;
  protected float _height;
  protected float _length;
  protected Vector3 _startPos;
  protected Vector3 _midPos;
  protected Vector3 _endPos;
  protected float _move1Duration;
  protected float _move2Duration;
  protected float _startTimeOffset;
  protected float _obstacleDuration;
  protected bool _passedThreeQuartersOfMove2Reported;
  protected bool _passedAvoidedMarkReported;
  protected float _passedAvoidedMarkTime;
  protected float _finishMovementTime;
  protected Bounds _bounds;
  protected bool _dissolving;
  protected ObstacleData _obstacleData;
  protected Color _color;
  protected Quaternion _worldRotation;
  protected Quaternion _inverseWorldRotation;

  public event System.Action<ObstacleController> finishedMovementEvent;

  public event System.Action<ObstacleController> passedThreeQuartersOfMove2Event;

  public event System.Action<ObstacleController> passedAvoidedMarkEvent;

  public event System.Action<ObstacleController> didDissolveEvent;

  public event System.Action<ObstacleController, float> didUpdateProgress;

  public Bounds bounds => this._bounds;

  public ObstacleData obstacleData => this._obstacleData;

  public bool hasPassedAvoidedMark => this._passedAvoidedMarkReported;

  public float move1Duration => this._move1Duration;

  public float move2Duration => this._move2Duration;

  public float width => this._width;

  public float height => this._height;

  public float length => this._length;

  public Color color => this._color;

  public virtual void Init(
    ObstacleData obstacleData,
    float worldRotation,
    Vector3 startPos,
    Vector3 midPos,
    Vector3 endPos,
    float move1Duration,
    float move2Duration,
    float singleLineWidth,
    float height)
  {
    this._worldRotation = Quaternion.Euler(0.0f, worldRotation, 0.0f);
    this._inverseWorldRotation = Quaternion.Euler(0.0f, -worldRotation, 0.0f);
    this._obstacleData = obstacleData;
    this._obstacleDuration = obstacleData.duration;
    this._height = height;
    this._color = this._colorManager.obstaclesColor;
    this._width = (float) obstacleData.width * singleLineWidth;
    Vector3 vector3 = new Vector3((float) (((double) this._width - (double) singleLineWidth) * 0.5), 0.0f, 0.0f);
    this._startPos = startPos + vector3;
    this._midPos = midPos + vector3;
    this._endPos = endPos + vector3;
    this._move1Duration = move1Duration;
    this._move2Duration = move2Duration;
    this._startTimeOffset = (float) ((double) obstacleData.time - (double) this._move1Duration - (double) this._move2Duration * 0.5);
    this._length = (this._endPos - this._midPos).magnitude / move2Duration * obstacleData.duration;
    this._stretchableObstacle.SetSizeAndColor(this._width * 0.98f, this._height, this._length, this._color);
    this._bounds = this._stretchableObstacle.bounds;
    this._passedThreeQuartersOfMove2Reported = false;
    this._passedAvoidedMarkReported = false;
    this._passedAvoidedMarkTime = (float) ((double) this._move1Duration + (double) this._move2Duration * 0.5 + (double) this._obstacleDuration + 0.15000000596046448);
    this._finishMovementTime = this._move1Duration + this._move2Duration + this._obstacleDuration;
    this.transform.localPosition = startPos;
    this.transform.localRotation = this._worldRotation;
    this.InvokeDidInitEvent((ObstacleControllerBase) this);
  }

  public virtual void Update() => this.ManualUpdate();

  public virtual void ManualUpdate()
  {
    float time = this._audioTimeSyncController.songTime - this._startTimeOffset;
    this.transform.localPosition = this._worldRotation * this.GetPosForTime(time);
    System.Action<ObstacleController, float> didUpdateProgress = this.didUpdateProgress;
    if (didUpdateProgress != null)
      didUpdateProgress(this, time);
    if (!this._passedThreeQuartersOfMove2Reported && (double) time > (double) this._move1Duration + (double) this._move2Duration * 0.75)
    {
      this._passedThreeQuartersOfMove2Reported = true;
      System.Action<ObstacleController> quartersOfMove2Event = this.passedThreeQuartersOfMove2Event;
      if (quartersOfMove2Event != null)
        quartersOfMove2Event(this);
    }
    if (!this._passedAvoidedMarkReported && (double) time > (double) this._passedAvoidedMarkTime)
    {
      this._passedAvoidedMarkReported = true;
      System.Action<ObstacleController> avoidedMarkEvent = this.passedAvoidedMarkEvent;
      if (avoidedMarkEvent != null)
        avoidedMarkEvent(this);
    }
    if ((double) time <= (double) this._finishMovementTime)
      return;
    System.Action<ObstacleController> finishedMovementEvent = this.finishedMovementEvent;
    if (finishedMovementEvent == null)
      return;
    finishedMovementEvent(this);
  }

  public virtual Vector3 GetPosForTime(float time)
  {
    Vector3 posForTime;
    if ((double) time < (double) this._move1Duration)
    {
      posForTime = Vector3.LerpUnclamped(this._startPos, this._midPos, (double) this._move1Duration < (double) Mathf.Epsilon ? 0.0f : time / this._move1Duration);
    }
    else
    {
      float t1 = (time - this._move1Duration) / this._move2Duration;
      posForTime.x = this._startPos.x;
      posForTime.y = this._startPos.y;
      posForTime.z = this._playerTransforms.MoveTowardsHead(this._midPos.z, this._endPos.z, this._inverseWorldRotation, t1);
      if (this._passedAvoidedMarkReported)
      {
        float num = (float) (((double) time - (double) this._passedAvoidedMarkTime) / ((double) this._finishMovementTime - (double) this._passedAvoidedMarkTime));
        float t2 = num * num * num;
        posForTime.z -= Mathf.LerpUnclamped(0.0f, this._endDistanceOffset, t2);
      }
    }
    return posForTime;
  }

  public virtual IEnumerator DissolveCoroutine(float duration)
  {
    ObstacleController obstacleController = this;
    obstacleController.InvokeDidStartDissolvingEvent((ObstacleControllerBase) obstacleController, duration);
    yield return (object) new WaitForSeconds(duration);
    obstacleController._dissolving = false;
    System.Action<ObstacleController> didDissolveEvent = obstacleController.didDissolveEvent;
    if (didDissolveEvent != null)
      didDissolveEvent(obstacleController);
  }

  public virtual void Dissolve(float duration)
  {
    if (this._dissolving)
      return;
    this._dissolving = true;
    this.StartCoroutine(this.DissolveCoroutine(duration));
  }

  public virtual void Hide(bool hide)
  {
    foreach (GameObject visualWrapper in this._visualWrappers)
      visualWrapper.SetActive(!hide);
  }

  public virtual void Pause(bool pause) => this.enabled = !pause;

  public class Pool : MonoMemoryPool<ObstacleController>
  {
  }
}
