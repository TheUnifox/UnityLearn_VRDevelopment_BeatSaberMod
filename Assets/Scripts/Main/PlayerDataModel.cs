// Decompiled with JetBrains decompiler
// Type: PlayerDataModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class PlayerDataModel : MonoBehaviour
{
  [SerializeField]
  protected PlayerDataFileManagerSO _playerDataFileManager;
  protected PlayerData _playerData;

  public PlayerDataFileManagerSO playerDataFileManager => this._playerDataFileManager;

  public PlayerData playerData => this._playerData;

  public virtual void OnEnable() => this.Load();

  public virtual void OnApplicationPause(bool pauseStatus)
  {
    if (!pauseStatus)
      return;
    this.Save();
  }

  public virtual void OnDisable() => this.Save();

  public virtual void ResetData() => this._playerData = this._playerDataFileManager.CreateDefaultPlayerData();

  public virtual void Save() => this._playerDataFileManager.Save(this._playerData);

  public virtual void Load() => this._playerData = this._playerDataFileManager.Load();
}
