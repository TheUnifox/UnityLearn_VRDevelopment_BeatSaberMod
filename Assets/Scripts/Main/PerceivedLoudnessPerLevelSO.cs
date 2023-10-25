// Decompiled with JetBrains decompiler
// Type: PerceivedLoudnessPerLevelSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PerceivedLoudnessPerLevelSO : ScriptableObject
{
  public PerceivedLoudnessPerLevelSO.PerceivedLevelLoudnessPair[] perceivedLoudnessPerLevel;

  public virtual Dictionary<string, PerceivedLoudnessPerLevelSO.PerceivedLevelLoudnessPair> ToDictionary()
  {
    Dictionary<string, PerceivedLoudnessPerLevelSO.PerceivedLevelLoudnessPair> dictionary = new Dictionary<string, PerceivedLoudnessPerLevelSO.PerceivedLevelLoudnessPair>();
    foreach (PerceivedLoudnessPerLevelSO.PerceivedLevelLoudnessPair levelLoudnessPair in this.perceivedLoudnessPerLevel)
      dictionary[levelLoudnessPair.levelId] = levelLoudnessPair;
    return dictionary;
  }

  public virtual void SetLoudnessData(
    Dictionary<string, PerceivedLoudnessPerLevelSO.PerceivedLevelLoudnessPair> loudnessDictionary)
  {
    this.perceivedLoudnessPerLevel = loudnessDictionary.Values.ToArray<PerceivedLoudnessPerLevelSO.PerceivedLevelLoudnessPair>();
  }

  [Serializable]
  public class PerceivedLevelLoudnessPair
  {
    [SerializeField]
    protected string _levelId;
    [SerializeField]
    protected float _perceivedLoudness;
    [SerializeField]
    protected string _checkSum;

    public string levelId => this._levelId;

    public float perceivedLoudness => this._perceivedLoudness;

    public string checksum => this._checkSum;

    public PerceivedLevelLoudnessPair(string levelId, float perceivedLoudness, string checkSum)
    {
      this._levelId = levelId;
      this._perceivedLoudness = perceivedLoudness;
      this._checkSum = checkSum;
    }
  }
}
