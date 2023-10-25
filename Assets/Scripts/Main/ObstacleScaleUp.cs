// Decompiled with JetBrains decompiler
// Type: ObstacleScaleUp
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class ObstacleScaleUp : MonoBehaviour
{
  [SerializeField]
  protected float _fullScalePart = 0.125f;
  [Space]
  [SerializeField]
  protected Transform _targetTransform;
  [SerializeField]
  protected ObstacleController _obstacleController;

  public virtual void Awake()
  {
    this._obstacleController.didInitEvent += new System.Action<ObstacleControllerBase>(this.HandleObstacleControllerDidInit);
    this.UpdateScale(0.0f);
  }

  public virtual void OnDestroy()
  {
    if (!((UnityEngine.Object) this._obstacleController != (UnityEngine.Object) null))
      return;
    this._obstacleController.didInitEvent -= new System.Action<ObstacleControllerBase>(this.HandleObstacleControllerDidInit);
    this._obstacleController.didUpdateProgress -= new System.Action<ObstacleController, float>(this.HandleObstacleControllerDidUpdateProgress);
  }

  public virtual void UpdateScale(float progress)
  {
    if ((double) progress >= (double) this._fullScalePart)
    {
      this._targetTransform.localScale = Vector3.one;
      this._obstacleController.didUpdateProgress -= new System.Action<ObstacleController, float>(this.HandleObstacleControllerDidUpdateProgress);
    }
    else
    {
      float num = Easing.OutQuad(progress / this._fullScalePart);
      this._targetTransform.localScale = new Vector3(num, num, 1f);
    }
  }

  public virtual void HandleObstacleControllerDidUpdateProgress(
    ObstacleController obstacleController,
    float time)
  {
    this.UpdateScale(Mathf.Max((time - obstacleController.move1Duration) / obstacleController.move2Duration, 0.0f));
  }

  public virtual void HandleObstacleControllerDidInit(ObstacleControllerBase obstacleController)
  {
    this.UpdateScale(0.0f);
    this._obstacleController.didUpdateProgress -= new System.Action<ObstacleController, float>(this.HandleObstacleControllerDidUpdateProgress);
    this._obstacleController.didUpdateProgress += new System.Action<ObstacleController, float>(this.HandleObstacleControllerDidUpdateProgress);
  }
}
