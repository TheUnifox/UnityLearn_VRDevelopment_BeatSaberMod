// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.InputSignals.EventBoxGroupTrackMouseInputController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Types;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.InputSignals
{
  public abstract class EventBoxGroupTrackMouseInputController : 
    AbstractMouseInputController<BeatmapEditorObjectId>
  {
    [SerializeField]
    private EventTrackInputMouseInputSource _eventSource;
    [Inject]
    private readonly BeatmapState _beatmapState;

    protected abstract int id { get; }

    protected abstract EventBoxGroupEditorData.EventBoxGroupType type { get; }

    protected virtual void OnEnable()
    {
      this._eventSource.gridPointerDownEvent += new Action<MouseInputType>(HandleMouseInputSelectionGridPointerDown);
      this._eventSource.gridPointerUpEvent += new Action<MouseInputType>((this).HandleMouseInputSelectionGridPointerUp);
      this._eventSource.gridPointerHoverEvent += new Action<MouseInputType>(HandleMouseInputSelectionGridPointerHover);
      this._eventSource.objectPointerDownEvent += new Action<MouseInputType, BeatmapEditorObjectId>(HandleMouseInputEventSourceObjectPointerDown);
      this._eventSource.objectPointerUpEvent += new Action<MouseInputType, BeatmapEditorObjectId>(HandleMouseInputEventSourceObjectPointerUp);
      this._eventSource.objectPointerHoverEvent += new Action<MouseInputType, BeatmapEditorObjectId>(HandleMouseInputEventSourceObjectPointerHover);
      this.gridActions = new Dictionary<int, Action>()
      {
        {
          AbstractMouseInputController<BeatmapEditorObjectId>.HashInput(MouseInputEventOrigin.Grid, MouseInputType.Left, MouseEventType.Up, false, false, false),
          new Action(this.HandleEventBoxGroupCreate)
        }
      };
      this.objectActions = new Dictionary<int, Action<BeatmapEditorObjectId>>()
      {
        {
          AbstractMouseInputController<BeatmapEditorObjectId>.HashInput(MouseInputEventOrigin.Object, MouseInputType.Left, MouseEventType.Up, false, false, false),
          new Action<BeatmapEditorObjectId>(this.HandleEventBoxGroupObjectClick)
        },
        {
          AbstractMouseInputController<BeatmapEditorObjectId>.HashInput(MouseInputEventOrigin.Object, MouseInputType.Middle, MouseEventType.Up, true, false, false),
          new Action<BeatmapEditorObjectId>(this.HandleEventBoxGroupObjectDelete)
        },
        {
          AbstractMouseInputController<BeatmapEditorObjectId>.HashInput(MouseInputEventOrigin.Object, MouseInputType.Left, MouseEventType.Up, false, true, false),
          new Action<BeatmapEditorObjectId>(this.HandleEventBoxGroupObjectSelect)
        }
      };
    }

    protected void OnDisable()
    {
      this._eventSource.gridPointerDownEvent -= new Action<MouseInputType>(HandleMouseInputSelectionGridPointerDown);
      this._eventSource.gridPointerUpEvent -= new Action<MouseInputType>(HandleMouseInputSelectionGridPointerUp);
      this._eventSource.gridPointerHoverEvent -= new Action<MouseInputType>(HandleMouseInputSelectionGridPointerHover);
      this._eventSource.objectPointerDownEvent -= new Action<MouseInputType, BeatmapEditorObjectId>(HandleMouseInputEventSourceObjectPointerDown);
      this._eventSource.objectPointerUpEvent -= new Action<MouseInputType, BeatmapEditorObjectId>(HandleMouseInputEventSourceObjectPointerUp);
      this._eventSource.objectPointerHoverEvent -= new Action<MouseInputType, BeatmapEditorObjectId>(HandleMouseInputEventSourceObjectPointerHover);
    }

    private void HandleEventBoxGroupCreate()
    {
      if (this._beatmapState.interactionMode != InteractionMode.Place)
        return;
      this.signalBus.Fire<PlaceEventBoxGroupSignal>(new PlaceEventBoxGroupSignal(this.id, this.type));
    }

    private void HandleEventBoxGroupObjectClick(BeatmapEditorObjectId id)
    {
      switch (this._beatmapState.interactionMode)
      {
        case InteractionMode.Place:
          this.signalBus.Fire<EditEventBoxGroupSignal>(new EditEventBoxGroupSignal(id));
          break;
        case InteractionMode.Delete:
          this.signalBus.Fire<DeleteEventBoxGroupSignal>(new DeleteEventBoxGroupSignal(id));
          break;
      }
    }

    private void HandleEventBoxGroupObjectDelete(BeatmapEditorObjectId id) => this.signalBus.Fire<DeleteEventBoxGroupSignal>(new DeleteEventBoxGroupSignal(id));

    private void HandleEventBoxGroupObjectSelect(BeatmapEditorObjectId id) => this.signalBus.Fire<SelectSingleEventBoxGroupSignal>(new SelectSingleEventBoxGroupSignal(id, true));
  }
}
