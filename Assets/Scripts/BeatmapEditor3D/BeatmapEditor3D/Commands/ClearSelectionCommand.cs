// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.ClearSelectionCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class ClearSelectionCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly BeatmapObjectsSelectionState _beatmapObjectsSelectionState;
    [Inject]
    private readonly EventsSelectionState _eventsSelectionState;

    public void Execute()
    {
      this._beatmapObjectsSelectionState.Clear();
      this._eventsSelectionState.Clear();
      this._signalBus.Fire<BeatmapObjectsSelectionStateUpdatedSignal>();
      this._signalBus.Fire<EventsSelectionStateUpdatedSignal>();
    }
  }
}
