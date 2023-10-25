// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.SerializedData.VersionSerializedData
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using UnityEngine;

namespace BeatmapEditor3D.SerializedData
{
  [Serializable]
  public class VersionSerializedData
  {
    [SerializeField]
    private string _version;
    [SerializeField]
    private string version;

    public string v => !string.IsNullOrEmpty(this._version) ? this._version : this.version;
  }
}
