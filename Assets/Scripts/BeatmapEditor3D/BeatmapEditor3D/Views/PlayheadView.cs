// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.PlayheadView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using UnityEngine;

namespace BeatmapEditor3D.Views
{
  [RequireComponent(typeof (RectTransform))]
  public class PlayheadView : MonoBehaviour
  {
    [SerializeField]
    public RectTransform _playheadGraphic;

    public void Start() => this.GetComponent<RectTransform>();

    public void SetPlayhead(float relativePlayheadPosition) => this._playheadGraphic.localPosition = Vector3.right * relativePlayheadPosition * this.GetComponent<RectTransform>().rect.width;
  }
}
