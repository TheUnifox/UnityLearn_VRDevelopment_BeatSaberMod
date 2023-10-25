// Decompiled with JetBrains decompiler
// Type: MultiplayerConnectedPlayerDisappearingArrowController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class MultiplayerConnectedPlayerDisappearingArrowController : 
  DisappearingArrowControllerBase<MultiplayerConnectedPlayerGameNoteController>
{
  [SerializeField]
  protected MultiplayerConnectedPlayerGameNoteController _gameNoteController;

  protected override MultiplayerConnectedPlayerGameNoteController gameNoteController => this._gameNoteController;
}
