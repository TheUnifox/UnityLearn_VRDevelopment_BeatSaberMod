// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.EventMarkerObject
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.InputSignals;
using UnityEngine;

namespace BeatmapEditor3D
{
  public class EventMarkerObject : MonoBehaviour, IBasicEventObjectDataProvider
  {
    [SerializeField]
    private Transform _transform;
    [Space]
    [SerializeField]
    private Color _defaultColor;
    [Space]
    [SerializeField]
    private MaterialPropertyBlockController _materialPropertyBlockController;
    [Space]
    [SerializeField]
    private GameObject _selectionHighlightGameObject;
    [DoesNotRequireDomainReloadInit]
    private static readonly int _colorId = Shader.PropertyToID("_Color");
    [DoesNotRequireDomainReloadInit]
    private static readonly int _highlightId = Shader.PropertyToID("_Highlight");
    [DoesNotRequireDomainReloadInit]
    private static readonly int _dimId = Shader.PropertyToID("_Dim");
    private bool _onBeat;
    private bool _pastBeat;

    public BasicEventEditorData basicEventData { get; private set; }

    public bool isEnd { get; private set; }

    public new Transform transform => this._transform;

    public BasicEventEditorData GetEventData() => this.basicEventData;

    public virtual void Init(BasicEventEditorData basicEventData) => this.Init(basicEventData, this._defaultColor);

    public virtual void Init(BasicEventEditorData basicEventData, Color color)
    {
      this.basicEventData = basicEventData;
      this.isEnd = this.isEnd;
      this._materialPropertyBlockController.materialPropertyBlock.SetColor(EventMarkerObject._colorId, color);
      this._materialPropertyBlockController.ApplyChanges();
    }

    public virtual void SetState(bool onBeat, bool pastBeat, bool selected)
    {
      if (selected != this._selectionHighlightGameObject.activeSelf)
        this._selectionHighlightGameObject.SetActive(selected);
      if (this._onBeat == onBeat && this._pastBeat == pastBeat)
        return;
      this._onBeat = onBeat;
      this._pastBeat = pastBeat;
      this._materialPropertyBlockController.materialPropertyBlock.SetFloat(EventMarkerObject._highlightId, onBeat ? 1f : 0.0f);
      this._materialPropertyBlockController.materialPropertyBlock.SetFloat(EventMarkerObject._dimId, pastBeat ? 1f : 0.0f);
      this._materialPropertyBlockController.ApplyChanges();
    }
  }
}
