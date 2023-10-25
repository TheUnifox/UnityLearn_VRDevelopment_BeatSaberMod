// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapObjectGridFsm
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands.LevelEditor;
using BeatmapEditor3D.LevelEditor;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapObjectGridFsm
  {
    [Inject]
    private readonly BeatmapObjectGridFsmSharedData _beatmapObjectGridFsmSharedData;
    private IBeatmapObjectGridFsmState _currentState;

    public IBeatmapObjectGridFsmState currentState => this._currentState;

    public void SwitchState(IBeatmapObjectGridFsmState state)
    {
      this._currentState?.Exit();
      this._currentState = state;
      this._currentState.Enter();
    }

    public void Enter()
    {
    }

    public void Exit()
    {
    }

    public void Update()
    {
      IBeatmapObjectGridFsmState state = this._currentState?.Update();
      if (state == null)
        return;
      this.SwitchState(state);
    }

    public void HandleMouseScroll(float delta) => this._currentState?.HandleMouseScroll(delta);

    public void HandleMousePointerDown() => this._currentState?.HandleMousePointerDown();

    public void HandleMousePointerUp() => this._currentState?.HandleMousePointerUp();

    public void HandleCancelAction(bool pressed) => this._currentState?.HandleCancelAction(pressed);

    public void HandleGridCellPointerDown(int column, int row) => this._currentState?.HandleGridCellPointerDown(column, row);

    public void HandleGridCellPointerUp(int column, int row) => this._currentState?.HandleGridCellPointerUp(column, row);

    public void HandleGridCellPointerEnter(int column, int row)
    {
      this._beatmapObjectGridFsmSharedData.pointerLastGridPosition = new Vector2Int?(new Vector2Int(column, row));
      this._currentState?.HandleGridCellPointerEnter(column, row);
    }

    public void HandleGridCellPointerExit(int column, int row)
    {
      this._beatmapObjectGridFsmSharedData.pointerLastGridPosition = new Vector2Int?();
      this._currentState?.HandleGridCellPointerExit(column, row);
    }

    public void HandleBeatmapObjectTypeChanged(
      BeatmapObjectTypeChangedSignal beatmapObjectTypeChangedSignal)
    {
      this._currentState?.HandleBeatmapObjectTypeChanged(beatmapObjectTypeChangedSignal);
    }

    public void HandleBeatmapObjectModeChanged(
      InteractionModeChangedSignal interactionModeChangedSignal)
    {
      this._currentState?.HandleBeatmapObjectModeChanged(interactionModeChangedSignal);
    }

    public void HandleLevelEditorModeSwitched(
      BeatmapEditingModeSwitched beatmapEditingStateModeSwitched)
    {
      this._currentState?.HandleLevelEditorStateModeSwitched(beatmapEditingStateModeSwitched);
    }

    public void HandleBeatmapLevelStateTimeUpdated() => this._currentState?.HandleBeatmapLevelStateTimeUpdated();

    public void HandleBeatmapTimeScaleChanged() => this._currentState?.HandleBeatmapTimeScaleChanged();

    public void HandleLevelEditorStateZenModeUpdated(
      LevelEditorStateZenModeUpdatedSignal levelEditorStateZenModeUpdatedSignal)
    {
      this._currentState?.HandleLevelEditorZenModeUpdated(levelEditorStateZenModeUpdatedSignal);
    }

    public void HandleObstacleDurationChanged() => this._currentState?.HandleObstacleDurationChanged();
  }
}
