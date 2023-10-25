// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.InputSignals.LightRotationTrackMouseInputController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Types;
using BeatmapEditor3D.Views;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.InputSignals
{
  public class LightRotationTrackMouseInputController : 
    AbstractMouseInputController<BeatmapEditorObjectId>
  {
    [SerializeField]
    private LightRotationEventBoxTrackView _lightRotationEventBoxTrackView;
    [SerializeField]
    private EventTrackInputMouseInputSource _eventTrackInputMouseInputSource;
    [Inject]
    private readonly BeatmapState _beatmapState;
    [Inject]
    private readonly SignalBus _signalBus;

    protected void OnEnable()
    {
      this._eventTrackInputMouseInputSource.gridPointerDownEvent += new Action<MouseInputType>(((AbstractMouseInputController<BeatmapEditorObjectId>) this).HandleMouseInputSelectionGridPointerDown);
      this._eventTrackInputMouseInputSource.gridPointerUpEvent += new Action<MouseInputType>(((AbstractMouseInputController<BeatmapEditorObjectId>) this).HandleMouseInputSelectionGridPointerUp);
      this._eventTrackInputMouseInputSource.gridPointerHoverEvent += new Action<MouseInputType>(((AbstractMouseInputController<BeatmapEditorObjectId>) this).HandleMouseInputSelectionGridPointerHover);
      this._eventTrackInputMouseInputSource.objectPointerHoverEvent += new Action<MouseInputType, BeatmapEditorObjectId>(((AbstractMouseInputController<BeatmapEditorObjectId>) this).HandleMouseInputEventSourceObjectPointerHover);
      this._eventTrackInputMouseInputSource.objectPointerDownEvent += new Action<MouseInputType, BeatmapEditorObjectId>(((AbstractMouseInputController<BeatmapEditorObjectId>) this).HandleMouseInputEventSourceObjectPointerDown);
      this._eventTrackInputMouseInputSource.objectPointerUpEvent += new Action<MouseInputType, BeatmapEditorObjectId>(((AbstractMouseInputController<BeatmapEditorObjectId>) this).HandleMouseInputEventSourceObjectPointerUp);
      this._eventTrackInputMouseInputSource.objectPointerScrollEvent += new Action<MouseInputType, BeatmapEditorObjectId>(((AbstractMouseInputController<BeatmapEditorObjectId>) this).HandleMouseInputEventSourceObjectPointerScroll);
      this.gridActions = new Dictionary<int, Action>()
      {
        {
          AbstractMouseInputController<BeatmapEditorObjectId>.HashInput(MouseInputEventOrigin.Grid, MouseInputType.Left, MouseEventType.Up, false, false, false),
          new Action(this.HandleEventGridCreateObject)
        }
      };
      this.objectActions = new Dictionary<int, Action<BeatmapEditorObjectId>>()
      {
        {
          AbstractMouseInputController<BeatmapEditorObjectId>.HashInput(MouseInputEventOrigin.Object, MouseInputType.Left, MouseEventType.Up, false, false, false),
          new Action<BeatmapEditorObjectId>(this.HandleEventObjectClick)
        },
        {
          AbstractMouseInputController<BeatmapEditorObjectId>.HashInput(MouseInputEventOrigin.Object, MouseInputType.Left, MouseEventType.Up, false, true, false),
          new Action<BeatmapEditorObjectId>(this.HandleEventObjectSelect)
        },
        {
          AbstractMouseInputController<BeatmapEditorObjectId>.HashInput(MouseInputEventOrigin.Object, MouseInputType.Middle, MouseEventType.Up, true, false, false),
          new Action<BeatmapEditorObjectId>(this.HandleEventObjectDelete)
        },
        {
          AbstractMouseInputController<BeatmapEditorObjectId>.HashInput(MouseInputEventOrigin.Object, MouseInputType.Enter, MouseEventType.None, false, false, false),
          new Action<BeatmapEditorObjectId>(this.HandleEventObjectEnter)
        },
        {
          AbstractMouseInputController<BeatmapEditorObjectId>.HashInput(MouseInputEventOrigin.Object, MouseInputType.Exit, MouseEventType.None, false, false, false),
          new Action<BeatmapEditorObjectId>(this.HandleEventObjectExit)
        },
        {
          AbstractMouseInputController<BeatmapEditorObjectId>.HashInput(MouseInputEventOrigin.Object, MouseInputType.ScrollUp, MouseEventType.None, false, false, true),
          new Action<BeatmapEditorObjectId>(this.HandleEventObjectRotationUp)
        },
        {
          AbstractMouseInputController<BeatmapEditorObjectId>.HashInput(MouseInputEventOrigin.Object, MouseInputType.ScrollDown, MouseEventType.None, false, false, true),
          new Action<BeatmapEditorObjectId>(this.HandleEventObjectRotationDown)
        }
      };
    }

    protected void OnDisable()
    {
      this.mouseBinder.ClearBindings();
      this._eventTrackInputMouseInputSource.gridPointerDownEvent -= new Action<MouseInputType>(((AbstractMouseInputController<BeatmapEditorObjectId>) this).HandleMouseInputSelectionGridPointerDown);
      this._eventTrackInputMouseInputSource.gridPointerUpEvent -= new Action<MouseInputType>(((AbstractMouseInputController<BeatmapEditorObjectId>) this).HandleMouseInputSelectionGridPointerUp);
      this._eventTrackInputMouseInputSource.gridPointerHoverEvent -= new Action<MouseInputType>(((AbstractMouseInputController<BeatmapEditorObjectId>) this).HandleMouseInputSelectionGridPointerHover);
      this._eventTrackInputMouseInputSource.objectPointerHoverEvent -= new Action<MouseInputType, BeatmapEditorObjectId>(((AbstractMouseInputController<BeatmapEditorObjectId>) this).HandleMouseInputEventSourceObjectPointerHover);
      this._eventTrackInputMouseInputSource.objectPointerDownEvent -= new Action<MouseInputType, BeatmapEditorObjectId>(((AbstractMouseInputController<BeatmapEditorObjectId>) this).HandleMouseInputEventSourceObjectPointerDown);
      this._eventTrackInputMouseInputSource.objectPointerUpEvent -= new Action<MouseInputType, BeatmapEditorObjectId>(((AbstractMouseInputController<BeatmapEditorObjectId>) this).HandleMouseInputEventSourceObjectPointerUp);
      this._eventTrackInputMouseInputSource.objectPointerScrollEvent -= new Action<MouseInputType, BeatmapEditorObjectId>(((AbstractMouseInputController<BeatmapEditorObjectId>) this).HandleMouseInputEventSourceObjectPointerScroll);
    }

    protected override void HandleMouseInputEventSourceObjectPointerHover(
      MouseInputType mouseInputType,
      BeatmapEditorObjectId objectId)
    {
      base.HandleMouseInputEventSourceObjectPointerHover(mouseInputType, objectId);
      this._lightRotationEventBoxTrackView.eventObjects[objectId].SetHighlight(mouseInputType == MouseInputType.Enter);
    }

    private void HandleEventObjectSelect(BeatmapEditorObjectId id) => this._signalBus.Fire<SelectSingleEventBoxesEventSignal>(new SelectSingleEventBoxesEventSignal(this._lightRotationEventBoxTrackView.id, id, true));

    private void HandleEventGridCreateObject()
    {
      if (this._beatmapState.interactionMode != InteractionMode.Place)
        return;
      this._signalBus.Fire<PlaceLightRotationEventSignal>(new PlaceLightRotationEventSignal(this._lightRotationEventBoxTrackView.startBeat, this._lightRotationEventBoxTrackView.id));
    }

    private void HandleEventObjectClick(BeatmapEditorObjectId id)
    {
      if (this._beatmapState.interactionMode != InteractionMode.Delete)
        return;
      this._signalBus.Fire<DeleteLightEventSignal>(new DeleteLightEventSignal(this._lightRotationEventBoxTrackView.id, id));
    }

    private void HandleEventObjectEnter(BeatmapEditorObjectId id) => this._signalBus.Fire<ChangeHoverEventBoxBaseEventSignal>(new ChangeHoverEventBoxBaseEventSignal(this._lightRotationEventBoxTrackView.id, id));

    private void HandleEventObjectExit(BeatmapEditorObjectId id) => this._signalBus.Fire<ChangeHoverEventBoxBaseEventSignal>(new ChangeHoverEventBoxBaseEventSignal(this._lightRotationEventBoxTrackView.id, id));

    private void HandleEventObjectRotationUp(BeatmapEditorObjectId id) => this._signalBus.Fire<ModifyHoveredLightRotationDeltaRotationSignal>(new ModifyHoveredLightRotationDeltaRotationSignal(1f));

    private void HandleEventObjectRotationDown(BeatmapEditorObjectId id) => this._signalBus.Fire<ModifyHoveredLightRotationDeltaRotationSignal>(new ModifyHoveredLightRotationDeltaRotationSignal(-1f));

    private void HandleEventObjectDelete(BeatmapEditorObjectId id) => this._signalBus.Fire<DeleteLightEventSignal>(new DeleteLightEventSignal(this._lightRotationEventBoxTrackView.id, id));
  }
}
