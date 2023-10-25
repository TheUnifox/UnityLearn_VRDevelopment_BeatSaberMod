// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Visuals.BeatmapObjectBeatLine
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Visuals
{
  public class BeatmapObjectBeatLine : MonoBehaviour
  {
    [SerializeField]
    private Transform _transform;
    [SerializeField]
    private MeshRenderer _meshRenderer;
    private float _width;

        public void SetData(MaterialPropertyBlock propertyBlock, float bonusWidth, float height)
        {
            this._meshRenderer.SetPropertyBlock(propertyBlock);
            Vector3 localScale = this._transform.localScale;
            localScale.x = this._width + bonusWidth;
            localScale.z = height;
            this._transform.localScale = localScale;
        }

        private void Initialize(float width) => this._width = width;

    public class Pool : MonoMemoryPool<float, BeatmapObjectBeatLine>
    {
      protected override void Reinitialize(float width, BeatmapObjectBeatLine item)
      {
        base.Reinitialize(width, item);
        item.Initialize(width);
      }
    }
  }
}
