// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.StatusBarWaveformView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using UnityEngine;
using UnityEngine.UI;

namespace BeatmapEditor3D.Views
{
  public class StatusBarWaveformView : BeatmapEditorView
  {
    [SerializeField]
    private float _hiddenSize;
    [SerializeField]
    private float _smallSize;
    [SerializeField]
    private float _expandedSize;
    [Space]
    [SerializeField]
    private LayoutElement _layoutElement;
    [SerializeField]
    private RectTransform _smallWaveformRectTransform;
    [SerializeField]
    private GameObject _smallGameObject;
    [SerializeField]
    private GameObject _expandedGameObject;

    public void SetType(BeatmapEditor3D.Types.WaveformType type)
    {
      switch (type)
      {
        case BeatmapEditor3D.Types.WaveformType.Expanded:
          this._layoutElement.minHeight = this._expandedSize;
          break;
        case BeatmapEditor3D.Types.WaveformType.Small:
          this._layoutElement.minHeight = this._smallSize;
          break;
        case BeatmapEditor3D.Types.WaveformType.Hidden:
          this._layoutElement.minHeight = this._hiddenSize;
          break;
      }
      this.SetSmallWaveformSize(type == BeatmapEditor3D.Types.WaveformType.Hidden ? this._hiddenSize : this._smallSize);
      this._smallGameObject.SetActive(type != BeatmapEditor3D.Types.WaveformType.Hidden);
      this._expandedGameObject.SetActive(type == BeatmapEditor3D.Types.WaveformType.Expanded);
    }

        private void SetSmallWaveformSize(float size)
        {
            Vector2 sizeDelta = this._smallWaveformRectTransform.sizeDelta;
            sizeDelta.y = size;
            this._smallWaveformRectTransform.sizeDelta = sizeDelta;
        }
    }
}
