// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.InputSignals.BasicEventObjectMouseInputController
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
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace BeatmapEditor3D.InputSignals
{
  public class BasicEventObjectMouseInputController : 
    AbstractMouseInputController<BasicEventEditorData>
  {
    [SerializeField]
    private BasicEventObjectMouseInputEventSource _eventSource;
    [Inject]
    private readonly BeatmapState _beatmapState;
    [Inject]
    private readonly BasicEventsState _basicBasicEventsState;
    [Inject]
    private readonly IReadonlyEventsSelectionState _eventsSelectionState;

    protected void OnEnable()
    {
      this._eventSource.pointerDownEvent += new Action<MouseInputType, BasicEventEditorData>(((AbstractMouseInputController<BasicEventEditorData>) this).HandleMouseInputEventSourceObjectPointerDown);
      this._eventSource.pointerUpEvent += new Action<MouseInputType, BasicEventEditorData>(((AbstractMouseInputController<BasicEventEditorData>) this).HandleMouseInputEventSourceObjectPointerUp);
      this._eventSource.pointerHoverEvent += new Action<MouseInputType, BasicEventEditorData>(((AbstractMouseInputController<BasicEventEditorData>) this).HandleMouseInputEventSourceObjectPointerHover);
      this.mouseBinder.AddScrollBinding(new UnityAction<float>(this.HandleMouseScroll));
      this.mouseBinder.AddButtonBinding(MouseBinder.ButtonType.Primary, MouseBinder.MouseEventType.ButtonUp, new UnityAction(this.HandleEndEventTrackDrag));
      this.objectActions = new Dictionary<int, Action<BasicEventEditorData>>()
      {
        {
          AbstractMouseInputController<BasicEventEditorData>.HashInput(MouseInputEventOrigin.Object, MouseInputType.Left, MouseEventType.Up, true, true, false),
          new Action<BasicEventEditorData>(this.HandleEventsClickSelection)
        },
        {
          AbstractMouseInputController<BasicEventEditorData>.HashInput(MouseInputEventOrigin.Object, MouseInputType.Left, MouseEventType.Up, false, false, false),
          new Action<BasicEventEditorData>(this.HandleEventObjectClick)
        },
        {
          AbstractMouseInputController<BasicEventEditorData>.HashInput(MouseInputEventOrigin.Object, MouseInputType.Left, MouseEventType.Up, false, true, false),
          new Action<BasicEventEditorData>(this.HandleSelectEventObject)
        },
        {
          AbstractMouseInputController<BasicEventEditorData>.HashInput(MouseInputEventOrigin.Object, MouseInputType.Left, MouseEventType.Down, false, false, true),
          new Action<BasicEventEditorData>(this.HandleMoveEventToBeatLine)
        },
        {
          AbstractMouseInputController<BasicEventEditorData>.HashInput(MouseInputEventOrigin.Object, MouseInputType.Left, MouseEventType.Up, true, false, false),
          new Action<BasicEventEditorData>(this.HandleScrollToEventObject)
        },
        {
          AbstractMouseInputController<BasicEventEditorData>.HashInput(MouseInputEventOrigin.Object, MouseInputType.Middle, MouseEventType.Up, true, false, false),
          new Action<BasicEventEditorData>(this.HandleDeleteEventObject)
        },
        {
          AbstractMouseInputController<BasicEventEditorData>.HashInput(MouseInputEventOrigin.Object, MouseInputType.Middle, MouseEventType.Up, false, false, false),
          new Action<BasicEventEditorData>(this.HandleModifyEventObject)
        },
        {
          AbstractMouseInputController<BasicEventEditorData>.HashInput(MouseInputEventOrigin.Object, MouseInputType.Enter, MouseEventType.None, false, false, false),
          new Action<BasicEventEditorData>(this.HandleEnterChangeHoverEventObject)
        },
        {
          AbstractMouseInputController<BasicEventEditorData>.HashInput(MouseInputEventOrigin.Object, MouseInputType.Exit, MouseEventType.None, false, false, false),
          new Action<BasicEventEditorData>(this.HandleExitChangeHoverEventObject)
        }
      };
    }

    protected void OnDisable()
    {
      this.mouseBinder.ClearBindings();
      this._eventSource.pointerDownEvent -= new Action<MouseInputType, BasicEventEditorData>(((AbstractMouseInputController<BasicEventEditorData>) this).HandleMouseInputEventSourceObjectPointerDown);
      this._eventSource.pointerUpEvent -= new Action<MouseInputType, BasicEventEditorData>(((AbstractMouseInputController<BasicEventEditorData>) this).HandleMouseInputEventSourceObjectPointerUp);
      this._eventSource.pointerHoverEvent -= new Action<MouseInputType, BasicEventEditorData>(((AbstractMouseInputController<BasicEventEditorData>) this).HandleMouseInputEventSourceObjectPointerHover);
    }

    private void HandleMouseScroll(float delta)
    {
      if (!Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.RightAlt))
        return;
      this.signalBus.Fire<ModifyHoveredLaserRotationSpeedSignal>(new ModifyHoveredLaserRotationSpeedSignal(delta));
      this.signalBus.Fire<ModifyHoveredLightEventDeltaIntensitySignal>(new ModifyHoveredLightEventDeltaIntensitySignal(delta));
      this.signalBus.Fire<ModifyHoveredDeltaFloatEventValueSignal>(new ModifyHoveredDeltaFloatEventValueSignal(delta));
    }

    private void HandleScrollToEventObject(BasicEventEditorData basicEventEditorData) => this.signalBus.Fire<UpdatePlayHeadSignal>(new UpdatePlayHeadSignal(basicEventEditorData.beat, UpdatePlayHeadSignal.SnapType.None));

    private void HandleSelectEventObject(BasicEventEditorData basicEventEditorData)
    {
      if (this._eventsSelectionState.showSelection)
        return;
      this.signalBus.Fire<SelectSingleEventSignal>(new SelectSingleEventSignal()
      {
        addToSelection = true,
        basicEventEditorData = basicEventEditorData,
        beat = basicEventEditorData.beat
      });
    }

    private void HandleEventHoverUpdated() => this.signalBus.Fire<MoveEventToTrackSignal>(new MoveEventToTrackSignal(this._eventsSelectionState.draggedBasicEventData, this._basicBasicEventsState.currentHoverPageTrackId));

    private void HandleEndEventTrackDrag()
    {
      if (this._eventsSelectionState.draggedBasicEventData == (BasicEventEditorData) null)
        return;
      this.signalBus.Unsubscribe<EventHoverUpdatedSignal>(new Action(this.HandleEventHoverUpdated));
      this.signalBus.Fire<ClearDraggedEventSignal>();
    }

    private void HandleMoveEventToBeatLine(BasicEventEditorData basicEventEditorData)
    {
      this.signalBus.Fire<MoveEventToBeatLineSignal>(new MoveEventToBeatLineSignal(basicEventEditorData));
      this.signalBus.Fire<MoveEventToTrackSignal>(new MoveEventToTrackSignal(this._eventsSelectionState.draggedBasicEventData, this._basicBasicEventsState.currentHoverPageTrackId));
      this.signalBus.Subscribe<EventHoverUpdatedSignal>(new Action(this.HandleEventHoverUpdated));
    }

    private void HandleEventObjectClick(BasicEventEditorData basicEventEditorData)
    {
      switch (this._beatmapState.interactionMode)
      {
        case InteractionMode.ClickSelect:
          this.signalBus.Fire<EndEventsSelectionSignal>(new EndEventsSelectionSignal(basicEventEditorData.beat, basicEventEditorData.type, true));
          break;
        case InteractionMode.Delete:
          this.signalBus.Fire<RemoveEventSignal>(new RemoveEventSignal()
          {
            basicEventData = basicEventEditorData
          });
          break;
      }
    }

    private void HandleEventsClickSelection(BasicEventEditorData basicEventEditorData)
    {
      if (this._beatmapState.interactionMode == InteractionMode.ClickSelect)
        this.signalBus.Fire<EndEventsSelectionSignal>(new EndEventsSelectionSignal(basicEventEditorData.beat, basicEventEditorData.type, true));
      else
        this.signalBus.Fire<StartEventsSelectionSignal>(new StartEventsSelectionSignal(basicEventEditorData.beat, basicEventEditorData.type));
    }

    private void HandleModifyEventObject(BasicEventEditorData basicEventEditorData) => this.signalBus.Fire<ModifyLightEventColorSignal>(new ModifyLightEventColorSignal(basicEventEditorData));

    private void HandleDeleteEventObject(BasicEventEditorData basicEventData) => this.signalBus.Fire<RemoveEventSignal>(new RemoveEventSignal()
    {
      basicEventData = basicEventData
    });

    private void HandleEnterChangeHoverEventObject(BasicEventEditorData basicEventEditorData) => this.signalBus.Fire<ChangeHoverEventObjectSignal>(new ChangeHoverEventObjectSignal(basicEventEditorData));

    private void HandleExitChangeHoverEventObject(BasicEventEditorData basicEventEditorData) => this.signalBus.Fire<ChangeHoverEventObjectSignal>(new ChangeHoverEventObjectSignal((BasicEventEditorData) null));
  }
}
