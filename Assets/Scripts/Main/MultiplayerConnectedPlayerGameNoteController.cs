// Decompiled with JetBrains decompiler
// Type: MultiplayerConnectedPlayerGameNoteController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class MultiplayerConnectedPlayerGameNoteController : 
  MultiplayerConnectedPlayerNoteController,
  ICubeNoteControllerInitializable<MultiplayerConnectedPlayerGameNoteController>,
  INoteVisualModifierTypeProvider,
  INoteMovementProvider
{
  protected NoteVisualModifierType _noteVisualModifierType;
  protected NoteData.GameplayType _gameplayType;

  public event System.Action<MultiplayerConnectedPlayerGameNoteController> cubeNoteControllerDidInitEvent;

  public NoteMovement noteMovement => this._noteMovement;

  public NoteData.GameplayType gameplayType => this._gameplayType;

  public NoteVisualModifierType noteVisualModifierType => this._noteVisualModifierType;

  public virtual void Init(
    NoteData noteData,
    float worldRotation,
    Vector3 moveStartPos,
    Vector3 moveEndPos,
    Vector3 jumpEndPos,
    float moveDuration,
    float jumpDuration,
    float jumpGravity,
    NoteVisualModifierType noteVisualModifierType,
    float uniformScale)
  {
    this._noteVisualModifierType = noteVisualModifierType;
    this._gameplayType = noteData.gameplayType;
    bool rotateTowardsPlayer = noteData.gameplayType == NoteData.GameplayType.Normal;
    bool useRandomRotation = noteData.gameplayType == NoteData.GameplayType.Normal;
    this.Init(noteData, worldRotation, moveStartPos, moveEndPos, jumpEndPos, moveDuration, jumpDuration, jumpGravity, noteData.cutDirection.RotationAngle() + noteData.cutDirectionAngleOffset, uniformScale, rotateTowardsPlayer, useRandomRotation);
    System.Action<MultiplayerConnectedPlayerGameNoteController> controllerDidInitEvent = this.cubeNoteControllerDidInitEvent;
    if (controllerDidInitEvent == null)
      return;
    controllerDidInitEvent(this);
  }

  public class Pool : MonoMemoryPool<MultiplayerConnectedPlayerGameNoteController>
  {
  }
}
