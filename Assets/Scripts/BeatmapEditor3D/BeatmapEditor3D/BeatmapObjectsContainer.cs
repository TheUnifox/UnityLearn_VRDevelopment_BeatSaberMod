// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapObjectsContainer
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Visuals;
using UnityEngine;

namespace BeatmapEditor3D
{
  public class BeatmapObjectsContainer : MonoBehaviour
  {
    [SerializeField]
    private BeatGridContainer _beatGridContainer;

    protected void OnEnable()
    {
      this._beatGridContainer.SetDataToBeatContainers(4f, 0.8f);
      this._beatGridContainer.Enable();
    }

    protected void OnDisable() => this._beatGridContainer.Disable();
  }
}
