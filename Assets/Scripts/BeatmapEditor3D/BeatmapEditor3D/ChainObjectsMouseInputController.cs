// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.ChainObjectsMouseInputController
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
  public class ChainObjectsMouseInputController : BeatmapObjectsMouseInputController
  {
    [SerializeField]
    private ChainObjectsMouseInputSource _chainObjectsMouseInputSource;
    [SerializeField]
    private ChainBeatmapObjectsView _chainBeatmapObjectsView;

    protected override void OnEnable()
    {
      base.OnEnable();
      this._chainObjectsMouseInputSource.objectPointerDownEvent += new Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)>(HandleMouseInputEventSourceObjectPointerDown);
      this._chainObjectsMouseInputSource.objectPointerUpEvent += new Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)>(HandleMouseInputEventSourceObjectPointerUp);
      this._chainObjectsMouseInputSource.objectPointerHoverEvent += new Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)>(HandleMouseInputEventSourceObjectPointerHover);
      this._chainObjectsMouseInputSource.objectPointerScrollEvent += new Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)>(HandleMouseInputEventSourceObjectPointerScroll);
      this.objectActions[AbstractMouseInputController<(BeatmapEditorObjectId, BeatmapObjectCellData)>.HashInput(MouseInputEventOrigin.Object, MouseInputType.Middle, MouseEventType.Up, false, false, false)] = new Action<(BeatmapEditorObjectId, BeatmapObjectCellData)>(this.HandleBeatmapObjectInvertColor);
      this.objectActions[AbstractMouseInputController<(BeatmapEditorObjectId, BeatmapObjectCellData)>.HashInput(MouseInputEventOrigin.Object, MouseInputType.ScrollUp, MouseEventType.None, false, false, true)] = new Action<(BeatmapEditorObjectId, BeatmapObjectCellData)>(this.HandleChainObjectIncreaseSliceCount);
      this.objectActions[AbstractMouseInputController<(BeatmapEditorObjectId, BeatmapObjectCellData)>.HashInput(MouseInputEventOrigin.Object, MouseInputType.ScrollDown, MouseEventType.None, false, false, true)] = new Action<(BeatmapEditorObjectId, BeatmapObjectCellData)>(this.HandleChainObjectDecreaseSliceCount);
      this.objectActions[AbstractMouseInputController<(BeatmapEditorObjectId, BeatmapObjectCellData)>.HashInput(MouseInputEventOrigin.Object, MouseInputType.ScrollUp, MouseEventType.None, true, false, true)] = new Action<(BeatmapEditorObjectId, BeatmapObjectCellData)>(this.HandleChainObjectIncreaseSquish);
      this.objectActions[AbstractMouseInputController<(BeatmapEditorObjectId, BeatmapObjectCellData)>.HashInput(MouseInputEventOrigin.Object, MouseInputType.ScrollDown, MouseEventType.None, true, false, true)] = new Action<(BeatmapEditorObjectId, BeatmapObjectCellData)>(this.HandleChainObjectDecreaseSquish);
      this.objectActions[AbstractMouseInputController<(BeatmapEditorObjectId, BeatmapObjectCellData)>.HashInput(MouseInputEventOrigin.Object, MouseInputType.Left, MouseEventType.Down, true, false, true)] = new Action<(BeatmapEditorObjectId, BeatmapObjectCellData)>(this.HandleMoveChainToBeatLine);
    }

    protected override void OnDisable()
    {
      base.OnDisable();
      this._chainObjectsMouseInputSource.objectPointerDownEvent -= new Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)>(HandleMouseInputEventSourceObjectPointerDown);
      this._chainObjectsMouseInputSource.objectPointerUpEvent -= new Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)>(HandleMouseInputEventSourceObjectPointerUp);
      this._chainObjectsMouseInputSource.objectPointerHoverEvent -= new Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)>(HandleMouseInputEventSourceObjectPointerHover);
      this._chainObjectsMouseInputSource.objectPointerScrollEvent -= new Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)>(HandleMouseInputEventSourceObjectPointerScroll);
    }

    protected override void HandleMouseInputEventSourceObjectPointerHover(
      MouseInputType mouseInputType,
      (BeatmapEditorObjectId id, BeatmapObjectCellData cellData) payload)
    {
      base.HandleMouseInputEventSourceObjectPointerHover(mouseInputType, payload);
      ChainNoteView chainNoteView;
      if (!this._chainBeatmapObjectsView.chainObjects.TryGetValue(payload.id, out chainNoteView))
        return;
      chainNoteView.SetHighlight(mouseInputType == MouseInputType.Enter);
    }

    private void HandleBeatmapObjectInvertColor(
      (BeatmapEditorObjectId id, BeatmapObjectCellData cellData) payload)
    {
      this.signalBus.Fire<InvertHoveredChainColorSignal>(new InvertHoveredChainColorSignal(payload.id));
    }

    protected override void HandleMoveBeatmapObjectToBeatLine(
      (BeatmapEditorObjectId id, BeatmapObjectCellData cellData) payload)
    {
      this.signalBus.Fire<MoveChainToBeatLineSignal>(new MoveChainToBeatLineSignal(payload.id, payload.cellData, true));
    }

    private void HandleMoveChainToBeatLine(
      (BeatmapEditorObjectId id, BeatmapObjectCellData cellData) payload)
    {
      this.signalBus.Fire<MoveChainToBeatLineSignal>(new MoveChainToBeatLineSignal(payload.id, payload.cellData, false));
    }

    protected override void HandleClickOnBeatmapObject(
      (BeatmapEditorObjectId id, BeatmapObjectCellData cellData) payload)
    {
      base.HandleClickOnBeatmapObject(payload);
      if (this.beatmapState.interactionMode != InteractionMode.Delete)
        return;
      this.signalBus.Fire<DeleteChainSignal>(new DeleteChainSignal(payload.id));
    }

    protected override void HandleDeleteBeatmapObject(
      (BeatmapEditorObjectId id, BeatmapObjectCellData cellData) payload)
    {
      this.signalBus.Fire<DeleteChainSignal>(new DeleteChainSignal(payload.id));
    }

    protected override void SelectBeatmapObject(BeatmapEditorObjectId id) => this.signalBus.Fire<SelectChainSignal>(new SelectChainSignal(id));

    private void HandleChainObjectIncreaseSliceCount(
      (BeatmapEditorObjectId id, BeatmapObjectCellData cellData) payload)
    {
      this.signalBus.Fire<ChangeHoveredChainSliceCountSignal>(new ChangeHoveredChainSliceCountSignal(1f));
    }

    private void HandleChainObjectDecreaseSliceCount(
      (BeatmapEditorObjectId id, BeatmapObjectCellData cellData) payload)
    {
      this.signalBus.Fire<ChangeHoveredChainSliceCountSignal>(new ChangeHoveredChainSliceCountSignal(-1f));
    }

    private void HandleChainObjectIncreaseSquish(
      (BeatmapEditorObjectId id, BeatmapObjectCellData cellData) payload)
    {
      this.signalBus.Fire<ChangeHoveredChainSquishAmountSignal>(new ChangeHoveredChainSquishAmountSignal(1f));
    }

    private void HandleChainObjectDecreaseSquish(
      (BeatmapEditorObjectId id, BeatmapObjectCellData cellData) payload)
    {
      this.signalBus.Fire<ChangeHoveredChainSquishAmountSignal>(new ChangeHoveredChainSquishAmountSignal(-1f));
    }
  }
}
