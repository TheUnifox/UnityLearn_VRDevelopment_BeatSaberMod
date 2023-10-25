// Decompiled with JetBrains decompiler
// Type: DirectionalLightWithIdsGroupEntry
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using System;
using UnityEngine;

public class DirectionalLightWithIdsGroupEntry : MonoBehaviour
{
  [SerializeField]
  protected DirectionalLightWithIds _directionalLightWithIds;
  [Space]
  [SerializeField]
  protected DirectionalLightWithIdsGroupEntry.GroupLightData[] _groupLightData;
  [SerializeField]
  protected DirectionalLightWithIdsGroupEntry.LightIntensitiesWithId[] _individualLightData;
  [SerializeField]
  protected int[] _excludedLightIds;
  [Space]
  [SerializeField]
  protected DirectionalLightWithIdsGroupEntry.GroupLightsWeighting _groupLightsWeighting;

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

    public LightGroupSO lightGroup => this._lightGroup;

    public float groupIntensity => this._groupIntensity;
  }

  [Serializable]
  public class LightIntensitiesWithId
  {
    [SerializeField]
    protected int _lightId;
    [SerializeField]
    protected float _intensity;

    public int lightId => this._lightId;

    public float intensity => this._intensity;

    public LightIntensitiesWithId(int lightId, float lightIntensity)
    {
      this._lightId = lightId;
      this._intensity = lightIntensity;
    }
  }
}
