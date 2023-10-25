// Decompiled with JetBrains decompiler
// Type: EnvironmentInfoSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;

public class EnvironmentInfoSO : PersistentScriptableObject
{
  public const string kLightGroupSubDir = "LightGroups";
  [SerializeField]
  protected string _environmentName;
  [SerializeField]
  protected ColorSchemeSO _colorScheme;
  [SerializeField]
  protected SceneInfo _sceneInfo;
  [SerializeField]
  protected string _serializedName;
  [Tooltip("Subdirectory of /Assets/Environment/Specific")]
  [SerializeField]
  protected string _environmentAssetDirectory = "";
  [SerializeField]
  protected EnvironmentTypeSO _environmentType;
  [SerializeField]
  protected EnvironmentSizeData _environmentSizeData;
  [SerializeField]
  protected EnvironmentIntensityReductionOptions _environmentIntensityReductionOptions;
  [SerializeField]
  protected List<string> _environmentKeywords;
  [ContextMenuItem("Attempt to autofill light groups", "AutofillLightGroups")]
  [SerializeField]
  protected EnvironmentLightGroups _lightGroups;
  [SerializeField]
  protected DefaultEnvironmentEvents _defaultEnvironmentEvents;

  public SceneInfo sceneInfo => this._sceneInfo;

  public string environmentName => this._environmentName;

  public ColorSchemeSO colorScheme => this._colorScheme;

  public string serializedName => this._serializedName;

  public string environmentAssetDirectory => this._environmentAssetDirectory;

  public EnvironmentTypeSO environmentType => this._environmentType;

  public EnvironmentSizeData environmentSizeData => this._environmentSizeData;

  public EnvironmentIntensityReductionOptions environmentIntensityReductionOptions => this._environmentIntensityReductionOptions;

  public IReadOnlyList<string> environmentKeywords => (IReadOnlyList<string>) this._environmentKeywords;

  public EnvironmentLightGroups lightGroups => this._lightGroups;

  public DefaultEnvironmentEvents defaultEnvironmentEvents => this._defaultEnvironmentEvents;
}
