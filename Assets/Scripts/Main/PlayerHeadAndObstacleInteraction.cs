// Decompiled with JetBrains decompiler
// Type: PlayerHeadAndObstacleInteraction
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerHeadAndObstacleInteraction : MonoBehaviour
{
  [Inject]
  protected readonly PlayerTransforms _playerTransforms;
  [Inject]
  protected readonly BeatmapObjectManager _beatmapObjectManager;
  protected int _lastFrameNumCheck = -1;
  protected readonly HashSet<ObstacleController> _intersectingObstacles = new HashSet<ObstacleController>();
  protected int _prevFrameNumberOfIntersectingObstaclesCount;

  public event System.Action headDidEnterObstaclesEvent;

  public event System.Action<ObstacleController> headDidEnterObstacleEvent;

  public bool playerHeadIsInObstacle => this._intersectingObstacles.Count > 0;

  public virtual void RefreshIntersectingObstacles(Vector3 worldPos)
  {
    int frameCount = Time.frameCount;
    if (this._lastFrameNumCheck == frameCount)
      return;
    this._lastFrameNumCheck = frameCount;
    foreach (ObstacleController obstacleController in this._beatmapObjectManager.activeObstacleControllers)
    {
      if (obstacleController.isActiveAndEnabled)
      {
        if (!obstacleController.hasPassedAvoidedMark)
        {
          Vector3 point = obstacleController.transform.InverseTransformPoint(worldPos);
          if (obstacleController.bounds.Contains(point))
          {
            if (!this._intersectingObstacles.Contains(obstacleController))
            {
              this._intersectingObstacles.Add(obstacleController);
              System.Action<ObstacleController> enterObstacleEvent = this.headDidEnterObstacleEvent;
              if (enterObstacleEvent != null)
                enterObstacleEvent(obstacleController);
            }
          }
          else
            this._intersectingObstacles.Remove(obstacleController);
        }
        else if (obstacleController.hasPassedAvoidedMark)
          this._intersectingObstacles.Remove(obstacleController);
      }
    }
  }

  public virtual void Update()
  {
    this.RefreshIntersectingObstacles(this._playerTransforms.headWorldPos);
    int count = this._intersectingObstacles.Count;
    if (this._prevFrameNumberOfIntersectingObstaclesCount == 0 && count > 0)
    {
      System.Action enterObstaclesEvent = this.headDidEnterObstaclesEvent;
      if (enterObstaclesEvent != null)
        enterObstaclesEvent();
    }
    this._prevFrameNumberOfIntersectingObstaclesCount = count;
  }
}
