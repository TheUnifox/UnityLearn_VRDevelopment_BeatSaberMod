// Decompiled with JetBrains decompiler
// Type: RelativeSfxVolumePerLevelSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;

public class RelativeSfxVolumePerLevelSO : ScriptableObject
{
  [SerializeField]
  protected RelativeSfxVolumePerLevelSO.RelativeSfxVolumePair[] _relativeSfxVolumePerLevel;

  public RelativeSfxVolumePerLevelSO.RelativeSfxVolumePair[] relativeSfxVolumePerLevel => this._relativeSfxVolumePerLevel;

  [Serializable]
  public class RelativeSfxVolumePair
  {
    [SerializeField]
    protected PreviewBeatmapLevelSO _previewLevel;
    [SerializeField]
    protected float _relativeSfxVolume;

    public string levelId => this._previewLevel.levelID;

    public float relativeSfxVolume => this._relativeSfxVolume;
  }
}
