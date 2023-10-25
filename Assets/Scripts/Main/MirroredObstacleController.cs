// Decompiled with JetBrains decompiler
// Type: MirroredObstacleController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class MirroredObstacleController : ObstacleControllerBase
{
  [SerializeField]
  protected StretchableObstacle _stretchableObstacle;
  protected ObstacleController _followedObstacle;
  protected Transform _transform;
  protected Transform _followedTransform;

  public bool hide
  {
    set => this.gameObject.SetActive(!value);
  }

  public virtual void Awake() => this._transform = this.transform;

  public virtual void OnDestroy() => this.RemoveListeners();

  public virtual void Update() => this.UpdatePositionAndRotation();

  public virtual void RemoveListeners()
  {
    if ((UnityEngine.Object) this._followedObstacle != (UnityEngine.Object) null)
      this._followedObstacle.didStartDissolvingEvent -= new System.Action<ObstacleControllerBase, float>(this.HandleDidStartDissolving);
    this._followedObstacle = (ObstacleController) null;
  }

  public virtual void UpdatePositionAndRotation()
  {
    Vector3 position = this._followedTransform.position;
    position.y = -position.y;
    this._transform.SetPositionAndRotation(position, this._followedTransform.rotation.Reflect(Vector3.up));
  }

  public virtual void Mirror(ObstacleController obstacleController)
  {
    this.RemoveListeners();
    this._stretchableObstacle.SetSizeAndColor(obstacleController.width * 0.98f, obstacleController.height, obstacleController.length, obstacleController.color);
    obstacleController.didStartDissolvingEvent += new System.Action<ObstacleControllerBase, float>(this.HandleDidStartDissolving);
    this._followedTransform = obstacleController.transform;
    this.UpdatePositionAndRotation();
    this.InvokeDidInitEvent((ObstacleControllerBase) this);
  }

  public virtual void HandleDidStartDissolving(
    ObstacleControllerBase obstacleController,
    float duration)
  {
    this.InvokeDidStartDissolvingEvent((ObstacleControllerBase) this, duration);
  }

  public class Pool : MonoMemoryPool<MirroredObstacleController>
  {
  }
}
