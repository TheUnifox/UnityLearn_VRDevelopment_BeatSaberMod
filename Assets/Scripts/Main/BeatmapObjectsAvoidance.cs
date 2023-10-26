// Decompiled with JetBrains decompiler
// Type: BeatmapObjectsAvoidance
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class BeatmapObjectsAvoidance : MonoBehaviour
{
  [Header("Offsets")]
  [SerializeField]
  protected float _zOffset = 4f;
  [SerializeField]
  protected float _yOffset = 0.15f;
  [Header("Tilting")]
  [SerializeField]
  protected Vector2 _gravity = (Vector2) new Vector3(0.0f, -9.81f);
  [Header("Rotation towards player")]
  [SerializeField]
  protected Transform _towardsPlayerWrapperTransform;
  [Inject]
  protected readonly IAudioTimeSource _audioTimeSource;
  [Inject]
  protected readonly IReadonlyBeatmapData _beatmapData;
  [Inject]
  protected readonly IBeatmapObjectSpawnController _beatmapObjectSpawnController;
  [Inject]
  protected readonly PlayerTransforms _playerTransforms;
  protected BeatmapObjectAvoidanceYOffsetEvaluator _avoidanceYOffsetEvaluatorProvider;
  protected BeatmapObjectAvoidancePathEvaluator _pathEvaluator;
  protected BeatmapObjectAvoidanceTiltEvaluator _tiltEvaluator;
  protected BezierSplineEvaluator _pathBezierSplineEvaluator;
  protected BezierSplineEvaluator _accelerationBezierSplineEvaluator;
  protected Transform _transform;

  public virtual void Awake() => this._transform = this.transform;

  public virtual void Start()
  {
    this.enabled = false;
    if (this._beatmapObjectSpawnController.isInitialized)
      this.SetupAndRun();
    else
      this._beatmapObjectSpawnController.didInitEvent += new System.Action(this.HandleBeatmapObjectSpawnControllerDidInit);
  }

  public virtual void OnDestroy()
  {
    if (this._beatmapObjectSpawnController == null)
      return;
    this._beatmapObjectSpawnController.didInitEvent -= new System.Action(this.HandleBeatmapObjectSpawnControllerDidInit);
  }

    public virtual void Update()
    {
        this._avoidanceYOffsetEvaluatorProvider.ManualUpdate();
        float jumpOffsetYAtJumpStartSongTime = this._avoidanceYOffsetEvaluatorProvider.GetJumpOffsetYAtJumpStartSongTime(Time.deltaTime);
        Vector3 currentPathPosition = this._pathEvaluator.GetCurrentPathPosition();
        float tiltAngle = this._tiltEvaluator.GetTiltAngle();
        this._transform.position = currentPathPosition + Vector3.up * jumpOffsetYAtJumpStartSongTime;
        this._transform.localRotation = Quaternion.Euler(0f, 0f, tiltAngle);
        Vector3 position = this._transform.InverseTransformPoint(this._playerTransforms.headWorldPos);
        position.y = 0f;
        this._towardsPlayerWrapperTransform.LookAt(this._transform.TransformPoint(position), this._transform.up);
    }

    public virtual void SetupAndRun()
  {
    BeatmapObjectSpawnMovementData.NoteSpawnData jumpingNoteSpawnData = this._beatmapObjectSpawnController.beatmapObjectSpawnMovementData.GetJumpingNoteSpawnData(NoteData.CreateBasicNoteData(0.0f, 0, NoteLineLayer.Base, ColorType.None, NoteCutDirection.Any));
    float jumpMovementSpeed = this._beatmapObjectSpawnController.noteJumpMovementSpeed;
    float num1 = (float) (-(double) this._beatmapObjectSpawnController.verticalLayerDistance * 0.5);
    float z1 = jumpingNoteSpawnData.moveEndPos.z;
    float z2 = jumpingNoteSpawnData.jumpEndPos.z;
    float num2 = (float) (((double) z1 + (double) z2) * 0.5);
    float num3 = Mathf.Abs(z1 - z2);
    float moveToPlayerHeadTParam = Mathf.Abs(z1 - (num2 + this._zOffset)) / num3;
    if (!this.BuildAnimationCurvePath())
      return;
    this._avoidanceYOffsetEvaluatorProvider = new BeatmapObjectAvoidanceYOffsetEvaluator(this._audioTimeSource, this._beatmapObjectSpawnController, moveToPlayerHeadTParam, jumpingNoteSpawnData);
    this._pathEvaluator = new BeatmapObjectAvoidancePathEvaluator(this._audioTimeSource, this._playerTransforms, this._pathBezierSplineEvaluator, z1, z2, this._yOffset + num1, this._zOffset, jumpMovementSpeed, moveToPlayerHeadTParam);
    this._tiltEvaluator = new BeatmapObjectAvoidanceTiltEvaluator(this._audioTimeSource, this._accelerationBezierSplineEvaluator, this._gravity);
    this.enabled = true;
  }

    public virtual bool BuildAnimationCurvePath()
    {
        BezierSpline bezierSpline = new BezierSpline();
        int num = 0;
        foreach (WaypointData waypointData in this._beatmapData.GetBeatmapDataItems<WaypointData>(0))
        {
            Vector2 point = this._beatmapObjectSpawnController.Get2DNoteOffset(waypointData.lineIndex, waypointData.lineLayer);
            point.y = this._beatmapObjectSpawnController.JumpPosYForLineLayerAtDistanceFromPlayerWithoutJumpOffset(waypointData.lineLayer, this._zOffset);
            this.AdjustPositionWithOffsetDirection(ref point, waypointData.lineIndex, waypointData.offsetDirection);
            bezierSpline.AddPoint(waypointData.time, point);
            num++;
        }
        if (num > 0)
        {
            bezierSpline.AddArtificialStartAndFinishPoint();
            bezierSpline.ComputeControlPoints();
        }
        this._pathBezierSplineEvaluator = new BezierSplineEvaluator(bezierSpline);
        this._accelerationBezierSplineEvaluator = new BezierSplineEvaluator(bezierSpline);
        return num > 0;
    }

    public virtual void AdjustPositionWithOffsetDirection(
    ref Vector2 position,
    int lineIndex,
    OffsetDirection offsetDirection)
  {
    Vector2 vector2 = new Vector2();
    switch (offsetDirection)
    {
      case OffsetDirection.Up:
        vector2.y = this._beatmapObjectSpawnController.verticalLayerDistance * 0.5f;
        break;
      case OffsetDirection.Down:
        vector2.y = (float) (-(double) this._beatmapObjectSpawnController.verticalLayerDistance * 0.5);
        break;
      case OffsetDirection.Left:
        vector2.x = -this._beatmapObjectSpawnController.noteLinesDistance;
        break;
      case OffsetDirection.Right:
        vector2.x = this._beatmapObjectSpawnController.noteLinesDistance;
        break;
    }
    if (lineIndex != 0 && lineIndex != this._beatmapData.numberOfLines - 1)
      vector2.x *= 0.5f;
    position += vector2;
  }

  public virtual void HandleBeatmapObjectSpawnControllerDidInit() => this.SetupAndRun();
}
