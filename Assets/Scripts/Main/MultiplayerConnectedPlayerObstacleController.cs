// Decompiled with JetBrains decompiler
// Type: MultiplayerConnectedPlayerObstacleController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class MultiplayerConnectedPlayerObstacleController : ObstacleController
{
  [SerializeField]
  protected MultiplayerConnectedPlayerObstacleClippingController _multiplayerConnectedPlayerObstacleClippingController;

  public override void Init(
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
    base.Init(obstacleData, worldRotation, startPos, midPos, endPos, move1Duration, move2Duration, singleLineWidth, height);
    Transform parent = this.transform.parent;
    this._multiplayerConnectedPlayerObstacleClippingController.SetClippingParams((Object) parent != (Object) null ? parent.TransformPoint(midPos) : midPos, (Object) parent != (Object) null ? parent.TransformDirection(new Vector3(0.0f, 0.0f, -1f)) : new Vector3(0.0f, 0.0f, -1f));
  }

  public new class Pool : MonoMemoryPool<MultiplayerConnectedPlayerObstacleController>
  {
  }
}
