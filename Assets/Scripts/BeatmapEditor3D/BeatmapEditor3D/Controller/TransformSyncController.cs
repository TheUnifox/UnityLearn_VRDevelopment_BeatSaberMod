// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Controller.TransformSyncController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System.Collections.Generic;
using UnityEngine;

namespace BeatmapEditor3D.Controller
{
  public class TransformSyncController : MonoBehaviour
  {
    [SerializeField]
    private Transform _referenceTransform;
    private readonly List<Transform> _syncedTransforms = new List<Transform>();

    public void AddSyncTransform(Transform transform) => this._syncedTransforms.Add(transform);

    public void RemoveSyncTransform(Transform transform) => this._syncedTransforms.Remove(transform);

    protected void LateUpdate()
    {
      foreach (Transform syncedTransform in this._syncedTransforms)
      {
        syncedTransform.position = this._referenceTransform.position;
        syncedTransform.rotation = this._referenceTransform.rotation;
        syncedTransform.localScale = this._referenceTransform.localScale;
      }
    }
  }
}
