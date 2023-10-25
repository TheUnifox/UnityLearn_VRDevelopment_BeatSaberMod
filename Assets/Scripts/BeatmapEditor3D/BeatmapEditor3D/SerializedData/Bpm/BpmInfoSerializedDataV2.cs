// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.SerializedData.Bpm.BpmInfoSerializedDataV2
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BeatmapEditor3D.SerializedData.Bpm
{
  [Serializable]
  public class BpmInfoSerializedDataV2
  {
    private const string kCurrentVersion = "2.0.0";
    [SerializeField]
    private string _version;
    [SerializeField]
    private int _songSampleCount;
    [SerializeField]
    private int _songFrequency;
    [SerializeField]
    private List<BpmInfoSerializedDataV2.BpmRegionSerializedData> _regions;

    public string version => this._version;

    public int songSampleCount => this._songSampleCount;

    public int songFrequency => this._songFrequency;

    public List<BpmInfoSerializedDataV2.BpmRegionSerializedData> regions => this._regions;

    public BpmInfoSerializedDataV2(
      int songSampleCount,
      int songFrequency,
      List<BpmInfoSerializedDataV2.BpmRegionSerializedData> regions)
    {
      this._version = "2.0.0";
      this._songSampleCount = songSampleCount;
      this._songFrequency = songFrequency;
      this._regions = regions;
    }

    [Serializable]
    public class BpmRegionSerializedData
    {
      [SerializeField]
      private int _startSampleIndex;
      [SerializeField]
      private int _endSampleIndex;
      [SerializeField]
      private float _startBeat;
      [SerializeField]
      private float _endBeat;

      public int startSampleIndex => this._startSampleIndex;

      public int endSampleIndex => this._endSampleIndex;

      public float startBeat => this._startBeat;

      public float endBeat => this._endBeat;

      public BpmRegionSerializedData(
        int startSampleIndex,
        int endSampleIndex,
        float startBeat,
        float endBeat)
      {
        this._startSampleIndex = startSampleIndex;
        this._endSampleIndex = endSampleIndex;
        this._startBeat = startBeat;
        this._endBeat = endBeat;
      }
    }
  }
}
