// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.ChangeSliderSliceCountCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class ChangeSliderSliceCountCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly ChangeSliderSliceCountSignal _signal;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly BeatmapObjectsState _beatmapObjectsState;

    public void Execute()
    {
      this._beatmapObjectsState.sliderSliceCount = this._signal.sliceCount;
      this._signalBus.Fire<SliderSliceCountChangedSignal>(new SliderSliceCountChangedSignal(this._beatmapObjectsState.sliderSliceCount));
    }
  }
}
