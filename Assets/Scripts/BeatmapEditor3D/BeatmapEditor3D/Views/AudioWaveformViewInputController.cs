// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.AudioWaveformViewInputController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BeatmapEditor3D.Views
{
  public class AudioWaveformViewInputController : AudioWaveformViewGenericInputController
  {
    public override void OnBeginDrag(PointerEventData eventData) => this.UpdatePlayHead(eventData, UpdatePlayHeadSignal.SnapType.None);

    public override void OnEndDrag(PointerEventData eventData) => this.UpdatePlayHead(eventData, UpdatePlayHeadSignal.SnapType.CurrentSubdivision);

    protected override void OnEnable() => this._signalBus.Subscribe<BeatmapLevelStateTimeUpdated>(new Action(this.HandleLevelEditorStateTimeUpdated));

    protected override void OnDisable() => this._signalBus.TryUnsubscribe<BeatmapLevelStateTimeUpdated>(new Action(this.HandleLevelEditorStateTimeUpdated));

    private void HandleLevelEditorStateTimeUpdated() => this.UpdateWaveformMarker((float) this._beatmapDataModel.bpmData.BeatToSample(this._beatmapState.beat), this._waveformCurrentTimeMarker);

        private void UpdateWaveformMarker(float sample, RectTransform marker)
        {
            float x = Mathf.InverseLerp(0f, (float)this._beatmapState.previewData.maxLength, sample) * this._waveformTransform.rect.width;
            Vector2 anchoredPosition = marker.anchoredPosition;
            anchoredPosition.x = x;
            marker.anchoredPosition = anchoredPosition;
        }

        protected override int ScreenPositionToIndex(Vector2 screenPosition)
    {
      Vector2 localPoint;
      RectTransformUtility.ScreenPointToLocalPointInRectangle(this._waveformTransform, screenPosition, (Camera) null, out localPoint);
      Rect rect = this._waveformTransform.rect;
      float num = Mathf.Clamp(localPoint.x, rect.x, rect.x + rect.width);
      return (int) ((double) Mathf.InverseLerp(rect.x, rect.x + rect.width, num) * (double) this._beatmapState.previewData.maxLength);
    }
  }
}
