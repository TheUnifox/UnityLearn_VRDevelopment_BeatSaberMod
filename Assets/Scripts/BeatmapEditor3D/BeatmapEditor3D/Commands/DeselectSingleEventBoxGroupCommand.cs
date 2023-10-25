// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.DeselectSingleEventBoxGroupCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class DeselectSingleEventBoxGroupCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly DeselectSingleEventBoxGroupSignal _signal;
    [Inject]
    private readonly EventBoxGroupsSelectionState _selectionState;
    [Inject]
    private readonly SignalBus _signalBus;

    public void Execute()
    {
      this._selectionState.Remove(this._signal.eventBoxGroupEditorData.id);
      this._signalBus.Fire<EventBoxGroupsSelectionStateUpdatedSignal>();
    }
  }
}
