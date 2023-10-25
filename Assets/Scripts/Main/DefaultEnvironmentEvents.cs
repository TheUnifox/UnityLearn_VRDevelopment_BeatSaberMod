// Decompiled with JetBrains decompiler
// Type: DefaultEnvironmentEvents
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;

[Serializable]
public class DefaultEnvironmentEvents
{
  [SerializeField]
  protected DefaultEnvironmentEvents.BasicBeatmapEvent[] _basicBeatmapEvents;
  [SerializeField]
  protected DefaultEnvironmentEvents.LightGroupEvent[] _lightGroupEvents;

  public DefaultEnvironmentEvents.BasicBeatmapEvent[] basicBeatmapEvents => this._basicBeatmapEvents;

  public DefaultEnvironmentEvents.LightGroupEvent[] lightGroupEvents => this._lightGroupEvents;

  public bool isEmpty
  {
    get
    {
      if (this._basicBeatmapEvents != null && this._basicBeatmapEvents.Length != 0)
        return false;
      return this._lightGroupEvents == null || this._lightGroupEvents.Length == 0;
    }
  }

  [Serializable]
  public class BasicBeatmapEvent
  {
    [SerializeField]
    protected BasicBeatmapEventType _eventType;
    [SerializeField]
    [Tooltip("For Environment Color 0 use 5, for Environment Color 1 use 1")]
    protected int _value;
    [SerializeField]
    protected float _floatValue;

    public BasicBeatmapEventType eventType => this._eventType;

    public int value => this._value;

    public float floatValue => this._floatValue;
  }

  [Serializable]
  public class LightGroupDistribution
  {
    [SerializeField]
    protected bool _useDistribution;
    [SerializeField]
    protected float _distributionParam;
    [SerializeField]
    protected BeatmapEventDataBox.DistributionParamType _distributionParamType;

    public bool useDistribution => this._useDistribution;

    public float distributionParam => this._distributionParam;

    public BeatmapEventDataBox.DistributionParamType distributionParamType => this._distributionParamType;
  }

  [Serializable]
  public class LightGroupFiltering
  {
    [SerializeField]
    protected bool _useFiltering;
    [SerializeField]
    protected IndexFilter.IndexFilterRandomType _randomType;
    [SerializeField]
    protected float _limit;
    [SerializeField]
    protected IndexFilter.IndexFilterLimitAlsoAffectType _alsoAffectType;
    [SerializeField]
    protected int _seed;

    public bool useFiltering => this._useFiltering;

    public IndexFilter.IndexFilterRandomType randomType => this._randomType;

    public float limit => this._limit;

    public IndexFilter.IndexFilterLimitAlsoAffectType alsoAffectType => this._alsoAffectType;

    public int seed => this._seed;
  }

  [Serializable]
  public class LightGroupEvent
  {
    [SerializeField]
    protected LightGroupSO _lightGroup;
    [Header("Color")]
    [SerializeField]
    protected EnvironmentColorType _environmentColorType;
    [SerializeField]
    protected float _brightness;
    [SerializeField]
    protected DefaultEnvironmentEvents.LightGroupDistribution _brightnessDistribution;
    [SerializeField]
    protected DefaultEnvironmentEvents.LightGroupFiltering _brightnessFiltering;
    [Header("Rotation")]
    [SerializeField]
    protected float _rotationX;
    [SerializeField]
    protected float _rotationY;
    [SerializeField]
    protected float _rotationZ;
    [SerializeField]
    protected DefaultEnvironmentEvents.LightGroupDistribution _rotationXDistribution;
    [SerializeField]
    protected DefaultEnvironmentEvents.LightGroupDistribution _rotationYDistribution;
    [SerializeField]
    protected DefaultEnvironmentEvents.LightGroupDistribution _rotationZDistribution;
    [SerializeField]
    protected DefaultEnvironmentEvents.LightGroupFiltering _rotationFiltering;
    [Header("Translation")]
    [SerializeField]
    protected float _translationX;
    [SerializeField]
    protected float _translationY;
    [SerializeField]
    protected float _translationZ;
    [SerializeField]
    protected DefaultEnvironmentEvents.LightGroupDistribution _translationXDistribution;
    [SerializeField]
    protected DefaultEnvironmentEvents.LightGroupDistribution _translationYDistribution;
    [SerializeField]
    protected DefaultEnvironmentEvents.LightGroupDistribution _translationZDistribution;
    [SerializeField]
    protected DefaultEnvironmentEvents.LightGroupFiltering _translationFiltering;

    public LightGroupSO lightGroup => this._lightGroup;

    public EnvironmentColorType environmentColorType => this._environmentColorType;

    public float brightness => this._brightness;

    public DefaultEnvironmentEvents.LightGroupDistribution brightnessDistribution => this._brightnessDistribution;

    public DefaultEnvironmentEvents.LightGroupFiltering brightnessFiltering => this._brightnessFiltering;

    public float rotationX => this._rotationX;

    public float rotationY => this._rotationY;

    public float rotationZ => this._rotationZ;

    public DefaultEnvironmentEvents.LightGroupDistribution rotationXDistribution => this._rotationXDistribution;

    public DefaultEnvironmentEvents.LightGroupDistribution rotationYDistribution => this._rotationYDistribution;

    public DefaultEnvironmentEvents.LightGroupDistribution rotationZDistribution => this._rotationZDistribution;

    public DefaultEnvironmentEvents.LightGroupFiltering rotationFiltering => this._rotationFiltering;

    public float translationX => this._translationX;

    public float translationY => this._translationY;

    public float translationZ => this._translationZ;

    public DefaultEnvironmentEvents.LightGroupDistribution translationXDistribution => this._translationXDistribution;

    public DefaultEnvironmentEvents.LightGroupDistribution translationYDistribution => this._translationYDistribution;

    public DefaultEnvironmentEvents.LightGroupDistribution translationZDistribution => this._translationZDistribution;

    public DefaultEnvironmentEvents.LightGroupFiltering translationFiltering => this._translationFiltering;
  }
}
