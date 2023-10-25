// Decompiled with JetBrains decompiler
// Type: PhysicsPositionsMovementEventEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class PhysicsPositionsMovementEventEffect : MonoBehaviour
{
  [SerializeField]
  protected BasicBeatmapEventType _event = BasicBeatmapEventType.VoidEvent;
  [SerializeField]
  protected Vector3 _movementVector = Vector3.up;
  [SerializeField]
  protected float _stepSize = 1f;
  [SerializeField]
  protected float _elasticity = 1f;
  [SerializeField]
  protected float _friction = 0.85f;
  [SerializeField]
  protected float _minMaxSpeed = 1f;
  [SerializeField]
  protected float _maxMaxSpeed = 2f;
  [SerializeField]
  protected float _maxAcceleration = 2f;
  [Inject]
  protected readonly BeatmapCallbacksController _beatmapCallbacksController;
  [Inject]
  protected readonly SongTimeFixedUpdateController _songTimeFixedUpdateController;
  protected Transform _transform;
  protected Vector3 _startPos;
  protected Vector3 _velocity;
  protected Vector3 _prevPosition;
  protected Vector3 _position;
  protected Vector3 _targetPosition;
  protected float _maxSpeed;
  protected float _sqrMaxSpeed;
  protected BeatmapDataCallbackWrapper _beatmapDataCallbackWrapper;

  public virtual void Start()
  {
    this._transform = this.transform;
    this._startPos = this._transform.localPosition;
    this._beatmapDataCallbackWrapper = this._beatmapCallbacksController.AddBeatmapCallback<BasicBeatmapEventData>(new BeatmapDataCallback<BasicBeatmapEventData>(this.HandleBeatmapEvent), BasicBeatmapEventData.SubtypeIdentifier(this._event));
    this._songTimeFixedUpdateController.songControllerFixedTimeDidUpdateEvent += new System.Action<float>(this.HandleSongTimeFixedUpdate);
    this._songTimeFixedUpdateController.songControllerTimeDidUpdateEvent += new System.Action(this.HandleSongTimeUpdate);
    this._position = this.transform.localPosition;
    this._prevPosition = this._position;
    this._targetPosition = this._startPos;
  }

  public virtual void OnDestroy()
  {
    if (this._beatmapCallbacksController != null)
      this._beatmapCallbacksController.RemoveBeatmapCallback(this._beatmapDataCallbackWrapper);
    if (!((UnityEngine.Object) this._songTimeFixedUpdateController != (UnityEngine.Object) null))
      return;
    this._songTimeFixedUpdateController.songControllerFixedTimeDidUpdateEvent -= new System.Action<float>(this.HandleSongTimeFixedUpdate);
    this._songTimeFixedUpdateController.songControllerTimeDidUpdateEvent -= new System.Action(this.HandleSongTimeUpdate);
  }

  public virtual void HandleSongTimeFixedUpdate(float fixedDeltaTime)
  {
    this._prevPosition = this._position;
    this._position += this._velocity * fixedDeltaTime;
    Vector3 vector3 = (this._targetPosition - this._position) * this._elasticity;
    float sqrMagnitude1 = vector3.sqrMagnitude;
    if ((double) sqrMagnitude1 > (double) this._maxAcceleration * (double) this._maxAcceleration)
      vector3 *= this._maxAcceleration / Mathf.Sqrt(sqrMagnitude1);
    this._velocity += vector3;
    this._velocity *= this._friction;
    float sqrMagnitude2 = this._velocity.sqrMagnitude;
    if ((double) sqrMagnitude2 <= (double) this._sqrMaxSpeed)
      return;
    this._velocity *= this._maxSpeed / Mathf.Sqrt(sqrMagnitude2);
  }

  public virtual void HandleSongTimeUpdate() => this.transform.localPosition = Vector3.Lerp(this._prevPosition, this._position, this._songTimeFixedUpdateController.interpolationFactor);

  public virtual void HandleBeatmapEvent(BasicBeatmapEventData basicBeatmapEventData)
  {
    this._targetPosition = this._startPos + this._movementVector * (this._stepSize * (float) (basicBeatmapEventData.value - 4));
    this._maxSpeed = Mathf.Lerp(this._minMaxSpeed, this._maxMaxSpeed, basicBeatmapEventData.floatValue);
    this._sqrMaxSpeed = this._maxSpeed * this._maxSpeed;
  }
}
