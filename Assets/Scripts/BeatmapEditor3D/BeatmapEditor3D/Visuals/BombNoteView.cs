// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Visuals.BombNoteView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Visuals
{
  public class BombNoteView : MonoBehaviour
  {
    [SerializeField]
    private GameObject _highlightGameObject;
    [Space]
    [SerializeField]
    private MaterialPropertyBlockController _materialPropertyBlockController;
    private bool _onBeat;
    private bool _pastBeat;
    private bool _selected;
    [DoesNotRequireDomainReloadInit]
    private static readonly int _colorId = Shader.PropertyToID("_Color");
    [DoesNotRequireDomainReloadInit]
    private static readonly int _highlightId = Shader.PropertyToID("_Highlight");
    [DoesNotRequireDomainReloadInit]
    private static readonly int _dimId = Shader.PropertyToID("_Dim");

    public NoteEditorData noteData { get; private set; }

    public void Init(NoteEditorData noteData, Color color)
    {
      this.noteData = noteData;
      this._materialPropertyBlockController.materialPropertyBlock.SetColor(BombNoteView._colorId, color.ColorWithAlpha(1f));
      this._materialPropertyBlockController.ApplyChanges();
    }

    public void SetState(bool onBeat, bool pastBeat, bool selected)
    {
      if (selected != this._highlightGameObject.activeSelf)
        this._highlightGameObject.SetActive(selected);
      if (onBeat == this._onBeat && pastBeat == this._pastBeat)
        return;
      this._onBeat = onBeat;
      this._pastBeat = pastBeat;
      MaterialPropertyBlock materialPropertyBlock = this._materialPropertyBlockController.materialPropertyBlock;
      materialPropertyBlock.SetFloat(BombNoteView._highlightId, onBeat ? 1f : 0.0f);
      materialPropertyBlock.SetFloat(BombNoteView._dimId, pastBeat ? 1f : 0.0f);
      this._materialPropertyBlockController.ApplyChanges();
    }

    public void SetHighlight(bool highlighted)
    {
    }

    public class Pool : MonoMemoryPool<BombNoteView>
    {
    }
  }
}
