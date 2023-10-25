// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.BpmEditorMetronomeController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.BpmEditor.Commands;
using BeatmapEditor3D.DataModels;
using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.BpmEditor
{
  public class BpmEditorMetronomeController : MonoBehaviour
  {
    [SerializeField]
    private AudioSource _metronomeAudioSource;
    [SerializeField]
    private AudioClip _metronomeTickAudioClip;
    [Inject]
    private readonly BpmEditorSongPreviewController _bpmEditorSongPreviewController;
    [Inject]
    private readonly BpmEditorDataModel _bpmEditorDataModel;
    [Inject]
    private readonly BpmEditorState _bpmEditorState;
    [Inject]
    private readonly SignalBus _signalBus;
    private int _prevBeat = -1;

    public void Start() => this._signalBus.Subscribe<PlayHeadUpdatedSignal>(new Action(this.HandlePlayHeadUpdated));

    public void OnDestroy() => this._signalBus.TryUnsubscribe<PlayHeadUpdatedSignal>(new Action(this.HandlePlayHeadUpdated));

    private void HandlePlayHeadUpdated()
    {
      int sample = this._bpmEditorState.sample;
      BpmRegion region = this._bpmEditorDataModel.regions[this._bpmEditorState.currentBpmRegionIdx];
      int num = Mathf.FloorToInt(region.startBeat + (float) (sample - region.startSampleIndex) / (float) region.samplesPerBeat);
      if ((double) Mathf.Abs(num - this._prevBeat) < 1.0)
        return;
      this._prevBeat = num;
      if (!this._bpmEditorSongPreviewController.isPlaying)
        return;
      this._metronomeAudioSource.PlayOneShot(this._metronomeTickAudioClip);
    }
  }
}
