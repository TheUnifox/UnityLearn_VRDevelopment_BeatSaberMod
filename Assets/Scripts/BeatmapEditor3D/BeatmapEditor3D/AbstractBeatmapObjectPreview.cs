// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.AbstractBeatmapObjectPreview
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using UnityEngine;

namespace BeatmapEditor3D
{
  public abstract class AbstractBeatmapObjectPreview : MonoBehaviour
  {
    [SerializeField]
    protected Transform _objectTransform;

    public Transform objectTransform => this._objectTransform;

    public virtual void Show() => this.gameObject.SetActive(true);

    public virtual void Hide() => this.gameObject.SetActive(false);

    public abstract void Preview(RectInt objectRect);
  }
}
