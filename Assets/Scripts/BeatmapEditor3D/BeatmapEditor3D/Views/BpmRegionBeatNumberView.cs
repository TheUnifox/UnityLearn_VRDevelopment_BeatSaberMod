// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.BpmRegionBeatNumberView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using TMPro;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Views
{
  public class BpmRegionBeatNumberView : MonoBehaviour
  {
    [SerializeField]
    private TMP_Text _label;

    public void SetBeatNumber(float beatNumber) => this._label.SetText("{0}", beatNumber);

    public class Pool : MonoMemoryPool<BpmRegionBeatNumberView>
    {
    }
  }
}
