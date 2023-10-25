// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.BpmBeatMarkerView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace BeatmapEditor3D.BpmEditor
{
  public class BpmBeatMarkerView : 
    MonoBehaviour,
    IPointerDownHandler,
    IEventSystemHandler,
    IPointerUpHandler
  {
    [SerializeField]
    private RectTransform _rectTransform;
    [SerializeField]
    private TextMeshProUGUI _beatLabelText;
    private Action<int, int> _mouseDownAction;
    private Action<int, int> _mouseUpAction;
    private int _sample;
    private int _beat;

    public RectTransform rectTransform => this._rectTransform;

    public void SetData(
      int sample,
      int beat,
      Action<int, int> mouseDownAction,
      Action<int, int> mouseUpAction)
    {
      this._sample = sample;
      this._beat = beat;
      this._mouseDownAction = mouseDownAction;
      this._mouseUpAction = mouseUpAction;
      this._beatLabelText.SetText(StringsRepository.GetIntString(beat));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
      if (eventData.button != PointerEventData.InputButton.Left)
        return;
      Action<int, int> mouseDownAction = this._mouseDownAction;
      if (mouseDownAction == null)
        return;
      mouseDownAction(this._sample, this._beat);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
      if (eventData.button != PointerEventData.InputButton.Left)
        return;
      Action<int, int> mouseUpAction = this._mouseUpAction;
      if (mouseUpAction == null)
        return;
      mouseUpAction(this._sample, this._beat);
    }

    public class Pool : MonoMemoryPool<BpmBeatMarkerView>
    {
    }
  }
}
