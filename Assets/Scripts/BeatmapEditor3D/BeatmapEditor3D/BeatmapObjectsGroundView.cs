// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapObjectsGroundView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using BeatmapEditor3D.Types;
using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapObjectsGroundView : MonoBehaviour
  {
    [Inject]
    private readonly BeatmapEditorGroundView _beatmapEditorGroundView;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly BeatmapObjectPlacementHelper _beatmapObjectPlacementHelper;
    private Vector2 _lastPosition;

    public event Action<float> mouseStartDragEvent;

    public event Action<float> mouseDragEvent;

    public event Action<float> mouseEndDragEvent;

    public event Action<float> mouseMoveEvent;

    protected void Start()
    {
      this._signalBus.Subscribe<BeatmapEditingModeSwitched>(new Action(this.HandleLevelEditorModeSwitched));
      this.UnsubscribeToGroundViewEvents();
      if (this._beatmapState.editingMode != BeatmapEditingMode.Objects)
        return;
      this.SubscribeToGroundViewEvents();
      this.UpdateGroundScale();
    }

    protected void OnDestroy()
    {
      this.UnsubscribeToGroundViewEvents();
      this._signalBus.TryUnsubscribe<BeatmapEditingModeSwitched>(new Action(this.HandleLevelEditorModeSwitched));
    }

    private void HandleLevelEditorModeSwitched()
    {
      this.UnsubscribeToGroundViewEvents();
      if (this._beatmapState.editingMode != BeatmapEditingMode.Objects)
        return;
      this.SubscribeToGroundViewEvents();
      this.UpdateGroundScale();
    }

    private void HandleBeatmapEditorGroundViewMouseStartDrag(Vector2 position)
    {
      this._lastPosition = position;
      float beatFromPosition = this.GetBeatFromPosition(position);
      Action<float> mouseStartDragEvent = this.mouseStartDragEvent;
      if (mouseStartDragEvent == null)
        return;
      mouseStartDragEvent(beatFromPosition);
    }

    private void HandleBeatmapEditorGroundViewMouseDrag(Vector2 position)
    {
      this._lastPosition = position;
      float beatFromPosition = this.GetBeatFromPosition(position);
      Action<float> mouseDragEvent = this.mouseDragEvent;
      if (mouseDragEvent == null)
        return;
      mouseDragEvent(beatFromPosition);
    }

    private void HandleBeatmapEditorGroundViewMouseEndDrag(Vector2 position)
    {
      this._lastPosition = position;
      float beatFromPosition = this.GetBeatFromPosition(position);
      Action<float> mouseEndDragEvent = this.mouseEndDragEvent;
      if (mouseEndDragEvent == null)
        return;
      mouseEndDragEvent(beatFromPosition);
    }

    private void HandleBeatmapEditorGroundViewMouseMove(Vector2 position)
    {
      this._lastPosition = position;
      float beatFromPosition = this.GetBeatFromPosition(position);
      Action<float> mouseMoveEvent = this.mouseMoveEvent;
      if (mouseMoveEvent == null)
        return;
      mouseMoveEvent(beatFromPosition);
    }

    private float GetBeatFromPosition(Vector2 position) => this._beatmapObjectPlacementHelper.PositionToBeat(position.y);

    private void UpdateGroundScale() => this._beatmapEditorGroundView.UpdateGroundScale(TrackPlacementHelper.TrackCountToScale(4) + 1f);

    private void SubscribeToGroundViewEvents()
    {
      this._beatmapEditorGroundView.mouseStartDragEvent += new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseStartDrag);
      this._beatmapEditorGroundView.mouseDragEvent += new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseDrag);
      this._beatmapEditorGroundView.mouseEndDragEvent += new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseEndDrag);
      this._beatmapEditorGroundView.mouseMoveEvent += new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseMove);
    }

    private void UnsubscribeToGroundViewEvents()
    {
      this._beatmapEditorGroundView.mouseStartDragEvent -= new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseStartDrag);
      this._beatmapEditorGroundView.mouseDragEvent -= new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseDrag);
      this._beatmapEditorGroundView.mouseEndDragEvent -= new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseEndDrag);
      this._beatmapEditorGroundView.mouseMoveEvent -= new Action<Vector2>(this.HandleBeatmapEditorGroundViewMouseMove);
    }
  }
}
