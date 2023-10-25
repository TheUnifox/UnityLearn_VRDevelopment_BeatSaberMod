// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.AudioWaveformInteractiveViewInputController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using UnityEngine;
using UnityEngine.EventSystems;

namespace BeatmapEditor3D.Views
{
  public class AudioWaveformInteractiveViewInputController : AudioWaveformViewGenericInputController
  {
    [Header("Waveform Views")]
    [SerializeField]
    private InteractiveAudioWaveformView _waveformView;
    [SerializeField]
    private InteractiveAudioWaveformBeatsView _waveformBeatsView;
    private bool _isDragging;
    private BeatmapPreviewData _previewData;

    public override void OnBeginDrag(PointerEventData eventData)
    {
      this.SetIsDragging(true);
      this._previewData = this._beatmapState.previewData.Copy();
      this.UpdatePlayHead(eventData, UpdatePlayHeadSignal.SnapType.None);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
      this.SetIsDragging(false);
      this.UpdatePlayHead(eventData, UpdatePlayHeadSignal.SnapType.CurrentSubdivision);
      this._previewData = (BeatmapPreviewData) null;
    }

    protected override void OnEnable() => this._waveformHoverMarker.gameObject.SetActive(false);

    protected override int ScreenPositionToIndex(Vector2 screenPosition)
    {
      Vector2 localPoint;
      RectTransformUtility.ScreenPointToLocalPointInRectangle(this._waveformTransform, screenPosition, (Camera) null, out localPoint);
      Rect rect = this._waveformTransform.rect;
      float num1 = Mathf.Clamp(localPoint.x, rect.x, rect.x + rect.width);
      BeatmapPreviewData beatmapPreviewData = this._previewData ?? this._beatmapState.previewData;
      int num2 = beatmapPreviewData.end - beatmapPreviewData.start;
      return beatmapPreviewData.start + (int) ((double) Mathf.InverseLerp(rect.x, rect.x + rect.width, num1) * (double) num2);
    }

    private void SetIsDragging(bool isDragging)
    {
      this._waveformCurrentTimeMarker.gameObject.SetActive(!isDragging);
      this._isDragging = isDragging;
      this._waveformView.SetAutomaticUpdate(!this._isDragging);
      this._waveformBeatsView.SetAutomaticUpdate(!this._isDragging);
    }
  }
}
