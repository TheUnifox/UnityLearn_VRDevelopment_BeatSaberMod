// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapPreviewData
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using UnityEngine;

namespace BeatmapEditor3D
{
  public class BeatmapPreviewData
  {
    private const float kScaleMultiplier = 0.8f;
    private const float kScrollMultiplier = 0.008f;
    private int _maxLength;
    private int _maxScale = 2000;
    private int _start;
    private int _length;
    private int _playhead;
    private float _scale = 1f;

    public int start => this._start;

    public int end => this._start + this.length;

    public int playhead => this._playhead;

    public int length => this._length;

    public int maxLength => this._maxLength;

    public float relativePlayhead => (float) (this.playhead - this.start) / (float) this.length;

    public void Init(int maxLength)
    {
      this._maxLength = maxLength;
      this._start = 0;
      this._playhead = 0;
      this._length = 0;
    }

    public void ZoomSelection(float zoomMultiplier)
    {
      this._scale = Mathf.Clamp((double) zoomMultiplier > 0.0 ? this._scale * 0.8f : this._scale / 0.8f, 1f, (float) this._maxScale);
      this._length = (int) ((double) this._maxLength / (double) this._scale);
    }

    public void SetPreviewLength(int length)
    {
      this._length = length;
      this._scale = (float) this._maxLength / (float) this._length;
    }

    public void SetPlayHead(int samplePosition) => this._playhead = samplePosition;

    public void CenterPreviewStart() => this._start = this._playhead - this._length / 2;

    public BeatmapPreviewData Copy() => new BeatmapPreviewData()
    {
      _length = this._length,
      _playhead = this._playhead,
      _scale = this._scale,
      _start = this._start,
      _maxLength = this._maxLength,
      _maxScale = this._maxScale
    };
  }
}
