// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.AudioWaveformBeatsView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.BpmEditor.UI;
using BeatmapEditor3D.DataModels;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Views
{
  public abstract class AudioWaveformBeatsView : BeatmapEditorView
  {
    [SerializeField]
    private RectTransform _containerTransform;
    [SerializeField]
    private int _beatsIncrement;
    [SerializeField]
    private BeatMarkerView _beatMarkerViewPrefab;
    [Inject]
    protected readonly BeatmapDataModel _beatmapDataModel;
    private readonly List<BeatMarkerView> _beatMarkers = new List<BeatMarkerView>();

    protected override void DidDeactivate()
    {
      foreach (Component beatMarker in this._beatMarkers)
        beatMarker.gameObject.SetActive(false);
    }

    protected void DisplayBeats(int previewStartSample, int previewEndSample)
    {
      int sampleIndex1 = Mathf.Max(0, previewStartSample);
      int sampleIndex2 = Mathf.Min(previewEndSample, this._beatmapDataModel.audioClip.samples);
      int num1 = Mathf.CeilToInt(this._beatmapDataModel.bpmData.SampleToBeat(sampleIndex1));
      int num2 = Mathf.FloorToInt(this._beatmapDataModel.bpmData.SampleToBeat(sampleIndex2));
      int index1 = 0;
      for (int beat = num1; beat <= num2; beat += this._beatsIncrement)
      {
        int sample = this._beatmapDataModel.bpmData.BeatToSample((float) beat);
        if (index1 >= this._beatMarkers.Count)
          this._beatMarkers.Add(Object.Instantiate<BeatMarkerView>(this._beatMarkerViewPrefab, (Transform) this._containerTransform, false));
        this._beatMarkers[index1].SetData(beat);
        if (!this._beatMarkers[index1].gameObject.activeSelf)
          this._beatMarkers[index1].gameObject.SetActive(true);
        Vector2 anchoredPosition = this._beatMarkers[index1].rectTransform.anchoredPosition with
        {
          x = WaveformPlacementHelper.CalculateRegionPosition(this._containerTransform, sample, previewStartSample, previewEndSample)
        };
        this._beatMarkers[index1].rectTransform.anchoredPosition = anchoredPosition;
        ++index1;
      }
      for (int index2 = index1; index2 < this._beatMarkers.Count; ++index2)
        this._beatMarkers[index2].gameObject.SetActive(false);
    }
  }
}
