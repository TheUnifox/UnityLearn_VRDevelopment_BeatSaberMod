// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BombBeatmapObjectPreview
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using UnityEngine;

namespace BeatmapEditor3D
{
  public class BombBeatmapObjectPreview : AbstractBeatmapObjectPreview
  {
    public override void Preview(RectInt objectRect) => this._objectTransform.localPosition = new Vector3((float) (((double) objectRect.x - 1.5) * 0.800000011920929), (float) objectRect.y * 0.8f, 0.0f);
  }
}
