// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.ZoomPreviewSelectionCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using Zenject;

namespace BeatmapEditor3D
{
  public class ZoomPreviewSelectionCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly ZoomPreviewSelectionSignal _signal;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;

    public void Execute()
    {
      this._beatmapState.previewData.ZoomSelection(this._signal.scrollDelta);
      this._beatmapState.previewData.CenterPreviewStart();
      this._signalBus.Fire<BeatmapLevelStatePreviewUpdated>();
    }
  }
}
