// Decompiled with JetBrains decompiler
// Type: MultiplayerConnectedPlayerNoteController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class MultiplayerConnectedPlayerNoteController : NoteController
{
  [SerializeField]
  protected GameObject _visualsWrapperGo;

  protected override void HiddenStateDidChange(bool hide) => this._visualsWrapperGo.SetActive(!hide);

  public override void Pause(bool pause) => this.enabled = !pause;
}
