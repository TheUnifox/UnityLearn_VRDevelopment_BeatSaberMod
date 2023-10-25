// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Visuals.ObstacleView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Visuals
{
  public class ObstacleView : MonoBehaviour
  {
    [SerializeField]
    private StretchableObstacle _stretchableObstacle;
    [SerializeField]
    private MaterialPropertyBlockController _materialPropertyBlockController;
    [DoesNotRequireDomainReloadInit]
    private static readonly int _sizeId = Shader.PropertyToID("_Size");
    private float _width;
    private float _height;
    private float _length;
    private Color _color;

    public ObstacleEditorData obstacleData { get; private set; }

    public void Init(
      ObstacleEditorData obstacleData,
      float width,
      float height,
      float length,
      Color color)
    {
      this.obstacleData = obstacleData;
      this._stretchableObstacle.SetSizeAndColor(width, height, length, color);
      this._width = width;
      this._height = height;
      this._length = length;
      this._color = color;
      this._materialPropertyBlockController.materialPropertyBlock.SetVector(ObstacleView._sizeId, (Vector4) new Vector3(width, height, length));
      this._materialPropertyBlockController.ApplyChanges();
    }

    public void SetState(Color color)
    {
      if (color == this._color)
        return;
      this._color = color;
      this._stretchableObstacle.SetSizeAndColor(this._width, this._height, this._length, color);
    }

    public void SetLength(float length)
    {
      this._length = length;
      this._stretchableObstacle.SetSizeAndColor(this._width, this._height, this._length, this._color);
    }

    public void SetHighlight(bool highlighted)
    {
    }

    public class Pool : MonoMemoryPool<ObstacleView>
    {
    }
  }
}
