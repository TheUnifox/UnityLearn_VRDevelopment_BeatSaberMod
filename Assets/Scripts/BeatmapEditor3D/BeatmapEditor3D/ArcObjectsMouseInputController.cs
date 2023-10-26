using BeatmapEditor3D.Commands;
using BeatmapEditor3D.InputSignals;
using BeatmapEditor3D.LevelEditor;
using BeatmapEditor3D.Types;
using BeatmapEditor3D.Views;
using BeatmapEditor3D.Visuals;
using HMUI;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace BeatmapEditor3D
{
  public class ArcObjectsMouseInputController : BeatmapObjectsMouseInputController
  {
    [SerializeField]
    private ArcObjectsMouseInputSource _arcObjectsMouseInputSource;
    [SerializeField]
    private ArcBeatmapObjectsView _arcBeatmapObjectsView;
    private readonly KeyboardBinder _keyboardBinder = new KeyboardBinder();
    private bool _dragging;
    private bool _changingControlPoint;
    private bool _changingMidAnchorMode;
    private (BeatmapEditorObjectId id, BeatmapObjectCellData cellData, NoteCutDirection cutDirection, float controlPointLengthMultiplier, Vector3 worldSpaceOrigin, bool flipDirection) _controlPointChangePayload;
    private (BeatmapEditorObjectId id, BeatmapObjectCellData cellData, SliderMidAnchorMode midAnchorMode, Vector3 worldSpaceOrigin) _midAnchorModeChangePayload;
    private Vector3 _screenSpaceHandleStartPoint;
    private float _currentLength;
    private NoteCutDirection _currentCutDirection;
    private SliderMidAnchorMode _currentMidAnchorMode;

    protected override void OnEnable()
    {
      base.OnEnable();
      this._arcObjectsMouseInputSource.objectPointerDownEvent += new Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)>(HandleMouseInputEventSourceObjectPointerDown);
      this._arcObjectsMouseInputSource.objectPointerUpEvent += new Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)>(HandleMouseInputEventSourceObjectPointerUp);
      this._arcObjectsMouseInputSource.objectPointerHoverEvent += new Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)>(HandleMouseInputEventSourceObjectPointerHover);
      this._arcObjectsMouseInputSource.arcObjectHeadMoveControlPointDownEvent += new Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)>(this.HandleArcObjectsMouseInputSourceHeadMoveControlPointDown);
      this._arcObjectsMouseInputSource.arcObjectControlPointChangeEvent += new Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData, NoteCutDirection, float, Vector3, bool)>(this.HandleArcObjectsMouseInputSourceControlPointLengthChange);
      this._arcObjectsMouseInputSource.arcObjectTailMoveControlPointDownEvent += new Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)>(this.HandleArcObjectMouseInputSourceTailMoveControlPointDown);
      this._arcObjectsMouseInputSource.arcObjectMidControlPointDownEvent += new Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData, SliderMidAnchorMode, Vector3)>(this.HandleArcObjectsMouseInputSourceMidControlPointDown);
      this.objectActions[AbstractMouseInputController<(BeatmapEditorObjectId, BeatmapObjectCellData)>.HashInput(MouseInputEventOrigin.Object, MouseInputType.Middle, BeatmapEditor3D.InputSignals.MouseEventType.Up, false, false, false)] = new Action<(BeatmapEditorObjectId, BeatmapObjectCellData)>(this.HandleBeatmapObjectInvertColor);
      this.mouseBinder.AddButtonBinding(MouseBinder.ButtonType.Primary, MouseBinder.MouseEventType.ButtonUp, new UnityAction(this.HandleArcObjectsControlPointMouseUp));
      this._keyboardBinder.AddBinding(KeyCode.Escape, KeyboardBinder.KeyBindingType.KeyUp, new Action<bool>(this.HandleCancelModification));
    }

    protected override void OnDisable()
    {
      base.OnDisable();
      this._arcObjectsMouseInputSource.objectPointerDownEvent -= new Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)>(HandleMouseInputEventSourceObjectPointerDown);
      this._arcObjectsMouseInputSource.objectPointerUpEvent -= new Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)>(HandleMouseInputEventSourceObjectPointerUp);
      this._arcObjectsMouseInputSource.objectPointerHoverEvent -= new Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)>(HandleMouseInputEventSourceObjectPointerHover);
      this._arcObjectsMouseInputSource.arcObjectHeadMoveControlPointDownEvent -= new Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)>(this.HandleArcObjectsMouseInputSourceHeadMoveControlPointDown);
      this._arcObjectsMouseInputSource.arcObjectControlPointChangeEvent -= new Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData, NoteCutDirection, float, Vector3, bool)>(this.HandleArcObjectsMouseInputSourceControlPointLengthChange);
      this._arcObjectsMouseInputSource.arcObjectTailMoveControlPointDownEvent -= new Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)>(this.HandleArcObjectMouseInputSourceTailMoveControlPointDown);
      this._arcObjectsMouseInputSource.arcObjectMidControlPointDownEvent -= new Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData, SliderMidAnchorMode, Vector3)>(this.HandleArcObjectsMouseInputSourceMidControlPointDown);
    }

    protected override void Update()
    {
      base.Update();
      this._keyboardBinder.ManualUpdate();
      if (!this._dragging)
        return;
      if (this._changingControlPoint)
        this.UpdateControlPoint();
      if (!this._changingMidAnchorMode)
        return;
      this.UpdateMidAnchorMode();
    }

    protected override void HandleMouseInputEventSourceObjectPointerHover(
      MouseInputType mouseInputType,
      (BeatmapEditorObjectId id, BeatmapObjectCellData cellData) payload)
    {
      base.HandleMouseInputEventSourceObjectPointerHover(mouseInputType, payload);
      ArcView arcView;
      if (!this._arcBeatmapObjectsView.arcObjects.TryGetValue(payload.id, out arcView))
        return;
      arcView.SetHighlight(mouseInputType == MouseInputType.Enter);
    }

    private void HandleArcObjectsMouseInputSourceHeadMoveControlPointDown(
      MouseInputType mouseInputType,
      (BeatmapEditorObjectId id, BeatmapObjectCellData cellData) payload)
    {
      this.signalBus.Fire<MoveArcToBeatLineSignal>(new MoveArcToBeatLineSignal(payload.id, payload.cellData));
    }

    private void HandleArcObjectsMouseInputSourceControlPointLengthChange(
      MouseInputType mouseInputType,
      (BeatmapEditorObjectId, BeatmapObjectCellData, NoteCutDirection, float, Vector3, bool) payload)
    {
      if (this.beatmapState.interactionMode != InteractionMode.Modify)
        return;
      this._dragging = true;
      this._changingControlPoint = true;
      this._controlPointChangePayload = payload;
      this._currentLength = this._controlPointChangePayload.controlPointLengthMultiplier;
      this._currentCutDirection = this._controlPointChangePayload.cutDirection;
      this._screenSpaceHandleStartPoint = Input.mousePosition;
    }

    private void HandleArcObjectMouseInputSourceTailMoveControlPointDown(
      MouseInputType mouseInputType,
      (BeatmapEditorObjectId id, BeatmapObjectCellData cellData) payload)
    {
      this.signalBus.Fire<MoveArcToBeatLineSignal>(new MoveArcToBeatLineSignal(payload.id, payload.cellData));
    }

    private void HandleArcObjectsMouseInputSourceMidControlPointDown(
      MouseInputType mouseInputType,
      (BeatmapEditorObjectId, BeatmapObjectCellData, SliderMidAnchorMode, Vector3) payload)
    {
      if (this.beatmapState.interactionMode != InteractionMode.Modify)
        return;
      this._dragging = true;
      this._changingMidAnchorMode = true;
      this._midAnchorModeChangePayload = payload;
      this._currentMidAnchorMode = this._midAnchorModeChangePayload.midAnchorMode;
      this._screenSpaceHandleStartPoint = Input.mousePosition;
    }

    protected override void HandleClickOnBeatmapObject(
      (BeatmapEditorObjectId id, BeatmapObjectCellData cellData) payload)
    {
      base.HandleClickOnBeatmapObject(payload);
      if (this.beatmapState.interactionMode != InteractionMode.Delete)
        return;
      this.signalBus.Fire<DeleteArcSignal>(new DeleteArcSignal(payload.id));
    }

    protected override void HandleDeleteBeatmapObject(
      (BeatmapEditorObjectId id, BeatmapObjectCellData cellData) payload)
    {
      this.signalBus.Fire<DeleteArcSignal>(new DeleteArcSignal(payload.id));
    }

    protected override void SelectBeatmapObject(BeatmapEditorObjectId id) => this.signalBus.Fire<SelectArcSignal>(new SelectArcSignal(id));

    private void HandleBeatmapObjectInvertColor(
      (BeatmapEditorObjectId id, BeatmapObjectCellData cellData) payload)
    {
      this.signalBus.Fire<InvertHoveredArcColorSignal>(new InvertHoveredArcColorSignal(payload.id));
    }

    private void HandleArcObjectsControlPointMouseUp()
    {
      if (!this._dragging)
        return;
      if (this._changingControlPoint && (this._currentCutDirection != this._controlPointChangePayload.cutDirection || !Mathf.Approximately(this._currentLength, this._controlPointChangePayload.controlPointLengthMultiplier)))
        this.signalBus.Fire<ChangeHoveredArcControlPointCutDirectionAndLengthSignal>(new ChangeHoveredArcControlPointCutDirectionAndLengthSignal(this._controlPointChangePayload.id, this._controlPointChangePayload.cellData, this._currentCutDirection, this._currentLength));
      if (this._changingMidAnchorMode && this._currentMidAnchorMode != this._midAnchorModeChangePayload.midAnchorMode)
        this.signalBus.Fire<ChangeHoveredArcMidAnchorModeSignal>(new ChangeHoveredArcMidAnchorModeSignal(this._midAnchorModeChangePayload.id, this._currentMidAnchorMode));
      this._dragging = false;
      this._changingControlPoint = false;
      this._changingMidAnchorMode = false;
    }

    private void HandleCancelModification(bool obj)
    {
      this._arcBeatmapObjectsView.ResetDataOverride(this._controlPointChangePayload.id, this._controlPointChangePayload.cellData);
      this._controlPointChangePayload = default(ValueTuple<BeatmapEditorObjectId, BeatmapObjectCellData, NoteCutDirection, float, Vector3, bool>);
      this._midAnchorModeChangePayload = default(ValueTuple<BeatmapEditorObjectId, BeatmapObjectCellData, SliderMidAnchorMode, Vector3>);
      this._dragging = false;
      this._changingControlPoint = false;
      this._changingMidAnchorMode = false;
    }

    private void UpdateControlPoint()
    {
      Vector3 screenPoint1 = Camera.main.WorldToScreenPoint(this._controlPointChangePayload.worldSpaceOrigin);
      NoteCutDirection cutDirection = NoteCutDirectionExtensions.NoteCutDirectionFromDirection((this._controlPointChangePayload.flipDirection ? screenPoint1 - Input.mousePosition : Input.mousePosition - screenPoint1).normalized);
      float a = this._currentLength;
      if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
      {
        Vector2 vector2 = this._controlPointChangePayload.flipDirection ? cutDirection.Opposite().Direction() : cutDirection.Direction();
        Vector3 screenPoint2 = Camera.main.WorldToScreenPoint(this._controlPointChangePayload.worldSpaceOrigin + new Vector3(vector2.x, vector2.y, 0.0f));
        Vector3 vector3 = screenPoint2 - screenPoint1;
        a = Mathf.Max(0.0f, Mathf.Round(Vector3Extensions.InverseLerp(screenPoint1 + vector3 * 0.5f, screenPoint2, Input.mousePosition) * 20f) / 20f);
      }
      if (cutDirection != this._currentCutDirection || !Mathf.Approximately(a, this._currentLength))
        this._arcBeatmapObjectsView.SetControlPointsDataOverride(this._controlPointChangePayload.id, this._controlPointChangePayload.cellData, new float?(a), new NoteCutDirection?(cutDirection));
      this._currentCutDirection = cutDirection;
      this._currentLength = a;
    }

    private void UpdateMidAnchorMode()
    {
      Vector3 screenPoint = Camera.main.WorldToScreenPoint(this._midAnchorModeChangePayload.worldSpaceOrigin);
      SliderMidAnchorMode midAnchorMode = SliderMidAnchorModeExtensions.SliderMidAnchorModeFromDirection((Input.mousePosition - screenPoint).normalized);
      if (midAnchorMode != this._currentMidAnchorMode)
        this._screenSpaceHandleStartPoint = this._screenSpaceHandleStartPoint.RotatedAroundPivot(midAnchorMode.Rotation() * Quaternion.Inverse(this._currentMidAnchorMode.Rotation()), screenPoint);
      if (midAnchorMode != this._currentMidAnchorMode)
        this._arcBeatmapObjectsView.SetMidAnchorModeDataOverride(this._midAnchorModeChangePayload.id, new SliderMidAnchorMode?(midAnchorMode));
      this._currentMidAnchorMode = midAnchorMode;
    }
  }
}
