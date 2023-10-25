// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.TextOnlyEventMarkerObject
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using TMPro;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class TextOnlyEventMarkerObject : MonoBehaviour, IEventMarkerObject
  {
    [SerializeField]
    private MaterialPropertyBlockController _materialPropertyBlockController;
    [SerializeField]
    private GameObject _selectionHighlightGameObject;
    [Space]
    [SerializeField]
    private TextMeshPro _sideText;
    [SerializeField]
    private TextMeshPro _topText;
    [DoesNotRequireDomainReloadInit]
    private static readonly int _colorId = Shader.PropertyToID("_Color");
    [DoesNotRequireDomainReloadInit]
    private static readonly int _highlightId = Shader.PropertyToID("_Highlight");
    [DoesNotRequireDomainReloadInit]
    private static readonly int _dimId = Shader.PropertyToID("_Dim");
    private bool _onBeat;
    private bool _pastBeat;

    public void Init(string text)
    {
      this._sideText.text = text;
      this._topText.text = text;
      this.SetMaterial(false, false);
    }

    public void SetState(bool onBeat, bool pastBeat, bool selected)
    {
      if (selected != this._selectionHighlightGameObject.activeSelf)
        this._selectionHighlightGameObject.SetActive(selected);
      if (this._onBeat == onBeat && this._pastBeat == pastBeat)
        return;
      this._onBeat = onBeat;
      this._pastBeat = pastBeat;
      this.SetMaterial(onBeat, pastBeat);
    }

    public void SetHighlight(bool highlighted)
    {
      this._materialPropertyBlockController.materialPropertyBlock.SetColor(TextOnlyEventMarkerObject._colorId, Color.gray * (highlighted ? 0.8f : 1f));
      this._materialPropertyBlockController.ApplyChanges();
    }

    private void SetMaterial(bool onBeat, bool pastBeat)
    {
      this._materialPropertyBlockController.materialPropertyBlock.SetColor(TextOnlyEventMarkerObject._colorId, Color.gray);
      this._materialPropertyBlockController.materialPropertyBlock.SetFloat(TextOnlyEventMarkerObject._highlightId, onBeat ? 1f : 0.0f);
      this._materialPropertyBlockController.materialPropertyBlock.SetFloat(TextOnlyEventMarkerObject._dimId, pastBeat ? 1f : 0.0f);
      this._materialPropertyBlockController.ApplyChanges();
    }

    public class Pool : MonoMemoryPool<TextOnlyEventMarkerObject>
    {
    }
  }
}
