// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.ChangeNoteColorTypeCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Types;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class ChangeNoteColorTypeCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly ChangeNoteColorTypeSignal _signal;
    [Inject]
    private readonly BeatmapObjectsState _readonlyBeatmapObjectsState;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly SignalBus _signalBus;

    public void Execute()
    {
      this._readonlyBeatmapObjectsState.noteColorType = this._signal.colorType;
      this._signalBus.Fire<NoteColorTypeChangedSignal>();
      if (this._beatmapState.interactionMode != InteractionMode.Modify)
        return;
      this._signalBus.Fire<ChangeHoveredNoteColorTypeSignal>(new ChangeHoveredNoteColorTypeSignal(this._signal.colorType));
    }
  }
}
