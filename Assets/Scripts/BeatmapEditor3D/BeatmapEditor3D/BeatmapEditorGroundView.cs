// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapEditorGroundView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Types;
using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapEditorGroundView : MonoBehaviour
  {
    [SerializeField]
    private float _initialDragDelta = 0.5f;
    [Space]
    [SerializeField]
    private Transform _groundTransform;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly SignalBus _signalBus;
    private Plane _groundPlane = new Plane(Vector3.up, -0.2f);
    private Bounds _bounds;
    private bool _mouseOver;
    private bool _isDragging;
    private bool _wasDraggingThisFrame;
    private Vector3 _mouseButtonDownPosition;
    private Vector2 _prevPosition;
    private float _squaredDragDelta;

    public event Action mouseEnterEvent;

    public event Action mouseExitEvent;

    public event Action<Vector2> mouseMoveEvent;

    public event Action<Vector2> mouseDownEvent;

    public event Action<Vector2> mouseUpEvent;

    public event Action<Vector2> mouseStartDragEvent;

    public event Action<Vector2> mouseDragEvent;

    public event Action<Vector2> mouseEndDragEvent;

    public bool isDragging => this._isDragging;

    public bool wasDraggingThisFrame => this._wasDraggingThisFrame;

    public void UpdateGroundScale(float width)
    {
      this._bounds = new Bounds(new Vector3(0.0f, 0.0f, 15f), new Vector3(width, 2f, 40f));
      this._groundTransform.localScale = this._groundTransform.localScale with
      {
        x = width
      };
    }

    protected void Start() => this._squaredDragDelta = this._initialDragDelta * this._initialDragDelta;

    protected void OnEnable() => this._signalBus.Subscribe<BeatmapLevelStateTimeUpdated>(new Action(this.HandleBeatmapLevelStateTimeUpdated));

    protected void OnDisable() => this._signalBus.TryUnsubscribe<BeatmapLevelStateTimeUpdated>(new Action(this.HandleBeatmapLevelStateTimeUpdated));

    private void HandleBeatmapLevelStateTimeUpdated() => this._prevPosition = Vector2.negativeInfinity;

    protected void Update()
    {
      this._wasDraggingThisFrame = this._isDragging;
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      float enter;
      if (!this._groundPlane.Raycast(ray, out enter))
        return;
      Vector3 point = ray.GetPoint(enter);
      int num = this._mouseOver ? 1 : 0;
      this._mouseOver = this._bounds.Contains(point);
      if (num == 0)
      {
        if (this._mouseOver)
        {
          Action mouseEnterEvent = this.mouseEnterEvent;
          if (mouseEnterEvent != null)
            mouseEnterEvent();
        }
      }
      else if (!this._mouseOver)
      {
        Action mouseExitEvent = this.mouseExitEvent;
        if (mouseExitEvent != null)
          mouseExitEvent();
      }
      Vector2 vector2 = new Vector2(point.x, point.z);
      if (this._mouseOver)
      {
        if (Input.GetMouseButtonDown(0))
        {
          Action<Vector2> mouseDownEvent = this.mouseDownEvent;
          if (mouseDownEvent != null)
            mouseDownEvent(vector2);
        }
        if (Input.GetMouseButtonUp(0))
        {
          Action<Vector2> mouseUpEvent = this.mouseUpEvent;
          if (mouseUpEvent != null)
            mouseUpEvent(vector2);
        }
      }
      if (this._mouseOver && (!Mathf.Approximately(this._prevPosition.x, vector2.x) || !Mathf.Approximately(this._prevPosition.y, vector2.y)))
      {
        Action<Vector2> mouseMoveEvent = this.mouseMoveEvent;
        if (mouseMoveEvent != null)
          mouseMoveEvent(vector2);
      }
      if (Input.GetMouseButtonDown(0))
        this._mouseButtonDownPosition = Input.mousePosition;
      bool flag1 = Input.GetMouseButton(0) && this._beatmapState.interactionMode == InteractionMode.Select;
      bool flag2 = (double) (this._mouseButtonDownPosition - Input.mousePosition).sqrMagnitude > (double) this._squaredDragDelta;
      if (((!this._mouseOver ? 0 : (!this._isDragging ? 1 : 0)) & (flag1 ? 1 : 0) & (flag2 ? 1 : 0)) != 0)
      {
        Action<Vector2> mouseStartDragEvent = this.mouseStartDragEvent;
        if (mouseStartDragEvent != null)
          mouseStartDragEvent(vector2);
        this._isDragging = true;
      }
      if (this._isDragging && !flag1)
      {
        Action<Vector2> mouseEndDragEvent = this.mouseEndDragEvent;
        if (mouseEndDragEvent != null)
          mouseEndDragEvent(vector2);
        this._isDragging = false;
      }
      if (this._isDragging)
      {
        Action<Vector2> mouseDragEvent = this.mouseDragEvent;
        if (mouseDragEvent != null)
          mouseDragEvent(vector2);
      }
      this._prevPosition = vector2;
    }
  }
}
