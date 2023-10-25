// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.InteractiveAudioWaveformView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System;
using Zenject;

namespace BeatmapEditor3D.Views
{
  public class InteractiveAudioWaveformView : AudioWaveformView
  {
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    private bool _updateIsEnabled = true;

    public void SetAutomaticUpdate(bool isEnabled) => this._updateIsEnabled = isEnabled;

    protected override void DidActivate()
    {
      base.DidActivate();
      this._updateIsEnabled = true;
      this._signalBus.Subscribe<BeatmapLevelStatePreviewUpdated>(new Action(this.HandleBeatmapLevelStatePreviewUpdated));
    }

    protected override void DidDeactivate()
    {
      base.DidDeactivate();
      this._signalBus.TryUnsubscribe<BeatmapLevelStatePreviewUpdated>(new Action(this.HandleBeatmapLevelStatePreviewUpdated));
    }

    private void HandleBeatmapLevelStatePreviewUpdated()
    {
      if (!this._updateIsEnabled)
        return;
      this.RenderWaveform(this._beatmapState.previewData.start, this._beatmapState.previewData.end);
    }
  }
}
