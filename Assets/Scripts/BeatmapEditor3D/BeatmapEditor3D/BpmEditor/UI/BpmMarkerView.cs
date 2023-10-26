// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.UI.BpmMarkerView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Views;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.BpmEditor.UI
{
  public abstract class BpmMarkerView : BeatmapEditorView
  {
    [SerializeField]
    [NullAllowed(NullAllowed.Context.Prefab)]
    private RectTransform _containerTransform;
    [SerializeField]
    private RectTransform _markerTransform;
    [Inject]
    protected readonly BpmEditorState _bpmEditorState;
    [Inject]
    protected readonly SignalBus _signalBus;
    private int _startSample;
    private int _endSample;

    public void Setup(int startSample, int endSample, int currentSample)
    {
      this._startSample = startSample;
      this._endSample = endSample;
      this.SetPosition(currentSample);
    }

        protected void SetPosition(int sample)
        {
            Vector2 anchoredPosition = this._markerTransform.anchoredPosition;
            anchoredPosition.x = WaveformPlacementHelper.CalculateRegionPosition(this._containerTransform, sample, this._startSample, this._endSample);
            this._markerTransform.anchoredPosition = anchoredPosition;
        }
    }
}
