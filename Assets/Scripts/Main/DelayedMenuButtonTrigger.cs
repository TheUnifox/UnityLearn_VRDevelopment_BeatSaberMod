// Decompiled with JetBrains decompiler
// Type: DelayedMenuButtonTrigger
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class DelayedMenuButtonTrigger : ITickable, IMenuButtonTrigger
{
  [Inject]
  protected float _pressDuration = 0.3f;
  protected float _timer;
  protected bool _waitingForButtonRelease;
  [Inject]
  protected VRControllersInputManager _vrControllersInputManager;

  public event System.Action menuButtonTriggeredEvent;

  public virtual void Tick()
  {
    if (this._vrControllersInputManager.MenuButton() && !this._waitingForButtonRelease)
    {
      this._timer += Time.deltaTime;
      if ((double) this._timer <= (double) this._pressDuration)
        return;
      this._waitingForButtonRelease = true;
      System.Action buttonTriggeredEvent = this.menuButtonTriggeredEvent;
      if (buttonTriggeredEvent == null)
        return;
      buttonTriggeredEvent();
    }
    else
    {
      this._waitingForButtonRelease = false;
      this._timer = 0.0f;
    }
  }
}
