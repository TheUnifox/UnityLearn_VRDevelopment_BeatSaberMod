// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.ChangeDotNoteAngleCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class ChangeDotNoteAngleCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly ChangeDotNoteAngleSignal _signal;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly BeatmapObjectsState _readonlyBeatmapObjectsState;

    public void Execute()
    {
      this._readonlyBeatmapObjectsState.noteAngle = this._signal.angle != -1 ? this._signal.angle : (this._readonlyBeatmapObjectsState.noteAngle == 0 ? 45 : 0);
      this._signalBus.Fire<DotNoteAngleChangedSignal>(new DotNoteAngleChangedSignal(this._readonlyBeatmapObjectsState.noteAngle));
    }
  }
}
