// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.ObstacleObjectsMouseInputController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.InputSignals;
using BeatmapEditor3D.LevelEditor;
using BeatmapEditor3D.Types;
using BeatmapEditor3D.Views;
using BeatmapEditor3D.Visuals;
using System;
using UnityEngine;

namespace BeatmapEditor3D
{
  public class ObstacleObjectsMouseInputController : BeatmapObjectsMouseInputController
  {
    [SerializeField]
    private ObstacleObjectsMouseInputSource _obstacleObjectsMouseInputSource;
    [SerializeField]
    private ObstacleBeatmapObjectView _obstacleBeatmapObjectView;

    protected override void OnEnable()
    {
      base.OnEnable();
      this._obstacleObjectsMouseInputSource.objectPointerDownEvent += new Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)>(HandleMouseInputEventSourceObjectPointerDown);
      this._obstacleObjectsMouseInputSource.objectPointerUpEvent += new Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)>(HandleMouseInputEventSourceObjectPointerUp);
      this._obstacleObjectsMouseInputSource.objectPointerHoverEvent += new Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)>(HandleMouseInputEventSourceObjectPointerHover);
    }

    protected override void OnDisable()
    {
      base.OnDisable();
      this._obstacleObjectsMouseInputSource.objectPointerDownEvent -= new Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)>(HandleMouseInputEventSourceObjectPointerDown);
      this._obstacleObjectsMouseInputSource.objectPointerUpEvent -= new Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)>(HandleMouseInputEventSourceObjectPointerUp);
      this._obstacleObjectsMouseInputSource.objectPointerHoverEvent -= new Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)>(HandleMouseInputEventSourceObjectPointerHover);
    }

    protected override void HandleMouseInputEventSourceObjectPointerHover(
      MouseInputType mouseInputType,
      (BeatmapEditorObjectId id, BeatmapObjectCellData cellData) payload)
    {
      base.HandleMouseInputEventSourceObjectPointerHover(mouseInputType, payload);
      ObstacleView obstacleView;
      if (!this._obstacleBeatmapObjectView.obstacleObjects.TryGetValue(payload.id, out obstacleView))
        return;
      obstacleView.SetHighlight(mouseInputType == MouseInputType.Enter);
    }

    protected override void HandleClickOnBeatmapObject(
      (BeatmapEditorObjectId id, BeatmapObjectCellData cellData) payload)
    {
      base.HandleClickOnBeatmapObject(payload);
      if (this.beatmapState.interactionMode != InteractionMode.Delete)
        return;
      this.signalBus.Fire<DeleteObstacleSignal>(new DeleteObstacleSignal(payload.id));
    }

    protected override void HandleDeleteBeatmapObject(
      (BeatmapEditorObjectId id, BeatmapObjectCellData cellData) payload)
    {
      this.signalBus.Fire<DeleteObstacleSignal>(new DeleteObstacleSignal(payload.id));
    }

    protected override void HandleMoveBeatmapObjectToBeatLine(
      (BeatmapEditorObjectId id, BeatmapObjectCellData cellData) payload)
    {
      this.signalBus.Fire<MoveObstacleToBeatLineSignal>(new MoveObstacleToBeatLineSignal(payload.id, payload.cellData));
    }

    protected override void SelectBeatmapObject(BeatmapEditorObjectId id) => this.signalBus.Fire<SelectObstacleSignal>(new SelectObstacleSignal(id));
  }
}
