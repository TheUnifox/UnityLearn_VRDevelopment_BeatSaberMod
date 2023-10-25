// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapObjectGridFsmState
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands.LevelEditor;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using BeatmapEditor3D.Views;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapObjectGridFsmState : IBeatmapObjectGridFsmState
  {
    [Inject]
    protected readonly IReadonlyBeatmapObjectsSelectionState beatmapObjectsSelectionState;
    [Inject]
    protected readonly IReadonlyBeatmapState beatmapState;
    [Inject]
    protected readonly BeatmapLevelDataModel beatmapLevelDataModel;
    [Inject]
    protected readonly BeatmapObjectsState beatmapObjectsState;
    [Inject]
    protected readonly SignalBus signalBus;
    [Inject]
    protected readonly BeatmapObjectGridFsmStateHidden.Factory beatmapObjectGridFsmStateHiddenFactory;
    [Inject]
    protected readonly BeatmapObjectEditGridView beatmapObjectEditGridView;
    [Inject]
    protected readonly BeatmapObjectGridHoverView hoverView;
    [Inject]
    protected readonly BeatmapObjectGridFsmSharedData _beatmapObjectGridFsmSharedData;
    protected IBeatmapObjectGridFsmState nextState;

    public virtual void Enter()
    {
      if (!this._beatmapObjectGridFsmSharedData.pointerLastGridPosition.HasValue)
        return;
      Vector2Int vector2Int = this._beatmapObjectGridFsmSharedData.pointerLastGridPosition.Value;
      this.HandleGridCellPointerEnter(vector2Int.x, vector2Int.y);
    }

    public virtual void Exit()
    {
    }

    public virtual IBeatmapObjectGridFsmState Update() => this.nextState;

    public virtual void HandleMouseScroll(float delta)
    {
    }

    public virtual void HandleMousePointerDown()
    {
    }

    public virtual void HandleMousePointerUp()
    {
    }

    public virtual void HandleCancelAction(bool pressed)
    {
    }

    public virtual void HandleGridCellPointerDown(int column, int row)
    {
    }

    public virtual void HandleGridCellPointerUp(int column, int row)
    {
    }

    public virtual void HandleGridCellPointerEnter(int column, int row)
    {
    }

    public virtual void HandleGridCellPointerExit(int column, int row)
    {
    }

    public virtual void HandleBeatmapObjectTypeChanged(
      BeatmapObjectTypeChangedSignal beatmapObjectTypeChangedSignal)
    {
    }

    public virtual void HandleBeatmapObjectModeChanged(
      InteractionModeChangedSignal interactionModeChangedSignal)
    {
    }

    public virtual void HandleLevelEditorStateModeSwitched(
      BeatmapEditingModeSwitched beatmapEditingStateModeSwitched)
    {
    }

    public virtual void HandleBeatmapLevelStateTimeUpdated()
    {
    }

    public virtual void HandleBeatmapTimeScaleChanged()
    {
    }

    public virtual void HandleLevelEditorZenModeUpdated(
      LevelEditorStateZenModeUpdatedSignal levelEditorStateZenModeUpdatedSignal)
    {
    }

    public virtual void HandleObstacleDurationChanged()
    {
    }
  }
}
