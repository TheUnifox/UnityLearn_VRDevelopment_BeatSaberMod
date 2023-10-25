// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.AudioWaveformViewGenericInputController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace BeatmapEditor3D.Views
{
  public abstract class AudioWaveformViewGenericInputController : 
    MonoBehaviour,
    IPointerEnterHandler,
    IEventSystemHandler,
    IPointerExitHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler,
    IPointerClickHandler
  {
    [Header("Transforms")]
    [SerializeField]
    protected RectTransform _waveformTransform;
    [SerializeField]
    protected RectTransform _waveformCurrentTimeMarker;
    [SerializeField]
    protected RectTransform _waveformHoverMarker;
    [Inject]
    protected readonly SignalBus _signalBus;
    [Inject]
    protected readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    protected readonly IBeatmapDataModel _beatmapDataModel;
    protected bool _mouseHovering;

    public void OnPointerEnter(PointerEventData eventData)
    {
      this._mouseHovering = true;
      this._waveformHoverMarker.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
      this._mouseHovering = false;
      this._waveformHoverMarker.gameObject.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData) => this.UpdatePlayHead(eventData, UpdatePlayHeadSignal.SnapType.None);

    public void OnPointerClick(PointerEventData eventData) => this.UpdatePlayHead(eventData, UpdatePlayHeadSignal.SnapType.CurrentSubdivision);

    protected void UpdatePlayHead(
      PointerEventData eventData,
      UpdatePlayHeadSignal.SnapType snapType)
    {
      this._signalBus.Fire<UpdatePlayHeadSignal>(new UpdatePlayHeadSignal(this.ScreenPositionToIndex(eventData.position), snapType, true));
    }

    protected void Update()
    {
      if (!this._mouseHovering)
        return;
      Vector2 localPoint;
      RectTransformUtility.ScreenPointToLocalPointInRectangle(this._waveformTransform, (Vector2) Input.mousePosition, (Camera) null, out localPoint);
      this._waveformHoverMarker.anchoredPosition = this._waveformHoverMarker.anchoredPosition with
      {
        x = localPoint.x + this._waveformTransform.rect.width / 2f
      };
    }

    protected virtual void OnEnable()
    {
    }

    protected virtual void OnDisable()
    {
    }

    public abstract void OnBeginDrag(PointerEventData eventData);

    public abstract void OnEndDrag(PointerEventData eventData);

    protected abstract int ScreenPositionToIndex(Vector2 screenPosition);
  }
}
