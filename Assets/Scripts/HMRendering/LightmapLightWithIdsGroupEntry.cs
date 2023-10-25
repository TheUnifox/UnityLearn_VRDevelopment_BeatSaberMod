// Decompiled with JetBrains decompiler
// Type: LightmapLightWithIdsGroupEntry
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using System;
using UnityEngine;

public class LightmapLightWithIdsGroupEntry : MonoBehaviour
{
  [SerializeField]
  protected LightmapLightWithIds _lightmapLightWithIds;
  [Space]
  [SerializeField]
  protected LightmapLightWithIdsGroupEntry.GroupLightData[] _groupLightData;
  [SerializeField]
  protected LightmapLightWithIdsGroupEntry.LightIntensitiesWithId[] _individualLightData;
  [SerializeField]
  protected int[] _excludedLightIds;
  [Space]
  [SerializeField]
  protected LightmapLightWithIdsGroupEntry.GroupLightsWeighting _groupLightsWeighting;

  public LightmapLightWithIds lightmapLightWithIds => this._lightmapLightWithIds;

  public enum GroupLightsWeighting
  {
    Maximum,
    LinearFraction,
  }

  [Serializable]
  public class GroupLightData
  {
    [SerializeField]
    protected LightGroupSO _lightGroup;
    [SerializeField]
    protected float _groupIntensity;
    [SerializeField]
    protected float _groupProbeHighlightsIntensityMultiplier;

    public LightGroupSO lightGroup => this._lightGroup;

    public float groupIntensity => this._groupIntensity;

    public float groupProbeHighlightsIntensityMultiplier => this._groupProbeHighlightsIntensityMultiplier;

    public GroupLightData(
      LightGroupSO lightGroup,
      float groupIntensity,
      float groupProbeHighlightsIntensityMultiplier)
    {
      this._lightGroup = lightGroup;
      this._groupIntensity = groupIntensity;
      this._groupProbeHighlightsIntensityMultiplier = groupProbeHighlightsIntensityMultiplier;
    }
  }

  [Serializable]
  public class LightIntensitiesWithId
  {
    [SerializeField]
    protected int _lightId;
    [SerializeField]
    protected float _intensity;
    [SerializeField]
    protected float _probeHighlightsIntensityMultiplier = 1f;

    public int lightId => this._lightId;

    public float intensity => this._intensity;

    public float probeHighlightsIntensityMultiplier => this._probeHighlightsIntensityMultiplier;

    public LightIntensitiesWithId(
      int lightId,
      float intensity,
      float probeHighlightsIntensityMultiplier)
    {
      this._lightId = lightId;
      this._intensity = intensity;
      this._probeHighlightsIntensityMultiplier = probeHighlightsIntensityMultiplier;
    }
  }
}
