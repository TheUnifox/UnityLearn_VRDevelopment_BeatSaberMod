// Decompiled with JetBrains decompiler
// Type: BeatmapObjectAvoidancePathEvaluator
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class BeatmapObjectAvoidancePathEvaluator
{
  protected readonly float _jumpStartZ;
  protected readonly float _jumpEndZ;
  protected readonly float _zOffset;
  protected readonly float _yOffset;
  protected readonly float _noteJumpSpeed;
  protected readonly float _moveToPlayerHeadTParam;
  protected readonly BezierSplineEvaluator _pathBezierCurveEvaluator;
  protected readonly IAudioTimeSource _audioTimeSource;
  protected readonly PlayerTransforms _playerTransforms;

  public BeatmapObjectAvoidancePathEvaluator(
    IAudioTimeSource audioTimeSource,
    PlayerTransforms playerTransforms,
    BezierSplineEvaluator pathBezierCurveEvaluator,
    float jumpStartZ,
    float jumpEndZ,
    float yOffset,
    float zOffset,
    float noteJumpSeed,
    float moveToPlayerHeadTParam)
  {
    this._audioTimeSource = audioTimeSource;
    this._playerTransforms = playerTransforms;
    this._pathBezierCurveEvaluator = pathBezierCurveEvaluator;
    this._jumpStartZ = jumpStartZ;
    this._jumpEndZ = jumpEndZ;
    this._yOffset = yOffset;
    this._zOffset = zOffset;
    this._noteJumpSpeed = noteJumpSeed;
    this._moveToPlayerHeadTParam = moveToPlayerHeadTParam;
  }

  public virtual Vector3 GetCurrentPathPosition()
  {
    float time = this._audioTimeSource.songTime + this._zOffset / this._noteJumpSpeed;
    float offsetByHeadPosAtTime = this._playerTransforms.GetZPosOffsetByHeadPosAtTime(this._jumpStartZ, this._jumpEndZ, this._moveToPlayerHeadTParam);
    Vector3 position = this._pathBezierCurveEvaluator.EvaluatePosition(time);
    return new Vector3(position.x, position.y + this._yOffset, offsetByHeadPosAtTime);
  }
}
