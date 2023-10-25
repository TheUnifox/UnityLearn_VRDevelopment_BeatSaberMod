// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DeselectAllBeatmapObjectsCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using Zenject;

namespace BeatmapEditor3D
{
  public class DeselectAllBeatmapObjectsCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly BeatmapObjectsSelectionState _beatmapObjectsSelectionState;

    public void Execute()
    {
      this._beatmapObjectsSelectionState.Clear();
      this._signalBus.Fire<BeatmapObjectsSelectionStateUpdatedSignal>();
    }
  }
}
