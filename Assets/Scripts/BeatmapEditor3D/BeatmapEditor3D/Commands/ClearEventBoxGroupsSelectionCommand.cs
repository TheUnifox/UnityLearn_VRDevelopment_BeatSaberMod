// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.ClearEventBoxGroupsSelectionCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class ClearEventBoxGroupsSelectionCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly EventBoxGroupsSelectionState _selectionState;
    [Inject]
    private readonly SignalBus _signalBus;

    public void Execute()
    {
      this._selectionState.Clear();
      this._signalBus.Fire<EventBoxGroupsSelectionStateUpdatedSignal>();
    }
  }
}
