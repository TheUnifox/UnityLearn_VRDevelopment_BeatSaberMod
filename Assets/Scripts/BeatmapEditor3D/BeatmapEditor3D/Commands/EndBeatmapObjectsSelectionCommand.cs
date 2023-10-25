// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.EndBeatmapObjectsSelectionCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands.LevelEditor;
using BeatmapEditor3D.Types;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class EndBeatmapObjectsSelectionCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly EndBeatmapObjectsSelectionSignal _signal;
    [Inject]
    private readonly SignalBus _signalBus;

    public void Execute()
    {
      this._signalBus.Fire<BeatmapObjectsChangeRectangleSelectionSignal>(new BeatmapObjectsChangeRectangleSelectionSignal(this._signal.beat, BeatmapObjectsChangeRectangleSelectionSignal.ChangeType.End));
      this._signalBus.Fire<ChangeInteractionModeSignal>(new ChangeInteractionModeSignal(InteractionMode.Place));
      this._signalBus.Fire<SelectMultipleBeatmapObjectsSignal>(new SelectMultipleBeatmapObjectsSignal(this._signal.commit, true));
    }
  }
}
