// Decompiled with JetBrains decompiler
// Type: PS4Helper
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public class PS4Helper : PersistentSingleton<PS4Helper>
{
  protected bool _backgroundExecution;

  public event System.Action didGoToBackgroundExecutionEvent;

  public event System.Action didGoToForegroundExecutionEvent;

  public virtual void Update()
  {
    if (!this._backgroundExecution)
      return;
    this._backgroundExecution = false;
    System.Action foregroundExecutionEvent = this.didGoToForegroundExecutionEvent;
    if (foregroundExecutionEvent == null)
      return;
    foregroundExecutionEvent();
  }
  
}
