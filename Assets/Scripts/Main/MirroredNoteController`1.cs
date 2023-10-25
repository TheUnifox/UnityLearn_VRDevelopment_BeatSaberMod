// Decompiled with JetBrains decompiler
// Type: MirroredNoteController`1
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public abstract class MirroredNoteController<T> : 
  NoteControllerBase,
  INoteControllerNoteDidStartDissolvingEvent,
  INoteControllerNoteDidPassJumpThreeQuartersEvent
  where T : INoteMirrorable
{
  [SerializeField]
  private Transform _noteTransform;
  protected T followedNote;
  private Transform _followedNoteTransform;
  private readonly LazyCopyHashSet<INoteControllerDidInitEvent> _didInitEvent = new LazyCopyHashSet<INoteControllerDidInitEvent>();
  private readonly LazyCopyHashSet<INoteControllerNoteDidPassJumpThreeQuartersEvent> _noteDidPassJumpThreeQuartersEvent = new LazyCopyHashSet<INoteControllerNoteDidPassJumpThreeQuartersEvent>();
  private readonly LazyCopyHashSet<INoteControllerNoteDidStartDissolvingEvent> _noteDidStartDissolvingEvent = new LazyCopyHashSet<INoteControllerNoteDidStartDissolvingEvent>();

  public override ILazyCopyHashSet<INoteControllerDidInitEvent> didInitEvent => (ILazyCopyHashSet<INoteControllerDidInitEvent>) this._didInitEvent;

  public override ILazyCopyHashSet<INoteControllerNoteDidPassJumpThreeQuartersEvent> noteDidPassJumpThreeQuartersEvent => (ILazyCopyHashSet<INoteControllerNoteDidPassJumpThreeQuartersEvent>) this._noteDidPassJumpThreeQuartersEvent;

  public override ILazyCopyHashSet<INoteControllerNoteDidStartDissolvingEvent> noteDidStartDissolvingEvent => (ILazyCopyHashSet<INoteControllerNoteDidStartDissolvingEvent>) this._noteDidStartDissolvingEvent;

  public override NoteData noteData => this.followedNote.noteData;

  protected void Update() => this.UpdatePositionAndRotation();

  protected void OnDestroy() => this.RemoveListeners();

  private void UpdatePositionAndRotation()
  {
    Vector3 position = this._followedNoteTransform.position;
    Quaternion rotation1 = this._followedNoteTransform.rotation;
    position.y = -position.y;
    Quaternion rotation2 = rotation1.Reflect(Vector3.up);
    this._noteTransform.SetPositionAndRotation(position, rotation2);
  }

  public virtual void Mirror(T noteController)
  {
    this.RemoveListeners();
    this.followedNote = noteController;
    this._followedNoteTransform = this.followedNote.noteTransform;
    this.followedNote.noteDidStartDissolvingEvent.Add((INoteControllerNoteDidStartDissolvingEvent) this);
    this._noteTransform.localScale = this._followedNoteTransform.localScale;
    this.UpdatePositionAndRotation();
    foreach (INoteControllerDidInitEvent controllerDidInitEvent in this._didInitEvent.items)
      controllerDidInitEvent.HandleNoteControllerDidInit((NoteControllerBase) this);
  }

  private void RemoveListeners()
  {
    if ((object) this.followedNote != null)
      this.followedNote.noteDidStartDissolvingEvent.Remove((INoteControllerNoteDidStartDissolvingEvent) this);
    this.followedNote = default (T);
  }

  public void HandleNoteControllerNoteDidStartDissolving(
    NoteControllerBase noteController,
    float duration)
  {
    foreach (INoteControllerNoteDidStartDissolvingEvent startDissolvingEvent in this._noteDidStartDissolvingEvent.items)
      startDissolvingEvent.HandleNoteControllerNoteDidStartDissolving((NoteControllerBase) this, duration);
  }

  public void HandleNoteControllerNoteDidPassJumpThreeQuarters(NoteControllerBase noteController)
  {
    foreach (INoteControllerNoteDidPassJumpThreeQuartersEvent threeQuartersEvent in this._noteDidPassJumpThreeQuartersEvent.items)
      threeQuartersEvent.HandleNoteControllerNoteDidPassJumpThreeQuarters((NoteControllerBase) this);
  }

  public void Hide(bool hide) => this.gameObject.SetActive(!hide);
}
