// Decompiled with JetBrains decompiler
// Type: InstantMenuButtonTrigger
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Zenject;

public class InstantMenuButtonTrigger : ITickable, IMenuButtonTrigger
{
  [Inject]
  protected readonly VRControllersInputManager _vrControllersInputManager;

  public event System.Action menuButtonTriggeredEvent;

  public virtual void Tick()
  {
    if (!this._vrControllersInputManager.MenuButtonDown())
      return;
    System.Action buttonTriggeredEvent = this.menuButtonTriggeredEvent;
    if (buttonTriggeredEvent == null)
      return;
    buttonTriggeredEvent();
  }
}
