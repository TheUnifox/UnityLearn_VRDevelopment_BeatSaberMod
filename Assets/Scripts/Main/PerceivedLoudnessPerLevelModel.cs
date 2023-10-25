// Decompiled with JetBrains decompiler
// Type: PerceivedLoudnessPerLevelModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;

public class PerceivedLoudnessPerLevelModel
{
  protected const float kDefaultLoudness = -6f;
  protected const float kPerceivedLoudnessTarget = -11f;
  protected const float kPerceivedLoudnessToMaxSfxLevelDifference = -10f;
  protected readonly Dictionary<string, float> _loudnessLevelPerLevelId = new Dictionary<string, float>();

  public PerceivedLoudnessPerLevelModel(PerceivedLoudnessPerLevelSO loudnessPerLeveData)
  {
    foreach (PerceivedLoudnessPerLevelSO.PerceivedLevelLoudnessPair levelLoudnessPair in loudnessPerLeveData.perceivedLoudnessPerLevel)
      this._loudnessLevelPerLevelId[levelLoudnessPair.levelId] = levelLoudnessPair.perceivedLoudness;
  }

  public virtual bool ContainsLevelId(string levelId) => this._loudnessLevelPerLevelId.ContainsKey(levelId);

  public virtual float GetLoudnessCorrectionByLevelId(string levelId) => Mathf.Min(0.0f, -11f - this.GetLoudnessByLevelId(levelId));

  public virtual float GetMaxSfxVolumeByLevelId(string levelId) => this.GetLoudnessByLevelId(levelId) - -10f;

  public virtual float GetLoudnessByLevelId(string levelId)
  {
    float num;
    return !this._loudnessLevelPerLevelId.TryGetValue(levelId, out num) ? -6f : num;
  }
}
