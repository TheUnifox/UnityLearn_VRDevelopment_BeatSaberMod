// Decompiled with JetBrains decompiler
// Type: RelativeSfxVolumePerLevelModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;

public class RelativeSfxVolumePerLevelModel
{
  protected const float kDefaultSfxVolume = 0.0f;
  protected readonly Dictionary<string, float> _relativeSfxVolumePerLevelId = new Dictionary<string, float>();

  public RelativeSfxVolumePerLevelModel(
    RelativeSfxVolumePerLevelSO relativeSfxVolumePerLevelData)
  {
    foreach (RelativeSfxVolumePerLevelSO.RelativeSfxVolumePair relativeSfxVolumePair in relativeSfxVolumePerLevelData.relativeSfxVolumePerLevel)
      this._relativeSfxVolumePerLevelId[relativeSfxVolumePair.levelId] = relativeSfxVolumePair.relativeSfxVolume;
  }

  public virtual float GetRelativeSfxVolume(string levelId)
  {
    float num;
    return !this._relativeSfxVolumePerLevelId.TryGetValue(levelId, out num) ? 0.0f : num;
  }
}
