// Decompiled with JetBrains decompiler
// Type: MultiplayerConnectedPlayerBombNoteController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class MultiplayerConnectedPlayerBombNoteController : MultiplayerConnectedPlayerNoteController
{
  public virtual void Init(
    NoteData noteData,
    float worldRotation,
    Vector3 moveStartPos,
    Vector3 moveEndPos,
    Vector3 jumpEndPos,
    float moveDuration,
    float jumpDuration,
    float jumpGravity)
  {
    this.Init(noteData, worldRotation, moveStartPos, moveEndPos, jumpEndPos, moveDuration, jumpDuration, jumpGravity, 0.0f, 1f, true, true);
  }

  public class Pool : MonoMemoryPool<MultiplayerConnectedPlayerBombNoteController>
  {
  }
}
