// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.ToggleAutoExposureCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using Zenject;

namespace BeatmapEditor3D
{
  public class ToggleAutoExposureCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly ToggleAutoExposureSignal _signal;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly BeatmapEditorSettingsDataModel _beatmapEditorSettingsDataModel;

    public void Execute()
    {
      this._beatmapEditorSettingsDataModel.SetAutoExposure(this._signal.autoExposureIsOn);
      this._signalBus.Fire<LevelEditorStateAutoExposureUpdatedSignal>(new LevelEditorStateAutoExposureUpdatedSignal(this._beatmapEditorSettingsDataModel.autoExposure));
    }
  }
}
