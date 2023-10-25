// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.IBeatmapObjectGridFsmState
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands.LevelEditor;
using BeatmapEditor3D.LevelEditor;

namespace BeatmapEditor3D
{
  public interface IBeatmapObjectGridFsmState
  {
    void Enter();

    void Exit();

    IBeatmapObjectGridFsmState Update();

    void HandleMouseScroll(float delta);

    void HandleMousePointerDown();

    void HandleMousePointerUp();

    void HandleCancelAction(bool pressed);

    void HandleGridCellPointerDown(int column, int row);

    void HandleGridCellPointerUp(int column, int row);

    void HandleGridCellPointerEnter(int column, int row);

    void HandleGridCellPointerExit(int column, int row);

    void HandleBeatmapObjectTypeChanged(
      BeatmapObjectTypeChangedSignal beatmapObjectTypeChangedSignal);

    void HandleBeatmapObjectModeChanged(
      InteractionModeChangedSignal interactionModeChangedSignal);

    void HandleLevelEditorStateModeSwitched(
      BeatmapEditingModeSwitched beatmapEditingStateModeSwitched);

    void HandleBeatmapLevelStateTimeUpdated();

    void HandleBeatmapTimeScaleChanged();

    void HandleLevelEditorZenModeUpdated(
      LevelEditorStateZenModeUpdatedSignal levelEditorStateZenModeUpdatedSignal);

    void HandleObstacleDurationChanged();
  }
}
