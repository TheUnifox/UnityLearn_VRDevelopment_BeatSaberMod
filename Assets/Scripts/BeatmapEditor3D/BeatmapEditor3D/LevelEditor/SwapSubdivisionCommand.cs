// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.LevelEditor.SwapSubdivisionCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Types;
using Zenject;

namespace BeatmapEditor3D.LevelEditor
{
  public class SwapSubdivisionCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;

    public void Execute()
    {
      if (this._beatmapState.interactionMode == InteractionMode.Modify)
        return;
      this._beatmapState.beatSubdivisionsModel.Swap();
      this._signalBus.Fire<SubdivisionChangedSignal>();
      this._signalBus.Fire<SubdivisionSwappedSignal>();
    }
  }
}
