// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.AudioWaveformView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BeatmapEditor3D.Views
{
  [RequireComponent(typeof (RectTransform))]
  public abstract class AudioWaveformView : BeatmapEditorView
  {
    [SerializeField]
    private AudioWaveformController _audioWaveformController;
    [SerializeField]
    private RawImage _waveformImage;
    [SerializeField]
    private RectTransform _waveformImageRectTransform;
    [Inject]
    protected readonly SignalBus _signalBus;
    [Inject]
    private readonly BeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly WaveformDataModel _waveformDataModel;
    protected AudioClip _audioClip;
    private RenderTexture _waveformTexture;

    public void SetAudioClip(AudioClip audioClip) => this._audioClip = audioClip;

    protected override void WillActivate()
    {
      if ((UnityEngine.Object) this._waveformTexture == (UnityEngine.Object) null)
      {
        RenderTexture renderTexture = new RenderTexture(2048, 256, 0);
        renderTexture.filterMode = FilterMode.Bilinear;
        renderTexture.enableRandomWrite = true;
        this._waveformTexture = renderTexture;
        this._waveformTexture.Create();
        this._waveformImage.texture = (Texture) this._waveformTexture;
      }
      this._audioWaveformController.Setup(this._waveformTexture, this._waveformDataModel.computeBuffer, this._beatmapDataModel.bpmData.frequency);
    }

    protected override void DidActivate()
    {
      this._waveformImage.enabled = false;
      if (this._waveformDataModel.waveformDataCreated)
        this.SetupWaveform();
      else
        this._signalBus.Subscribe<BeatmapDataModelSignals.WaveformDataProcessedSignal>(new Action(this.HandleWaveformDataProcessed));
    }

    protected override void DidDeactivate() => this._signalBus.TryUnsubscribe<BeatmapDataModelSignals.WaveformDataProcessedSignal>(new Action(this.HandleWaveformDataProcessed));

    protected virtual void SetupWaveform() => this._waveformImage.enabled = true;

    private void HandleWaveformDataProcessed()
    {
      this._signalBus.TryUnsubscribe<BeatmapDataModelSignals.WaveformDataProcessedSignal>(new Action(this.HandleWaveformDataProcessed));
      this.SetupWaveform();
    }

    protected void RenderWaveform(int previewStartSample, int previewEndSample)
    {
      (int num1, int num2, int bracketSize) = WaveformProcessingUtils.GetWaveformSizing(this._waveformTexture.width, previewStartSample, previewEndSample);
      this._waveformImageRectTransform.anchorMax = new Vector2((float) this._waveformTexture.width / (float) (num2 - num1), 1f);
      this._audioWaveformController.RenderWaveform(num1, num2, bracketSize, previewEndSample - previewStartSample);
    }
  }
}
