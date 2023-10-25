// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Visuals.NormalNoteView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Visuals
{
  public class NormalNoteView : MonoBehaviour
  {
    [SerializeField]
    private GameObject _highlightGameObject;
    [Space]
    [SerializeField]
    private float _arrowGlowIntensity = 0.6f;
    [Space]
    [SerializeField]
    private SpriteRenderer _arrowGlowSpriteRenderer;
    [SerializeField]
    private SpriteRenderer _circleGlowSpriteRenderer;
    [SerializeField]
    private MaterialPropertyBlockController[] _materialPropertyBlockControllers;
    [SerializeField]
    private MeshRenderer _arrowMeshRenderer;
    private bool _onBeat;
    private bool _pastBeat;
    private bool _selected;

    public NoteEditorData noteData { get; private set; }

    private bool showArrow
    {
      set
      {
        this._arrowMeshRenderer.enabled = value;
        this._arrowGlowSpriteRenderer.enabled = value;
      }
    }

    private bool showCircle
    {
      set => this._circleGlowSpriteRenderer.enabled = value;
    }

    public void Init(
      NoteEditorData noteData,
      Color color,
      NoteCutDirection cutDirection,
      int angle)
    {
      this.noteData = noteData;
      bool flag = cutDirection == NoteCutDirection.Any;
      this.showCircle = flag;
      this.showArrow = !flag;
      this.transform.localRotation = Quaternion.Euler(Vector3.forward * (cutDirection.RotationAngle() + (float) angle));
      this._arrowGlowSpriteRenderer.color = color.ColorWithAlpha(this._arrowGlowIntensity);
      BeatmapObjectMaterialHelpers.SetColorDataToMaterialPropertyBlocks((IEnumerable<MaterialPropertyBlockController>) this._materialPropertyBlockControllers, color.ColorWithAlpha(1f));
    }

    public void SetState(bool onBeat, bool pastBeat, bool selected)
    {
      if (selected != this._highlightGameObject.activeSelf)
        this._highlightGameObject.SetActive(selected);
      if (onBeat == this._onBeat && pastBeat == this._pastBeat)
        return;
      this._onBeat = onBeat;
      this._pastBeat = pastBeat;
      BeatmapObjectMaterialHelpers.SetBeatDataToMaterialPropertyBlocks((IEnumerable<MaterialPropertyBlockController>) this._materialPropertyBlockControllers, onBeat, pastBeat);
    }

    public void SetHighlight(bool highlighted)
    {
    }

    public class Pool : MonoMemoryPool<NormalNoteView>
    {
    }
  }
}
