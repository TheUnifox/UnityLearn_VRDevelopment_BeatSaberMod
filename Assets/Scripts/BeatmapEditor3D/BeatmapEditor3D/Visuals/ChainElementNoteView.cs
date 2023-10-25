// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Visuals.ChainElementNoteView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Visuals
{
  public class ChainElementNoteView : MonoBehaviour
  {
    [SerializeField]
    private Transform _transform;
    [SerializeField]
    private MaterialPropertyBlockController[] _materialPropertyBlockControllers;
    [SerializeField]
    private TextMeshPro _linkIdxText;

    public void Init(int linkId, Color color, float rotation, Vector3 localPosition)
    {
      this._transform.localRotation = Quaternion.Euler(Vector3.forward * rotation);
      this._transform.localPosition = localPosition;
      this._linkIdxText.transform.localRotation = Quaternion.Euler(Vector3.back * rotation);
      this._linkIdxText.text = linkId.ToString();
      BeatmapObjectMaterialHelpers.SetColorDataToMaterialPropertyBlocks((IEnumerable<MaterialPropertyBlockController>) this._materialPropertyBlockControllers, color.ColorWithAlpha(1f));
    }

    public void SetState(bool pastBeat) => BeatmapObjectMaterialHelpers.SetBeatDataToMaterialPropertyBlocks((IEnumerable<MaterialPropertyBlockController>) this._materialPropertyBlockControllers, false, pastBeat);

    public class Pool : MonoMemoryPool<ChainElementNoteView>
    {
    }
  }
}
