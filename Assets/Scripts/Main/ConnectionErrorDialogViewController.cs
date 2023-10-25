// Decompiled with JetBrains decompiler
// Type: ConnectionErrorDialogViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;

public class ConnectionErrorDialogViewController : SimpleDialogPromptViewController
{
  public virtual void Init(DisconnectedReason reason, System.Action buttonAction) => this.Init(Localization.Get("TITLE_CLIENT_DISCONNECTED"), Localization.Get(reason.LocalizedKey()) + " (" + reason.ErrorCode() + ")", Localization.Get("BUTTON_OK"), (System.Action<int>) (btnIdx =>
  {
    System.Action action = buttonAction;
    if (action == null)
      return;
    action();
  }));
}
