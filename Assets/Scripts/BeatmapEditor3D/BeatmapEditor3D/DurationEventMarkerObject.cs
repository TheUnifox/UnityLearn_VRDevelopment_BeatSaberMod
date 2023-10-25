// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DurationEventMarkerObject
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class DurationEventMarkerObject : MonoBehaviour
  {
    [SerializeField]
    private Transform _transform;
    [SerializeField]
    private MaterialPropertyBlockController _materialPropertyBlockController;
    [DoesNotRequireDomainReloadInit]
    private static readonly int _colorAId = Shader.PropertyToID("_ColorA");
    [DoesNotRequireDomainReloadInit]
    private static readonly int _colorBId = Shader.PropertyToID("_ColorB");
    [DoesNotRequireDomainReloadInit]
    private static readonly int _sizeId = Shader.PropertyToID("_Size");

    public BasicEventEditorData basicEventData { get; private set; }

    public new Transform transform => this._transform;

    public void Init(BasicEventEditorData basicEventData, Color colorA, Color colorB)
    {
      this.basicEventData = basicEventData;
      this.SetColors(colorA, colorB);
    }

    public void SetScaleZ(float scaleZ)
    {
      this.transform.localScale = new Vector3(1f, 1f, scaleZ);
      this.SetScale(scaleZ);
    }

    private void SetColors(Color colorA, Color colorB)
    {
      this._materialPropertyBlockController.materialPropertyBlock.SetColor(DurationEventMarkerObject._colorAId, colorA);
      this._materialPropertyBlockController.materialPropertyBlock.SetColor(DurationEventMarkerObject._colorBId, colorB);
      this._materialPropertyBlockController.ApplyChanges();
    }

    private void SetScale(float scale)
    {
      this._materialPropertyBlockController.materialPropertyBlock.SetFloat(DurationEventMarkerObject._sizeId, scale);
      this._materialPropertyBlockController.ApplyChanges();
    }

    public class Pool : MonoMemoryPool<DurationEventMarkerObject>
    {
    }
  }
}
