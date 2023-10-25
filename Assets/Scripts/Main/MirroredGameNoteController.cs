// Decompiled with JetBrains decompiler
// Type: MirroredGameNoteController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class MirroredGameNoteController : 
  MirroredNoteController<IGameNoteMirrorable>,
  ICubeNoteControllerInitializable<MirroredGameNoteController>,
  INoteVisualModifierTypeProvider,
  INoteMovementProvider
{
  [SerializeField]
  protected MaterialPropertyBlockController _materialPropertyBlockController;
  [Inject]
  protected readonly ColorManager _colorManager;
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _colorId = Shader.PropertyToID("_Color");

  public event System.Action<MirroredGameNoteController> cubeNoteControllerDidInitEvent;

  public NoteMovement noteMovement => this.followedNote?.noteMovement;

  public NoteVisualModifierType noteVisualModifierType => this.followedNote.noteVisualModifierType;

  public override void Mirror(IGameNoteMirrorable noteController)
  {
    base.Mirror(noteController);
    Color color = this._colorManager.ColorForType(this.noteData.colorType);
    this._materialPropertyBlockController.materialPropertyBlock.SetColor(MirroredGameNoteController._colorId, color.ColorWithAlpha(1f));
    this._materialPropertyBlockController.ApplyChanges();
    System.Action<MirroredGameNoteController> controllerDidInitEvent = this.cubeNoteControllerDidInitEvent;
    if (controllerDidInitEvent == null)
      return;
    controllerDidInitEvent(this);
  }

  public class Pool : MonoMemoryPool<MirroredGameNoteController>
  {
  }
}
