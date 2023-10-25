// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.UI.BpmRegionDividerView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace BeatmapEditor3D.BpmEditor.UI
{
  public class BpmRegionDividerView : 
    MonoBehaviour,
    IPointerDownHandler,
    IEventSystemHandler,
    IPointerUpHandler
  {
    [SerializeField]
    private RectTransform _rectTransform;
    [SerializeField]
    private GameObject _handleGo;
    private Action<int, int> _pointerUpAction;
    private Action<int, int> _pointerDownAction;
    private int _endRegionIndex;
    private int _startRegionIndex;

    public RectTransform rectTransform => this._rectTransform;

    public void SetData(
      int endRegionIndex,
      int startRegionIndex,
      Action<int, int> pointerUpAction,
      Action<int, int> pointerDownAction)
    {
      this._endRegionIndex = endRegionIndex;
      this._startRegionIndex = startRegionIndex;
      this._pointerUpAction = pointerUpAction;
      this._pointerDownAction = pointerDownAction;
    }

    public void SetState(bool interactable) => this._handleGo.SetActive(interactable);

    public void OnPointerUp(PointerEventData eventData)
    {
      if (eventData.button != PointerEventData.InputButton.Left)
        return;
      Action<int, int> pointerUpAction = this._pointerUpAction;
      if (pointerUpAction == null)
        return;
      pointerUpAction(this._endRegionIndex, this._startRegionIndex);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
      if (eventData.button != PointerEventData.InputButton.Left)
        return;
      Action<int, int> pointerDownAction = this._pointerDownAction;
      if (pointerDownAction == null)
        return;
      pointerDownAction(this._endRegionIndex, this._startRegionIndex);
    }

    public class Pool : MonoMemoryPool<BpmRegionDividerView>
    {
    }
  }
}
