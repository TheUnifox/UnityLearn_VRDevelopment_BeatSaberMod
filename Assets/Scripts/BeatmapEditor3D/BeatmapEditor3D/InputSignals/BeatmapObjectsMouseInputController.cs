// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.InputSignals.BeatmapObjectsMouseInputController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using BeatmapEditor3D.Types;
using HMUI;
using System;
using System.Collections.Generic;
using UnityEngine.Events;
using Zenject;

namespace BeatmapEditor3D.InputSignals
{
  public class BeatmapObjectsMouseInputController : 
    AbstractMouseInputController<(BeatmapEditorObjectId id, BeatmapObjectCellData cellData)>
  {
    [Inject]
    protected readonly IReadonlyBeatmapState beatmapState;
    [Inject]
    private readonly IReadonlyBeatmapObjectsSelectionState _beatmapObjectsSelectionState;

    protected virtual void OnEnable()
    {
      this.mouseBinder.AddButtonBinding(MouseBinder.ButtonType.Primary, MouseBinder.MouseEventType.ButtonUp, new UnityAction(this.HandleEndBeatmapObjectDrag));
      this.objectActions = new Dictionary<int, Action<(BeatmapEditorObjectId, BeatmapObjectCellData)>>()
      {
        {
          AbstractMouseInputController<(BeatmapEditorObjectId, BeatmapObjectCellData)>.HashInput(MouseInputEventOrigin.Object, MouseInputType.Left, MouseEventType.Up, false, false, false),
          new Action<(BeatmapEditorObjectId, BeatmapObjectCellData)>(this.HandleClickOnBeatmapObject)
        },
        {
          AbstractMouseInputController<(BeatmapEditorObjectId, BeatmapObjectCellData)>.HashInput(MouseInputEventOrigin.Object, MouseInputType.Left, MouseEventType.Up, true, false, false),
          new Action<(BeatmapEditorObjectId, BeatmapObjectCellData)>(this.HandleScrollToBeatmapObject)
        },
        {
          AbstractMouseInputController<(BeatmapEditorObjectId, BeatmapObjectCellData)>.HashInput(MouseInputEventOrigin.Object, MouseInputType.Left, MouseEventType.Up, true, true, false),
          new Action<(BeatmapEditorObjectId, BeatmapObjectCellData)>(this.HandleBeatmapObjectClickSelection)
        },
        {
          AbstractMouseInputController<(BeatmapEditorObjectId, BeatmapObjectCellData)>.HashInput(MouseInputEventOrigin.Object, MouseInputType.Left, MouseEventType.Down, false, false, true),
          new Action<(BeatmapEditorObjectId, BeatmapObjectCellData)>(this.HandleMoveBeatmapObjectToBeatLine)
        },
        {
          AbstractMouseInputController<(BeatmapEditorObjectId, BeatmapObjectCellData)>.HashInput(MouseInputEventOrigin.Object, MouseInputType.Left, MouseEventType.Up, false, true, false),
          new Action<(BeatmapEditorObjectId, BeatmapObjectCellData)>(this.HandleSelectBeatmapObject)
        },
        {
          AbstractMouseInputController<(BeatmapEditorObjectId, BeatmapObjectCellData)>.HashInput(MouseInputEventOrigin.Object, MouseInputType.Middle, MouseEventType.Up, true, false, false),
          new Action<(BeatmapEditorObjectId, BeatmapObjectCellData)>(this.HandleDeleteBeatmapObject)
        },
        {
          AbstractMouseInputController<(BeatmapEditorObjectId, BeatmapObjectCellData)>.HashInput(MouseInputEventOrigin.Object, MouseInputType.Enter, MouseEventType.None, false, false, false),
          new Action<(BeatmapEditorObjectId, BeatmapObjectCellData)>(this.HandleEnterChangeHoverCellData)
        },
        {
          AbstractMouseInputController<(BeatmapEditorObjectId, BeatmapObjectCellData)>.HashInput(MouseInputEventOrigin.Object, MouseInputType.Exit, MouseEventType.None, false, false, false),
          new Action<(BeatmapEditorObjectId, BeatmapObjectCellData)>(this.HandleExitChangeHoverCellData)
        }
      };
    }

    protected virtual void OnDisable()
    {
    }

    private void HandleEnterChangeHoverCellData(
      (BeatmapEditorObjectId id, BeatmapObjectCellData cellData) payload)
    {
      this.signalBus.Fire<ChangeHoverSignal>(new ChangeHoverSignal(payload.id, payload.cellData, ChangeHoverSignal.HoverOrigin.Object));
    }

    private void HandleExitChangeHoverCellData(
      (BeatmapEditorObjectId id, BeatmapObjectCellData cellData) payload)
    {
      this.signalBus.Fire<ChangeHoverSignal>(new ChangeHoverSignal(BeatmapEditorObjectId.invalid, (BeatmapObjectCellData) null, ChangeHoverSignal.HoverOrigin.Object));
    }

    private void HandleBeatmapObjectClickSelection(
      (BeatmapEditorObjectId id, BeatmapObjectCellData cellData) payload)
    {
      if (this.beatmapState.interactionMode == InteractionMode.ClickSelect)
        this.signalBus.Fire<EndBeatmapObjectsSelectionSignal>(new EndBeatmapObjectsSelectionSignal(payload.cellData.beat, true));
      else
        this.signalBus.Fire<StartBeatmapObjectsSelectionSignal>(new StartBeatmapObjectsSelectionSignal(payload.cellData.beat));
    }

    private void HandleEndBeatmapObjectDrag() => this.signalBus.Fire<ClearDraggedBeatmapObjectSignal>();

    private void HandleScrollToBeatmapObject(
      (BeatmapEditorObjectId id, BeatmapObjectCellData cellData) payload)
    {
      this.signalBus.Fire<UpdatePlayHeadSignal>(new UpdatePlayHeadSignal(payload.cellData.beat, UpdatePlayHeadSignal.SnapType.None));
    }

    protected virtual void HandleClickOnBeatmapObject(
      (BeatmapEditorObjectId id, BeatmapObjectCellData cellData) payload)
    {
      if (this.beatmapState.interactionMode != InteractionMode.ClickSelect)
        return;
      this.signalBus.Fire<EndBeatmapObjectsSelectionSignal>(new EndBeatmapObjectsSelectionSignal(payload.cellData.beat, true));
    }

    protected void HandleSelectBeatmapObject(
      (BeatmapEditorObjectId id, BeatmapObjectCellData cellData) payload)
    {
      if (this._beatmapObjectsSelectionState.showSelection || this.beatmapState.interactionMode != InteractionMode.Select)
        return;
      this.SelectBeatmapObject(payload.id);
    }

    protected virtual void HandleMoveBeatmapObjectToBeatLine(
      (BeatmapEditorObjectId id, BeatmapObjectCellData cellData) payload)
    {
    }

    protected virtual void HandleDeleteBeatmapObject(
      (BeatmapEditorObjectId id, BeatmapObjectCellData cellData) payload)
    {
    }

    protected virtual void SelectBeatmapObject(BeatmapEditorObjectId id)
    {
    }
  }
}
