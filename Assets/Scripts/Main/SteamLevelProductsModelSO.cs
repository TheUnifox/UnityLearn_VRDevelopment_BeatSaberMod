// Decompiled with JetBrains decompiler
// Type: SteamLevelProductsModelSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class SteamLevelProductsModelSO : PersistentScriptableObject
{
  [SerializeField]
  protected SteamLevelProductsModelSO.LevelPackProductData[] _levelPackProductsData = new SteamLevelProductsModelSO.LevelPackProductData[0];
  protected Dictionary<string, SteamLevelProductsModelSO.LevelProductData> _levelIdToProductData = new Dictionary<string, SteamLevelProductsModelSO.LevelProductData>();
  protected Dictionary<string, SteamLevelProductsModelSO.LevelPackProductData> _levelPackIdToProductData = new Dictionary<string, SteamLevelProductsModelSO.LevelPackProductData>();

  public SteamLevelProductsModelSO.LevelPackProductData[] levelPackProductsData => this._levelPackProductsData;

  protected override void OnEnable()
  {
    base.OnEnable();
    this._levelPackIdToProductData.Clear();
    this._levelIdToProductData.Clear();
    foreach (SteamLevelProductsModelSO.LevelPackProductData levelPackProductData in this._levelPackProductsData)
    {
      this._levelPackIdToProductData.Add(levelPackProductData.levelPackId, levelPackProductData);
      foreach (SteamLevelProductsModelSO.LevelProductData levelProductData in levelPackProductData.levelProductsData)
        this._levelIdToProductData.Add(levelProductData.levelId, levelProductData);
    }
  }

  public virtual SteamLevelProductsModelSO.LevelProductData GetLevelProductData(string levelId)
  {
    SteamLevelProductsModelSO.LevelProductData levelProductData = (SteamLevelProductsModelSO.LevelProductData) null;
    return !this._levelIdToProductData.TryGetValue(levelId, out levelProductData) ? (SteamLevelProductsModelSO.LevelProductData) null : levelProductData;
  }

  public virtual SteamLevelProductsModelSO.LevelPackProductData GetLevelPackProductData(
    string levelPackId)
  {
    SteamLevelProductsModelSO.LevelPackProductData levelPackProductData = (SteamLevelProductsModelSO.LevelPackProductData) null;
    return !this._levelPackIdToProductData.TryGetValue(levelPackId, out levelPackProductData) ? (SteamLevelProductsModelSO.LevelPackProductData) null : levelPackProductData;
  }

  [Serializable]
  public class LevelProductData
  {
    [SerializeField]
    protected uint _appId;
    [SerializeField]
    protected string _levelId;

    public uint appId => this._appId;

    public string levelId => this._levelId;
  }

  [Serializable]
  public class LevelPackProductData
  {
    [SerializeField]
    protected uint _bundleId;
    [SerializeField]
    protected string _levelPackId;
    [SerializeField]
    protected SteamLevelProductsModelSO.LevelProductData[] _levelProductsData = new SteamLevelProductsModelSO.LevelProductData[0];

    public uint bundleId => this._bundleId;

    public string levelPackId => this._levelPackId;

    public SteamLevelProductsModelSO.LevelProductData[] levelProductsData => this._levelProductsData;
  }
}
