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
            int sampleIndex = Mathf.Max(0, previewStartSample);
            int sampleIndex2 = Mathf.Min(previewEndSample, this._beatmapDataModel.audioClip.samples);
            int num = Mathf.CeilToInt(this._beatmapDataModel.bpmData.SampleToBeat(sampleIndex));
            int num2 = Mathf.FloorToInt(this._beatmapDataModel.bpmData.SampleToBeat(sampleIndex2));
            int num3 = 0;
            for (int i = num; i <= num2; i += this._beatsIncrement)
            {
                int sample = this._beatmapDataModel.bpmData.BeatToSample((float)i);
                if (num3 >= this._beatMarkers.Count)
                {
                    BeatMarkerView item = UnityEngine.Object.Instantiate<BeatMarkerView>(this._beatMarkerViewPrefab, this._containerTransform, false);
                    this._beatMarkers.Add(item);
                }
                this._beatMarkers[num3].SetData(i);
                if (!this._beatMarkers[num3].gameObject.activeSelf)
                {
                    this._beatMarkers[num3].gameObject.SetActive(true);
                }
                Vector2 anchoredPosition = this._beatMarkers[num3].rectTransform.anchoredPosition;
                anchoredPosition.x = WaveformPlacementHelper.CalculateRegionPosition(this._containerTransform, sample, previewStartSample, previewEndSample);
                this._beatMarkers[num3].rectTransform.anchoredPosition = anchoredPosition;
                num3++;
            }
            for (int j = num3; j < this._beatMarkers.Count; j++)
            {
                this._beatMarkers[j].gameObject.SetActive(false);
            }
        }
    }
}
