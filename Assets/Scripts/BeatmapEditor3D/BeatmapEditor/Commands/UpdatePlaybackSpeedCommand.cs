// Decompiled with JetBrains decompiler
// Type: BeatmapEditor.Commands.UpdatePlaybackSpeedCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D;
using BeatmapEditor3D.Controller;
using BeatmapEditor3D.DataModels;
using Zenject;

namespace BeatmapEditor.Commands
{
  public class UpdatePlaybackSpeedCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly UpdatePlaybackSpeedSignal _signal;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly BeatmapEditorSettingsDataModel _beatmapEditorSettingsDataModel;
    [Inject]
    private readonly ISongPreviewController _songPreviewController;

    public void Execute()
    {
      this._beatmapEditorSettingsDataModel.SetPlaybackSpeed(this._signal.playbackSpeed);
      this._songPreviewController.SetPlaybackSpeed(this._beatmapEditorSettingsDataModel.playbackSpeed);
      this._signalBus.Fire<PlaybackSpeedUpdatedSignal>();
    }
  }
}
