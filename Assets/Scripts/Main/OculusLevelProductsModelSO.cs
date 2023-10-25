// Decompiled with JetBrains decompiler
// Type: OculusLevelProductsModelSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class OculusLevelProductsModelSO : PersistentScriptableObject
{
  [SerializeField]
  protected OculusLevelProductsModelSO.LevelPackProductData[] _levelPackProductsData = new OculusLevelProductsModelSO.LevelPackProductData[0];
  protected Dictionary<string, OculusLevelProductsModelSO.LevelProductData> _levelIdToProductData = new Dictionary<string, OculusLevelProductsModelSO.LevelProductData>();
  protected Dictionary<string, OculusLevelProductsModelSO.LevelPackProductData> _levelPackIdToProductData = new Dictionary<string, OculusLevelProductsModelSO.LevelPackProductData>();
  protected Dictionary<string, string> _assetFileToSku = new Dictionary<string, string>();

  public OculusLevelProductsModelSO.LevelPackProductData[] levelPackProductsData => this._levelPackProductsData;

  protected override void OnEnable()
  {
    base.OnEnable();
    this._levelPackIdToProductData.Clear();
    this._levelIdToProductData.Clear();
    this._assetFileToSku.Clear();
    foreach (OculusLevelProductsModelSO.LevelPackProductData levelPackProductData in this._levelPackProductsData)
    {
      this._levelPackIdToProductData.Add(levelPackProductData.levelPackId, levelPackProductData);
      foreach (OculusLevelProductsModelSO.LevelProductData levelProductData in levelPackProductData.levelProductsData)
      {
        this._levelIdToProductData.Add(levelProductData.levelId, levelProductData);
        this._assetFileToSku.Add(levelProductData.assetFile, levelProductData.sku);
      }
    }
  }

  public virtual OculusLevelProductsModelSO.LevelProductData GetLevelProductData(string levelId)
  {
    OculusLevelProductsModelSO.LevelProductData levelProductData = (OculusLevelProductsModelSO.LevelProductData) null;
    return !this._levelIdToProductData.TryGetValue(levelId, out levelProductData) ? (OculusLevelProductsModelSO.LevelProductData) null : levelProductData;
  }

  public virtual OculusLevelProductsModelSO.LevelPackProductData GetLevelPackProductData(
    string levelPackId)
  {
    OculusLevelProductsModelSO.LevelPackProductData levelPackProductData = (OculusLevelProductsModelSO.LevelPackProductData) null;
    return !this._levelPackIdToProductData.TryGetValue(levelPackId, out levelPackProductData) ? (OculusLevelProductsModelSO.LevelPackProductData) null : levelPackProductData;
  }

  public virtual string GetLevelSku(string assetFile)
  {
    string str = (string) null;
    return !this._assetFileToSku.TryGetValue(assetFile, out str) ? (string) null : str;
  }

  [Serializable]
  public class LevelProductData
  {
    [SerializeField]
    protected string _levelId;
    [SerializeField]
    protected string _sku;

    public string sku => this._sku;

    public string levelId => this._levelId;

    public string assetFile => this._levelId.ToLower();
  }

  [Serializable]
  public class LevelPackProductData
  {
    [SerializeField]
    protected string _sku;
    [SerializeField]
    protected string _levelPackId;
    [SerializeField]
    protected OculusLevelProductsModelSO.LevelProductData[] _levelProductsData = new OculusLevelProductsModelSO.LevelProductData[0];

    public string sku => this._sku;

    public string levelPackId => this._levelPackId;

    public OculusLevelProductsModelSO.LevelProductData[] levelProductsData => this._levelProductsData;
  }
}
