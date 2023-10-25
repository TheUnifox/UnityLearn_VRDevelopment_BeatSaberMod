// Decompiled with JetBrains decompiler
// Type: PS4LevelProductsModelSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class PS4LevelProductsModelSO : PersistentScriptableObject
{
  [SerializeField]
  protected PS4LevelProductsModelSO.LevelPackProductData[] _levelPackProductsData = new PS4LevelProductsModelSO.LevelPackProductData[0];
  protected Dictionary<string, PS4LevelProductsModelSO.LevelProductData> _levelIdToProductData = new Dictionary<string, PS4LevelProductsModelSO.LevelProductData>();
  protected Dictionary<string, PS4LevelProductsModelSO.LevelPackProductData> _levelPackIdToProductData = new Dictionary<string, PS4LevelProductsModelSO.LevelPackProductData>();

  public PS4LevelProductsModelSO.LevelPackProductData[] levelPackProductsData => this._levelPackProductsData;

  protected override void OnEnable()
  {
    base.OnEnable();
    this._levelPackIdToProductData.Clear();
    this._levelIdToProductData.Clear();
    foreach (PS4LevelProductsModelSO.LevelPackProductData levelPackProductData in this._levelPackProductsData)
    {
      this._levelPackIdToProductData.Add(levelPackProductData.levelPackId, levelPackProductData);
      foreach (PS4LevelProductsModelSO.LevelProductData levelProductData in levelPackProductData.levelProductsData)
        this._levelIdToProductData.Add(levelProductData.levelId, levelProductData);
    }
  }

  public virtual void SetLevelPackProductsData(
    PS4LevelProductsModelSO.LevelPackProductData[] levelPackProductsData)
  {
    this._levelPackProductsData = levelPackProductsData;
  }

  public virtual PS4LevelProductsModelSO.LevelProductData GetLevelProductData(string levelId)
  {
    PS4LevelProductsModelSO.LevelProductData levelProductData = (PS4LevelProductsModelSO.LevelProductData) null;
    return !this._levelIdToProductData.TryGetValue(levelId, out levelProductData) ? (PS4LevelProductsModelSO.LevelProductData) null : levelProductData;
  }

  public virtual PS4LevelProductsModelSO.LevelPackProductData GetLevelPackProductData(
    string levelPackId)
  {
    PS4LevelProductsModelSO.LevelPackProductData levelPackProductData = (PS4LevelProductsModelSO.LevelPackProductData) null;
    return !this._levelPackIdToProductData.TryGetValue(levelPackId, out levelPackProductData) ? (PS4LevelProductsModelSO.LevelPackProductData) null : levelPackProductData;
  }

  [Serializable]
  public class LevelProductData
  {
    [SerializeField]
    protected string _entitlementLabel;
    [SerializeField]
    protected string _productLabel;
    [SerializeField]
    protected string _levelId;

    public string entitlementLabel => this._entitlementLabel;

    public string productLabel => this._productLabel;

    public string levelId => this._levelId;

    public LevelProductData(string levelId, string productLabel, string entitlementLabel)
    {
      this._levelId = levelId;
      this._productLabel = productLabel;
      this._entitlementLabel = entitlementLabel;
    }
  }

  [Serializable]
  public class LevelPackProductData
  {
    [SerializeField]
    protected string _productLabel;
    [SerializeField]
    protected string _categoryLabel;
    [SerializeField]
    protected string _packId;
    [SerializeField]
    protected float _packLevelPriceDiscountMul;
    [SerializeField]
    protected PS4LevelProductsModelSO.LevelProductData[] _levelProductsData = new PS4LevelProductsModelSO.LevelProductData[0];

    public string productLabel => this._productLabel;

    public string categoryLabel => this._categoryLabel;

    public string levelPackId => this._packId;

    public float packLevelPriceDiscountMul => this._packLevelPriceDiscountMul;

    public PS4LevelProductsModelSO.LevelProductData[] levelProductsData => this._levelProductsData;

    public LevelPackProductData(
      string productLabel,
      string categoryLabel,
      string levelPackId,
      float packLevelPriceDiscountMul,
      PS4LevelProductsModelSO.LevelProductData[] levelProductsData)
    {
      this._productLabel = productLabel;
      this._categoryLabel = categoryLabel;
      this._packId = levelPackId;
      this._packLevelPriceDiscountMul = packLevelPriceDiscountMul;
      this._packLevelPriceDiscountMul = packLevelPriceDiscountMul;
      this._levelProductsData = levelProductsData;
    }
  }
}
