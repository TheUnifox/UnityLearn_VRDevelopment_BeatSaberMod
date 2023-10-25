// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.UI.CurrentBpmRegionInfoView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.BpmEditor.Commands;
using BeatmapEditor3D.BpmEditor.Commands.Tools;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Views;
using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.BpmEditor.UI
{
  public class CurrentBpmRegionInfoView : BeatmapEditorView
  {
    [SerializeField]
    private FloatInputFieldValidator _bpmInput;
    [SerializeField]
    private FloatInputFieldValidator _beatsInput;
    [Inject]
    private readonly BpmEditorDataModel _bpmEditorDataModel;
    [Inject]
    private readonly BpmEditorState _bpmEditorState;
    [Inject]
    private readonly BpmEditorSongPreviewController _bpmEditorSongPreviewController;
    [Inject]
    private readonly SignalBus _signalBus;

    protected override void DidActivate()
    {
      this._signalBus.Subscribe<CurrentBpmRegionChangedSignal>(new Action(this.HandleCurrentBpmRegionChanged));
      this._signalBus.Subscribe<BpmRegionBeatsUpdatedSignal>(new Action(this.HandleBpmRegionBeatsUpdated));
      this._signalBus.Subscribe<CurrentRegionUpdatedBpmSignal>(new Action(this.HandleCurrentRegionUpdatedBpm));
      this._bpmInput.onInputValidated += new Action<float>(this.HandleBpmInputOnEndEdit);
      this._beatsInput.onInputValidated += new Action<float>(this.HandleBeatsInputOnEndEdit);
      this.SetData();
    }

    protected override void DidDeactivate()
    {
      this._signalBus.TryUnsubscribe<CurrentBpmRegionChangedSignal>(new Action(this.HandleCurrentBpmRegionChanged));
      this._signalBus.TryUnsubscribe<BpmRegionBeatsUpdatedSignal>(new Action(this.HandleBpmRegionBeatsUpdated));
      this._signalBus.TryUnsubscribe<CurrentRegionUpdatedBpmSignal>(new Action(this.HandleCurrentRegionUpdatedBpm));
      this._bpmInput.onInputValidated -= new Action<float>(this.HandleBpmInputOnEndEdit);
      this._beatsInput.onInputValidated -= new Action<float>(this.HandleBeatsInputOnEndEdit);
    }

    private void HandleCurrentBpmRegionChanged() => this.SetData();

    private void HandleBpmRegionBeatsUpdated() => this.SetData();

    private void HandleCurrentRegionUpdatedBpm() => this.SetData();

    private void HandleBpmInputOnEndEdit(float bpm)
    {
      this._signalBus.Fire<CurrentRegionUpdateBpmSignal>(new CurrentRegionUpdateBpmSignal(bpm));
      this._signalBus.Fire<EndCurrentRegionUpdateBpmSignal>();
    }

    private void HandleBeatsInputOnEndEdit(float beats)
    {
      this._signalBus.Fire<CurrentRegionUpdateBeatsSignal>(new CurrentRegionUpdateBeatsSignal(beats));
      this._signalBus.Fire<EndCurrentRegionUpdateBeatsSignal>();
    }

    private void SetData()
    {
      BpmRegion region = this._bpmEditorDataModel.regions[this._bpmEditorState.currentBpmRegionIdx];
      float beats = region.beats;
      this._bpmInput.value = AudioTimeHelper.SamplesToBPM(region.sampleCount, this._bpmEditorSongPreviewController.audioClip.frequency, beats);
      this._beatsInput.value = beats;
    }
  }
}
