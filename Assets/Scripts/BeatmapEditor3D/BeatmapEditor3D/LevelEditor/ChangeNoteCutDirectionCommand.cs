// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.LevelEditor.ChangeNoteCutDirectionCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Types;
using Zenject;

namespace BeatmapEditor3D.LevelEditor
{
  public class ChangeNoteCutDirectionCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly BeatmapObjectsState _readonlyBeatmapObjectsState;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly ChangeNoteCutDirectionSignal _signal;
    private NoteEditorData _changedNote;

    public void Execute()
    {
      this._readonlyBeatmapObjectsState.noteAngle = 0;
      this._signalBus.Fire<DotNoteAngleChangedSignal>(new DotNoteAngleChangedSignal(this._readonlyBeatmapObjectsState.noteAngle));
      this._readonlyBeatmapObjectsState.noteCutDirection = this._signal.noteCutDirection;
      this._signalBus.Fire<NoteCutDirectionChangedSignal>(new NoteCutDirectionChangedSignal(this._readonlyBeatmapObjectsState.noteCutDirection));
      if (this._beatmapState.interactionMode != InteractionMode.Modify)
        return;
      this._signalBus.Fire<ChangeHoveredNoteCutDirectionSignal>(new ChangeHoveredNoteCutDirectionSignal(this._signal.noteCutDirection));
    }
  }
}
