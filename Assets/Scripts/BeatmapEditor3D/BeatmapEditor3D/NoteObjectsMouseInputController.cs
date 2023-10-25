// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.NoteObjectsMouseInputController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using BeatmapEditor3D.InputSignals;
using BeatmapEditor3D.LevelEditor;
using BeatmapEditor3D.Types;
using BeatmapEditor3D.Views;
using BeatmapEditor3D.Visuals;
using System;
using UnityEngine;

namespace BeatmapEditor3D
{
  public class NoteObjectsMouseInputController : BeatmapObjectsMouseInputController
  {
    [SerializeField]
    private NoteObjectsMouseInputSource _noteObjectsMouseInputSource;
    [SerializeField]
    private NoteBeatmapObjectView _noteBeatmapObjectView;

    protected override void OnEnable()
    {
      base.OnEnable();
      this._noteObjectsMouseInputSource.objectPointerDownEvent += new Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)>(((AbstractMouseInputController<(BeatmapEditorObjectId, BeatmapObjectCellData)>) this).HandleMouseInputEventSourceObjectPointerDown);
      this._noteObjectsMouseInputSource.objectPointerUpEvent += new Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)>(((AbstractMouseInputController<(BeatmapEditorObjectId, BeatmapObjectCellData)>) this).HandleMouseInputEventSourceObjectPointerUp);
      this._noteObjectsMouseInputSource.objectPointerHoverEvent += new Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)>(((AbstractMouseInputController<(BeatmapEditorObjectId, BeatmapObjectCellData)>) this).HandleMouseInputEventSourceObjectPointerHover);
      this._noteObjectsMouseInputSource.objectPointerScrollEvent += new Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)>(((AbstractMouseInputController<(BeatmapEditorObjectId, BeatmapObjectCellData)>) this).HandleMouseInputEventSourceObjectPointerScroll);
      this.objectActions[AbstractMouseInputController<(BeatmapEditorObjectId, BeatmapObjectCellData)>.HashInput(MouseInputEventOrigin.Object, MouseInputType.Middle, MouseEventType.Up, false, false, false)] = new Action<(BeatmapEditorObjectId, BeatmapObjectCellData)>(this.HandleNoteInvertColor);
      this.objectActions[AbstractMouseInputController<(BeatmapEditorObjectId, BeatmapObjectCellData)>.HashInput(MouseInputEventOrigin.Object, MouseInputType.ScrollUp, MouseEventType.None, false, false, true)] = new Action<(BeatmapEditorObjectId, BeatmapObjectCellData)>(this.HandleNoteScrollUp);
      this.objectActions[AbstractMouseInputController<(BeatmapEditorObjectId, BeatmapObjectCellData)>.HashInput(MouseInputEventOrigin.Object, MouseInputType.ScrollDown, MouseEventType.None, false, false, true)] = new Action<(BeatmapEditorObjectId, BeatmapObjectCellData)>(this.HandleNoteScrollDown);
    }

    protected override void OnDisable()
    {
      base.OnDisable();
      this._noteObjectsMouseInputSource.objectPointerDownEvent -= new Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)>(((AbstractMouseInputController<(BeatmapEditorObjectId, BeatmapObjectCellData)>) this).HandleMouseInputEventSourceObjectPointerDown);
      this._noteObjectsMouseInputSource.objectPointerUpEvent -= new Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)>(((AbstractMouseInputController<(BeatmapEditorObjectId, BeatmapObjectCellData)>) this).HandleMouseInputEventSourceObjectPointerUp);
      this._noteObjectsMouseInputSource.objectPointerHoverEvent -= new Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)>(((AbstractMouseInputController<(BeatmapEditorObjectId, BeatmapObjectCellData)>) this).HandleMouseInputEventSourceObjectPointerHover);
      this._noteObjectsMouseInputSource.objectPointerScrollEvent -= new Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)>(((AbstractMouseInputController<(BeatmapEditorObjectId, BeatmapObjectCellData)>) this).HandleMouseInputEventSourceObjectPointerScroll);
    }

    protected override void HandleMouseInputEventSourceObjectPointerHover(
      MouseInputType mouseInputType,
      (BeatmapEditorObjectId id, BeatmapObjectCellData cellData) payload)
    {
      base.HandleMouseInputEventSourceObjectPointerHover(mouseInputType, payload);
      NormalNoteView normalNoteView;
      if (this._noteBeatmapObjectView.noteObjects.TryGetValue(payload.id, out normalNoteView))
        normalNoteView.SetHighlight(mouseInputType == MouseInputType.Enter);
      BombNoteView bombNoteView;
      if (!this._noteBeatmapObjectView.bombObjects.TryGetValue(payload.id, out bombNoteView))
        return;
      bombNoteView.SetHighlight(mouseInputType == MouseInputType.Enter);
    }

    protected override void HandleMoveBeatmapObjectToBeatLine(
      (BeatmapEditorObjectId id, BeatmapObjectCellData cellData) payload)
    {
      this.signalBus.Fire<MoveNoteToBeatLineSignal>(new MoveNoteToBeatLineSignal(payload.id, payload.cellData));
    }

    protected override void HandleClickOnBeatmapObject(
      (BeatmapEditorObjectId id, BeatmapObjectCellData cellData) payload)
    {
      base.HandleClickOnBeatmapObject(payload);
      if (this.beatmapState.interactionMode != InteractionMode.Delete)
        return;
      this.signalBus.Fire<DeleteNoteSignal>(new DeleteNoteSignal(payload.id));
    }

    protected override void HandleDeleteBeatmapObject(
      (BeatmapEditorObjectId id, BeatmapObjectCellData cellData) payload)
    {
      this.signalBus.Fire<DeleteNoteSignal>(new DeleteNoteSignal(payload.id));
    }

    protected override void SelectBeatmapObject(BeatmapEditorObjectId id) => this.signalBus.Fire<SelectNoteSignal>(new SelectNoteSignal(id));

    private void HandleNoteInvertColor(
      (BeatmapEditorObjectId id, BeatmapObjectCellData cellData) payload)
    {
      this.signalBus.Fire<InvertHoveredNoteColorSignal>(new InvertHoveredNoteColorSignal(payload.id));
    }

    private void HandleNoteScrollUp(
      (BeatmapEditorObjectId id, BeatmapObjectCellData cellData) payload)
    {
      this.signalBus.Fire<ChangeHoveredArcControlPointLengthSignal>(new ChangeHoveredArcControlPointLengthSignal(payload.cellData, 1f));
    }

    private void HandleNoteScrollDown(
      (BeatmapEditorObjectId id, BeatmapObjectCellData cellData) payload)
    {
      this.signalBus.Fire<ChangeHoveredArcControlPointLengthSignal>(new ChangeHoveredArcControlPointLengthSignal(payload.cellData, -1f));
    }
  }
}
