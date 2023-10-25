// Decompiled with JetBrains decompiler
// Type: ObstacleControllerBase
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public abstract class ObstacleControllerBase : MonoBehaviour
{
  public event System.Action<ObstacleControllerBase> didInitEvent;

  public event System.Action<ObstacleControllerBase, float> didStartDissolvingEvent;

  protected void InvokeDidInitEvent(ObstacleControllerBase obstacleController)
  {
    System.Action<ObstacleControllerBase> didInitEvent = this.didInitEvent;
    if (didInitEvent == null)
      return;
    didInitEvent(obstacleController);
  }

  protected void InvokeDidStartDissolvingEvent(
    ObstacleControllerBase obstacleController,
    float duration)
  {
    System.Action<ObstacleControllerBase, float> startDissolvingEvent = this.didStartDissolvingEvent;
    if (startDissolvingEvent == null)
      return;
    startDissolvingEvent(obstacleController, duration);
  }
}
