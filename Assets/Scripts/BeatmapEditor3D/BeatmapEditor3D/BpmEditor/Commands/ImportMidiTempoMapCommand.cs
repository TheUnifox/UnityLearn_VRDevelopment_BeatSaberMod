// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.Commands.ImportMidiTempoMapCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using MidiParser;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.BpmEditor.Commands
{
  public class ImportMidiTempoMapCommand : IBeatmapEditorCommandWithHistory, IBeatmapEditorCommand
  {
    [Inject]
    private readonly ImportMidiTempoMapSignal _signal;
    [Inject]
    private readonly BpmEditorDataModel _bpmEditorDataModel;
    [Inject]
    private readonly BpmEditorSongPreviewController _songPreviewController;
    [Inject]
    private readonly SignalBus _signalBus;
    private const float kMicroSecondsInSecond = 1000000f;
    private List<BpmRegion> _originalBpmRegions;
    private List<BpmRegion> _newBpmRegions;

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      int frequency = this._songPreviewController.audioClip.frequency;
      MidiFile midiFile = new MidiFile(this._signal.midiFilePath);
      List<MidiEvent> midiEventList = new List<MidiEvent>();
      foreach (MidiTrack track in midiFile.tracks)
        midiEventList.AddRange((IEnumerable<MidiEvent>) track.MidiEvents);
      if (midiEventList.Count == 0)
        return;
      this._originalBpmRegions = new List<BpmRegion>((IEnumerable<BpmRegion>) this._bpmEditorDataModel.regions);
      this._newBpmRegions = new List<BpmRegion>(midiEventList.Count);
      int ticksPerQuarterNote = midiFile.ticksPerQuarterNote;
      int num1 = 0;
      int bpm = midiEventList[0].Arg2;
      int num2 = midiEventList[0].Arg3;
      float startBeat = 0.0f;
      int startSampleIndex = 0;
      for (int index = 1; index < midiEventList.Count; ++index)
      {
        float num3 = (float) (midiEventList[index].AbsoluteTicksTime - num1) / (float) ticksPerQuarterNote;
        int num4 = Mathf.RoundToInt((float) ((double) num3 * (double) num2 / 1000000.0) * (float) frequency);
        this._newBpmRegions.Add(new BpmRegion(startSampleIndex, startSampleIndex + num4, startBeat, startBeat + num3, frequency));
        startBeat += num3;
        startSampleIndex += num4 + 1;
        bpm = midiEventList[index].Arg2;
        num2 = midiEventList[index].Arg3;
        num1 = midiEventList[index].AbsoluteTicksTime;
      }
      int samplesCount = this._songPreviewController.samplesCount;
      float beats = AudioTimeHelper.SamplesToBeats(samplesCount - startSampleIndex, frequency, (float) bpm);
      this._newBpmRegions.Add(new BpmRegion(startSampleIndex, samplesCount, startBeat, startBeat + beats, frequency));
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      this._bpmEditorDataModel.ReplaceAllRegions(this._originalBpmRegions);
      this._signalBus.Fire<BpmRegionsChangedSignal>();
    }

    public void Redo()
    {
      this._bpmEditorDataModel.ReplaceAllRegions(this._newBpmRegions);
      this._signalBus.Fire<BpmRegionsChangedSignal>();
    }
  }
}
