// Decompiled with JetBrains decompiler
// Type: PS4PublisherSKUSettingsSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class PS4PublisherSKUSettingsSO : PersistentScriptableObject
{
  [SerializeField]
  protected string _skuName;
  [SerializeField]
  protected string _serviceIdPrefix;
  [SerializeField]
  protected string _titleId;
  [SerializeField]
  protected string _productLabel;
  [SerializeField]
  protected int _parentalLockLevel;
  [SerializeField]
  protected string _npTitleFilenamePath;
  [SerializeField]
  protected int _defaultAgeRestriction;
  [SerializeField]
  protected PS4ApplicationCategory _applicationCategory;
  [SerializeField]
  protected string _masterVersion;
  [SerializeField]
  protected string _applicationVersion;

  public string skuName => this._skuName;

  public string serviceIdPrefix => this._serviceIdPrefix;

  public string titleId => this._titleId;

  public string productLabel => this._productLabel;

  public int parentalLockLevel => this._parentalLockLevel;

  public string npTitleFilenamePath => this._npTitleFilenamePath;

  public int defaultAgeRestriction => this._defaultAgeRestriction;

  public PS4ApplicationCategory applicationCategory => this._applicationCategory;

  public string masterVersion => this._masterVersion;

  public string applicationVersion => this._applicationVersion;
}
